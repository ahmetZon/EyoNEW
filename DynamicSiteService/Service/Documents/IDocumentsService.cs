using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;


public interface IDocumentsService : IGenericRepo<Documents>
{
    RModel<Documents> InsertOrUpdate(Documents model);

}

