using NUnit.Framework;
using Shopcart;

namespace Shopcart.Tests
{     
    [TestFixture]
    public class ShopcartTests
    {
        [Test]
        public void AddProduct_ShouldIncreaseCount()
        {
            var cart = new Shopcart();
            cart.AddProduct(new Product("Mouse", 50.00m));

            Assert.AreEqual(1, cart.ProductCount());
        }

        [Test]
        public void GetTotal_ShouldReturnCorrectSum()
        {
            var cart = new Shopcart();
            cart.AddProduct(new Product("Mouse", 50.00m));
            cart.AddProduct(new Product("Teclado", 80.00m));

            var total = cart.GetTotal();
            Assert.AreEqual(130.00m, total);
        }

        [Test]
        public void GetTotal_EmptyCart_ReturnsZero()
        {
            var cart = new Shopcart();
            var total = cart.GetTotal();

            Assert.AreEqual(0.00m, total);
        }
    }
}
