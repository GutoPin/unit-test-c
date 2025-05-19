using NUnit.Framework;
using Moq;
using Shopcart;

namespace Shopcart.Tests
{
    [TestFixture]
    public class ShopcartIntegrationTests
    {
        private Mock<IShopcart> _shopcartMock;
        private ShopcartManager _manager;

        [SetUp]
        public void Setup()
        {
            _shopcartMock = new Mock<IShopcart>();

            _manager = new ShopcartManager(_shopcartMock.Object);
        }

        [Test]
        public void AddItemsAndCheck_ShouldCallAddProductForEachItem()
        {
            var products = new[]
            {
                new Product("Mouse", 50m),
                new Product("Teclado", 80m)
            };

            _manager.AddItemsAndCheck(products);

            _shopcartMock.Verify(s => s.AddProduct(It.IsAny<Product>()), Times.Exactly(products.Length));
        }

        [Test]
        public void IsOverLimit_WhenTotalIsGreaterThanLimit_ReturnsTrue()
        {
            
            _shopcartMock.Setup(s => s.GetTotal()).Returns(150m);

            
            var result = _manager.IsOverLimit(100m);

            
            Assert.IsTrue(result);
            _shopcartMock.Verify(s => s.GetTotal(), Times.Once);
        }

        [Test]
        public void IsOverLimit_WhenTotalIsLessThanLimit_ReturnsFalse()
        {
            
            _shopcartMock.Setup(s => s.GetTotal()).Returns(80m);

           
            var result = _manager.IsOverLimit(100m);

            
            Assert.IsFalse(result);
            _shopcartMock.Verify(s => s.GetTotal(), Times.Once);
        }
    }
}
