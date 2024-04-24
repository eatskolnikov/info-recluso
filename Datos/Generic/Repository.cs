using Datos.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Datos.Generic;

public class Repository
{
    private readonly string _noFount = "Datos No encontrado";

    private readonly AppDbContext _db;
    public Repository(AppDbContext db) => (_db) = (db);

    private async void Dispose() { await _db.DisposeAsync(); }

    public async Task<int> SaveAsync() => await _db.SaveChangesAsync();

    public async Task<(T entity, Response response)> GetByIdAsync<T>(string id) where T : class
    {
        try
        {
            var entidad = await _db.Set<T>().FindAsync(id);
            return (entidad, entidad is not null ? Response.Ok() : Response.NoFound(_noFount));
        }
        catch (Exception ex)
        {
            return (default, Response.Error(ex.Message));
        }
    }

    public async Task<(IEnumerable<T> list, Response response)> GetAllAsync<T>(Expression<Func<T, bool>> expression = null) where T : class
    {
        try
        {
            List<T> results = default;
            if (expression == null)
                results = await _db.Set<T>().AsNoTrackingWithIdentityResolution().ToListAsync();
            else
                results = await _db.Set<T>().Where(expression)
                                  .AsNoTrackingWithIdentityResolution()
                                  .ToListAsync();

            return (results, results is not null ? Response.Ok() : Response.NoFound(_noFount));
        }
        catch (Exception ex)
        {
            return (default, Response.Error(ex.Message));
        }
    }

    public async Task<(IEnumerable<T> list, Response response)> GetAllAsync<T>(Expression<Func<T, bool>> expression = null, params Expression<Func<T, object>>[] includes) where T : class
    {
        try
        {
            List<T> results = default;
            if (expression == null)
                results = await _db.Set<T>().AsNoTrackingWithIdentityResolution().IncludeMultiple(includes).ToListAsync();
            else
                results = await _db.Set<T>()
                    .Where(expression)
                    .AsNoTrackingWithIdentityResolution()
                    .IncludeMultiple(includes)
                    .ToListAsync();

            return (results, results is not null ? Response.Ok() : Response.Fail(_noFount));
        }
        catch (Exception ex)
        {
            return (default, Response.Fail(ex.Message));
        }
    }

    public async Task<(T entity, Response response)> FindAsync<T>(Expression<Func<T, bool>> expression) where T : class
    {
        try
        {
            var result = await _db.Set<T>().Where(expression).FirstOrDefaultAsync();
            return (result, result is not null ? Response.Ok() : Response.NoFound(_noFount));
        }
        catch (Exception ex)
        {
            return (default, Response.Error(ex.Message));
        }
    }

    public async Task<(T entity, Response response)> FindAsync<T>(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes) where T : class
    {
        try
        {
            var result = await _db.Set<T>().Where(expression)
                                           .AsNoTrackingWithIdentityResolution()
                                           .IncludeMultiple(includes)
                                           .FirstOrDefaultAsync();

            return (result, result is not null ? Response.Ok() : Response.NoFound(_noFount));
        }
        catch (Exception ex)
        {
            return (default, Response.Error(ex.InnerException.Message));
        }
    }

    public async Task<Response> AddAsync<T>(T entity) where T : class
    {
        using var transaction = await _db.Database.BeginTransactionAsync();
        EntityEntry<T> estado = null!;

        try
        {
            estado = await _db.Set<T>().AddAsync(entity);
            await SaveAsync();
            transaction.Commit();
            estado.State = EntityState.Detached;
            entity = default;
            return Response.Ok(message: "Agregado");
        }
        catch (Exception ex)
        {
            if (estado is not null && estado.State != EntityState.Detached)
                estado.State = EntityState.Detached;

            return Response.Error(ex?.InnerException?.Message);
        }
    }

    public async Task<Response> UpdateAsync<T>(T UpdateDto) where T : class
    { 
        using var transaction = await _db.Database.BeginTransactionAsync();

        try
        {
            var entity = _db.Entry(UpdateDto).State = EntityState.Modified;
            _db.Entry(UpdateDto).PropertyCheckNotUpdate(UpdateDto);
            var estatus = await SaveAsync() > 0;
            transaction.Commit();
            _db.Entry(UpdateDto).State = EntityState.Detached;
            entity = EntityState.Detached;
            entity = default;

            return Response.Ok();
        }
        catch (Exception ex)
        {
            _db.Entry(UpdateDto).State = EntityState.Detached;
            return Response.Error(ex?.InnerException?.Message);
        }
    }

    public async Task<(int AffectedRows, Response respnse)> RemoveByIdAsync<T>(string id) where T : class
    {
        try
        {
            var result = await GetByIdAsync<T>(id);

            if (result.response != Response.Ok()) return default;
            _db.Set<T>().Remove(result.entity);

            var rest = await SaveAsync();
            return (rest, Response.Ok());
        }
        catch (Exception ex)
        {
            return (default, Response.Error(ex.Message));
        }
    }

    public async Task<Response> RemoveAsync<T>(T entity) where T : class
    {
        try
        {
            _db.Set<T>().Remove(entity);
            var rest = await SaveAsync();
            return Response.Ok();
        }
        catch (Exception ex)
        {
            return Response.Error(ex?.InnerException?.Message);
        }
    }
}