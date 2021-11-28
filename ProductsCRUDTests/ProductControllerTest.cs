using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsCRUD.Controllers;
using ProductsCRUD.DomainModels;
using ProductsCRUD.DTOs;
using ProductsCRUD.Profiles;
using ProductsCRUD.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProductsCRUDTests
{
    public class ProductControllerTest
    {
        private readonly IMapper _mapper;

        private ProductDomainModel[] GetTestProducts() => new ProductDomainModel[] 
        {
            new ProductDomainModel {ProductID = 0, ProductName = "TestProduct", ProductDescription = "Tasty and savory.", ProductPrice = 1.56, ProductQuantity = 4},
            new ProductDomainModel {ProductID = 1, ProductName = "Heineken 0.5L", ProductDescription = "Blonde beer. Yummy.", ProductPrice = 2.56, ProductQuantity = 45},
            new ProductDomainModel { ProductID = 2, ProductName = "Bread", ProductDescription = "Fresh.", ProductPrice = 0.56, ProductQuantity = 1243 },
            new ProductDomainModel { ProductID = 3, ProductName = "Nails", ProductDescription = "Pack of 50 nails.", ProductPrice = 4.5, ProductQuantity = 23 },
            new ProductDomainModel { ProductID = 4, ProductName = "Candle", ProductDescription = "Aroma like you've never smelled before.", ProductPrice = 3.56, ProductQuantity = 43 }
        };

        private ProductReadDTO[] GetTestReadProducts() => new ProductReadDTO[]
        {
            new ProductReadDTO {ProductID = 0, ProductName = "TestProduct", ProductDescription = "Tasty and savory.", ProductPrice = 1.56, ProductQuantity = 4},
            new ProductReadDTO {ProductID = 1, ProductName = "Heineken 0.5L", ProductDescription = "Blonde beer. Yummy.", ProductPrice = 2.56, ProductQuantity = 45},
            new ProductReadDTO { ProductID = 2, ProductName = "Bread", ProductDescription = "Fresh.", ProductPrice = 0.56, ProductQuantity = 1243 },
            new ProductReadDTO { ProductID = 3, ProductName = "Nails", ProductDescription = "Pack of 50 nails.", ProductPrice = 4.5, ProductQuantity = 23 },
            new ProductReadDTO { ProductID = 4, ProductName = "Candle", ProductDescription = "Aroma like you've never smelled before.", ProductPrice = 3.56, ProductQuantity = 43 }
        };

        public ProductControllerTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductProfile());
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsAllItems()
        {
            // Arrange
            
            var expected = GetTestProducts();
            var mockProducts = new Mock<IProductsRepository>(MockBehavior.Strict);
            mockProducts.Setup(r => r.GetAllProductsAsync())
                .ReturnsAsync(expected)
                .Verifiable();
            var controller = new ProductController(mockProducts.Object, _mapper);
            controller.ModelState.AddModelError("Something", "Something");

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductReadDTO>>(
                viewResult.Value);
            Assert.Equal(expected.Length, model.Count());
            mockProducts.Verify(r => r.GetAllProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsCorrectItems()
        {
            // Arrange

            var expected = GetTestProducts();
            var mockProducts = new Mock<IProductsRepository>(MockBehavior.Strict);
            mockProducts.Setup(r => r.GetAllProductsAsync())
                .ReturnsAsync(expected)
                .Verifiable();
            var controller = new ProductController(mockProducts.Object, _mapper);
            controller.ModelState.AddModelError("Something", "Something");

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductReadDTO>>(
                viewResult.Value);
            var modelFirst = model.First();
            Assert.IsAssignableFrom<ProductReadDTO>(modelFirst);
            Assert.Equal("Tasty and savory.", modelFirst.ProductDescription);
            mockProducts.Verify(r => r.GetAllProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsOkResult()
        {
            // Arrange

            var expected = GetTestProducts();
            var mockProducts = new Mock<IProductsRepository>(MockBehavior.Strict);
            mockProducts.Setup(r => r.GetAllProductsAsync())
                .ReturnsAsync(expected)
                .Verifiable();
            var controller = new ProductController(mockProducts.Object, _mapper);
            controller.ModelState.AddModelError("Something", "Something");

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetAll_WhenCalledWithNoProducts_ReturnsNoItems()
        {
            // Arrange
            var expected = new ProductDomainModel[] { };
            var mockProducts = new Mock<IProductsRepository>(MockBehavior.Strict);
            mockProducts.Setup(r => r.GetAllProductsAsync())
                .ReturnsAsync(expected)
                .Verifiable();
            var controller = new ProductController(mockProducts.Object, _mapper);
            controller.ModelState.AddModelError("Something", "Something");

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductReadDTO>>(
                viewResult.Value);
            Assert.Equal(expected.Length, model.Count());
            mockProducts.Verify(r => r.GetAllProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenBadServiceCall_ShouldInternalError()
        {
            // Arrange
            var mockProducts = new Mock<IProductsRepository>(MockBehavior.Strict);
            mockProducts.Setup(r => r.GetAllProductsAsync())
                .ThrowsAsync(new Exception())
                .Verifiable();
            var controller = new ProductController(mockProducts.Object, _mapper);

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError,
                            statusCodeResult.StatusCode);
            mockProducts.Verify(r => r.GetAllProductsAsync(), Times.Once);
        }
    }
}
