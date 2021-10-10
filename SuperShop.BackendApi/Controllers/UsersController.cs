using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Application.Systems.Roles;
using SuperShop.Application.Systems.User;
using SuperShop.ViewModels.Systems.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IRoleServices _roleServices;

        public UsersController(IUserServices userServices, IRoleServices roleServices)
        {
            _userServices = userServices;
            _roleServices = roleServices;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userServices.Authenticate(request);
            if (string.IsNullOrEmpty(result.ResultObj))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userServices.Register(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userServices.UpdateUser(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userServices.GetById(id);
            return Ok(user);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetUserPaging([FromQuery] GetUserRequest request)
        {
            var user = await _userServices.GetUserPaging(request);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userServices.DeleteUser(id);
            return Ok(result);
        }

        [HttpPut("{id}/roles")]
        public async Task<IActionResult> RoleAsign(Guid id, RoleAsignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userServices.RoleAsign(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleServices.GetAll();
            return Ok(roles);
        }
    }
}