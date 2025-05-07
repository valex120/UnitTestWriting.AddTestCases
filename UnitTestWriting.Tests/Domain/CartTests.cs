using Xunit;
using UnitTestWriting.Domain;

namespace UnitTestWriting.Tests.Domain
{
    public class CartTests
    {
        [Fact]
        public void Test()
        {
            // Arrange

            // Act

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void AddProduct_ShouldAddNewProductToCart()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                BirthDate = DateTime.Today,
                Premium = false,
                CreatedAt = DateTimeOffset.Now
            };
            var cart = new Cart(user);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Article = "TP001",
                Price = 1000
            };

            cart.AddProduct(product, 3);

            Assert.Single(cart.Products);
            Assert.Equal(product.Id, cart.Products[0].Product.Id);
            Assert.Equal(3, cart.Products[0].Amount);
        }
    }
}