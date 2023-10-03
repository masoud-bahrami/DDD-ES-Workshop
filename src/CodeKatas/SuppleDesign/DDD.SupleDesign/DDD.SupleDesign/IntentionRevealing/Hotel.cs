namespace DDD.SuppleDesign.IntentionRevealing;


public class Room
{
    public Room(int number, string hotelId)
    {
        Number = number;
        HotelId = hotelId;
    }

    public string Name { get; set; }

    public string HotelId { get; set; }
    public int Number { get; set; }
}


public class Hotel
{
    public string Id { get; set; }
    public ICollection<Room> Rooms { get; set; }

    public Room AddRome(int number)
    {
        return new Room(number, Id);
    }
}