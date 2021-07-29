using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineShopAPI.Models;

namespace OnlineShopAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// Get a list of all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            List<Product> ProductList;
            ProductList = Products.getAllProducts();
            return Ok(ProductList);
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="Product_id"></param>
        /// <returns></returns>
        [HttpGet("{Product_id}")]
        public IActionResult GetProductById(ulong Product_id)
        {
            Product OneProduct = Products.getProductById(Product_id);
            if (OneProduct != null)
            {
                return Ok(OneProduct);
            }
            else
            {
                return StatusCode(404, "El producto no existe.");
            }
        }

        /// <summary>
        /// Get a list of products by category
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        [HttpGet("getByCategory/{Category}")]
        public IActionResult GetProductByCategory(string Category)
        {
            List<Product> ProductList;
            ProductList = Products.getProductByCategory(Category);
            if (ProductList != null)
            {
                return Ok(ProductList);
            }
            else
            {
                return StatusCode(404, "La categoria no existe.");
            }
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="CreateProduct"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateNewProduct(Product CreateProduct)
        {
            string Errors;
            Product NewProduct = Products.createNewProduct(CreateProduct, out Errors);

            if (NewProduct != null)
            {
                return Ok(NewProduct);
            }
            else
            {
                return StatusCode(401, Errors);
            }
        }

        /// <summary>
        /// Delete a product by id
        /// </summary>
        /// <param name="Product_id"></param>
        /// <returns></returns>
        [HttpDelete("{Product_id}")]
        public IActionResult DeleteProductById(ulong Product_id)
        {
            String Errors;
            bool Success = Products.deleteProductById(Product_id, out Errors);
            if (Success)
            {
                return Ok("Producto borrado. Id:" + Product_id);
            }
            else
            {
                return StatusCode(404, Errors);
            }
        }
    }
}
