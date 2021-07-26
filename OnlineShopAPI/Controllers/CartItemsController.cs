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
        [HttpGet]
        public IActionResult GetAllCartItems()
        {
            List<CartItem> CartItemList;
            CartItemList = CartItems.getAllCartItems();
            return Ok(CartItemList);
        }

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

    }
}
