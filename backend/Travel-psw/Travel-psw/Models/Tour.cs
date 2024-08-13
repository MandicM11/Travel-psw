using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace Travel_psw.Models
{
    public class Tour
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Difficulty { get; set; }

        [Required]
        public string Category { get; set; } // Povezivanje sa interesovanjima korisnika

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        public TourStatus Status { get; set; } = TourStatus.Draft; // draft ili published

        public List<KeyPoint> KeyPoints { get; set; } = new List<KeyPoint>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum TourStatus
    {
        Draft,
        Published,
        Archived
    }


}
