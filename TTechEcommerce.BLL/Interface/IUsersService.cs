using EcommerceApi.Entities;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.Interface
{
    public interface IUserService
    {
        Task Delete(int userId);
        //Task<User> GetUserById(int userId);
        Task<IEnumerable<UserResponseModel>> GetAllUsers();
        Task<UserResponseModel> Register(UserRequestModel model);
        Task<AuthenticateResponseModel> Authenticate(AuthenticateRequestModel model);
        Task<UserResponseModel> Update(UserRequestModel model, int userId);

    }
}
