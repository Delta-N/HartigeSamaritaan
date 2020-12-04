using System.Collections.Generic;
using RoosterPlanner.Models.FilterModels;

namespace RoosterPlanner.Api.Models
{
    public class SearchResultViewModel<T> where T : class
    {
        public int Totalcount { get; set; }
        public List<T> ResultList { get; set; }

        public SearchResultViewModel(int totalcount, List<T> resultList)
        {
            Totalcount = totalcount;
            ResultList = resultList;
        }
    }
}