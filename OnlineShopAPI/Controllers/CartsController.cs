using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineShopAPI.Models;

namespace OnlineShopAPI.Controllers
{
    [ApiController]
    [Route("api/carts")]
    public class CartsController : ControllerBase
    {
        /// <summary>
        /// Get a list of all carts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllCarts()
        {
            List<Cart> CartList;
            CartList = Carts.getAllCarts();
            return Ok(CartList);
        }

        /// <summary>
        /// Get a cart by id
        /// </summary>
        /// <param name="Cart_id"></param>
        /// <returns></returns>
        [HttpGet("{Cart_id}")]
        public IActionResult GetCartById(ulong Cart_id)
        {
            Cart OneCart = Carts.getCartById(Cart_id);
            if (OneCart != null)
            {
                return Ok(OneCart);
            }
            else
            {
                return StatusCode(404, "El carrito no existe.");
            }
        }

        /// <summary>
        /// Create a new cart
        /// </summary>
        /// <param name="CreateCart"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateNewCart(Cart CreateCart)
        {
            string Errors;
            CreateCart.Creation_date = DateTime.Now;
            CreateCart.Purchased = false;
            Cart NewCart = Carts.createNewCart(CreateCart, out Errors);

            if (NewCart != null)
            {
                return Ok(NewCart);
            }
            else
            {
                return StatusCode(401, Errors);
            }
        }

        /// <summary>
        /// Delete a cart by id
        /// </summary>
        /// <param name="Cart_id"></param>
        /// <returns></returns>
        [HttpDelete("{Cart_id}")]
        public IActionResult DeleteCartById(ulong Cart_id)
        {
            String Errors;
            bool Success = Carts.deleteCartById(Cart_id, out Errors);
            if (Success)
            {
                return Ok("Carrito borrado. Id:" + Cart_id);
            }
            else
            {
                return StatusCode(404, Errors);
            }
        }

        /// <summary>
        /// Update the "purchased" status of a cart
        /// </summary>
        /// <param name="Cart_Id"></param>
        /// <param name="PatchCart"></param>
        /// <returns></returns>
        [HttpPatch("{Cart_id}")]
        public IActionResult PatchCartById(ulong Cart_Id, Cart PatchCart)
        {
            string Errors;
            Cart UpdatedCart = Carts.patchCartById(Cart_Id, PatchCart, out Errors);

            if (UpdatedCart != null)
            {
                return Ok(UpdatedCart);
            }
            else
            {
                return StatusCode(401, Errors);
            }
        }

    }
}
