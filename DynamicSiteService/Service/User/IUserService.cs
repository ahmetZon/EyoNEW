using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

using System;



public interface IUserService : IGenericRepo<User>
{
    RModel<UserModel> InsertOrUpdate(User model);

}

