namespace ShoppingCart.Models.Goods;

public class TomatoFlamenko : Product
{
    public TomatoFlamenko()
    {
        productId = 4;
        price = 244.99;
        weight = 1.00;
        name = "Tomato Flamenko";
        date = new DateOnly(2024, 11, 4);
        parametr = "Russia";
    }
    public override string ToString()
    {
        return $"{name}, {price}, {weight}, {parametr}";
    }
}