using ProductsCRUD.Context;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ProductsCRUD.DomainModels;
using ProductsCRUD.Repositories.Concrete;
using ProductsCRUD.Controllers;

namespace ProductsCRUDTests
{
    public class ProductSqlRepoTest
    {
        public ProductSqlRepoTest() { }

        private ProductDomainModel[] GetTestProducts() => new ProductDomainModel[]
        {
            new ProductDomainModel {ProductID = 0, ProductName = "TestProduct", ProductDescription = "Tasty and savory.", ProductPrice = 1.56, ProductQuantity = 4},
            new ProductDomainModel {ProductID = 1, ProductName = "Heineken 0.5L", ProductDescription = "Blonde beer. Yummy.", ProductPrice = 2.56, ProductQuantity = 45},
            new ProductDomainModel { ProductID = 2, ProductName = "Bread", ProductDescription = "Fresh.", ProductPrice = 0.56, ProductQuantity = 1243 },
            new ProductDomainModel { ProductID = 3, ProductName = "Nails", ProductDescription = "Pack of 50 nails.", ProductPrice = 4.5, ProductQuantity = 23 },
            new ProductDomainModel { ProductID = 4, ProductName = "Candle", ProductDescription = "Aroma like you've never smelled before.", ProductPrice = 3.56, ProductQuantity = 43 }
        };

        private ProductPricesDomainModel[] GetTestPrices() => new ProductPricesDomainModel[]
        {
            new ProductPricesDomainModel { ProductPriceID = 1, ProductID = 1, ProductPrice = 10, PriceChangeDate = new DateTime(2020, 1, 1) },
            new ProductPricesDomainModel { ProductPriceID = 2, ProductID = 1, ProductPrice = 2.56, PriceChangeDate = new DateTime(2020, 1, 26) },
            new ProductPricesDomainModel { ProductPriceID = 3, ProductID = 2, ProductPrice = 11.82, PriceChangeDate = new DateTime(2020, 1, 1) },
            new ProductPricesDomainModel { ProductPriceID = 4, ProductID = 2, ProductPrice = 0.56, PriceChangeDate = new DateTime(2020, 1, 1) },
            new ProductPricesDomainModel { ProductPriceID = 5, ProductID = 3, ProductPrice = 5.4, PriceChangeDate = new DateTime(2020, 1, 21) }
        };

        private Mock<Context> GetDbContext()
        {
            var context = new Mock<Context>();
            context.Object.AddRange(GetTestProducts());
            context.Object.SaveChanges();

            return context;
        }

        private Mock<DbSet<ProductDomainModel>> GetMockDbSet()
        {
            return GetTestProducts().AsQueryable().BuildMockDbSet();
        }

        private Mock<DbSet<ProductPricesDomainModel>> GetMockDbSetPrices()
        {
            return GetTestPrices().AsQueryable().BuildMockDbSet();
        }

        [Fact]
        public async void GetAllProductsAsync_ShouldReturnListOfProducts()
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._products).Returns(dbSetMock.Object);
            var sqlProductsCRUDRepository = new SqlProductsRepository(dbContextMock.Object);
            var expectedResult = GetTestProducts();

