
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


public interface IGenericRepo<T> where T : class
{
    DTResult<T> GetPaging(
        Expression<Func<T, bool>> filter = null
       , bool AsNoTracking = true
       , DTParameters<T> param = null
       , bool IsDeletedShow = false
       , params Expression<Func<T, object>>[] includes
       );
    RModel<T> Where(
          Expression<Func<T, bool>> filter = null
          , bool AsNoTracking = true
          , bool IsDeletedShow = false
          , params Expression<Func<T, object>>[] includes
          );
    T Find(int id, bool AsNoTracking = false, bool ShowIsDeleted = false);
    T Add(T t);
    List<T> AddBulk(List<T> tList);
    T Delete(int id);
    T Delete(T t);
    List<T> DeleteBulk(List<T> tList);
    T Update(T t);
    List<T> UpdateBulk(List<T> tList);
    int SaveChanges();
    void Dispose();

}
