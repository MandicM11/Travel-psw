using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Travel_psw.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }  // Strani ključ za korisnika

        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; } = 0;

        // Relacija prema stavkama u korpi
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        // Navigaciono svojstvo za korisnika
        public User User { get; set; }  // Navigacija prema korisniku

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
