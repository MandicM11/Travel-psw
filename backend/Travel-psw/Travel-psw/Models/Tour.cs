using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


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
        public string Category { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        public TourStatus Status { get; set; } = TourStatus.Draft;

        public List<KeyPoint> KeyPoints { get; set; } = new List<KeyPoint>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public int AuthorId { get; set; }
        
        public List<Sale> Sales { get; set; } = new List<Sale>();
        [JsonIgnore]
        public User Author { get; set; } // Navigaciono svojstvo za User
    }

    public enum TourStatus
    {
        Draft,
        Published,
        Archived
    }
}
