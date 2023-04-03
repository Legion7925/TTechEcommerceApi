using AutoMapper;
using BCrypt.Net;
using EcommerceApi.Entities;
using Microsoft.EntityFrameworkCore;
using TTechEcommerceApi.Helper;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Model;
using TTechEcommerceApi.Shared.Enum;

namespace TTechEcommerceApi.Repository
{
    public class UserService : IUserService
    {
        private readonly EcommerceContext context;
        private readonly IMapper mapper;
        private readonly IJwtUtilities jwtUtilities;

        public UserService(EcommerceContext context, IMapper mapper , IJwtUtilities jwtUtilities)
        {
            this.context = context;
            this.mapper = mapper;
            this.jwtUtilities = jwtUtilities;
        }

        public IEnumerable<UserResponseModel> GetAllUsers()
        {
            var users = context.Users.AsNoTracking();
            return mapper.Map<IEnumerable<UserResponseModel>>(users);
        }

        private async Task<User> GetUserById(int userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(i => i.Id == userId);
            if (user == null)
                throw new TTechException("User Not Found!");
            return user;
        }

        public async Task<UserResponseModel> Register(UserRequestModel model)
        {
            var usernameExits = context.Users.Any(i => i.Username == model.Username);
            if (usernameExits)
                throw new TTechException("username already exists please choose a different one");

            var emailExits = context.Users.Any(i => i.Email == model.Email);
            if (emailExits)
            {
                //todo this block should send an email to the already registered user 
                throw new TTechException("email already exists!");
            }

            var user = mapper.Map<User>(model);

            var isFirstUser = context.Users.Any();

            user.Role = isFirstUser ? Role.User : Role.Admin;
            user.Created = DateTime.Now;

            //hashing password 
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            //todo should send user verification email with a verification token

            return mapper.Map<UserResponseModel>(user);
        }

        public async Task<AuthenticateResponseModel> Authenticate(AuthenticateRequestModel model)
        {
            var findUser = await context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (findUser == null || !BCrypt.Net.BCrypt.Verify(model.Password, findUser.PasswordHash))
                throw new TTechException("user name or password is incorrect !");

            var jwtToken = jwtUtilities.GenerateJwtToken(findUser);

            var response = mapper.Map<AuthenticateResponseModel>(findUser);
            response.JwtToken = jwtToken;

            return response;
        }

        public async Task<UserResponseModel> Update(UserRequestModel model, int userId)
        {
            var user = await GetUserById(userId);

            if (model.Email != user.Email && context.Users.Any(i => i.Email == model.Email))
                throw new TTechException($"Email {model.Email} already exists please choose a different one !");

            if (model.Username != user.Username && context.Users.Any(i => i.Username == model.Username))
                throw new TTechException($"User name {model.Username} already exists");

            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            mapper.Map(model, user);

            await context.SaveChangesAsync();
            return mapper.Map<UserResponseModel>(model);
        }

        public async Task Delete(int userId)
        {
            var user = await GetUserById(userId);
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}
