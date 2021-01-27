using System.Collections.Generic;

namespace RoosterPlanner.Api.Models
{
    /// <summary>
    /// DTO for searchresults from searchmethods using a filter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SearchResultViewModel<T> where T : class
    {
        /// <summary>
        /// Gets or sets the TotalCount 
        /// </summary>
        public int Totalcount { get; set; }

        /// <summary>
        /// Gets or sets the ResultList 
        /// </summary>
        public List<T> ResultList { get; set; }

        //Constructor
        public SearchResultViewModel(int totalcount, List<T> resultList)
        {
            Totalcount = totalcount;
            ResultList = resultList;
        }
    }
}