using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface IMatchService
    {
        Task<TaskResult<Availability>> SetMatchForParticipateAsync(Guid participateId, Guid shiftId);
    }

    public class MatchService : IMatchService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        #endregion

        //Constructor
        public MatchService(IUnitOfWork unitOfWork, ILogger<MatchService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<TaskResult<Availability>> SetMatchForParticipateAsync(Guid participateId, Guid shiftId)
        {
            if(participateId == Guid.Empty)
            {
                throw new ArgumentNullException("participateId");
            }

            if (shiftId == Guid.Empty)
            {
                throw new ArgumentNullException("shiftId");
            }

            var taskResult = new TaskResult<Availability>();
            try
            {
                var match = new Availability
                {
                    ParticipationId = participateId,
                    ShiftId = shiftId,
                    Type = Models.Types.AvailibilityType.Ok
                };
                
                taskResult.Data = unitOfWork.MatchRepository.AddOrUpdate(match);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error,ex.ToString());
                taskResult.Error = ex;
            }
            return taskResult;
        }
    }
}
