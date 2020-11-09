using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RoosterPlanner.Models.FilterModels
{
    public static class EntityFilterExtensions
    {
        public static IQueryable<TEntity> ApplyFilter<TEntity>(this IQueryable<TEntity> query, EntityFilterBase filter)
        {
            if (filter == null) throw new ArgumentNullException("filter");

            return filter.SetFilter(query);
        }
    }

    public abstract class EntityFilterBase
    {
        #region Fields
        private static string[] defaultSort = new string[] { "Id", "ASC" };
        private string[] sort = new string[] { };
        internal List<SortType> sortingList = new List<SortType>();
        #endregion

        #region Properties
        // Sorting
        public virtual string[] Sort
        {
            get { return sort; }
            set
            {
                // Set the field
                sort = value != null && value.Length > 0 ? value : defaultSort;

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
        public EntityFilterBase() : this(defaultSort)
        {
        }

        //Constructor - Overload
        public EntityFilterBase(string[] sort)
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
        public virtual IQueryable<T> SetFilter<T>(IQueryable<T> queryable)
        {
            if (queryable == null)
                return queryable;

            if (Sort != null && sortingList.Count > 0)
            {
                // New way: create an expression so 'ThenBy' ordering can be done
                queryable = CreateOrderedQuery<T>(queryable, sortingList[0], sortingList[0].Direction.ToLower().Equals("asc") ? "OrderBy" : "OrderByDescending");

                if (sortingList.Count > 1)
                {
                    for (int i = 1; i < sortingList.Count; i++)
                        queryable = CreateOrderedQuery<T>(queryable, sortingList[i], sortingList[i].Direction.ToLower().Equals("asc") ? "ThenBy" : "ThenByDescending");
                }
            }

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
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), orderingMethod, new Type[] { type, property.PropertyType }, queryable.Expression, Expression.Quote(orderByExp));
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
                throw new ArgumentNullException("fieldName");

            if (string.IsNullOrEmpty(direction))
                throw new ArgumentNullException("direction");

            if (char.IsLower(fieldName[0]))
            {
                var firstChar = char.ToUpperInvariant(fieldName[0]);

                if (fieldName.Length == 1)
                    fieldName = firstChar.ToString();
                else
                    fieldName = string.Format("{0}{1}", firstChar, fieldName.Substring(1));
            }

            FieldName = fieldName;
            Direction = direction.ToUpper();
        }
    }
}
