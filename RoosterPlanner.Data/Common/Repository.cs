﻿using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Common
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        protected DbContext DataContext { get; private set; }
        protected virtual DbSet<TEntity> EntitySet { get; private set; }

        #region Fields
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        public Repository(DbContext dataContext)
        {
            DataContext = dataContext;

            EntitySet = DataContext.Set<TEntity>();
            if (EntitySet == null)
                throw new InvalidOperationException($"No entity set found in the context for the type {typeof(TEntity).Name}");
        }

        /// <summary>
        /// Get the entity with the provided id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The entity that matched the id or <c>null</c> if no match has been found.</returns>
        public virtual TEntity Get(Guid id)
        {
            return EntitySet.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Get the entity with the provided id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The entity that matched the id or <c>null</c> if no match has been found.</returns>
        public virtual Task<TEntity> GetAsync(Guid id)
        {
            return EntitySet.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Lookup the entity with the provided id's from cache, if not found then retrieved from datastore.
        /// </summary>
        /// <param name="ids">The primary keys of the entity.</param>
        /// <returns>The entity that matched the id's.</returns>
        public virtual TEntity Find(params Guid[] ids)
        {
            return EntitySet.Find(ids);
        }

        /// <summary>
        /// Lookup the entity with the provided id's from cache, if not found then retrieved from datastore.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>The list of records that matched one of the id's.</returns>
        public virtual async Task<TEntity> FindAsync(params Guid[] ids)
        {
            return await EntitySet.FindAsync(ids);
        }

        /// <summary>
        /// Adds the specified entity. This will update the whole object graph.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The added entity.</returns>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        public virtual TEntity Add(TEntity entity)
        {
            if (entity == null)
                return null;

            try
            {
                ValidationContext validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext, true);
            }
            catch (ValidationException valEx)
            {
                if (valEx.ValidationResult.MemberNames != null && valEx.ValidationResult.MemberNames.Any())
                {
                    throw new ValidationException(ComposeErrorMessage(valEx), valEx);
                }
                throw;
            }

            EntityEntry<TEntity> entry = DataContext.Entry(entity);
            if (entity.Id == Guid.Empty)
                entity.SetKey(Guid.NewGuid());

            entity.LastEditDate = DateTime.UtcNow;
            entry = EntitySet.Add(entity);

            return entry.Entity;
        }

        /// <summary>
        /// Updates the specified entity. This will update the whole object graph.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The modified entity.</returns>
        public virtual TEntity Update(TEntity entity)
        {
            if (entity == null)
                return null;

            try
            {
                ValidationContext validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext, true);
            }
            catch (ValidationException valEx)
            {
                if (valEx.ValidationResult.MemberNames != null && valEx.ValidationResult.MemberNames.Any())
                {
                    throw new ValidationException(ComposeErrorMessage(valEx), valEx);
                }
                throw;
            }

            entity.LastEditDate = DateTime.UtcNow;
            EntityEntry<TEntity> entry = EntitySet.Attach(entity);
            entry.State = EntityState.Modified;

            return entry.Entity;
        }

        /// <summary>
        /// Adds or update the specified entity. This will update the whole object graph.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The added or modified entity.</returns>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        public virtual TEntity AddOrUpdate(TEntity entity)
        {
            if (entity == null)
                return null;

            try
            {
                ValidationContext validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext, true);
            }
            catch (ValidationException valEx)
            {
                if (valEx.ValidationResult.MemberNames != null && valEx.ValidationResult.MemberNames.Any())
                {
                    throw new ValidationException(ComposeErrorMessage(valEx), valEx);
                }
                throw;
            }

            EntityEntry<TEntity> entry = DataContext.Entry(entity);

            TEntity attachedEntity = null;
            if (entity.Id == Guid.Empty)
            {
                //Insert
                entity.SetKey(Guid.NewGuid());
                entry = EntitySet.Add(entity);
            }
            else if (entry.State == EntityState.Detached)
            {
                //Update
                attachedEntity = EntitySet.Local.SingleOrDefault(e => e.Id.Equals(entity.Id));
                if (attachedEntity != null)
                {
                    entity.LastEditDate = DateTime.UtcNow;
                    DataContext.Entry(attachedEntity).CurrentValues.SetValues(entity);
                }
                else
                {
                    entry = EntitySet.Attach(entry.Entity);
                    entry.State = EntityState.Modified;
                }
            }
            else
            {
                EntitySet.Update(entity);
            }

            if (attachedEntity == null)
            {
                entity.LastEditDate = DateTime.UtcNow;
            }

            return entity;
        }

        /// <summary>
        /// Begins tracking the given entity in the EntityState.Deleted state 
        /// such that it will be removed from the database when SaveChanges is called.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="rowversion">The rowversion of the entity.</param>
        /// <returns>The entity in deleted state.</returns>
        public virtual TEntity Remove(Guid id, byte[] rowversion)
        {
            return Remove(new { Id = id, RowVersion = rowversion } as TEntity);
        }

        /// <summary>
        /// Begins tracking the given entity in the EntityState.Deleted state 
        /// such that it will be removed from the database when SaveChanges is called.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <returns>The entity in deleted state.</returns>
        public virtual TEntity Remove(TEntity entity)
        {
            return entity != null ? EntitySet.Remove(entity).Entity : null;
        }

        /// <summary>
        /// Immediately deletes the entity from the database.
        /// </summary>
        /// <remarks>The entity is not removed, but immediately deleted without the need to call SaveChanges.</remarks>
        /// <param name="id">The id of the entity.</param>
        /// <returns>The number of affected rows.</returns>
        public int Delete(Guid id)
        {
            if (id != Guid.Empty)
                return DataContext.Database.ExecuteSqlRaw($"DELETE FROM [{nameof(TEntity)}] WHERE[AuthorId] = @p0;", id);
            return 0;
        }

        #region Private Methods
        /// <summary>
        /// Creates an error message of the validation data.
        /// </summary>
        /// <param name="validationEx"></param>
        /// <returns></returns>
        private static string ComposeErrorMessage(ValidationException validationEx)
        {
            if (validationEx == null)
                return string.Empty;

            StringBuilder msg = new StringBuilder("Het item kan niet opgeslagen worden omdat het niet geldig is.");
            if (validationEx.Data != null && validationEx.Data.Count != 0)
            {
                msg.AppendLine().AppendLine();
                foreach (DictionaryEntry valError in validationEx.Data)
                    msg.AppendFormat("Veld {0}: {1}{2}", valError.Key, valError.Value, Environment.NewLine);
            }
            else if (validationEx.ValidationAttribute is MaxLengthAttribute)
            {
                string key = validationEx.ValidationResult.MemberNames.FirstOrDefault();
                if (string.IsNullOrEmpty(key)) return msg.ToString();
                msg.AppendLine().AppendLine();
                msg.AppendFormat("Veld {0}: {1}{2}", key, validationEx.ValidationResult.ErrorMessage, Environment.NewLine);
            }
            return msg.ToString();
        } 
        #endregion
    }
}
