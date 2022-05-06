using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.User
{
    public class UserAdd
    {
        [MaxLength(30), Required]
        public string FirstName { get; set; }
        [MaxLength(30), Required]
        public string LastName { get; set; }
        [MaxLength(20), Required]
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
