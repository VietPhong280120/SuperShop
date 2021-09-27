using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SuperShop.Data.Entities;
using SuperShop.ViewModels.Common;
using SuperShop.ViewModels.Systems.User;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Application.Systems.User
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserServices(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new ApiErrorResult<string>("Account doesn't Exist");
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<string>("Incorrect Login");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claim = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role,string.Join(";",roles))
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"],
                claim,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<ApiResult<bool>> DeleteUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return new ApiErrorResult<bool>("Delete is unsuccessful");
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<UserVm>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = new UserVm()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Dob = user.Dob,
                UserName = user.UserName,
                Role = roles
            };
            return new ApiSuccessResult<UserVm>(userVm);
        }

        public async Task<ApiResult<PageResult<UserVm>>> GetUserPaging(GetUserRequest request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                query = query.Where(x => x.UserName.Contains(request.KeyWord) || x.PhoneNumber.Contains(request.KeyWord)
                || x.FirstName.Contains(request.KeyWord) || x.LastName.Contains(request.KeyWord)
                || x.Email.Contains(request.KeyWord));
            }
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserVm()
                {
                    Id = x.Id,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName
                }).ToListAsync();
            var pageResult = new PageResult<UserVm>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PageResult<UserVm>>(pageResult);
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                return new ApiErrorResult<bool>("Account is Exist");
            }
            var email = await _userManager.FindByEmailAsync(request.Email);
            if (email != null)
            {
                return new ApiErrorResult<bool>("Email already Used");
            }
            user = new AppUser()
            {
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Dob = request.Dob,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<bool>("Register is unsuccessful");
            }
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> RoleAsign(Guid id, RoleAsignRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var removedRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();
            foreach (var roleName in removedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == true)
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }
            }
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            var addRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();
            foreach (var roleNames in addRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleNames) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleNames);
                }
            }

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> UpdateUser(Guid id, UpdateUserRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id))
            {
                return new ApiErrorResult<bool>("Email already Used");
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<bool>("Update is unsuccessful");
            }
            return new ApiSuccessResult<bool>();
        }
    }
}