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

        ITaskRepository TaskRepository { get; }

        ICategoryRepository CategoryRepository { get; }

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
        private ITaskRepository taskRepository;
        private ICategoryRepository categoryRepository;
        #endregion

        public IProjectRepository ProjectRepository
        {
            get
            {
                if (projectRepository == null)
                    this.projectRepository = new ProjectRepository(this.DataContext, null);
                return this.projectRepository;
            }
        }

        public ITaskRepository TaskRepository
        {
            get
            {
                if (taskRepository == null)
                    this.taskRepository = new TaskRepository(this.DataContext, null);
                return this.taskRepository;
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (categoryRepository == null)
                    this.categoryRepository = new CategoryRepository(this.DataContext, null);
                return this.categoryRepository;
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
