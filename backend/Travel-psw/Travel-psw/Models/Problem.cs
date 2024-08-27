namespace Travel_psw.Models
{
    public class Problem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ProblemStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public int TourId { get; set; }
        public int TouristId { get; set; }

        public Tour Tour { get; set; }
    }

    public enum ProblemStatus
    {
        Pending,
        Resolved,
        UnderReview,
        Rejected
    }

}
