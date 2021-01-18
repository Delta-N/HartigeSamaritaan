using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RoosterPlanner.Models.FilterModels
{
    public abstract class EntityFilterBase
    {
        #region Fields
        private static readonly string[] DefaultSort = { "Id", "ASC" };
        private string[] sort = Array.Empty<string>();
        private readonly List<SortType> sortingList = new();
        #endregion

        #region Properties
        // Sorting
        public string[] Sort
        {
            get => sort;
            set
            {
                // Set the field
                sort = value != null && value.Length > 0 ? value : DefaultSort;

                //Always clear list.
                sortingList.Clear();

                // Sort always has a value so set the list
                for (int i = 0; i < sort.Length; i++)
                {
                    SortType sortType = new SortType(sort[i]);
                    if (sort.Length >= i + 2)
                    {
                        i++;
                        sortType.Direction = sort[i];
                    }
                    sortingList.Add(sortType);
                }
            }
        }
        #endregion

        #region Constructor
        //Constructor
        protected EntityFilterBase() : this(DefaultSort)
        {
        }

        //Constructor - Overload
        protected EntityFilterBase(string[] sort)
        {
            Sort = sort;
        }
        #endregion

        /// <summary>
        /// Applies the filter to the IQueryable object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <returns></returns>
        public IQueryable<T> SetFilter<T>(IQueryable<T> queryable)
        {
            if (queryable == null)
                return null;

            if (Sort == null || sortingList.Count <= 0) return queryable;
            
            // New way: create an expression so 'ThenBy' ordering can be done
            queryable = CreateOrderedQuery(queryable, sortingList[0], sortingList[0].Direction.ToLower().Equals("asc") ? "OrderBy" : "OrderByDescending");

            if (sortingList.Count <= 1) return queryable;
            for (int i = 1; i < sortingList.Count; i++)
                queryable = CreateOrderedQuery(queryable, sortingList[i], sortingList[i].Direction.ToLower().Equals("asc") ? "ThenBy" : "ThenByDescending");

            return queryable;
        }

        /// <summary>
        /// Create an ordered query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The query.</param>
        /// <param name="sortType">Type of the sort.</param>
        /// <returns></returns>
        private static IQueryable<T> CreateOrderedQuery<T>(IQueryable<T> queryable, SortType sortType, string orderingMethod)
        {
            var type = typeof(T);
            var property = type.GetProperty(sortType.FieldName);
            var parameter = Expression.Parameter(type, "t");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property ?? throw new InvalidOperationException());
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), orderingMethod, new[] { type, property.PropertyType }, queryable.Expression, Expression.Quote(orderByExp));
            return queryable.Provider.CreateQuery<T>(resultExp);
        }
    }

    internal class SortType
    {
        public string FieldName { get; set; }
        public string Direction { get; set; }

        public SortType(string fieldName) : this(fieldName, "ASC")
        {
        }

        public SortType(string fieldName, string direction)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentNullException(nameof(fieldName));

            if (string.IsNullOrEmpty(direction))
                throw new ArgumentNullException(nameof(direction));

            if (char.IsLower(fieldName[0]))
            {
                var firstChar = char.ToUpperInvariant(fieldName[0]);

                fieldName = fieldName.Length == 1 ? firstChar.ToString() : $"{firstChar}{fieldName.Substring(1)}";
            }

            FieldName = fieldName;
            Direction = direction.ToUpper();
        }
    }
}
