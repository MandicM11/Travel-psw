using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Travel_psw.Models
{
    public class User
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress] 
        public string Email { get; set; }

        public List<string> Interests { get; set; } 
    }
}
