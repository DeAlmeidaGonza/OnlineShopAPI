using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineShopAPI.Models;

namespace OnlineShopAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        [HttpGet("{Email}/{Password}")]
        public IActionResult UserLogin(string Email, string Password)
        {
            User OneUser = Users.userLogin(Email,Password);
            if (OneUser != null)
            {
                return Ok(OneUser);
            }
            else
            {
                return StatusCode(404, "El usuario no existe.");
            }
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            List<User> UserList;
            UserList = Users.getAllUsers();
            return Ok(UserList);
        }

        [HttpGet("{User_id}")]
        public IActionResult GetUserById(ulong User_id)
        {
            User OneUser = Users.getUserById(User_id);
            if (OneUser != null)
            {
                return Ok(OneUser);
            }
            else
            {
                return StatusCode(404, "El carrito no existe.");
            }
        }

        [HttpPost]
        public IActionResult UserRegistration(User CreateUser)
        {
            string Errors;
            User NewUser = Users.userRegistration(CreateUser, out Errors);

            if (NewUser != null)
            {
                return Ok(NewUser);
            }
            else
            {
                return StatusCode(401, Errors);
            }
        }

        [HttpDelete("{User_id}")]
        public IActionResult DeleteUserById(ulong User_id)
        {
            String Errors;
            bool Success = Users.deleteUserById(User_id, out Errors);
            if (Success)
            {
                return Ok("Usuario borrado. Id:" + User_id);
            }
            else
            {
                return StatusCode(404, Errors);
            }
        }


    }
}
