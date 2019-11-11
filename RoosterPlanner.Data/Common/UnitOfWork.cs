using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Data.Repositories;

namespace RoosterPlanner.Data.Common
{
    public interface IUnitOfWork
    {
        IProjectRepository ProjectRepository { get; }

        IParticipationRepository ParticipationRepository { get; }

        IPersonRepository PersonRepository { get; }

        IProjectPersonRepository ProjectPersonRepository { get; }

        ITaskRepository TaskRepository { get; }

        IShiftRepository ShiftRepository { get; }

        ICategoryRepository CategoryRepository { get; }

        IMatchRepository MatchRepository { get; }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Saves the changes.
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Dispose
        /// </summary>
        void Dispose();
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region Fields
        /// <summary>
        /// Gets the data context.
        /// </summary>
        protected RoosterPlannerContext DataContext { get; private set; }
        protected ILogger Logger { get; private set; }

        private IProjectRepository projectRepository;
        private IParticipationRepository participationRepository;
        private IProjectPersonRepository projectPersonRepository;
        private IPersonRepository personRepository;
        private ITaskRepository taskRepository;
        private ICategoryRepository categoryRepository;

        private IMatchRepository matchRepository;

        private IShiftRepository shiftRepository;
        #endregion

        public IProjectRepository ProjectRepository
        {
            get
            {
                if (projectRepository == null)
                    this.projectRepository = new ProjectRepository(this.DataContext, this.Logger);
                return this.projectRepository;
            }
        }


        public IShiftRepository ShiftRepository
        {
            get
            {
                if (shiftRepository == null)
                    this.shiftRepository = new ShiftRepository(this.DataContext, null);
                return this.shiftRepository;
            }
        }

        public IMatchRepository MatchRepository
        {
            get
            {
                if (matchRepository == null)
                    this.matchRepository = new MatchRepository(this.DataContext, null);
                return this.matchRepository;
            }
        }


        public IProjectPersonRepository ProjectPersonRepository
        {
            get
            {
                if (projectPersonRepository == null)
                    projectPersonRepository = new ProjectPersonRepository(this.DataContext, this.Logger);
                return projectPersonRepository;
            }
        }

        public ITaskRepository TaskRepository
        {
            get
            {
                if (taskRepository == null)
                    this.taskRepository = new TaskRepository(this.DataContext, this.Logger);
                return this.taskRepository;
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (categoryRepository == null)
                    this.categoryRepository = new CategoryRepository(this.DataContext, this.Logger);
                return this.categoryRepository;
            }
        }

        public IParticipationRepository ParticipationRepository
        {
            get
            {
                if (participationRepository == null)
                    this.participationRepository = new ParticipationRepository(this.DataContext, this.Logger);
                return this.participationRepository;
            }
        }

        public IPersonRepository PersonRepository
        {
            get
            {
                if (personRepository == null)
                    this.personRepository = new PersonRepository(this.DataContext, this.Logger);
                return this.personRepository;
            }
        }

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        public UnitOfWork(RoosterPlannerContext dataContext, ILogger logger)
        {
            this.DataContext = dataContext;
            this.Logger = logger;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Saves the changes.
        /// </summary>
        public int SaveChanges()
        {
            return DataContext.SaveChanges();
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public Task<int> SaveChangesAsync()
        {
            return DataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.DataContext.Dispose();
        }
        #endregion
    }
}
