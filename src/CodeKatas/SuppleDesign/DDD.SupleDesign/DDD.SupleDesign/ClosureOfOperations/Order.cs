namespace DDD.SuppleDesign.ClosureOfOperations;

public class Order
{
    public Guid OrderId { get; private set; }
    private List<OrderItem> _orderItems;

    public Order(Guid orderId)
    {
        OrderId = orderId;
        _orderItems = new List<OrderItem>();
    }

    public void AddOrderItem(OrderItem orderItem)
    {
        // Perform any necessary validation or business rules here
        _orderItems.Add(orderItem);
    }

    public void RemoveOrderItem(OrderItem orderItem)
    {
        // Perform any necessary validation or business rules here
        _orderItems.Remove(orderItem);
    }

    // Other methods and properties...
}