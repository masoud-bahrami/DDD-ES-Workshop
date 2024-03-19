namespace Zero.EventSourcing;

public class PagedEventStreamViewModel : EventStreamViewModel
{

    public NextPage NextPage { get; set; }
    public int RemainingEventCount { get; set; }
    public PagedEventStreamViewModel(ICollection<EventViewModel> events
        , int version
        , bool hasAnyEventYet
        , int remainingEventCount
        , NextPage nextPage)
        : base(events, version, hasAnyEventYet)
    {
        RemainingEventCount = remainingEventCount;
        NextPage = nextPage;
    }

    public static PagedEventStreamViewModel Empty() =>
        new(new List<EventViewModel>(), 0, false, 0, NextPage.Empty());
}