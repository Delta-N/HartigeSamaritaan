using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Context.Seed;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Context
{
    public class RoosterPlannerContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<CertificateType> CertificateTypes { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Collaboration> Collaborations { get; set; }


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
                .HasKey(pt => new {pt.ProjectId, pt.TaskId});
            modelBuilder.Entity<ProjectTask>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(pt => pt.ProjectId);
            modelBuilder.Entity<ProjectTask>()
                .HasOne(pt => pt.Task)
                .WithMany(t => t.ProjectTasks)
                .HasForeignKey(pt => pt.TaskId);

            modelBuilder.Entity<Project>(pro =>
            {
                pro.HasMany(pt => pt.ProjectTasks)
                    .WithOne(p => p.Project);
            });
            
            modelBuilder.Entity<Task>(tsk =>
            {
                tsk.HasMany(t => t.ProjectTasks)
                    .WithOne(t => t.Task);
            });

            
            modelBuilder.Entity<Requirement>()
                .HasOne(t => t.Task)
                .WithMany(req => req.Requirements)
                .HasForeignKey(req => req.TaskId);
            modelBuilder.Entity<Requirement>()
                .HasOne(cer => cer.CertificateType)
                .WithMany(req => req.Requirements)
                .HasForeignKey(req => req.CertificateTypeId);
            
            modelBuilder.Entity<Task>(tsk =>
            {
                tsk.HasMany(t => t.Requirements)
                    .WithOne(t => t.Task);
            });
            
            modelBuilder.Entity<CertificateType>(ct =>
            {
                ct.HasMany(c => c.Requirements)
                    .WithOne(req => req.CertificateType);
            });


            modelBuilder.Entity<Person>()
                .HasIndex(p => p.Oid)
                .IsUnique();


            modelBuilder.Entity<Availability>()
                .HasOne(p => p.Participation)
                .WithMany(a => a.Availabilities)
                .HasForeignKey(a => a.ParticipationId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Availability>()
                .HasOne(s => s.Shift)
                .WithMany(a => a.Availabilities)
                .HasForeignKey(a => a.ShiftId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Participation>(par =>
            {
                par.HasMany(m => m.Availabilities)
                    .WithOne(p => p.Participation);
            });

            modelBuilder.Entity<Shift>(tsk =>
            {
                tsk.HasMany(m => m.Availabilities)
                    .WithOne(t => t.Shift);
            });
            
            modelBuilder.Entity<Certificate>()
                .HasOne(p => p.Person)
                .WithMany(c => c.Certificates)
                .HasForeignKey(c => c.PersonId);
            modelBuilder.Entity<Certificate>()
                .HasOne(ct => ct.CertificateType)
                .WithMany(c=>c.Certificates)
                .HasForeignKey(c=>c.CertificateTypeId);
            
            modelBuilder.Entity<Person>(per =>
            {
                per.HasMany(cer=>cer.Certificates)
                    .WithOne(p=>p.Person);
            });
            modelBuilder.Entity<CertificateType>(ct =>
            {
                ct.HasMany(c => c.Certificates)
                    .WithOne(x => x.CertificateType);
            });

            modelBuilder.Entity<Participation>()
                .HasOne(p => p.Person)
                .WithMany(par => par.Participations)
                .HasForeignKey(par => par.PersonId);
            modelBuilder.Entity<Participation>()
                .HasOne(p => p.Project)
                .WithMany(par => par.Participations)
                .HasForeignKey(par => par.ProjectId);

            modelBuilder.Entity<Person>(per =>
            {
                per.HasMany(p => p.Participations)
                    .WithOne(x => x.Person);
            });
            modelBuilder.Entity<Project>(pr =>
            {
                pr.HasMany(p => p.Participations)
                    .WithOne(x => x.Project);
            });


            modelBuilder.Entity<Collaboration>()
                .HasOne(p => p.WantsToWorkWith)
                .WithMany(c => c.WantsToWorkWith)
                .HasForeignKey(p => p.WantsToWorkWithId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Collaboration>()
                .HasOne(p => p.IsWantedBy)
                .WithMany(c => c.IsWantedBy)
                .HasForeignKey(p => p.IsWantedById)
                .OnDelete(DeleteBehavior.Restrict);
            
                

            

            var categorySeed = new CategorySeed(modelBuilder);
            var categorieList = categorySeed.Seed();

            var personSeed = new PersonSeed(modelBuilder);
            var personList = personSeed.Seed();
            
            var projectseed = new ProjectSeed(modelBuilder);
            var projectList = projectseed.Seed();
        }
    }
}