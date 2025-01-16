using System.Collections.Generic;
namespace RoosterPlanner.Api.Models.HelperViewModels
{
    /// <summary>
    /// DTO for searchresults from searchmethods using a filter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SearchResultViewModel<T>(int totalcount, List<T> resultList)
    where T : class {
        /// <summary>
        /// Gets or sets the TotalCount 
        /// </summary>
        public int Totalcount { get; set; } = totalcount;

        /// <summary>
        /// Gets or sets the ResultList 
        /// </summary>
        public List<T> ResultList { get; set; } = resultList;

        //Constructor
    }
}
