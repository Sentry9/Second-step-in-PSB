using Moq;
using Calculator.Validator;
using Logger;
using ShoppingCart.Models;

namespace ShoppingCart.Tests
{
    [TestFixture]
    public class OrderCalcTests
    {
        private Mock<ILogger> _loggerMock;
        private Mock<IValidator> _validatorMock;
        private OrderCalc _orderCalc;
        private Product _product1;
        private Product _product2;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger>();
            _validatorMock = new Mock<IValidator>();
            _orderCalc = new OrderCalc(_loggerMock.Object, _validatorMock.Object);

            _product1 = new Product
            {
                productId = 0,
                name = "Cucumber Athlet",
                price = 214.99,
                weight = 1,
                date = new DateOnly(2024, 10, 10),
                parametr = "Georgia"
            };
            _product2 = new Product
            {
                productId = 2,
                name = "Mayo 365",
                price = 51.99,
                weight = 0.35,
                date = new DateOnly(2024, 10, 26),
                parametr = "67%"
            };
        }

        [Test]
        public void Calculate_AddProductToOrder_ReturnsUpdatedOrder()
        {
            // Arrange
            var _order1 = new Order
            {
                orderId = 1,
                orderSum = 214.99,
                orderWeight = 1,
                productsCount = new Dictionary<int, uint> { { 0, 1 } },
                DeliveryDate = new DateOnly(2024, 10, 10),
                products = new Dictionary<int, Product> { { 0, _product1} },
            };
            var expectedOrder = _order1 + _product2;

            // Act
            var result = _orderCalc.Calculate(_order1, "+", _product2);

            // Assert
            Assert.AreEqual(expectedOrder, result);
        }

        [Test]
        public void Calculate_RemoveProductFromOrder_ReturnsUpdatedOrder()
        {
            // Arrange
            var order = new Order
            {
                    orderId = 1,
                    orderSum = 266.98,
                    orderWeight = 1.35,
                    productsCount = new Dictionary<int, uint> { { 0, 1 }, { 2, 1 } },
                    DeliveryDate = new DateOnly(2024, 10, 26),
                    products = new Dictionary<int, Product> { { 0, _product1 }, { 2, _product2 } },
            };
            var expectedOrder = order - _product2;
            // Act
            var result = _orderCalc.Calculate(order, "-", _product2);
            // Assert
            Assert.AreEqual(expectedOrder, result);
        }
        [Test]
        public void Calculate_DivideOrderByProduct_ReturnsUpdatedOrder()
        {
            // Arrange
            var order = new Order
            {
                orderId = 1,
                orderSum = 429.98,
                orderWeight = 2,
                productsCount = new Dictionary<int, uint> { { 0, 2 } },
                DeliveryDate = new DateOnly(2024, 10, 10),
                products = new Dictionary<int, Product> { { 0, _product1 } },
            };
            var expectedOrder = order / _product1;
        
            // Act
            var result = _orderCalc.Calculate(order, "/", _product1);
        
            // Assert
            Assert.AreEqual(expectedOrder, result);
        }
        
        [Test]
        public void Calculate_SubtractOrders_ReturnsUpdatedOrder()
        {
            // Arrange
            var order1 = new Order
            {
                orderId = 1,
                orderSum = 266.98,
                orderWeight = 1.35,
                productsCount = new Dictionary<int, uint> { { 0, 1 }, { 2, 1 } },
                DeliveryDate = new DateOnly(2024, 10, 26),
                products = new Dictionary<int, Product> { { 0, _product1 }, { 2, _product2 } },
            };
            var order2 = new Order
            {
                orderId = 2,
                orderSum = 51.99,
                orderWeight = 0.35,
                productsCount = new Dictionary<int, uint> { { 2, 1 } },
                DeliveryDate = new DateOnly(2024, 10, 26),
                products = new Dictionary<int, Product> { { 2, _product2 } },
            };
            var expectedOrder = order1 - order2;
        
            // Act
            var result = _orderCalc.Calculate(order1, order2);
        
            // Assert
            Assert.AreEqual(expectedOrder, result);
        }
        
        [Test]
        public void Calculate_DivideOrderByNumber_ReturnsUpdatedOrder()
        {
            // Arrange
            var order = new Order
            {
                orderId = 1,
                orderSum = 429.98,
                orderWeight = 2,
                productsCount = new Dictionary<int, uint> { { 0, 2 } },
                DeliveryDate = new DateOnly(2024, 10, 10),
                products = new Dictionary<int, Product> { { 0, _product1 } },
            };
            uint divisor = 2;
            var expectedOrder = order / divisor;
        
            // Act
            var result = _orderCalc.Calculate(order, "/", divisor);
        
            // Assert
            Assert.AreEqual(expectedOrder, result);
        }
        
        [Test]
        public void Calculate_MultiplyOrderByNumber_ReturnsUpdatedOrder()
        {
            // Arrange
            var order = new Order
            {
                orderId = 1,
                orderSum = 214.99,
                orderWeight = 1,
                productsCount = new Dictionary<int, uint> { { 0, 1 } },
                DeliveryDate = new DateOnly(2024, 10, 10),
                products = new Dictionary<int, Product> { { 0, _product1 } },
            };
            uint multiplier = 3;
            var expectedOrder = order * multiplier;
        
            // Act
            var result = _orderCalc.Calculate(order, "*", multiplier);
        
            // Assert
            Assert.AreEqual(expectedOrder, result);
        }
        
        [Test]
        public void Calculate_AddProducts_ReturnsNewOrder()
        {
            // Arrange
            var expectedOrder = _product1 + _product2;
        
            // Act
            var result = _orderCalc.Calculate(_product1, _product2);
        
            // Assert
            Assert.AreEqual(expectedOrder.orderSum, result.orderSum);
            Assert.AreEqual(expectedOrder.orderWeight, result.orderWeight);
            Assert.AreEqual(expectedOrder.productsCount, result.productsCount);
            Assert.AreEqual(expectedOrder.products, result.products);
        }
        
        [Test]
        public void Calculate_InvalidOperation_ReturnsOriginalOrder()
        {
            // Arrange
            var order = new Order
            {
                orderId = 1,
                orderSum = 214.99,
                orderWeight = 1,
                productsCount = new Dictionary<int, uint> { { 0, 1 } },
                DeliveryDate = new DateOnly(2024, 10, 10),
                products = new Dictionary<int, Product> { { 0, _product1 } },
            };
        
            // Act
            var result = _orderCalc.Calculate(order, "invalid_op", _product2);
        
            // Assert
            Assert.AreEqual(order, result);
        }
        
        [Test]
        public void Calculate_InvalidOperationWithNumber_ReturnsOriginalOrder()
        {
            // Arrange
            var order = new Order
            {
                orderId = 1,
                orderSum = 214.99,
                orderWeight = 1,
                productsCount = new Dictionary<int, uint> { { 0, 1 } },
                DeliveryDate = new DateOnly(2024, 10, 10),
                products = new Dictionary<int, Product> { { 0, _product1 } },
            };
        
            // Act
            var result = _orderCalc.Calculate(order, "invalid_op", 2);
        
            // Assert
            Assert.AreEqual(order, result);
        }
    }
}
