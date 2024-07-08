namespace ShoppingCart.Models.Goods;

public class Mayo365 : Product
{
    public Mayo365()
    {
        productId = 3;
        price = 51.99;
        weight = 0.35;
        this.name = "Mayo 365";
        date = new DateOnly(2024, 10, 26);
        parametr = "67%";
    }
    public override string ToString()
    {
        return $"{name}, {price}, {weight}, {parametr}";
    }
}