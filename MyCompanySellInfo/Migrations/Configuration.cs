namespace MyCompanySellInfo.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MyCompanySellInfo.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MyCompanySellInfo.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MyCompanySellInfo.Models.ApplicationDbContext";
        }

        protected override void Seed(MyCompanySellInfo.Models.ApplicationDbContext context)
        {
            AddRoleAndUsers(context, "admin", "admin1234", "canEdit");
            AddRoleAndUsers(context, "superadmin", "superadmin1234", "canDelete");
            AddRole(context, "canView");

            //  This method will be called after migrating to the latest version.
            
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }

        private bool AddRole(ApplicationDbContext context, string p)
        {
            IdentityResult ir;
            var rm = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
            ir = rm.Create(new IdentityRole(p));
            return ir.Succeeded;
        }


        private bool AddRoleAndUsers(Models.ApplicationDbContext context, string login, string password, string role)
        {
            IdentityResult ir;
            var rm = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
            ir = rm.Create(new IdentityRole(role));
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser()
            {
                UserName = login,
            };
            ir = um.Create(user, password);
            if (ir.Succeeded == false)
                return ir.Succeeded;
            ir = um.AddToRole(user.Id, role);
            return ir.Succeeded;
        }
    }
}
