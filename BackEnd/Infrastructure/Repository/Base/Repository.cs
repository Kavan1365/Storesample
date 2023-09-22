using BaseCore.Core;
using BaseCore.Utilities;
using Core.Repositories.Base;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly BaseContext _dbContext;

        public Repository(BaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        ///// for edite and read////
        public virtual IQueryable<T> Table => _dbContext.Set<T>().Where(x => !x.IsSoftDeleted).OrderByDescending(x => x.Id);
        /////for only read////
        public virtual IQueryable<T> TableNoTracking => _dbContext.Set<T>().Where(x => !x.IsSoftDeleted).AsNoTracking().OrderByDescending(x => x.Id);


        #region Async Method
        public virtual ValueTask<T> GetByIdAsync(CancellationToken cancellationToken, params object[] ids)
        {
            return _dbContext.Set<T>().FindAsync(ids, cancellationToken);
        }
        public virtual Task<T> GetByGuidAsync(CancellationToken cancellationToken, Guid guid)
        {
            return _dbContext.Set<T>().SingleOrDefaultAsync(z => !z.IsSoftDeleted && z.Guid == guid, cancellationToken);
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
            if (saveNow)
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
            if (saveNow)
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));



            _dbContext.Set<T>().Update(entity);





            if (saveNow)
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            _dbContext.Set<T>().UpdateRange(entities);

            if (saveNow)
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        }

        public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken, bool isSoftDelete = false, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            if (isSoftDelete)
            {
                entity.IsSoftDeleted = true;
                await UpdateAsync(entity, cancellationToken);
                if (saveNow)
                    await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            }
            else
            {


                _dbContext.Set<T>().Remove(entity);


                if (saveNow)
                    await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);


            }




        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            _dbContext.Set<T>().RemoveRange(entities);


            if (saveNow)
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        }
        #endregion

        #region Sync Methods
        public virtual T GetByGuid(Guid guid)
        {
            return _dbContext.Set<T>().SingleOrDefault(z => !z.IsSoftDeleted && z.Guid == guid);
        }
        public virtual T GetById(params object[] ids)
        {
            return _dbContext.Set<T>().Find(ids);
        }
        public virtual void Add(T entity, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            _dbContext.Set<T>().Add(entity);
            if (saveNow)
                _dbContext.SaveChanges();
        }

        public virtual void AddRange(IEnumerable<T> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            _dbContext.Set<T>().AddRange(entities);
            if (saveNow)
                _dbContext.SaveChanges();
        }

        public virtual void Update(T entity, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            _dbContext.Set<T>().Update(entity);

            if (saveNow)
                _dbContext.SaveChanges();



        }

        public virtual void UpdateRange(IEnumerable<T> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            _dbContext.Set<T>().UpdateRange(entities);





            if (saveNow)
                _dbContext.SaveChanges();

        }

        public virtual void Delete(T entity, bool isSoftDelete = true, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            if (isSoftDelete)
            {


                entity.IsSoftDeleted = true;
                Update(entity);

                if (saveNow)
                    _dbContext.SaveChanges();



            }
            else
            {
                _dbContext.Set<T>().Remove(entity);

                if (saveNow)
                    _dbContext.SaveChanges();


            }
        }

        public virtual void DeleteRange(IEnumerable<T> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            _dbContext.Set<T>().RemoveRange(entities);
            if (saveNow)
                _dbContext.SaveChanges();


        }
        #endregion

        #region Attach & Detach
        public virtual void Detach(T entity)
        {
            Assert.NotNull(entity, nameof(entity));
            var entry = _dbContext.Entry(entity);
            if (entry != null)
                entry.State = EntityState.Detached;
        }

        public virtual void Attach(T entity)
        {
            Assert.NotNull(entity, nameof(entity));
            if (_dbContext.Entry(entity).State == EntityState.Detached)
                _dbContext.Set<T>().Attach(entity);
        }
        #endregion

        #region Explicit Loading
        public virtual async Task LoadCollectionAsync<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken)
            where TProperty : class
        {
            Attach(entity);

            var collection = _dbContext.Entry(entity).Collection(collectionProperty);
            if (!collection.IsLoaded)
                await collection.LoadAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual void LoadCollection<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> collectionProperty)
            where TProperty : class
        {
            Attach(entity);
            var collection = _dbContext.Entry(entity).Collection(collectionProperty);
            if (!collection.IsLoaded)
                collection.Load();
        }

        public virtual async Task LoadReferenceAsync<TProperty>(T entity, Expression<Func<T, TProperty>> referenceProperty, CancellationToken cancellationToken)
            where TProperty : class
        {
            Attach(entity);
            var reference = _dbContext.Entry(entity).Reference(referenceProperty);
            if (!reference.IsLoaded)
                await reference.LoadAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual void LoadReference<TProperty>(T entity, Expression<Func<T, TProperty>> referenceProperty)
            where TProperty : class
        {
            Attach(entity);
            var reference = _dbContext.Entry(entity).Reference(referenceProperty);
            if (!reference.IsLoaded)
                reference.Load();
        }
        #endregion
    }
}
