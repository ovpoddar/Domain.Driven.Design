using System.Data;
using System.Linq.Expressions;

namespace DDD.Application.Abstractions.Database;

public interface IDatabaseConnectionBase<T>
{
    IQueryable<T> FindAll(bool trackChanges);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
    IQueryable<TO> FindByConditionAndType<TO>(Expression<Func<TO, bool>> expression, bool trackChanges) where TO : class;
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveAsync();
    IDbConnection Initialized();
}
