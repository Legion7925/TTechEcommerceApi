using Microsoft.Extensions.Caching.Memory;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.Repository
{
    public class CachedUserService : IUserService
    {
        private readonly UserService decorated;
        private readonly IMemoryCache memoryCache;

        public CachedUserService(UserService decorated, IMemoryCache memoryCache)
        {
            this.decorated = decorated;
            this.memoryCache = memoryCache;
        }

        public Task<AuthenticateResponseModel> Authenticate(AuthenticateRequestModel model)
        {
            return decorated.Authenticate(model);
        }

        public Task Delete(int userId) => decorated.Delete(userId);

        public IEnumerable<UserResponseModel> GetAllUsers()
        {
            var key = "UserList";

             return memoryCache.GetOrCreate(key, entry =>
             {
                 entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                 entry.SetSlidingExpiration(TimeSpan.FromMinutes(1));
                 return decorated.GetAllUsers();
             });
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
