using System;
using System.Collections.Generic;

namespace RoosterPlanner.Models.FilterModels
{
    public class ShiftFilter : EntityFilterBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the ProjectId 
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the Tasks 
        /// </summary>
        public List<string> Tasks { get; set; }

        /// <summary>
        /// Gets or sets the Date 
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the Start 
        /// </summary>
        public string Start { get; set; }

        /// <summary>
        /// Gets or sets the End 
        /// </summary>
        public string End { get; set; }

        /// <summary>
        /// Gets or sets the number of ParticipantsRequired 
        /// </summary>
        public int ParticipantsRequired { get; set; }

        /// <summary>
        /// Gets or sets the Offset 
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the PageSize 
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the Total number of Items 
        /// </summary>
        public int TotalItemCount { get; set; }

        #endregion

        public ShiftFilter()
        {
            Sort = new[] {"Date", "ASC"};
            Offset = 0;
            PageSize = 0;
        }

        public ShiftFilter(int offset, int pageSize)
        {
            Offset = offset;
            PageSize = 20;
            if (pageSize > 0 && pageSize < 1000)
                PageSize = pageSize;
        }
    }
}