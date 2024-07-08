namespace ShoppingCart.Models;

public class Order
{
    public uint orderId;
    public double orderSum;
    public double orderWeight;
    private Dictionary<uint, Product> products;

    public Order(uint orderId)
    {
        this.orderId = orderId;
    }

    public void AddProduct(Product product, uint count)
    {
        products.Add(count, product);
    }
    
    
}