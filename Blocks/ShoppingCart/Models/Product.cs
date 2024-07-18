namespace ShoppingCart.Models;

public class Product
{
    public int productId { get; set; }
    public string? name { get; set; }
    public double price{ get; set; }
    public double weight{ get; set; }
    public DateOnly date{ get; set; }
    public string parametr{ get; set; }
    /// <summary>
    /// Перегрузка метода ToString для вывода информации о продукте
    /// </summary>
    /// <returns>Строку с информацией о продукте</returns>
    public override string ToString()
    {
        return $"Id: {productId}\nname: {name}\nprice: {price}\nweight: {weight}\ndate: {date}\nparam: {parametr}\n";
    }/// <summary>
     /// Перегрузка оператора сложения
     /// </summary>
     /// <param name="product">Продукт, который мы хотим добавить в заказ</param>
     /// <param name="product2">Продукт, который мы хотим добавить в заказ</param>
     /// <returns>Новый заказ с добавленными продуктами</returns>
    public static Order operator +(Product product, Product product2)
    {
        
        Order order = new Order();
        order.AddProduct(product.productId, 1);
        order.AddProduct(product2.productId, 1);
        return order;
    }
}