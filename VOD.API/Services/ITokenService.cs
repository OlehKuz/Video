using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOD.Common.DTOModels;

namespace VOD.API.Services
{
    public interface ITokenService
    {
        Task<TokenDTO> GenerateTokenAsync(LoginUserDTO loginUserDTO);
        Task<TokenDTO> GetTokenAsync(LoginUserDTO loginUserDTO, string userId); 
    }
}
