﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineShopAPI.Models;

namespace OnlineShopAPI.Controllers
{
    [ApiController]
    [Route("api/carts")]
    public class CartsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllCarts()
        {
            List<Cart> CartList;
            CartList = Carts.getAllCarts();
            return Ok(CartList);
        }

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

        [HttpPatch("{Cart_id}")]
        public IActionResult PatchCartById(ulong Cart_Id, Cart PatchCart)
        {
            string Errors;
            Cart UpdatedCart = Carts.patchCartbyId(Cart_Id, PatchCart, out Errors);

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
