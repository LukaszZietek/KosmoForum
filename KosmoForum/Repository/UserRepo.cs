﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KosmoForum.DbContext;
using KosmoForum.Models;
using KosmoForum.Repository.IRepository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KosmoForum.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;
        public UserRepo(ApplicationDbContext db, IOptions<AppSettings> appSettings)
        {
            _db = db;
            _appSettings = appSettings.Value;
        }
        public bool IsUniqueUser(string username)
        {
            return !(_db.Users.Any(x => x.Username == username));
        }

        public User GetUser(int userId)
        {
            var userObj = _db.Users.SingleOrDefault(x => x.Id == userId);
            return userObj;
        }

        public User Authenticate(string username, string password)
        {
            var user = _db.Users.SingleOrDefault(x => x.Username == username);
            if (user == null)
            {
                return null;
            }

            if (!(PasswordHasher.Verify(password, user.Password)))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
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

        public User Register(string username, string password, string email, byte[] avatar = null)
        {

            User userObj = new User()
            {
                Username = username,
                Password = PasswordHasher.Hash(password),
                Role = "CommonUser",
                Avatar = avatar,
                Email = email,
                JoinDateTime = DateTime.Now
            };

            _db.Users.Add(userObj);
            _db.SaveChanges();
            userObj.Password = "";
            return userObj;
        }

        public int GetUserIdUsingName(string username)
        {
            int?  id = _db.Users.FirstOrDefault(x => x.Username == username)?.Id;
            return id != null ? (int)id : 0;
        }

        public byte[] GetUserAvatar(int userId)
        {
            byte[] userAvatar = _db.Users.FirstOrDefault(x => x.Id == userId)?.Avatar;
            return userAvatar;
        }


        public bool UpdateUser(User userObj)
        {
            _db.Users.Update(userObj);
            return Save();
        }


        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
