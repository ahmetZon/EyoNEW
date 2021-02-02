using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;


public interface IFormTypeService : IGenericRepo<FormType>
{
    RModel<FormType> InsertOrUpdate(FormType model);

}

