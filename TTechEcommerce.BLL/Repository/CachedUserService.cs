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

            //if user list is not cached before it will cache the user
            //list and gets the list from database if it's cached and 
            //the expiration time is not over it will return the user
            //from memoery
            var cachedUserList = memoryCache.GetOrCreate(key, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                entry.SetSlidingExpiration(TimeSpan.FromMinutes(1));
                return decorated.GetAllUsers();
            });
            return cachedUserList ?? new List<UserResponseModel>();
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
