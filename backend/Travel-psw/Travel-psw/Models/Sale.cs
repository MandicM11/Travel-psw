using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel_psw.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TourId { get; set; }

        public Tour Tour { get; set; }  // Navigaciona osobina za Tour

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }  // Navigaciona osobina za User

        [Required]
        public DateTime SaleDate { get; set; }
    }
}
