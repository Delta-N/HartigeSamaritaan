﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RoosterPlanner.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Common
{
    public abstract class Repository<TKey, TEntity> : IRepository<TKey, TEntity> where TKey : struct where TEntity : class, IEntity<TKey>, new()
    {
        protected DbContext DataContext { get; private set; }
        protected virtual DbSet<TEntity> EntitySet { get; private set; }

        #region Fields
        private readonly ILogger logger = null;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="logger">Interface where log messages can be written to.</param>
        public Repository(DbContext dataContext, ILogger logger)
        {
            this.DataContext = dataContext ?? throw new ArgumentNullException("dataContext");

            this.EntitySet = DataContext.Set<TEntity>();
            if (EntitySet == null)
                throw new InvalidOperationException($"No entity set found in the context for the type {typeof(TEntity).Name}");

            this.logger = logger;
        }

        /// <summary>
        /// Get the entity with the provided id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The entity that matched the id or <c>null</c> if no match has been found.</returns>
        public virtual TEntity Get(TKey id)
        {
            return this.EntitySet.FirstOrDefault(x => x.Id.Equals(id));
        }

        /// <summary>
        /// Get the entity with the provided id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The entity that matched the id or <c>null</c> if no match has been found.</returns>
        public virtual Task<TEntity> GetAsync(TKey id)
        {
            return this.EntitySet.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        /// <summary>
        /// Lookup the entity with the provided id's from cache, if not found then retrieved from datastore.
        /// </summary>
        /// <param name="ids">The primary keys of the entity.</param>
        /// <returns>The entity that matched the id's.</returns>
        public virtual TEntity Find(params TKey[] ids)
        {
            return this.EntitySet.Find(ids);
        }

        /// <summary>
        /// Lookup the entity with the provided id's from cache, if not found then retrieved from datastore.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>The list of records that matched one of the id's.</returns>
        public virtual async Task<TEntity> FindAsync(params TKey[] ids)
        {
            return await this.EntitySet.FindAsync(ids);
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
                //Log
                logger.LogException(valEx, new Dictionary<string, string> { { "Message", valEx.Message }, { "FieldValue", valEx.Value.ToString() } });
                if (valEx.ValidationResult.MemberNames != null && valEx.ValidationResult.MemberNames.Count() != 0)
                {
                    throw new ValidationException(ComposeErrorMessage(valEx), valEx);
                }
                throw valEx;
            }

            EntityEntry<TEntity> entry = this.DataContext.Entry(entity);

            TEntity attachedEntity = null;
            if ((entity.Id is Guid && entity.Id.Equals(Guid.Empty))
                || (entity.Id is int && entity.Id.Equals(0))
                || (entity.Id is DateTime && entity.Id.Equals(new DateTime())))
            {
                //Insert
                entity.SetNewKey();
                entry = this.EntitySet.Add(entity);
            }
            else if (entry.State == EntityState.Detached)
            {
                //Update
                attachedEntity = this.EntitySet.Local.SingleOrDefault(e => e.Id.Equals(entity.Id));
                if (attachedEntity != null)
                {
                    entity.LastEditDate = DateTime.UtcNow;
                    this.DataContext.Entry<TEntity>(attachedEntity).CurrentValues.SetValues(entity);
                }
                else
                {
                    entry = this.EntitySet.Attach(entry.Entity);
                    entry.State = EntityState.Modified;
                }
            }
            else
            {
                this.EntitySet.Update(entity);
            }

            if (attachedEntity == null)
            {
                entity.LastEditBy = "System";
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
        public virtual TEntity Remove(TKey id, byte[] rowversion)
        {
            return this.Remove(new TEntity() { Id = id, RowVersion = rowversion });
        }

        /// <summary>
        /// Begins tracking the given entity in the EntityState.Deleted state 
        /// such that it will be removed from the database when SaveChanges is called.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <returns>The entity in deleted state.</returns>
        public virtual TEntity Remove(TEntity entity)
        {
            if (entity != null)
                return this.EntitySet.Remove(entity).Entity;
            return entity;
        }

        #region Private Methods
        /// <summary>
        /// Creates an error message of the validation data.
        /// </summary>
        /// <param name="validationEx"></param>
        /// <returns></returns>
        private string ComposeErrorMessage(ValidationException validationEx)
        {
            if (validationEx == null)
                return String.Empty;

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
                if (!String.IsNullOrEmpty(key))
                {
                    msg.AppendLine().AppendLine();
                    msg.AppendFormat("Veld {0}: {1}{2}", key, validationEx.ValidationResult.ErrorMessage, Environment.NewLine);
                }
            }
            return msg.ToString();
        } 
        #endregion
    }
}