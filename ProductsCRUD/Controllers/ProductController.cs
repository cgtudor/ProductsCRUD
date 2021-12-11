using AutoMapper;
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
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReadDTO>>> GetAllProducts()
        {
            if (_memoryCache.TryGetValue(_memoryCacheModel.Products, out List<ProductDomainModel> productValues))
                return Ok(_mapper.Map<IEnumerable<ProductReadDTO>>(productValues));

            var productsDomainModels = await _productsRepository.GetAllProductsAsync();
            return Ok(_mapper.Map<IEnumerable<ProductReadDTO>>(productsDomainModels));
        }

        /// <summary>
        /// GET individual product.
        /// /api/products/{id}
        /// </summary>
        /// <param name="ID">Represents the product ID and is used to get a specific product.</param>
        /// <returns></returns>
        [HttpGet("{ID}")]
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
        /// This function is used to create a product.
        /// /api/products
        /// </summary>
        /// <param name="orderCreateDTO">The properties supplied to create a product from the POSTing API.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("CreateProduct")]
        public async Task<ActionResult> CreateProduct([FromBody] ProductCreateDTO orderCreateDTO)
        {
            var productModel = _mapper.Map<ProductDomainModel>(orderCreateDTO);

            int newProductID = _productsRepository.CreateProduct(productModel);

            await _productsRepository.SaveChangesAsync();

            if (_memoryCache.TryGetValue(_memoryCacheModel.Products, out List<ProductDomainModel> productValues))
                productValues.Add(productModel);

            return CreatedAtAction(nameof(GetProduct), new { ID = newProductID }, productModel);
        }

        /// <summary>
        /// This function will update a product with the passed in parameters.
        /// </summary>
        /// <param name="ID">The ID of the product that will be updated.</param>
        /// <returns></returns>
        /// <response code="200">Patching of the product was successful</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpPatch("{ID}")]
        [Authorize("UpdateProduct")]
        public async Task<ActionResult> UpdateProduct(int ID, JsonPatchDocument<ProductEditDTO> productEditPatch)
        {
            var productModel = await _productsRepository.GetProductAsync(ID);
            if (productModel == null)
                return NotFound();

            var newProduct = _mapper.Map<ProductEditDTO>(productModel);
            productEditPatch.ApplyTo(newProduct, ModelState);

            if (!TryValidateModel(newProduct))
                return ValidationProblem(ModelState);

            _mapper.Map(newProduct, productModel);

            _productsRepository.UpdateProduct(productModel);
            await _productsRepository.SaveChangesAsync();

            //Update cache with newly updated orders.
            if (_memoryCache.TryGetValue(_memoryCacheModel.Products, out List<ProductDomainModel> productValues))
            {
                productValues.RemoveAll(o => o.ProductID == productModel.ProductID);
                productValues.Add(productModel);
            }

            return Ok();
        }

        /// <summary>
        /// DELETE individual product.
        /// /api/products/{id}
        /// </summary>
        /// <param name="ID">Represents the product ID and is used to delete a specific product.</param>
        /// <returns></returns>
        [HttpDelete("{ID}")]
        [Authorize("DeleteProduct")]
        [ActionName(nameof(GetProduct))]
        public async Task<ActionResult<ProductReadDTO>> DeleteProduct(int ID)
        {
            try
            {
                var productDomainModel = await _productsRepository.GetProductAsync(ID);

                if (productDomainModel == null)
                    return NotFound($"Product with ID = {ID} not found.");

                await _productsRepository.DeleteProductAsync(ID);

                await _productsRepository.SaveChangesAsync();

                //Update cache with newly updated orders.
                if (_memoryCache.TryGetValue(_memoryCacheModel.Products, out List<ProductDomainModel> productValues))
                {
                    productValues.RemoveAll(o => o.ProductID == productDomainModel.ProductID);
                }

                return NoContent();
            } catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }


    }
}
