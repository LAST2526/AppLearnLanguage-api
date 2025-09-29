using Last02.Data.Repositories.RepositoryBase;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveAsync();

        IMemberRepository Member { get; }
        IUserRepository User { get; }
        IMemberCourseRepository MemberCourse { get; }
        ICourseRepository Course { get; }
        ITopicRepository Topic { get; }
        IAudioRepository Audio { get; }
        IFlashcardRepository Flashcard { get; }
        IMemberFlashcardRepository MemberFlashcard { get; }
        IClaimsReprository Claim { get; }

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
