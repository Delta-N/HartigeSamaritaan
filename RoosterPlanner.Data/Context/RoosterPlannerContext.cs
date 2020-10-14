using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Context.Seed;
using RoosterPlanner.Models;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Context
{
    public class RoosterPlannerContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectTask> ProjectTasks { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Participation> Participations { get; set; }

        public DbSet<Availability> Matches { get; set; }

        //Constructor
        public RoosterPlannerContext(DbContextOptions<RoosterPlannerContext> options) : base(options)
        {
            base.ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Call base method first.
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectTask>()
                .HasKey(pt => new { pt.ProjectId, pt.TaskId });
            modelBuilder.Entity<ProjectTask>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(pt => pt.ProjectId);
            modelBuilder.Entity<ProjectTask>()
                .HasOne(pt => pt.Task)
                .WithMany(t => t.ProjectTasks)
                .HasForeignKey(pt => pt.TaskId);

            modelBuilder.Entity<Project>(pro => {
                pro.HasMany(pt => pt.ProjectTasks).WithOne(p => p.Project);
            });

            


            modelBuilder.Entity<Person>().HasIndex(p => p.Oid).IsUnique();

            modelBuilder.Entity<Task>(tsk => {
                tsk.HasMany(t => t.ProjectTasks).WithOne(t => t.Task);
            });

            modelBuilder.Entity<Participation>(par => {
                par.HasMany(m => m.Availabilities).WithOne(p => p.Participation);
            });

            modelBuilder.Entity<Shift>(tsk => {
                tsk.HasMany(m => m.Availabilities).WithOne(t => t.Shift);
            });

            var categorySeed = new CategorySeed(modelBuilder);
            var categorieList = categorySeed.Seed();

            var personSeed = new PersonSeed(modelBuilder);
            var personList = personSeed.Seed();
        }
    }
}
