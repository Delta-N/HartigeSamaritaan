using System;
using System.Threading.Tasks;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Data.Repositories;

namespace RoosterPlanner.Data.Common
{
    public interface IUnitOfWork
    {
        IProjectRepository ProjectRepository { get; }

        IParticipationRepository ParticipationRepository { get; }

        IPersonRepository PersonRepository { get; }

        ITaskRepository TaskRepository { get; }

        IShiftRepository ShiftRepository { get; }

        ICategoryRepository CategoryRepository { get; }
        
        IProjectTaskRepository ProjectTaskRepository { get; }
        IManagerRepository ManagerRepository { get; }
        IAvailabilityRepository AvailabilityRepository { get; }

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

        private IProjectRepository projectRepository;
        private IParticipationRepository participationRepository;
        private IPersonRepository personRepository;
        private ITaskRepository taskRepository;
        private ICategoryRepository categoryRepository;
        private IShiftRepository shiftRepository;
        private IProjectTaskRepository projectTaskRepository;
        private IManagerRepository managerRepository;
        private IAvailabilityRepository availabilityRepository;

        #endregion

        public IProjectRepository ProjectRepository => projectRepository ??= new ProjectRepository(DataContext);

        public IShiftRepository ShiftRepository => shiftRepository ??= new ShiftRepository(DataContext);
        
        public ITaskRepository TaskRepository => taskRepository ??= new TaskRepository(DataContext);

        public ICategoryRepository CategoryRepository => categoryRepository ??= new CategoryRepository(DataContext);

        public IParticipationRepository ParticipationRepository => participationRepository ??= new ParticipationRepository(DataContext);

        public IPersonRepository PersonRepository => personRepository ??= new PersonRepository(DataContext);

        public IProjectTaskRepository ProjectTaskRepository => projectTaskRepository ??= new ProjectTaskRepository(DataContext);

        public IManagerRepository ManagerRepository => managerRepository ??= new ManagerRepository(DataContext);
        public IAvailabilityRepository AvailabilityRepository => availabilityRepository ??= new AvailabilityRepository(DataContext);

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        public UnitOfWork(RoosterPlannerContext dataContext)
        {
            DataContext = dataContext;
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
            DataContext.Dispose();
        }

        #endregion
    }
}