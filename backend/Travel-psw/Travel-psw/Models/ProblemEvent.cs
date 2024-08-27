namespace Travel_psw.Models
{
    public abstract class ProblemEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int ProblemId { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
        public string EventType { get; set; }
    }

    public class ProblemReportedEvent : ProblemEvent
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class ProblemResolvedEvent : ProblemEvent
    {
        public DateTime ResolvedAt { get; set; }
    }

    public class ProblemSentForReviewEvent : ProblemEvent
    {
    }

    public class ProblemRejectedEvent : ProblemEvent
    {
    }

    public class ProblemStatusChangedEvent : ProblemEvent
    {
        public ProblemStatus NewStatus { get; set; }
    }


}
