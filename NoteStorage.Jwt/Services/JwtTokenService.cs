using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NoteStorage.Jwt.Interfaces;
using NoteStorage.Logics.ModelsDto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NoteStorage.Jwt.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IOptions<AuthOptions> authOptions;

        public JwtTokenService(IOptions<AuthOptions> authOptions)
        {
            this.authOptions = authOptions;
        }

        public string GenerateToken(UserDto userDto)
        {
            var authParams = authOptions.Value;
            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userDto.Login),
                new Claim(JwtRegisteredClaimNames.Sub, userDto.Id.ToString())
            };

            var token = new JwtSecurityToken(
                authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifeTime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
