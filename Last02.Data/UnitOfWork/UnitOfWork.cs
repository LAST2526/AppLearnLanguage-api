using Last02.Data.Repositories.RepositoryBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Data.Repositories.Implements;

namespace Last02.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction? _transaction = null;

        public IMemberRepository Member { get; }
        public IUserRepository User { get; }
        public IMemberCourseRepository MemberCourse { get; }
        public ICourseRepository Course { get; }
        public ITopicRepository Topic { get; }
        public IAudioRepository Audio { get; }
        public IFlashcardRepository Flashcard { get; }
        public IMemberFlashcardRepository MemberFlashcard { get; }
        public IClaimsReprository Claim { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context;
            Member = new MemberRepository(context);
            User = new UserRepository(context);
            MemberCourse = new MemberCourseRepository(context);
            Course = new CourseRepository(context);
            Claim = new ClaimsRepository(context);
            Topic = new TopicRepository(context);
            Audio = new AudioRepository(context);
            Flashcard = new FlashcardRepository(context);
            MemberFlashcard = new MemberFlashcardRepository(context);
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_dbContext == null) return;
            _dbContext.Dispose();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return _dbContext.Entry(entity);
        }
    }
}
