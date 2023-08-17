using EcommerceApi.Entities;
using Microsoft.Extensions.Logging;
using Nest;
using TTechEcommerce.BLL.Interface;

namespace TTechEcommerce.BLL.ElasticSearchRepository;

public class ProductElasticSearchRepository : IElasticService<Product>
{
    private readonly IElasticClient _elasticClient;
    private readonly ILogger<ProductElasticSearchRepository> _logger;

    private List<Product> _cacheProducts = new();

    public ProductElasticSearchRepository(ILogger<ProductElasticSearchRepository> logger, IElasticClient elasticClient)
    {
        _logger = logger;
        _elasticClient = elasticClient;
    }


    public async Task SaveManyAsync(Product[] products)
    {
        _cacheProducts.AddRange(products);
        var result = await _elasticClient.IndexManyAsync(products);
        if (result.Errors)
        {
            foreach (var item in result.ItemsWithErrors)
            {
                _logger.LogError("Failed to to index document {0}: {1}", item.Id, item.Error);
            }
        }
    }

    public async Task SaveBulkAsync(Product[] products)
    {
        _cacheProducts.AddRange(products);
        var result = await _elasticClient.BulkAsync(b => b.Index("Products").IndexMany(products));
        if (result.Errors)
        {
            foreach (var item in result.ItemsWithErrors)
            {
                _logger.LogError("Failed to to index document {0}: {1}", item.Id, item.Error);
            }
        }
    }

    public async Task<Product> SaveSingleAsync(Product product)
    {
        if (_cacheProducts.Any(p => p.Id == product.Id))
        {
            await _elasticClient.UpdateAsync<Product>(product, p => p.Doc(product));
        }
        else
        {
            var response = await _elasticClient.IndexDocumentAsync(product);
            if (!response.IsValid)
            {
                var errorMessage = response.OriginalException?.Message ?? response.ServerError?.ToString();
                _logger.LogError(errorMessage);
                throw new Exception("something went wrong with saving the product");
            }
        }
        return product;
    }

    public async Task<IEnumerable<Product>> SearchDocument(string query)
    {
        List<Product> result;
        if (string.IsNullOrWhiteSpace(query))
        {
            var response = await _elasticClient.SearchAsync<Product>(s => s.Query(q => q.MatchAll()));
            result = response.Hits.Select(i => i.Source).ToList();

        }
        else
        {
            //search by a specific field
            //var searchResponse = await _elasticClient.SearchAsync<Product>(s => s
            //    .Query(q => q
            //        .Match(m => m
            //            .Field(f => f.Name)
            //            .Query(query))));

            var searchResponse = await _elasticClient.SearchAsync<Product>(s => s
                .Query(q => q
                .QueryString(qs => qs
                .Query('*' + query + '*')))
                .Size(5000));

            result = searchResponse.Hits.Select(i => i.Source).ToList();
        }
        return result;
    }
}
