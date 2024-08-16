using System.Collections.Generic;

namespace Travel_psw.Models
{
    public class UserReport
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalTours { get; set; }
        public decimal PercentageChange { get; set; }
        public List<Tour> BestSellingTours { get; set; }
        public List<Tour> UnsoldTours { get; set; }
    }
}
