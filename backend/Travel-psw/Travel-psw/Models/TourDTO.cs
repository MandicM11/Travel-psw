namespace Travel_psw.Models
{
    public class TourDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public TourStatus Status { get; set; }

        public int AuthorId { get; set; }
    }

}
