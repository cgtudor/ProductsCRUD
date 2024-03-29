﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ProductsCRUD.AutomatedCacher.Model;
using ProductsCRUD.DomainModels;
using ProductsCRUD.DTOs;
using ProductsCRUD.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCRUD.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductsRepository _productsRepository;
        private IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheModel _memoryCacheModel;

        // Repository, mapper and memory cache are provided from Startup through dependency injection
        public ProductController(IProductsRepository productsRepository, IMapper mapper, IMemoryCache memoryCache,
            IOptions<MemoryCacheModel> memoryCacheModel)
        {
            _productsRepository = productsRepository;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _memoryCacheModel = memoryCacheModel.Value;
        }

        /// <summary>
        /// GET all products.
        /// /api/products
        /// </summary>
        /// <returns>A list of all products.</returns>
        /// <response code="200">Retrieval of the products was successful</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Lack of required permissions.</response>
        [HttpGet]
        [Authorize("GetProducts")]
        public async Task<ActionResult<IEnumerable<ProductReadDTO>>> GetAllProducts()
        {
            // If we have the list of products already cached, we return it from the cache.
            if (_memoryCache.TryGetValue(_memoryCacheModel.Products, out List<ProductDomainModel> productValues))
                return Ok(_mapper.Map<IEnumerable<ProductReadDTO>>(productValues));

            var productsDomainModels = await _productsRepository.GetAllProductsAsync();

            // We use the mapper to turn the domain models into read DTOs.
            return Ok(_mapper.Map<IEnumerable<ProductReadDTO>>(productsDomainModels));
        }

        /// <summary>
        /// GET all products' price history.
        /// /api/products/prices
        /// </summary>
        /// <returns>A list of all price changes ever made.</returns>
        /// <response code="200">Retrieval of the product price histories was successful</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Lack of required permissions.</response>
        [HttpGet("prices")]
        [Authorize("GetProductPrices")]
        public async Task<ActionResult<IEnumerable<ProductPricesReadDTO>>> GetAllProductPrices()
        {
            var productPricesDomainModels = await _productsRepository.GetAllProductsPricesAsync();

            // We use the mapper to turn the domain models into read DTOs.
            return Ok(_mapper.Map<IEnumerable<ProductPricesReadDTO>>(productPricesDomainModels));
        }

        /// <summary>
        /// GET individual product.
        /// /api/products/{id}
        /// </summary>
        /// <param name="ID">Represents the product ID and is used to get a specific product.</param>
        /// <returns>The product requested</returns>
        /// <response code="200">Retrieval of the product was successful</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Lack of required permissions.</response>
        /// <response code="404">Product with the provided ID does not exist.</response>
        [HttpGet("{ID}")]
        [Authorize("GetProduct")]
        [ActionName(nameof(GetProduct))]
        public async Task<ActionResult<ProductReadDTO>> GetProduct(int ID)
        {
            ProductDomainModel productDomainModel;
            //If cache exists and we find the entity.
            if (_memoryCache.TryGetValue(_memoryCacheModel.Products, out List<ProductDomainModel> productValues))
            {
                //Return the entity if we find it in the cache.
                productDomainModel = productValues.Find(o => o.ProductID == ID);
                if (productDomainModel != null)
                    return Ok(_mapper.Map<ProductReadDTO>(productDomainModel));

                //Otherwise, get the entity from the DB, add it to the cache and return it.
                productDomainModel = await _productsRepository.GetProductAsync(ID);
                if (productDomainModel != null)
                {
                    productValues.Add(productDomainModel);
                    return Ok(_mapper.Map<ProductReadDTO>(productDomainModel));
                }

                throw new ResourceNotFoundException("A resource for ID: " + ID + " does not exist.");
            }

            var dbProductDomainModel = await _productsRepository.GetProductAsync(ID);

            if (dbProductDomainModel != null)
                return Ok(_mapper.Map<ProductReadDTO>(dbProductDomainModel));

            throw new ResourceNotFoundException("A resource for ID: " + ID + " does not exist.");
        }

        /// <summary>
        /// GET an individual product's price history
        /// /api/products/{id}/prices
        /// </summary>
        /// <param name="ID">Represents the product ID and is used to get a specific product's history.</param>
        /// <returns>Price history of the product requested</returns>
        /// <response code="200">Retrieval of product price history was successful</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Lack of required permissions.</response>
        /// <response code="404">Product with the provided ID does not exist.</response>
        [HttpGet("{ID}/prices")]
        [Authorize("GetProductPriceHistory")]
        [ActionName(nameof(GetProductPriceHistory))]
        public async Task<ActionResult<IEnumerable<ProductPricesReadDTO>>> GetProductPriceHistory(int ID)
        {
            var productPricesDomainModels = await _productsRepository.GetProductPricesAsync(ID);
            return Ok(_mapper.Map<IEnumerable<ProductPricesReadDTO>>(productPricesDomainModels));
        }

        /// <summary>
        /// POST information for creating a product.
        /// Creates an item based on the information received
        /// /api/products
        /// </summary>
        /// <param name="productCreateDTO">The properties supplied to create a product from the POSTing API.</param>
        /// <returns>CreatedAtAction result containing the endpoint for reading the new item, the new item's ID and the model.</returns>
        /// <response code="200">Creation of the product was successful</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Lack of required permissions.</response>
        [HttpPost]
        [Authorize("CreateProduct")]
        public async Task<ActionResult> CreateProduct([FromBody] ProductCreateDTO productCreateDTO)
        {
            var productModel = _mapper.Map<ProductDomainModel>(productCreateDTO);

            int newProductID = _productsRepository.CreateProduct(productModel);

            await _productsRepository.SaveChangesAsync();

            // We add the new item to our cache for easier retrieval.
            if (_memoryCache.TryGetValue(_memoryCacheModel.Products, out List<ProductDomainModel> productValues))
                productValues.Add(productModel);

            return CreatedAtAction(nameof(GetProduct), new { ID = newProductID }, productModel);
        }

        /// <summary>
        /// This function will update a product with the passed in parameters.
        /// /api/products/{id}
        /// </summary>
        /// <param name="ID">The ID of the product that will be updated.</param>
        /// <returns></returns>
        /// <response code="200">Patching of the product was successful</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Lack of required permissions.</response>
        /// <response code="404">Product with the provided ID does not exist.</response>
        [HttpPatch("{ID}")]
        [Authorize("UpdateProduct")]
        public async Task<ActionResult> UpdateProduct(int ID, JsonPatchDocument<ProductEditDTO> productEditPatch)
        {
            var productModel = await _productsRepository.GetProductAsync(ID);
            if (productModel == null)
                throw new ResourceNotFoundException();

            var newProduct = _mapper.Map<ProductEditDTO>(productModel);
            productEditPatch.ApplyTo(newProduct, ModelState);

            if (!TryValidateModel(newProduct))
                throw new ArgumentException();

            _mapper.Map(newProduct, productModel);

            _productsRepository.UpdateProduct(productModel);
            await _productsRepository.SaveChangesAsync();

            //Update cache with newly updated products.
            if (_memoryCache.TryGetValue(_memoryCacheModel.Products, out List<ProductDomainModel> productValues))
            {
                productValues.RemoveAll(o => o.ProductID == productModel.ProductID);
                productValues.Add(productModel);
            }

            return Ok();
        }

        /// <summary>
        /// DELETE a product.
        /// /api/products/{id}
        /// </summary>
        /// <param name="ID">Represents the product ID and is used to delete a specific product.</param>
        /// <returns></returns>
        /// <response code="204">Deleting of the product was successful</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Lack of required permissions.</response>
        /// <response code="404">Product with the provided ID does not exist.</response>
        [HttpDelete("{ID}")]
        [Authorize("DeleteProduct")]
        [ActionName(nameof(DeleteProduct))]
        public async Task<ActionResult<ProductReadDTO>> DeleteProduct(int ID)
        {
            var productDomainModel = await _productsRepository.GetProductAsync(ID);

            if (productDomainModel == null)
                throw new ResourceNotFoundException();

            await _productsRepository.DeleteProductAsync(ID);

            await _productsRepository.SaveChangesAsync();

            //Update cache with newly updated products.
            if (_memoryCache.TryGetValue(_memoryCacheModel.Products, out List<ProductDomainModel> productValues))
            {
                productValues.RemoveAll(o => o.ProductID == productDomainModel.ProductID);
            }

            return NoContent();
        }

        /// <summary>
        /// This function is used to add stock to a product
        /// /api/products/{id}/stock
        /// </summary>
        /// <param name="addStockDTO">The stock to add to the product.</param>
        /// <param name="ID"> ID of the product to add stock to.</param>
        /// <returns></returns>
        /// <response code="200">Adding of stock to the product was successful</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Lack of required permissions.</response>
        /// <response code="404">Product with the provided ID does not exist.</response>
        [HttpPost("{ID}/stock")]
        [Authorize("UpdateProduct")]
        public async Task<ActionResult> AddStock(int ID, [FromBody] ProductAddStockDTO addStockDTO)
        {

            var dbProductDomainModel = await _productsRepository.GetProductAsync(ID);

            if (dbProductDomainModel == null)
                throw new ResourceNotFoundException("A resource for ID: " + ID + " does not exist.");

            _productsRepository.AddStock(ID, addStockDTO.ProductQuantityToAdd);

            await _productsRepository.SaveChangesAsync();

            // If we have the product in our cache, we update the cache as well.
            if (_memoryCache.TryGetValue(_memoryCacheModel.Products, out List<ProductDomainModel> productValues))
            {
                //If the entity is cached we add the quantity to it
                var productDomainModel = productValues.Find(o => o.ProductID == ID);
                if (productDomainModel != null)
                    productDomainModel.ProductQuantity += addStockDTO.ProductQuantityToAdd;
            }

            return Ok();
        }

    }
}
