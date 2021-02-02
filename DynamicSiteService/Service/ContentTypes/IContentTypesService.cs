using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;


public interface IContentTypesService : IGenericRepo<ContentTypes>
{
    RModel<ContentTypes> InsertOrUpdate(ContentTypes model);

}