            //Act
            var result = await sqlProductsCRUDRepository.GetAllProductsAsync();

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<List<ProductDomainModel>>(result);
            var model = Assert.IsAssignableFrom<List<ProductDomainModel>>(actionResult);
        }

        [Fact]
        public async void GetAllProductsAsync_ShouldReturnAllProducts()
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._products).Returns(dbSetMock.Object);
            var sqlProductsCRUDRepository = new SqlProductsRepository(dbContextMock.Object);
            var expectedResult = GetTestProducts();

            //Act
            var result = await sqlProductsCRUDRepository.GetAllProductsAsync();

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<List<ProductDomainModel>>(result);
            var model = Assert.IsAssignableFrom<List<ProductDomainModel>>(actionResult);
            Assert.Equal(expectedResult.Count(), model.Count());
        }

        [Fact]
        public async void GetAllProductsAsync_ShouldReturnAllCorrectProducts()
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._products).Returns(dbSetMock.Object);
            var sqlProductsCRUDRepository = new SqlProductsRepository(dbContextMock.Object);
            var expectedResult = GetTestProducts();

            //Act
            var result = await sqlProductsCRUDRepository.GetAllProductsAsync();

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<List<ProductDomainModel>>(result);
            var model = Assert.IsAssignableFrom<List<ProductDomainModel>>(actionResult);
            Assert.Equal(expectedResult.Count(), model.Count());
            model.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void GetAllProductsAsync_WhenNoProducts_ShouldReturnEmptyList()
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = new ProductDomainModel[0].AsQueryable().BuildMockDbSet();
            dbContextMock.SetupGet(c => c._products).Returns(dbSetMock.Object);
            var sqlProductsCRUDRepository = new SqlProductsRepository(dbContextMock.Object);
            var expectedResult = new ProductDomainModel[0];

            //Act
            var result = await sqlProductsCRUDRepository.GetAllProductsAsync();

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<List<ProductDomainModel>>(result);
            var model = Assert.IsAssignableFrom<List<ProductDomainModel>>(actionResult);
            Assert.Equal(expectedResult.Count(), model.Count());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(4)]
        public async void GetProductAsync_ShouldReturnProductModel(int ID)
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._products).Returns(dbSetMock.Object);
            var sqlProductsCRUDRepository = new SqlProductsRepository(dbContextMock.Object);
            var expectedResult = GetTestProducts().FirstOrDefault(dr => dr.ProductID == ID);

            //Act
            var result = await sqlProductsCRUDRepository.GetProductAsync(ID);

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ProductDomainModel>(result);
            var model = Assert.IsAssignableFrom<ProductDomainModel>(actionResult);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(4)]
        public async void GetProductAsync_ShouldReturnProduct(int ID)
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._products).Returns(dbSetMock.Object);
            var sqlProductsCRUDRepository = new SqlProductsRepository(dbContextMock.Object);
            var expectedResult = GetTestProducts().FirstOrDefault(dr => dr.ProductID == ID);

            //Act
            var result = await sqlProductsCRUDRepository.GetProductAsync(ID);

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ProductDomainModel>(result);
            var model = Assert.IsAssignableFrom<ProductDomainModel>(actionResult);
            model.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(-5)]
        [InlineData(int.MaxValue)]
        public async void GetProductAsync_WhenNotFound_ReturnsNull(int ID)
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._products).Returns(dbSetMock.Object);
            var sqlProductsCRUDRepository = new SqlProductsRepository(dbContextMock.Object);

            //Act
            var result = await sqlProductsCRUDRepository.GetProductAsync(ID);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void SaveChangesAsync_ShouldSaveChanges()
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._products).Returns(dbSetMock.Object);
            dbContextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                         .Verifiable();
            var sqlProductsCRUDRepository = new SqlProductsRepository(dbContextMock.Object);

            //Act
            await sqlProductsCRUDRepository.SaveChangesAsync();

            //Assert
            dbContextMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(4)]
        public async void DeleteProductAsync_ShouldReturnProductModel(int ID)
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._products).Returns(dbSetMock.Object);
            var sqlProductsCRUDRepository = new SqlProductsRepository(dbContextMock.Object);
            var expectedResult = GetTestProducts().FirstOrDefault(dr => dr.ProductID == ID);

            //Act
            var result = await sqlProductsCRUDRepository.DeleteProductAsync(ID);

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ProductDomainModel>(result);
            var model = Assert.IsAssignableFrom<ProductDomainModel>(actionResult);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(4)]
        public async void DeleteProductAsync_ShouldReturnProductDeleted(int ID)
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._products).Returns(dbSetMock.Object);
            var sqlProductsCRUDRepository = new SqlProductsRepository(dbContextMock.Object);
            var expectedResult = GetTestProducts().FirstOrDefault(dr => dr.ProductID == ID);

            //Act
            var result = await sqlProductsCRUDRepository.DeleteProductAsync(ID);

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ProductDomainModel>(result);
            var model = Assert.IsAssignableFrom<ProductDomainModel>(actionResult);
            model.Should().BeEquivalentTo(expectedResult);
        }

        [Theory, MemberData(nameof(SplitUpdateData))]
        public void UpdateProduct_ShouldUpdateProduct(ProductDomainModel productDomainModel)
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            var dbSetPricesMock = GetMockDbSetPrices();
            dbContextMock.SetupGet(c => c._products).Returns(dbSetMock.Object);
            dbContextMock.SetupGet(c => c._productPrices).Returns(dbSetPricesMock.Object);
            var sqlProductsCRUDRepository = new SqlProductsRepository(dbContextMock.Object);

            //Act
            sqlProductsCRUDRepository.UpdateProduct(productDomainModel);

            //Assert
            dbSetMock.Verify(r => r.Update(It.IsAny<ProductDomainModel>()), Times.Once);
        }

        [Fact]
        public void UpdateProduct_WhenNullPassed_ThrowsArgumentNull()
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._products).Returns(dbSetMock.Object);
            var sqlProductsCRUDRepository = new SqlProductsRepository(dbContextMock.Object);

            //Act
            Assert.Throws<ArgumentNullException>(() => sqlProductsCRUDRepository.UpdateProduct(null));
        }

        [Theory]
        [InlineData(0, 20)]
        [InlineData(2, 30)]
        [InlineData(4, 40)]
        public void AddStock_ShouldAddStock(int ID, int stockToAdd)
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._products).Returns(dbSetMock.Object);
            var sqlProductsCRUDRepository = new SqlProductsRepository(dbContextMock.Object);

            //Act
            sqlProductsCRUDRepository.AddStock(ID, stockToAdd);
        }

        public static IEnumerable<object[]> SplitUpdateData =>
        new List<object[]>
        {
            new object[] {new ProductDomainModel {ProductID = 0, ProductName = "TestProduct", ProductDescription = "Tasty and savory.", ProductPrice = 1.56, ProductQuantity = 4} },
            new object[] {new ProductDomainModel {ProductID = 1, ProductName = "Heineken 0.5L", ProductDescription = "Blonde beer. Yummy.", ProductPrice = 2.56, ProductQuantity = 45} },
            new object[] {new ProductDomainModel { ProductID = 2, ProductName = "Bread", ProductDescription = "Fresh.", ProductPrice = 0.56, ProductQuantity = 1243 } }
        };
    }
}