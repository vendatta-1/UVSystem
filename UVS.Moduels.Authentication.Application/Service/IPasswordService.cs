namespace UVS.Modules.Authentication.Application.Service;

public interface IPasswordService
{
    string HashPassword(string password);
    string HashPassword(string password, string salt);
    string GeneratePassword(int length=8);
}