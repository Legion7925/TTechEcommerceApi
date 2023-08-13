using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.Repository
{
    public class CachedUserService : IUserService
    {
        private readonly UserService decorated;
        private readonly IDistributedCache _distributedCache;

        public CachedUserService(UserService decorated, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.decorated = decorated;
            _distributedCache = distributedCache;
        }

        public Task<AuthenticateResponseModel> Authenticate(AuthenticateRequestModel model)
        {
            return decorated.Authenticate(model);
        }

        public Task Delete(int userId) => decorated.Delete(userId);

        public async Task<IEnumerable<UserResponseModel>> GetAllUsers()
        {
            var key = "users";

            byte[]? cachedUserData = await _distributedCache.GetAsync(key);


            IEnumerable<UserResponseModel>? users;
            if (cachedUserData is null)
            {
                users = decorated.GetAllUsers().GetAwaiter().GetResult();

                if (!users.Any())
                {
                    return users;
                }
                var cachedDataString = JsonConvert.SerializeObject(users);
                await _distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(cachedDataString));

                return users;
            }

            users = JsonConvert.DeserializeObject<IEnumerable<UserResponseModel>>(Encoding.UTF8.GetString(cachedUserData));

            return users ?? new List<UserResponseModel>();
        }

        public Task<UserResponseModel> Register(UserRequestModel model)
        {
            return decorated.Register(model);
        }

        public Task<UserResponseModel> Update(UserRequestModel model, int userId)
        {
            return decorated.Update(model, userId);
        }
    }
}
