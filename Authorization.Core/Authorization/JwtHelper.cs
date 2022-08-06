using Authorization.Abstractions.Jwt;
using Authorization.Entities.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Authorization.Core.Authorization
{
    public class JwtHelper
    {
        private static JwtOptions _jwtOptions;

        public JwtHelper(JwtOptions jwOptions)
        {
            _jwtOptions = jwOptions;
        }

        public string GenerateJwtToken(UserEntity user)
        {
            var securityKey = _jwtOptions.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (nameof(UserEntity.UserName), user.UserName)
            };

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims, 
                expires: DateTime.UtcNow.AddSeconds(_jwtOptions.TokenLifetime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
