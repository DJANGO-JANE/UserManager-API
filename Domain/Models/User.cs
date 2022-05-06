using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(30),Required]
        public string FirstName { get; set; }
        [MaxLength(30),Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [MaxLength(20)]
        public string Username { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateJoined { get; set; } = DateTime.Now;


    }
}
