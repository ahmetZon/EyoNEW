using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;


public interface ISpecContentTypeService : IGenericRepo<SpecContentType>
{
    RModel<SpecContentType> InsertOrUpdate(SpecContentType model);

}

