namespace RoosterPlanner.Models.FilterModels
{
    public class PersonFilter : EntityFilterBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Gets or sets the City  
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the UserRole 
        /// </summary>
        public string UserRole { get; set; }

        /// <summary>
        /// Get or sets the offset for retrieving records.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Get or sets the amount of records to retrieve.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Get or sets the total count of records that apply the filter.
        /// </summary>
        public int TotalItemCount { get; set; }

        #endregion

        public PersonFilter() : base()
        {
            Sort = new string[] {"LastName", "ASC"};
            Offset = 0;
            PageSize = 0;
        }
        
        //Constructor - Overload
        public PersonFilter(int offset, int pageSize) : base()
        {
            Sort = new string[] { "LastName", "ASC" };
            Offset = offset;
            PageSize = 20;
            if (pageSize > 0 && pageSize < 1000)
                PageSize = pageSize;
        }
    }
}