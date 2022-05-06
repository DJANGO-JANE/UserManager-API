using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUser
    {
        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="user"><see cref="User"/> object to login</param>
        /// <returns>A populated <see cref="User"/> object.</returns>
        Task<User> LoginAsync(User user);

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="user"><see cref="User"/> object to register</param>
        /// <returns>Newly registered User</returns>
        Task<User> RegisterAsync(User user);

        /// <summary>
        /// Get a list of all users in database
        /// </summary>
        /// <returns>List of all Users</returns>
        Task<IEnumerable<User>> RetrieveAllUsersAsync();

        /// <summary>
        /// Find <see cref="User"/> using their username
        /// </summary>
        /// <param name="username">Username of <see cref="User"/></param>
        /// <returns>User object if found</returns>
        Task<User> FindUserByUsernameAsync(string username);
        Task<IEnumerable<User>> FindUserByNameAsync(string firstname = null,string lastname = null);

        Task<User> UpdateInformation(string username, User user);

        bool SaveChanges();

    }
}
