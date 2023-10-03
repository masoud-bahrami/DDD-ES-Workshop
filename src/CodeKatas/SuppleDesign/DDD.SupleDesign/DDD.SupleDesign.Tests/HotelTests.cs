using DDD.SuppleDesign.IntentionRevealing;

namespace DDD.SupleDesign.Tests;

public class HotelTests
{
    [Fact]
    public void createHotel()
    {
        // action
        var hotel = new Hotel();
        
        //assertions
    }

    [Fact]
    public void testRoom()
    {
        //
        var hotel = new Hotel();

        Room room= hotel.AddRome(number: 1002);

        // 

        Assert.Equal(1002, room.Number);
    }
}