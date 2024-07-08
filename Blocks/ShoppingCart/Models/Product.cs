namespace ShoppingCart.Models;

public abstract class Product
{
    public int productId;
    public string name { get; set; }
    public double price{ get; set; }
    public double weight{ get; set; }
    public DateOnly date{ get; set; }
    public string parametr{ get; set; }
}