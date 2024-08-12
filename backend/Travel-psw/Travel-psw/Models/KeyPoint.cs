﻿using System.ComponentModel.DataAnnotations;

namespace Travel_psw.Models
{
    public class KeyPoint
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public int TourId { get; set; }

    }
}
