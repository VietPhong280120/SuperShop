using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SuperShop.ViewModels.Common;
using SuperShop.ViewModels.Systems.Roles;
using SuperShop.ViewModels.Systems.User;
using SuperShop.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Web.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserApiUser _userApiClient;
        private readonly IConfiguration _configuration;

        public UserController(IUserApiUser userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 1)
        {
            var request = new GetUserRequest()
            {
                KeyWord = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            var data = await _userApiClient.GetUserPaging(request);
            return View(data.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Tokens");
            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.RegisterUser(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Create is successful";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            return View(new UserDeleteRequest() { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.DeleteUser(request.Id);
            if (result)
            {
                TempData["result"] = "Delete is successful ! ";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Delete is unsuccessful !");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            var user = result.ResultObj;
            var updateUser = new UpdateUserRequest()
            {
                Id = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Dob = user.Dob,
                PhoneNumber = user.PhoneNumber
            };
            return View(updateUser);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.UpdateUser(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Update is successful ! ";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            return View(result.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssign = await GetRoleAssignRequest(id);
            return View(roleAssign);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAsignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.RoleAssign(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Add roles is successful!";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            var role = await GetRoleAssignRequest(request.Id);
            return View(role);
        }

        private async Task<RoleAsignRequest> GetRoleAssignRequest(Guid id)
        {
            var userObj = await _userApiClient.GetById(id);
            var roleObj = await _userApiClient.GetRole();
            var roleRequest = new RoleAsignRequest();

            foreach (RoleVm role in roleObj.ResultObj)
            {
                roleRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = userObj.ResultObj.Role.Contains(role.Name)
                });
            }
            return roleRequest;
        }
    }
}