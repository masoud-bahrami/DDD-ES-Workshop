namespace Zero.EventSourcing;

public class NextPage
{
    public long NextSkip { get; set; }
    public long NextTake { get; set; }

    public NextPage(long nextSkip, long nextTake)
    {
        NextSkip = nextSkip;
        NextTake = nextTake;
    }

    public static NextPage Empty()
        => new NextPage(0, 0);
}