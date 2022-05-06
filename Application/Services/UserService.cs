using Domain.Models;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class UserService : IUser
    {
        private readonly ManagerContext _context;
        public UserService( ManagerContext context)
        {
            _context = context;
        }

        #region ENCRYPTION
        string EncryptData(string valueToEncrypt)
        {
            string GenerateSalt()
            {
                RNGCryptoServiceProvider crypto = new();
                byte[] salt = new byte[32];
                crypto.GetBytes(salt);

                return Convert.ToBase64String(salt);
            }
            string EncryptValue(string strvalue)
            {
                string saltValue = GenerateSalt();
                byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue + strvalue);
                SHA256Managed hashstr = new SHA256Managed();
                byte[] hash = hashstr.ComputeHash(saltedPassword);

                return $"{Convert.ToBase64String(hash)}:{saltValue}";
            }
            return EncryptValue(valueToEncrypt);
        }
        bool ValidateEncryptedData(string valueToValidate, string valueFromDatabase)
        {
            string[] arrValues = valueFromDatabase.Split(':');
            string encryptedDbValue = arrValues[0];
            string salt = arrValues[1];
            byte[] saltedValue = Encoding.UTF8.GetBytes(salt + valueToValidate);
            SHA256Managed hashstr = new SHA256Managed();
            byte[] hash = hashstr.ComputeHash(saltedValue);
            string enteredValueToValidate = Convert.ToBase64String(hash);

            return encryptedDbValue == enteredValueToValidate;
        }


        #endregion

        public async Task<IEnumerable<User>> FindUserByNameAsync(string firstOrLastName)
        {
            return await _context.UserRegistry
                                        .Where(a =>
                                                a.FirstName == firstOrLastName ||
                                                a.LastName == firstOrLastName)
                                        .ToListAsync();
        }

        public async Task<User> FindUserByUsernameAsync(string username)
        {
            User user = null;
            if (!string.IsNullOrEmpty(username))
            {
                user = await _context.UserRegistry
                                            .SingleOrDefaultAsync(a=>
                                                         a.Username == username);

            }
            return user;
        }

        public async Task<User> LoginAsync(User user)
        {

            User usr = null;
            var obj = await _context.UserRegistry
                                    .SingleOrDefaultAsync(a => 
                                            a.Username == user.Username);

            if (ValidateEncryptedData(user.Password, obj.Password))
            {
                usr = obj;
            }            
            return usr;
        }

        public async Task<User> RegisterAsync(User user)
        {
            var duplicates = await _context.UserRegistry.Where(a => a.Username == user.Username).ToListAsync();

            if(duplicates.Any())
            {
                throw new ApplicationException($"Duplicate username found. {user.Username}.");
            }
            user.Password = EncryptData(user.Password);
            await _context.UserRegistry.AddAsync(user);
            _ = await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> RetrieveAllUsersAsync()
        {
            return await _context.UserRegistry
                                    .ToListAsync();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }

        public void UpdateInformation(User user)
        {
        }
    }
}
