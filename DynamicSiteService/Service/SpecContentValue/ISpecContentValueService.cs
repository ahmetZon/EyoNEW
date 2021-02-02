using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;


public interface ISpecContentValueService : IGenericRepo<SpecContentValue>
{
    RModel<SpecContentValue> InsertOrUpdate(SpecContentValue model);

}

