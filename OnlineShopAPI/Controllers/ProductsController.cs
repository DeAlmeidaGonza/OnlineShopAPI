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
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            List<Product> ProductList;
            ProductList = Products.getAllProducts();
            return Ok(ProductList);
        }

        [HttpGet("{Product_id}")]
        public IActionResult GetProductById(int Product_id)
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
    }
}
