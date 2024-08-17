using System.ComponentModel.DataAnnotations;

namespace Travel_psw.Models
{
    public class SaleDTO
    {
    [Required]
    public int TourId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public DateTime SaleDate { get; set; }
    }

}
