using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;


public interface ISpecService : IGenericRepo<Spec>
{
    RModel<Spec> InsertOrUpdate(Spec model);

}

