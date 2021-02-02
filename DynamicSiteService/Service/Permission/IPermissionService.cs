using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;


using System;



public interface IPermissionService : IGenericRepo<Permission>
{
    RModel<Permission> InsertOrUpdate(Permission model);

}

