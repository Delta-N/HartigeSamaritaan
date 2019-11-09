using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Context.Seed;
using RoosterPlanner.Models;

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

        public DbSet<Match> Matches { get; set; }

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
                .HasOne<Project>(pt => pt.Project)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(pt => pt.ProjectId);
            modelBuilder.Entity<ProjectTask>()
                .HasOne<Task>(pt => pt.Task)
                .WithMany(t => t.TaskProjects)
                .HasForeignKey(pt => pt.TaskId);

            modelBuilder.Entity<Project>(pro => {
                pro.HasMany<ProjectTask>(pt => pt.ProjectTasks).WithOne(p => p.Project);
            });

            modelBuilder.Entity<Task>(tsk => {
                tsk.HasMany<ProjectTask>(t => t.TaskProjects).WithOne(t => t.Task);
            });

            CategorySeed categorySeed = new CategorySeed(modelBuilder);
            PersonSeed personSeed = new PersonSeed(modelBuilder);
            List<Category> categorieList = categorySeed.Seed();
            List<Person> personList = personSeed.Seed();


        }
    }
}
