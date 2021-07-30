using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineShopAPI.Models;

namespace OnlineShopAPI.Controllers
{
    [ApiController]
    [Route("api/cartitems")]
    public class CartItemsController : ControllerBase
    {
        /// <summary>
        /// Get a list of all cart items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllCartItems()
        {
            List<CartItem> CartItemList;
            CartItemList = CartItems.getAllCartItems();
            return Ok(CartItemList);
        }

        /// <summary>
        /// Get a cart item by id
        /// </summary>
        /// <param name="Cart_item_id"></param>
        /// <returns></returns>
        [HttpGet("{Cart_item_id}")]
        public IActionResult GetCartItemById(ulong Cart_item_id)
        {
            CartItem OneCartItem = CartItems.getCartItemById(Cart_item_id);
            if (OneCartItem != null)
            {
                return Ok(OneCartItem);
            }
            else
            {
                return StatusCode(404, "El item no existe.");
            }
        }

        /// <summary>
        /// Create a new cart item
        /// </summary>
        /// <param name="CreateCartItem"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateNewCartItem(CartItem CreateCartItem)
        {
            string Errors;
            CartItem NewCartItem = CartItems.createNewCartItem(CreateCartItem, out Errors);

            if (NewCartItem != null)
            {
                return Ok(NewCartItem);
            }
            else
            {
                return StatusCode(401, Errors);
            }
        }

        /// <summary>
        /// Delete a cart item by id
        /// </summary>
        /// <param name="Cart_item_id"></param>
        /// <returns></returns>
        [HttpDelete("{Cart_item_id}")]
        public IActionResult DeleteCartItemById(ulong Cart_item_id)
        {
            String Errors;
            bool Success = CartItems.deleteCartItemById(Cart_item_id, out Errors);
            if (Success)
            {
                return Ok("Item borrado. Id:" + Cart_item_id);
            }
            else
            {
                return StatusCode(404, Errors);
            }
        }

        /// <summary>
        /// Get a list of all cart items from a cart
        /// </summary>
        /// <param name="Cart_id"></param>
        /// <returns></returns>
        [HttpGet("getByCartId/{Cart_id}")]
        public IActionResult GetCartItemsByCartId(string Cart_id)
        {
            List<CartItem> CartItemList;
            CartItemList = CartItems.getCartItemsByCartId(Cart_id);
            if (CartItemList != null)
            {
                return Ok(CartItemList);
            }
            else
            {
                return StatusCode(404, "El carrito no contiene articulos.");
            }
        }

    }
}
