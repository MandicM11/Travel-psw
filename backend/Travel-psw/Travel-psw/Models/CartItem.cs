using System.ComponentModel.DataAnnotations;

namespace Travel_psw.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }  // Strani ključ za korpu

        [Required]
        public int TourId { get; set; }  // Strani ključ za turu

        [Required]
        public int Quantity { get; set; } = 1;

        

        // Navigaciono svojstvo za turu
        public Tour Tour { get; set; }  // Navigacija prema turi

        // Navigaciono svojstvo za korpu
        public Cart Cart { get; set; }  // Navigacija prema korpi
    }
}
