﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Travel_psw.Models


{

    public enum UserRole
    {
        User,       // Običan korisnik
        Author,     // Autor
        Admin       // Administrator
    }
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

        [Required]
        public UserRole Role { get; set; } = UserRole.User;

        public ICollection<Tour> Tours { get; set; } = new List<Tour>();
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();

        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

        public int Points { get; set; } = 0;
        public bool IsAwarded { get; set; } = false;

        public bool IsBlocked { get; set; } = false;
        public bool IsMalicious { get; set; } = false;
        public int InvalidReportCount { get; set; } = 0; // Broj prijavljenih problema koji su odbačeni
        public int UnresolvedReviewCount { get; set; } = 0; // Broj neresenih reviews za korisnika 
    }
}
