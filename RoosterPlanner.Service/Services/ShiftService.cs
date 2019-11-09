using System;
using System.Threading.Tasks;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface IShiftService
    {
        Task<TaskListResult<Models.Shift>> GetActiveShiftsForProjectAsync(Guid projectId);
    }

    public class ShiftService : IShiftService
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork = null;
        private readonly ILogger logger = null;
        #endregion

        //Constructor
        public ShiftService(IUnitOfWork unitOfWork, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<TaskListResult<Models.Shift>> GetActiveShiftsForProjectAsync(Guid projectId)
        {
            TaskListResult<Models.Shift> taskResult = TaskListResult<Models.Shift>.CreateDefault();

            try
            {
                taskResult.Data = await unitOfWork.ShiftRepository.GetActiveShiftsForProjectAsync(projectId);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Fout bij het ophalen van actieve shifts.");
                taskResult.Error = ex;
            }
            return taskResult;
        }
    }
}
