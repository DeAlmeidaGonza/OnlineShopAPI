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
        /// <summary>
        /// User login
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get a list of all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            List<User> UserList;
            UserList = Users.getAllUsers();
            return Ok(UserList);
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <param name="User_id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="CreateUser"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete a user by id
        /// </summary>
        /// <param name="User_id"></param>
        /// <returns></returns>
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
