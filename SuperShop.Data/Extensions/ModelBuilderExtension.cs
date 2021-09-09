using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Data.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void seed(this ModelBuilder builder)
        {
            builder.Entity<AppConfig>().HasData(
                new AppConfig()
                {
                    Key = "HomeTitle",
                    Value = "This is home page SuperShop"
                },
                new AppConfig()
                {
                    Key = "KeyWord",
                    Value = "This is key word SuperShop"
                },
                new AppConfig()
                {
                    Key = "HomeDescription",
                    Value = "This is description SuperShop"
                }
                );
            builder.Entity<Language>().HasData(
                new Language()
                {
                    Id = "vi",
                    Name = "Tiếng Việt",
                    IsDefault = true
                },
                new Language()
                {
                    Id = "en",
                    Name = "English",
                    IsDefault = true
                }
                );
            var roleId = new Guid("400D7803-C8D3-4D9E-AC74-249A62370EA1");
            var adminId = new Guid("6C31BC35-90C6-443B-8A6B-2576BA12F19D");
            var userId = new Guid("ACDE886F-8525-4EE2-BC8B-803B46ADE679");
            builder.Entity<AppRole>().HasData(
                new AppRole()
                {
                    Id = roleId,
                    Name = "admin",
                    NormalizedName = "admin",
                    Description = "Administrator"
                },
                 new AppRole()
                 {
                     Id = userId,
                     Name = "user",
                     NormalizedName = "user",
                     Description = "User"
                 }
                );
            var hasher = new PasswordHasher<AppUser>();
            builder.Entity<AppUser>().HasData(
                new AppUser()
                {
                    Id = adminId,
                    UserName = "admin",
                    NormalizedUserName = "admin",
                    Email = "Vietphong2801@gmail.com",
                    NormalizedEmail = "Vietphong2801@gmail.com",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Phong123@"),
                    SecurityStamp = string.Empty,
                    FirstName = "Phong",
                    LastName = "Nguyen",
                    Dob = new DateTime(2000, 01, 28)
                }
                );
            builder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>()
                {
                    RoleId = roleId,
                    UserId = adminId
                }
                );
        }
    }
}