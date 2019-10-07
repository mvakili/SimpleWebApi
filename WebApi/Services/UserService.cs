using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Entities;
using WebApi.Configurations;
using WebApi.ViewModels;
using WebApi.ViewModels.UserModels;
using System.Threading.Tasks;
using WebApi.Exceptions.UserExceptions;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly JWTSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly WebApiDbContext _dbContext;

        public UserService(IMapper mapper, IOptions<JWTSettings> appSettings, WebApiDbContext dbContext)
        {
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public string Authenticate(LoginViewModel input)
        {
            var passwordHash = Sha1Hash(input.Password);
            var user = _dbContext.Users.SingleOrDefault(x => x.Username == input.Username && x.PasswordHash == passwordHash);

            if (user == null)
                throw new UserNotFoundException();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            return encodedJwt;
        }

        public UserViewModel GetUser(Guid id)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Id == id);
            return _mapper.Map<UserViewModel>(user);
        }

        public async Task RegisterAsync(RegisterViewModel input)
        {
            var passwordHash = Sha1Hash(input.Password);
            var user = new User()
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                PasswordHash = passwordHash,
                Username = input.Username
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        protected byte[] Sha1Hash(string data)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            var sha1data = sha1.ComputeHash(Encoding.ASCII.GetBytes(data));
            return sha1data;
        }
    }
}