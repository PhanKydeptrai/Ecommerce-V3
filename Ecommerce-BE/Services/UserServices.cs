using AutoMapper;
using Data;
using Domain.DTO_Models;
using Domain.Models;
using Ecommerce_BE.IServices;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Ecommerce_BE.Services
{
    public class UserServices : IUserServices
    {
        public UserServices() { }
        private readonly EcommerceDbContext _context;
        private readonly IMapper _mapper;
        public UserServices(EcommerceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //get all users
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        //get user by id
        public async Task<User> GetUserById(Guid id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == id);
        }

        //get user by email
        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == email);
        }

        //create user
        public async Task<bool> CreateUser(UserCreationDto user)
        {
            //Auto mapper
            _context.Users.AddAsync(_mapper.Map<User>(user));
            //Kiểm tra 
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                return true;
            }
            return false;
        }

        //Update
        public async Task<bool> UpdateUser(UserCreationDto user)
        {
            _context.Users.Update(_mapper.Map<User>(user));
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                return true;
            }
            return false;
        }

        //Remove
        public async Task<bool> RemoveUser(Guid id)
        {
            User user = await GetUserById(id);
            user.Status = "nhd";
            _context.Users.Update(user);
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                return true;
            }
            return false;
        }
        //Account Restore
        public async Task<bool> AccountRestore(Guid id)
        {
            User user = _mapper.Map<User>(await GetUserById(id));
            user.Status = "hd";
            _context.Users.Update(user);
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                return true;
            }
            return false;
        }
        //Encrypt password
        public async Task<string> EncryptPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }
        //SendMail
        public async Task SendEmail(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("kyfan2778@gmail.com", "pweb xfhe hdjx bjrd"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("kyfan2778@gmail.com"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
        //Random 
        public async Task<string> RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder sb = new StringBuilder(10);
            Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                int index = random.Next(chars.Length);
                sb.Append(chars[index]);
            }
            return sb.ToString();
        }
    }
}
