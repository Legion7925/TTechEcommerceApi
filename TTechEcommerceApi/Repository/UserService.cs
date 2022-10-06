using AutoMapper;
using BCrypt.Net;
using EcommerceApi.Entities;
using Microsoft.EntityFrameworkCore;
using TTechEcommerceApi.Helper;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.Repository
{
    public class UserService
    {
        private readonly EcommerceContext context;
        private readonly IMapper mapper;

        public UserService(EcommerceContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<User> GetUsers()
        {
            return context.Users.AsNoTracking();
        }

        public async Task<User> Register(UserRegisterRequest model)
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

            user.Role = isFirstUser ? Role.Admin : Role.User;
            user.Created = DateTime.Now;

            //hashing password 
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            //todo should send user verification email with a verification token

            return user;
        }
    }
}
