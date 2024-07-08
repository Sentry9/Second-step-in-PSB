namespace ShoppingCart.Models.Goods;

public class TomatoPink : Product
{
    public TomatoPink()
    {
        productId = 1;
        price = 139.99;
        weight = 1.00;
        name = "Tomato Pink";
        date = new DateOnly(2024, 11, 5);
        parametr = "Azerbaijan";
    }

    public override string ToString()
    {
        return $"{name}, {price}, {weight}, {parametr}";
    }
}