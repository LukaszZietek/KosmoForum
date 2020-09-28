using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForum.Models;
using KosmoForum.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KosmoForum.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public UsersController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthorizationModel model)
        {
            var user = _userRepo.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest(new {message = "Username or password is incorrect"});
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
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
    }
}
