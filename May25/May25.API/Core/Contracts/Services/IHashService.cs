namespace May25.API.Core.Contracts.Services
{
    public interface IHashService
    {
        string HashPassword(string password);
        bool ValidatePassword(string password, string passwordHash);
    }
}
