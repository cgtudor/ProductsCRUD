using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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

        public ProductController(IProductsRepository productsRepository, IMapper mapper)
        {
            _productsRepository = productsRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// GET all products.
        /// /api/products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductReadDTO>>> GetAllProducts()
        {
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
        [Authorize]
        [ActionName(nameof(GetProduct))]
        public async Task<ActionResult<ProductReadDTO>> GetProduct(int ID)
        {
            var productDomainModel = await _productsRepository.GetProductAsync(ID);

            if (productDomainModel != null)
                return Ok(_mapper.Map<ProductReadDTO>(productDomainModel));

            return NotFound();
        }

        /// <summary>
        /// This function is used to create a product.
        /// /api/products
        /// </summary>
        /// <param name="orderCreateDTO">The properties supplied to create a product from the POSTing API.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateProduct([FromBody] ProductCreateDTO orderCreateDTO)
        {
            var productModel = _mapper.Map<ProductDomainModel>(orderCreateDTO);

            int newProductID = _productsRepository.CreateProduct(productModel);

            await _productsRepository.SaveChangesAsync();

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
        [Authorize]
        public async Task<ActionResult> UpdateOrder(int ID, JsonPatchDocument<ProductEditDTO> productEditPatch)
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

            return Ok();
        }

    }
}
