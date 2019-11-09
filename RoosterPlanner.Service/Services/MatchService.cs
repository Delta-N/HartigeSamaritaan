using System;
using System.Threading.Tasks;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface IMatchService
    {
        Task<TaskResult<Match>> SetMatchForParticipateAsync(Guid participateId, Guid shiftId);
    }

    public class MatchService : IMatchService
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork = null;
        private readonly ILogger logger = null;
        #endregion

        //Constructor
        public MatchService(IUnitOfWork unitOfWork, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<TaskResult<Match>> SetMatchForParticipateAsync(Guid participateId, Guid shiftId)
        {
            if(participateId == Guid.Empty)
            {
                throw new ArgumentNullException("participateId");
            }

            if (shiftId == Guid.Empty)
            {
                throw new ArgumentNullException("shiftId");
            }

            var taskResult = new TaskResult<Match>();
            try
            {
                var match = new Match
                {
                    ParticipationId = participateId,
                    ShiftId = shiftId,
                    Type = Models.Models.Types.MatchType.Ok
                };
                
                taskResult.Data = unitOfWork.MatchRepository.AddOrUpdate(match);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Fout bij het updaten van match.");
                taskResult.Error = ex;
            }
            return taskResult;
        }
    }
}
