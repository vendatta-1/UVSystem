using System.Security.Cryptography;
using System.Text;
using UVS.Modules.Authentication.Application.Service;

namespace UVS.Authentication.Infrastructure.Services;

public sealed class PasswordService : IPasswordService
{
    private const string ValidChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";

    public string HashPassword(string password)
    {
        throw new NotImplementedException();
    }

    public string HashPassword(string password, string salt)
    {
        throw new NotImplementedException();
    }

    public string GeneratePassword(int length = 8)
    {
        
        var result = new StringBuilder();
        using var rng = RandomNumberGenerator.Create();

        byte[] buffer = new byte[sizeof(uint)];

        while (result.Length < length)
        {
            rng.GetBytes(buffer);
            uint num = BitConverter.ToUInt32(buffer, 0);
            var idx = (int)(num % (uint)ValidChars.Length);
            result.Append(ValidChars[idx]);
        }

        return result.ToString();
    }
}
