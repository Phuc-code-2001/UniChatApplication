using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniChatApplication.Daos;
using UniChatApplication.Models;

namespace UniChatApplication.Data
{
    public class SeedData
    {
        /// <summary>
        /// InitialAdminAccount
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void InitialAdminAccount(IServiceProvider serviceProvider)
        {
            UniChatDbContext context =
                new UniChatDbContext(serviceProvider
                        .GetRequiredService<DbContextOptions<UniChatDbContext>>(
                        ));

            if (context.AdminProfile.Any()) return;

            List<AdminProfile> admins =
                new List<AdminProfile>()
                {
                    new AdminProfile()
                    {
                        FullName = "Admin",
                        Email = "admin@fpt.edu.vn",
                        Phone = "0987499512",
                        Gender = true,
                        Account = AccountDAOs.CreateAccount("admin", "12345678", 3)
                    }
                };

            context.AddRange(admins);
            context.SaveChanges();
        }

        /// <summary>
        /// ResetDataServer
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void ResetDataServer(IServiceProvider serviceProvider)
        {

            UniChatDbContext context =
                new UniChatDbContext(serviceProvider
                        .GetRequiredService<DbContextOptions<UniChatDbContext>>(
                        ));

            // Reset Login Cookie
            List<LoginCookie> cookies = new List<LoginCookie>();
            foreach (LoginCookie item in context.LoginCookies)
            {
                if (item.ExpirationTime <= DateTime.Now)
                {
                    cookies.Add(item);
                }
            }
            context.LoginCookies.RemoveRange(cookies);
            context.SaveChanges();
        }

    }
}
