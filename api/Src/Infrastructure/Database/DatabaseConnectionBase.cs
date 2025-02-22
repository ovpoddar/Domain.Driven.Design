using DDD.Application.Abstractions.Database;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace DDD.Infrastructure.Database;

internal class DatabaseConnectionBase<T> : IDatabaseConnectionBase<T> where T : class
{
    private readonly ApplicationDbContext _applicationDB;

    public DatabaseConnectionBase(ApplicationDbContext applicationDB) =>
        _applicationDB = applicationDB ?? throw new ArgumentNullException(nameof(applicationDB));

    public void Create(T entity) =>
        _applicationDB.Set<T>().Add(entity);

    public void Delete(T entity) =>
        _applicationDB.Set<T>().Remove(entity);

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges
        ? _applicationDB.Set<T>().AsNoTracking()
        : _applicationDB.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        !trackChanges
        ? _applicationDB.Set<T>().Where(expression).AsNoTracking()
        : _applicationDB.Set<T>().Where(expression);

    public IQueryable<TO> FindByConditionAndType<TO>(Expression<Func<TO, bool>> expression, bool trackChanges) where TO : class =>
         !trackChanges
        ? _applicationDB.Set<TO>().Where(expression).AsNoTracking()
        : _applicationDB.Set<TO>().Where(expression);

    public IDbConnection Initialized() =>
        _applicationDB.Database.GetDbConnection();

    public Task SaveAsync() =>
        _applicationDB.SaveChangesAsync();

    public void Update(T entity) =>
        _applicationDB.Set<T>().Update(entity);
}
