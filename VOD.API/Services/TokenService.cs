using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOD.Common.DTOModels;
using VOD.Database.Services;

namespace VOD.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _users;
        
        public Task<TokenDTO> GenerateTokenAsync(LoginUserDTO loginUserDTO)
        {
            throw new NotImplementedException();
        }

        public Task<TokenDTO> GetTokenAsync(LoginUserDTO loginUserDTO, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
