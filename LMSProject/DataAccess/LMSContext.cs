﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using LMSProject.Models;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace LMSProject.DataAccess
{
	public class LMSContext : DbContext
	{
		public DbSet<user_student> user_students { get; set; }
		public DbSet<schoolClass> schoolClasses { get; set; }
		public DbSet<user_teacher> user_teachers { get; set; }
		public DbSet<task> tasks { get; set; }
		public DbSet<folder> folders { get; set; }
		public DbSet<folderType> folderTypes { get; set; }
		public DbSet<schedule> schedules { get; set; }
		public DbSet<scheduleDetail> scheduleDetails { get; set; }
		public DbSet<file> files { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
		}

		public LMSContext()
            : base("LMSprojectDb")
        {

        }
	}
}