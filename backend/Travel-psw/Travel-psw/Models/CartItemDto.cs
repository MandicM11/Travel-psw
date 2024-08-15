using System.ComponentModel.DataAnnotations;

namespace Travel_psw.Models
{
    public class CartItemDto
    {
        [Required]
        public int TourId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
