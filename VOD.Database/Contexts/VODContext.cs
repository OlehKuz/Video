﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VOD.Common.Entities;

namespace VOD.Database.Contexts
{
    public class VODContext:IdentityDbContext<VODUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Download> Downloads { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<Video> Videos { get; set; }

        public VODContext(DbContextOptions<VODContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserCourse>().HasKey(uc=>new { uc.UserId, uc.CourseId});

            foreach (var relationship in builder.Model.GetEntityTypes()
                .SelectMany(e=>e.GetForeignKeys())) 
            {
                //Console.WriteLine(relationship + " keys " );
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

       
    }
}
