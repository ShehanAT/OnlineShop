using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WebApplication1.Identity;

namespace Microsoft.WebApplication1.Models
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // this customizes the ASP.NET Identity model and overrides the defaults if needed
            // ex: you can rename the ASP.NET Identity table names and more 
            // Add your customizations after calling base.OnModelCreating(builder), which is this method
        }
    }
}
