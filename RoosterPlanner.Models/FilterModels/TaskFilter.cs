namespace RoosterPlanner.Models.FilterModels
{
    public class TaskFilter : EntityFilterBase
    {
        #region Properties

        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or sets the offset for retrieving records.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Get or sets the amount of records to retrieve.
        /// </summary>
        public int PageSize { get; set; }

        public int TotalItemCount { get; set; }

        #endregion

        //Constructor
        public TaskFilter()
        {
            Sort = new[] {"Name", "ASC"};
            Offset = 0;
            PageSize = 0;
        }

        //Constructor - Overload
        public TaskFilter(int offset, int pageSize)
        {
            Sort = new[] {"Name", "ASC"};
            Offset = offset;
            PageSize = 20;
            if (pageSize > 0 && pageSize < 1000)
                PageSize = pageSize;
        }
    }
}