﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using IdentityServer.Legacy;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;

//namespace IdentityServer.Data
//{
//    public class IdentityServerContext : IdentityDbContext<ApplicationUser>
//    {
//        public IdentityServerContext(DbContextOptions<IdentityServerContext> options)
//            : base(options)
//        {
//        }

//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);
//            // Customize the ASP.NET Identity model and override the defaults if needed.
//            // For example, you can rename the ASP.NET Identity table names and more.
//            // Add your customizations after calling base.OnModelCreating(builder);
//        }
//    }
//}
