namespace ShoppingCart.Models;

public class Order
{
    public int orderId;
    public double orderSum;
    public double orderWeight;
    public List<Product> products = new List<Product>();
    public int[] counts = new int[6];

    public Order(int orderId)
    {
        this.orderId = orderId;
    }

    public void AddProduct(Product product, int count)
    {
        products.Add(product);
        orderSum += product.price * count;
        orderWeight += product.weight * count;
        counts[products.Count] += count;
    }
    
    
}