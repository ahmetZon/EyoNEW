using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;


using System;



public interface IServiceConfigAuthService : IGenericRepo<ServiceConfigAuth>
{
    RModel<ServiceConfigAuth> InsertOrUpdate(ServiceConfigAuth model);

}

