using System;
using System.ComponentModel.DataAnnotations;

namespace Travel_psw.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TourId { get; set; }

        public Tour Tour { get; set; }

        public decimal Amount { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }

        [Required]
        public DateTime SaleDate { get; set; }
    }
}
