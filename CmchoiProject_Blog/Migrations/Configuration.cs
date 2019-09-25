namespace CmchoiProject_Blog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CmchoiProject_Blog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CmchoiProject_Blog.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }

            //I need to create a few users that will eventuall occupy the roles of either Admin or moderator
            var userManager = new UserManager<CmchoiProject_Blog.Models.ApplicationUser>(
                new UserStore<CmchoiProject_Blog.Models.ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "cmchoi.coder@gmail.com"))
            {
                userManager.Create(new CmchoiProject_Blog.Models.ApplicationUser
                {
                    UserName = "cmchoi.coder@gmail.com",
                    Email = "cmchoi.coder@gmail.com",
                    FirstName = "Caleb",
                    LastName = "Choi"
                }, "sda2380!");


            }

            if (!context.Users.Any(u => u.Email == "moderator@coderfoundry.com"))
            {
                userManager.Create(new CmchoiProject_Blog.Models.ApplicationUser
                {
                    UserName = "moderator@coderfoundry.com",
                    Email = "moderator@coderfoundry.com",
                    FirstName = "coder",
                    LastName = "foundry"
                }, "Password-1");

            }

            var userId = userManager.FindByEmail("cmchoi.coder@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            userId = userManager.FindByEmail("moderator@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Moderator");
        }
    }
}
