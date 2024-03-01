using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Context.Seed;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Context
{
    public class RoosterPlannerContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<CertificateType> CertificateTypes { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Document> Documents { get; set; }

        //Constructor
        public RoosterPlannerContext(DbContextOptions<RoosterPlannerContext> options) : base(options)
        {
            base.ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Call base method first.
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectTask>(p =>
            {
                p.HasOne(pt => pt.Project)
                    .WithMany(pt => pt.ProjectTasks)
                    .HasForeignKey(pt => pt.ProjectId);

                p.HasOne(pt => pt.Task)
                    .WithMany(t => t.ProjectTasks)
                    .HasForeignKey(pt => pt.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Project>(pro =>
            {
                pro.HasMany(pt => pt.ProjectTasks)
                    .WithOne(p => p.Project);
                pro.HasMany(p => p.Participations)
                    .WithOne(x => x.Project);
            });

            modelBuilder.Entity<Task>(tsk =>
            {
                tsk
                    .HasMany(t => t.ProjectTasks)
                    .WithOne(t => t.Task);

                tsk.HasMany(s => s.Shifts)
                    .WithOne(t => t.Task)
                    .OnDelete(DeleteBehavior.SetNull);
                tsk
                    .HasMany(t => t.Requirements)
                    .WithOne(t => t.Task);
            });

            modelBuilder.Entity<Document>(doc =>
            {
                doc.HasMany(d => d.Instructions)
                    .WithOne(i => i.Instruction)
                    .OnDelete(DeleteBehavior.SetNull);
                doc.HasMany(d => d.ProfilePictures)
                    .WithOne(i => i.ProfilePicture)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Category>(cat =>
            {
                cat.HasMany(c => c.Tasks)
                    .WithOne(t => t.Category)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Person>(p =>
            {
                p.HasIndex(per => per.Oid)
                    .IsUnique();
                p.HasMany(cer => cer.Certificates)
                    .WithOne(x => x.Person);
                p.HasMany(par => par.Participations)
                    .WithOne(x => x.Person);
            });

            modelBuilder.Entity<Availability>(ava =>
            {
                ava.HasOne(p => p.Participation)
                    .WithMany(a => a.Availabilities)
                    .HasForeignKey(a => a.ParticipationId)
                    .OnDelete(DeleteBehavior.Restrict);
                ava.HasOne(s => s.Shift)
                    .WithMany(a => a.Availabilities)
                    .HasForeignKey(a => a.ShiftId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Participation>(par =>
            {
                par.HasMany(m => m.Availabilities)
                    .WithOne(p => p.Participation);
                par.HasOne(p => p.Person)
                    .WithMany(ppar => ppar.Participations)
                    .HasForeignKey(ppar => ppar.PersonId);
                par.HasOne(p => p.Project)
                    .WithMany(ppar => ppar.Participations)
                    .HasForeignKey(ppar => ppar.ProjectId);
            });

            modelBuilder.Entity<Shift>(shf =>
            {
                shf.HasMany(m => m.Availabilities)
                    .WithOne(t => t.Shift);
            });

            modelBuilder.Entity<Certificate>(cer =>
            {
                cer.HasOne(p => p.Person)
                    .WithMany(c => c.Certificates)
                    .HasForeignKey(c => c.PersonId);
                cer.HasOne(ct => ct.CertificateType)
                    .WithMany(c => c.Certificates)
                    .HasForeignKey(c => c.CertificateTypeId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<CertificateType>(ct =>
            {
                ct.HasMany(c => c.Certificates)
                    .WithOne(x => x.CertificateType);
                ct.HasMany(c => c.Requirements)
                    .WithOne(req => req.CertificateType);
            });

            modelBuilder.Entity<Requirement>(req =>
            {
                req.HasOne(t => t.Task)
                    .WithMany(reqt => reqt.Requirements)
                    .HasForeignKey(reqt => reqt.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);
                req.HasOne(cer => cer.CertificateType)
                    .WithMany(reqt => reqt.Requirements)
                    .HasForeignKey(reqt => reqt.CertificateTypeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            var categorySeed = new CategorySeed(modelBuilder);
            categorySeed.Seed();

            var personSeed = new PersonSeed(modelBuilder);
            personSeed.Seed();

            var projectseed = new ProjectSeed(modelBuilder);
            projectseed.Seed();

            var participationSeed = new ParticipationSeed(modelBuilder);
            participationSeed.Seed();

            var taskseed = new TaskSeed(modelBuilder);
            taskseed.Seed();

            var documentSeed = new DocumentSeed(modelBuilder);
            documentSeed.Seed();

            var certificateTypeSeed = new CertificateTypeSeed(modelBuilder);
            certificateTypeSeed.Seed();
        }
    }
}