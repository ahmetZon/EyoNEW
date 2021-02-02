using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


public class GenericRepo<C, T> : IGenericRepo<T> where T : class, IBaseModel where C : DbContext, new()
{

    private C _context = new C();
    public C Context
    {

        get { return _context; }
        set { _context = value; }
    }


    protected IBaseSession sessionInfo;

    public GenericRepo(C _context, IBaseSession sessionInfo)
    {
        this.sessionInfo = sessionInfo;
        this._context = _context;
    }

    public DTResult<T> GetPaging(
        Expression<Func<T, bool>> filter = null
       , bool AsNoTracking = true
       , DTParameters<T> param = null
       , bool IsDeletedShow = false
       , params Expression<Func<T, object>>[] includes
       )
    {
        var query = Where(filter, AsNoTracking, IsDeletedShow, includes).Result;

        var GlobalSearchFilteredData = query.ToGlobalSearchInAllColumn<T>(param);
        var IndividualColSearchFilteredData = GlobalSearchFilteredData.ToIndividualColumnSearch(param);
        var SortedFilteredData = IndividualColSearchFilteredData.ToSorting(param);
        var SortedData = SortedFilteredData.ToPagination(param);

        var rSortedData = SortedData.ToList();

        int Count = query.Count();

        var resultData = new DTResult<T>
        {
            draw = param.Draw,
            data = rSortedData,
            recordsFiltered = Count,
            recordsTotal = Count
        };

        return resultData;

    }

    public RModel<T> Where(
        Expression<Func<T, bool>> filter = null
        , bool AsNoTracking = true
        , bool IsDeletedShow = false
        , params Expression<Func<T, object>>[] includes
        )
    {
        RModel<T> res = new RModel<T>();
        res.ResultType = new ResultType();
        res.ResultType.MessageList = new List<string>();
        try
        {
            var query = _context.Set<T>() as IQueryable<T>;

            //query = query.Where(o=>o.LangId == sessionInfo._BaseModel.LangId);

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (IsDeletedShow)
                query = query.IgnoreQueryFilters(); //Disable global query filters

            if (filter != null)
                query = query.Where(filter);

            if (includes != null && includes.Count() > 0)
            {
                if (IsDeletedShow)
                {
                    if (AsNoTracking)
                        query = includes.Aggregate(query, (current, include) => current.AsNoTracking().Include(include).IgnoreQueryFilters());
                    else
                        query = includes.Aggregate(query, (current, include) => current.Include(include).IgnoreQueryFilters());
                }
                else
                {
                    if (AsNoTracking)
                        query = includes.Aggregate(query, (current, include) => current.AsNoTracking().Include(include));
                    else
                        query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

            }


            res.Result = query;
            res.ResultType.RType = RType.OK;
        }
        catch (Exception ex)
        {
            res.ResultType.RType = RType.Error;
            res.ResultType.MessageList.Add(ex.Message);
        }
        //catch (DbEntityValidationException e)
        //{
        //    res._DbEntityValidationException = e;
        //    res.ResultType = RType.Error;
        //}
        return res;
    }


    public T Find(int id, bool AsNoTracking = false, bool IsDeletedShow = false)
    {
        var query = Where(o => o.Id == id, AsNoTracking, IsDeletedShow).Result.FirstOrDefault();
        return query;
    }


    public T Add(T t)
    {
        t.CreaUser = sessionInfo._BaseModel.Id;
        t.CreaDate = DateTime.Now;
        _context.Set<T>().Add(t);
        return t;
    }
    public List<T> AddBulk(List<T> tList)
    {
        //_context.Configuration.AutoDetectChangesEnabled = false;
        tList.ForEach(t =>
        {
            _context.Entry(t).State = EntityState.Added;
            t.CreaUser = sessionInfo._BaseModel.Id;
            t.CreaDate = DateTime.Now;
        });
        _context.Set<T>().AddRange(tList);
        //_context.Configuration.AutoDetectChangesEnabled = true;
        _context.ChangeTracker.DetectChanges();
        return tList;
    }
    public T Delete(int id)
    {
        var t = Find(id);
        if (t != null)
        {
            t.IsDeleted = DateTime.Now;
            Update(t);
        }
        return t;
    }
    public T Delete(T t)
    {
        t.IsDeleted = DateTime.Now;
        return Update(t);
    }
    public List<T> DeleteBulk(List<T> tList)
    {
        return UpdateBulk(tList, DateTime.Now);
    }
    public T Update(T t)
    {
        var dbEntityEntry = _context.Entry(t);
        dbEntityEntry.State = EntityState.Modified;
        dbEntityEntry.Property(o => o.CreaDate).IsModified = false;
        t.ModUser = sessionInfo._BaseModel.Id;
        t.ModDate = DateTime.Now;

        return t;
    }
    public List<T> UpdateBulk(List<T> tList)
    {
        return UpdateBulk(tList, null);
    }
    List<T> UpdateBulk(List<T> tList, DateTime? IsDeleted)
    {
        //_context.Configuration.AutoDetectChangesEnabled = false;
        tList.ForEach(t =>
        {
            if (IsDeleted != null)
                t.IsDeleted = DateTime.Now;
            Update(t);
        });
        //_context.Configuration.AutoDetectChangesEnabled = true;
        _context.ChangeTracker.DetectChanges();
        return tList;
    }
    public int SaveChanges()
    {
        var res = 1;
        try
        {
            res = _context.SaveChanges();
        }
        catch (Exception ex)
        {
            res = 0;
            throw ex;
        }
        //catch (DbEntityValidationException e)
        //{
        //    res._DbEntityValidationException = e;
        //    res.ResultType = RType.Error;
        //}
        return res;
    }
    private bool disposed = false;
    protected void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            this.disposed = true;
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}


