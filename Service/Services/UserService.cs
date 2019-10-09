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
        private readonly UserRepository _userRepository;

        public UserService(IMapper mapper, IOptions<JWTSettings> appSettings, UserRepository userRepository)
        {
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public string Authenticate(LoginViewModel input)
        {
            var passwordHash = Model.Helpers.CryptoHelper.SHA1Hash(input.Password);
            var user = _userRepository.FindByUsernameAndPasswordHash(input.Username, passwordHash);

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
            var user = _userRepository.SingleOrDefault(u => u.Id == id);
            return _mapper.Map<UserViewModel>(user);
        }

        public async Task RegisterAsync(RegisterViewModel input)
        {
            var user = _mapper.Map<User>(input);
            _userRepository.Add(user);
            await _userRepository.SaveAsync();
        }
    }
}