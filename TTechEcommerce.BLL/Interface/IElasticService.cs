namespace TTechEcommerce.BLL.Interface;

public interface IElasticService<T> where T : class
{
    Task<IEnumerable<T>> SearchDocument(string query);
    Task<T> SaveSingleAsync(T entity);
    Task SaveManyAsync(T[] entities);
    Task SaveBulkAsync(T[] entities);
}
