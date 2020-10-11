using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForum.Models;
using KosmoForum.Models.Dtos;
using KosmoForum.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KosmoForum.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/users")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public UsersController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }


        /// <summary>
        /// Return user's id who has appropriate username
        /// </summary>
        /// <param name="username"> User's name</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(int))]
        public IActionResult GetUserId([FromBody] string username)
        {
            var id = _userRepo.GetUserIdUsingName(username);
            return Ok(id);

        }
        /// <summary>
        /// Get avatar of user who are already authorize
        /// </summary>
        /// <returns></returns>

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetUserAvatar()
        {
            int userId = 0;
            if (User.Identity.Name == "0")
            {
                ModelState.AddModelError("","User isn't authorize so avatar is null");
                return BadRequest(ModelState);
            }
            try
            {
                userId = Int32.Parse(User.Identity.Name);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("","Error occurred during parsing userId");
                return StatusCode(500, ModelState);
            }

            byte[] userAvatar = _userRepo.GetUserAvatar(userId);
            if (userAvatar == null)
            {
                return NotFound();
            }

            return Ok(userAvatar);

        }

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="model">Model for authorization (username,password)</param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        [AllowAnonymous]
        [ProducesResponseType(200,Type = typeof(AuthorizationModel))]
        public IActionResult Authenticate([FromBody] AuthorizationModel model)
        {
            var user = _userRepo.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest(new {message = "Username or password is incorrect"});
            }

            return Ok(user);
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="model">User model</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesDefaultResponseType]
        public IActionResult Register([FromBody] UserCreateDto model)
        {
            bool isUserNameUnique = _userRepo.IsUniqueUser(model.Username);
            if (!isUserNameUnique)
            {
                return BadRequest(new {message = "Username already exists!"});
            }

            var user = _userRepo.Register(model.Username, model.Password,model.Email,model.Avatar);

            if (user == null)
            {
                return BadRequest(new {message = "Error occured during register!"});
            }

            return Ok();
        }

        /// <summary>
        /// Update user's avatar
        /// </summary>
        /// <param name="avatar">byte table which contain new avatar</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(500)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public IActionResult ChangeAvatar([FromBody] byte[] avatar)
        {
            if (User.Identity.Name == "0")
            {
                ModelState.AddModelError("","You should authorize earlier");
                return BadRequest(ModelState);
            }

            int userId = 0;
            userId = Int32.Parse(User.Identity.Name);


            if (avatar == null)
            {
                ModelState.AddModelError("","Avatar table is empty, try again");
                return BadRequest(ModelState);
            }

            var userObj = _userRepo.GetUser(userId);
            if (userObj == null)
            {
                ModelState.AddModelError("", "Something is wrong with your userId");
                return BadRequest(ModelState);
            }

            userObj.Avatar = avatar;

            if (!_userRepo.UpdateUser(userObj))
            {
                ModelState.AddModelError("","Error occured in database during updating avatar");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        /// <summary>
        /// Change the password for current user
        /// </summary>
        /// <param name="passwords">Old and new password</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(500)]
        [ProducesDefaultResponseType]
        public IActionResult ChangePassword([FromBody] UserUpdatePasswordDto passwords)
        {
            if (User.Identity.Name == "0")
            {
                ModelState.AddModelError("", "You should authorize earlier");
                return BadRequest(ModelState);
            }

            if (passwords.NewPassword == null || passwords.OldPassword == null)
            {
                ModelState.AddModelError("","One from two password is empty");
                return BadRequest(ModelState);
            }

            int userId = Int32.Parse(User.Identity.Name);
            var userObj = _userRepo.GetUser(userId);

            if (!PasswordHasher.Verify(passwords.OldPassword, userObj.Password))
            {
                ModelState.AddModelError("","Old password is incorrect");
                return BadRequest(ModelState);
            }

            userObj.Password = PasswordHasher.Hash(passwords.NewPassword);


            if (!_userRepo.UpdateUser(userObj))
            {
                ModelState.AddModelError("", "Error occured in database during updating password");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
