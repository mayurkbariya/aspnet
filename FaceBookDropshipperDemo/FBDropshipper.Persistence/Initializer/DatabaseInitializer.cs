using System;
using System.Collections.Generic;
using System.Linq;
using FBDropshipper.Common.Constants;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Constant;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using Microsoft.AspNetCore.Identity;

namespace FBDropshipper.Persistence.Initializer
{
    public class DatabaseInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            DatabaseInitializer initializer = new DatabaseInitializer();
            initializer.SeedEverything(context);
        }

        private void SeedEverything(ApplicationDbContext context)
        {
            SeedRoles(context);
            SeedUsers(context);
            SeedCatalog(context);
            SeedCategories(context);
        }

        private void SeedCategories(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var current =
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory, "Seed", "FBCategories.csv");
                var items = File.ReadAllLines(current).Where(p => !string.IsNullOrWhiteSpace(p))
                    .ToArray();
                foreach (var item in items)
                {
                    context.Categories.Add(new Category()
                    {
                        Name = item,
                        CategoryType = CategoryType.Facebook.ToInt(),
                    });
                }
                context.SaveChanges();
            }
        }
        private void SeedCatalog(ApplicationDbContext context)
        {
            if (!context.Catalogs.Any())
            {
                context.Catalogs.Add(new Catalog()
                {
                    Name = "Amazon",
                    CatalogType = CatalogType.Integrated.ToInt(),
                    CanBeDeleted = false
                });
                context.SaveChanges();
            }
        }
        private void SeedRoles(ApplicationDbContext context)
        {
            var roles = RoleNames.AllRoles;
            foreach (var role in roles)
            {
                if (!context.Roles.Any(p => p.Name == role))
                {
                    context.Roles.Add(new Role()
                    {
                        Name = role,
                        NormalizedName = role.ToUpper()
                    });
                }
            }

            context.SaveChanges();
        }

        private void SeedUsers(ApplicationDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            string adminRoleId = context.Roles.First(p => p.Name == RoleNames.Admin).Id;
            var admin = new User()
            {
                PhoneNumber = AppConstant.AdminPhone,
                Email = AppConstant.AdminEmail,
                IsEnabled = true,
                NormalizedEmail = AppConstant.AdminEmail.ToUpper(),
                FullName = "Administrator",
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };
            admin.UserRoles = new List<UserRole>()
            {
                new UserRole()
                {
                    RoleId = adminRoleId,
                    UserId = admin.Id
                }
            };
            admin.PasswordHash = passwordHasher.HashPassword(admin, AppConstant.AdminPassword);
            context.Users.Add(admin);
            context.SaveChanges();
        }
    }
}