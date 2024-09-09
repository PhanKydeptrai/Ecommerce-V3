
using Domain.DTO_Models;
using Domain.Models;

namespace Ecommerce_BE.IServices
{
    public interface IUserServices
    {
        //Get List of Users
        Task<IEnumerable<User>> GetUsers();
        //Get User by Id
        Task<User> GetUserById(Guid id);
        //Get User by Email
        Task<User> GetUserByEmail(string email);
        //Create User
        Task<bool> CreateUser(UserCreationDto user);
        //Update User
        Task<bool> UpdateUser(UserCreationDto user);
        //Remove User by Id
        Task<bool> RemoveUser(Guid id); //Just update the status of user
        //Encrypt password
        Task<string> EncryptPassword(string password);
        //Account Restore
        Task<bool> AccountRestore(Guid id);
        //SendMail
        Task SendEmail(string email, string subject, string message);
        //Random 
        Task<string> RandomString();
    }
}
