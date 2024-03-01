using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Models.Models;
using RoosterPlanner.Models.Models.Types;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public interface IShiftRepository : IRepository<Shift>
    {   /// <summary>
        /// Add multiple shifts to the database
        /// </summary>
        /// <param name="shifts"></param>
        /// <returns>A task of a list of shifts added to the database</returns>
        Task<List<Shift>> AddShiftsAsync(List<Shift> shifts);
        /// <summary>
        /// Get a shift based on shiftId
        /// </summary>
        /// <param name="shiftId"></param>
        /// <returns>A task of a shift </returns>
        Task<Shift> GetShiftAsync(Guid shiftId);
        /// <summary>
        /// Get a shift based on a shiftId including Availabilities
        /// </summary>
        /// <param name="shiftId"></param>
        /// <returns>A task of a shift including availabilities</returns>
        Task<Shift> GetShiftWithAvailabilitiesAsync(Guid shiftId);
        /// <summary>
        /// Get shifts based on a projectId and a Date including Availabilities
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="date"></param>
        /// <returns>A task of a list of shifts including availabilities</returns>
        Task<List<Shift>> GetByProjectAndDateWithAvailabilitiesAsync(Guid projectId, DateTime date);
        /// <summary>
        /// Get shifts based on a projectId, userId and a Date including Availabilities
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns>A task of a list of shifts including availabilities</returns>
        Task<List<Shift>> GetByProjectUserAndDateAsync(Guid projectId, Guid userId, DateTime date);

        /// <summary>
        /// Get shifts based on a projectId including Availabilities
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>A task of a list of shifts including availabilities</returns>
        Task<List<Shift>> GetByProjectWithAvailabilitiesAsync(Guid projectId);

        /// <summary>
        /// Get shifts based on a projectId and a userId including Availabilities
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns>A task of a list of shifts including availabilities</returns>
        Task<List<Shift>> GetByProjectWithAvailabilitiesAsync(Guid projectId, Guid userId);

        /// <summary>
        /// Search for shifts based on a filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A task of a list of shifts</returns>
        Task<List<Shift>> SearchProjectsAsync(ShiftFilter filter);
        /// <summary>
        /// Get a DTO with distinct Tasks, Dates, Starttimes, Endtimes and number of participantsrequired, based on a projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>DTO with distinct attributes</returns>
        Task<ShiftData> GetUniqueDataAsync(Guid projectId);
        /// <summary>
        /// Exports data nessesary for statistical analysis
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>a task of a list of shifts with relating data</returns>
        Task<List<Shift>> ExportDataAsync(Guid projectId);
    }

    public class ShiftRepository : Repository<Shift>, IShiftRepository
    {
        //Constructor
        public ShiftRepository(DbContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Get shifts based on a projectId including Availabilities
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>A task of a list of shifts including availabilities</returns>
        public async Task<List<Shift>> GetByProjectWithAvailabilitiesAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return await Task.FromResult<List<Shift>>(null);
            List<Shift> shifts = await EntitySet
                .AsNoTracking()
                .Include(s => s.Project)
                .Include(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Include(s => s.Availabilities)
                .Where(s => s.ProjectId == projectId && s.Date >= DateTime.Today)
                .OrderBy(s => s.Date)
                .ToListAsync();

            foreach (Shift shift in shifts)
            {
                foreach (Availability shiftAvailability in shift.Availabilities)
                {
                    shiftAvailability.Shift = null;
                }

                if (shift.Task != null)
                    shift.Task.Shifts = null;
            }

            return shifts;
        }

        /// <summary>
        /// Get shifts based on a projectId and a userId including Availabilities
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns>A task of a list of shifts including availabilities</returns>
        public async Task<List<Shift>> GetByProjectWithAvailabilitiesAsync(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty || userId==Guid.Empty)
                return await Task.FromResult<List<Shift>>(null);
            List<Shift> shifts = await EntitySet
                .AsNoTracking()
                .Include(s => s.Project)
                .Include(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Include(s => s.Task)
                .ThenInclude(t => t.Requirements)
                .ThenInclude(r => r.CertificateType)
                .ThenInclude(ct => ct.Certificates)
                .Include(s => s.Availabilities
                    .Where(a => a.Participation.PersonId == userId))
                .ThenInclude(a => a.Participation)
                .Where(s => s.ProjectId == projectId && s.Date >= DateTime.Today &&
                            s.Task.Requirements //user must have required certificate
                                .All(r => r.CertificateType.Certificates
                                    .Where(cert =>
                                        (cert.DateExpired != null && cert.DateExpired >= DateTime.UtcNow) ||
                                        cert.DateExpired == null).Any(cert => cert.PersonId == userId)))
                .OrderBy(s => s.Date)
                .ToListAsync();

            foreach (Shift shift in shifts)
            {
                foreach (Availability shiftAvailability in shift.Availabilities)
                {
                    if (shiftAvailability.Participation != null)
                        shiftAvailability.Participation.Availabilities = null;

                    shiftAvailability.Shift = null;
                }

                shift.Project.Shifts = null;

                if (shift.Task != null)
                {
                    foreach (Requirement requirement in shift.Task.Requirements)
                    {
                        requirement.Task.Requirements = null;
                        requirement.CertificateType.Certificates = null;
                        requirement.CertificateType.Requirements = null;
                    }

                    shift.Task.Shifts = null;
                }
            }

            return shifts;
        }

        /// <summary>
        /// Get a DTO with distinct Tasks, Dates, Starttimes, Endtimes and number of participantsrequired, based on a projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>DTO with distinct attributes</returns>
        public async Task<ShiftData> GetUniqueDataAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));
            ShiftData data = new ShiftData();

            var q = EntitySet
                .AsNoTracking()
                .Include(s => s.Task)
                .Where(s => s.ProjectId == projectId);
            data.Tasks = await q.Select(s => s.Task).Distinct().ToListAsync();
            data.Dates = await q.Select(s => s.Date).Distinct().ToListAsync();
            data.StartTimes = await q.Select(s => s.StartTime.ToString("hh\\:mm")).Distinct().ToListAsync();
            data.EndTimes = await q.Select(s => s.EndTime.ToString("hh\\:mm")).Distinct().ToListAsync();
            data.ParticipantsRequired = await q.Select(s => s.ParticipantsRequired).Distinct().ToListAsync();
            return data;
        }

        /// <summary>
        /// Exports data nessesary for statistical analysis
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>a task of a list of shifts with relating data</returns>
        public async Task<List<Shift>> ExportDataAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));
            List<Shift> shifts = await EntitySet
                .AsNoTracking()
                .Include(s => s.Task)
                .ThenInclude(t => t.Category)
                .Include(s => s.Availabilities.Where(a => a.Type == AvailibilityType.Scheduled))
                .ThenInclude(a => a.Participation)
                .Where(s => s.ProjectId == projectId && s.Availabilities.Any(a => a.Type == AvailibilityType.Scheduled))
                .OrderBy(s => s.Date)
                .ToListAsync();

            shifts.ForEach(s =>
            {
                foreach (Availability objAvailability in s.Availabilities)
                {
                    objAvailability.Participation.Availabilities = null;
                    objAvailability.Shift = null;
                }

                if (s.Task != null)
                    s.Task.Shifts = null;
            });
            return shifts;
        }

        /// <summary>
        /// Search for shifts based on a filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A task of a list of shifts</returns>
        public Task<List<Shift>> SearchProjectsAsync(ShiftFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            var q = EntitySet
                .AsNoTracking()
                .Include(s => s.Task)
                .Where(s => s.ProjectId == filter.ProjectId);
            q = filter.SetFilter(q);

            q = q.Where(x => x.TaskId == null || filter.Tasks.Any(t => t == x.TaskId.ToString()));
            q = q.Where(x => x.Date >= filter.Date);

            q = q.Where(x => x.StartTime >= TimeSpan.Parse(filter.Start));
            q = q.Where(x => x.EndTime >= TimeSpan.Parse(filter.End));
            q = q.Where(x => x.ParticipantsRequired >= filter.ParticipantsRequired);
            q = filter.SetFilter(q);
            filter.TotalItemCount = q.Count();
            Task<List<Shift>> shifts;
            if (filter.Offset >= 0 && filter.PageSize != 0)
                shifts = q.Skip(filter.Offset).Take(filter.PageSize).ToListAsync();
            else
                shifts = q.ToListAsync();

            return shifts;
        }

        /// <summary>
        /// Add multiple shifts to the database
        /// </summary>
        /// <param name="shifts"></param>
        /// <returns>A task of a list of shifts added to the database</returns>
        public Task<List<Shift>> AddShiftsAsync(List<Shift> shifts)
        {
            if (shifts == null || shifts.Count == 0)
                return Task.FromResult<List<Shift>>(null);
            foreach (Shift shift in shifts)
            {
                if (shift.Id == Guid.Empty)
                    shift.SetKey(Guid.NewGuid());
                shift.LastEditDate = DateTime.UtcNow;
                shift.Task = null;
                shift.Project = null;

                EntitySet.AddAsync(shift);
            }

            return Task.FromResult(shifts);
        }
        /// <summary>
        /// Get a shift based on shiftId
        /// </summary>
        /// <param name="shiftId"></param>
        /// <returns>A task of a shift </returns>
        public Task<Shift> GetShiftAsync(Guid shiftId)
        {
            if (shiftId == Guid.Empty)
                return Task.FromResult<Shift>(null);

            return EntitySet
                .AsNoTracking()
                .Include(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Include(s => s.Project)
                .Where(s => s.Id == shiftId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get a shift based on a shiftId including Availabilities
        /// </summary>
        /// <param name="shiftId"></param>
        /// <returns>A task of a shift including availabilities</returns>
        public async Task<Shift> GetShiftWithAvailabilitiesAsync(Guid shiftId)
        {
            if (shiftId == Guid.Empty)
                return await Task.FromResult<Shift>(null);

            Shift shift = await EntitySet
                .AsNoTracking()
                .Include(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Include(s => s.Project)
                .Include(s => s.Availabilities)
                .ThenInclude(a => a.Participation)
                .Where(s => s.Id == shiftId)
                .FirstOrDefaultAsync();

            if (shift.Task != null)
                shift.Task.Shifts = null;
            shift.Availabilities.ForEach(a =>
            {
                if (a.Participation != null)
                    a.Participation.Availabilities = null;
                a.Shift = null;
            });

            return shift;
        }

        /// <summary>
        /// Get shifts based on a projectId and a Date including Availabilities
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="date"></param>
        /// <returns>A task of a list of shifts including availabilities</returns>
        public async Task<List<Shift>> GetByProjectAndDateWithAvailabilitiesAsync(Guid projectId, DateTime date)
        {
            if (projectId == Guid.Empty)
                return null;
            List<Shift> listOfShifts = await EntitySet
                .AsNoTracking()
                .Include(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Include(s => s.Availabilities)
                .Where(s => s.ProjectId == projectId && s.Date == date && s.Task != null)
                .ToListAsync();
            listOfShifts?.ForEach(shift =>
            {
                if (shift.Task != null)
                    shift.Task.Shifts = null;
                shift.Availabilities.ForEach(a => { a.Shift = null; });
            });
            return listOfShifts;
        }

        /// <summary>
        /// Get shifts based on a projectId, userId and a Date including Availabilities
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns>A task of a list of shifts including availabilities</returns>
        public async Task<List<Shift>> GetByProjectUserAndDateAsync(Guid projectId, Guid userId, DateTime date)
        {
            if (projectId == Guid.Empty || userId == Guid.Empty)
                return null;
            List<Shift> listOfShifts = await EntitySet
                .AsNoTracking()
                .Include(s => s.Project)
                .Include(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Include(s => s.Task)
                .ThenInclude(t => t.Requirements)
                .ThenInclude(r => r.CertificateType)
                .ThenInclude(c => c.Certificates)
                .Include(s => s.Availabilities.Where(a => a.Participation.PersonId == userId))
                .ThenInclude(a => a.Participation)
                .Where(s => s.ProjectId == projectId &&
                            s.Date == date && s.Task != null &&
                            s.Task.Requirements //user must have required certificate
                                .All(r => r.CertificateType.Certificates
                                    .Where(cert =>
                                        (cert.DateExpired != null && cert.DateExpired >= DateTime.UtcNow) ||
                                        cert.DateExpired == null)
                                    .Any(cert => cert.PersonId == userId)))
                .ToListAsync();

            Parallel.ForEach(listOfShifts, shift =>
            {
                shift.Availabilities.ForEach(a =>
                {
                    a.Participation.Availabilities = null;
                    a.Shift = null;
                });
                if (shift.Task != null)
                {
                    shift.Task.Shifts = null;
                    shift.Task.Requirements.ForEach(r => r.Task = null);
                }
            });
            return listOfShifts;
        }
    }
}