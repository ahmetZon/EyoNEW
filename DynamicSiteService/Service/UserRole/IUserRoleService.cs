using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

using System;


public interface IUserRoleService : IGenericRepo<UserRole>
{
    RModel<UserRole> InsertOrUpdate(UserRole model);

}

