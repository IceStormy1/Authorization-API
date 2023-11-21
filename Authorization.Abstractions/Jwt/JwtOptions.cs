using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authorization.Abstractions.Jwt;

public class JwtOptions
{
    /// <summary>
    /// Кто генирирует токен 
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// Для кого предназначается токен
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// Секретная строка, которая будет использоваться для генерации ключа симметричного шифрования
    /// </summary>
    public string Secret { get; set; }

    /// <summary>
    /// Время жизни токена в секундах 
    /// </summary>
    public int TokenLifetime { get; set; }

    /// <summary>
    /// Генерация ключа шифрования
    /// </summary>
    /// <returns></returns>
    public SymmetricSecurityKey GetSymmetricSecurityKey()
        => new (Encoding.ASCII.GetBytes(Secret));
}