using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Model.ViewModels.UserModels;
using Model.Exceptions.UserExceptions;
using Business.Configurations;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using DataAccess.Repositories;
using Model.Entities;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly JWTSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IOptions<JWTSettings> appSettings, IUserRepository userRepository)
        {
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Authenticates user
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="UserNotFoundException">No user matches the input</exception>
        /// <returns>authentication token</returns>
        public string Authenticate(LoginViewModel input)
        {
            var passwordHash = Model.Helpers.CryptoHelper.SHA1Hash(input.Password);
            var user = _userRepository.SingleOrDefault(u => u.Username == input.Username && u.PasswordHash == passwordHash);

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

        public UserViewModel GetIdentityUser(ClaimsIdentity identity)
        {
            var id = Guid.Parse(identity.FindFirst(ClaimTypes.Name).Value);
            var user = _userRepository.SingleOrDefault(u => u.Id == id);
            return _mapper.Map<UserViewModel>(user);
        }

        public async Task RegisterAsync(RegisterViewModel input)
        {
            var user = _mapper.Map<User>(input);

            if(_userRepository.Any(u => u.Username == input.Username))
            {
                throw new DuplicateUsernameException();
            }
            _userRepository.Add(user);
            await _userRepository.SaveAsync();
        }
    }
}