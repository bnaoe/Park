using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkAPI.Data;
using ParkAPI.Models;
using ParkAPI.Repository.IRepository;

namespace ParkAPI.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDBContext _db;
        private readonly AppSettings _appSettings;

        public UserRepository(ApplicationDBContext db, IOptions<AppSettings> appSettings)
        {
            _db = db;
            _appSettings = appSettings.Value;
        }

        public bool IsUniqueUser(string username)
        {
            throw new NotImplementedException();

        }

        public User Authenticate(string username, string password)
        {
            var user = _db.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
            {
                return null;
            }

            //if user was found generate a jwtToken
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }), 
                
                Expires = DateTime.UtcNow.AddDays(7),
                
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            user.Token = tokenHandler.WriteToken(token);

            user.Password = "";

            return user;
        }

        public User Register(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
