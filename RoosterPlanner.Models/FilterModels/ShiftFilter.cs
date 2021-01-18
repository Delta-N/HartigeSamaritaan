using System;
using System.Collections.Generic;

namespace RoosterPlanner.Models.FilterModels
{
    public class ShiftFilter: EntityFilterBase
    {   
        #region Properties

        public Guid ProjectId { get; set; }

        public List<string> Tasks { get; set; }
        public DateTime Date { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int ParticipantsRequired { get; set; }
        public int Offset { get; set; }
        public int PageSize { get; set; }
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