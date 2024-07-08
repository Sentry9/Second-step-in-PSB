namespace ShoppingCart.Models;

public class Product
{
    public int productId { get; set; }
    public string name { get; set; }
    public double price{ get; set; }
    public double weight{ get; set; }
    public DateOnly date{ get; set; }
    public string parametr{ get; set; }

    public override string ToString()
    {
        return $"Id: {productId}\nname: {name}\nprice: {price}\nweight: {weight}\ndate: {date}\nparam: {parametr}\n";
    }
}