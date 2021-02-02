using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;


using System;



public class UserRoleService : GenericRepo<CMSDBContext,UserRole>, IUserRoleService
{


    public UserRoleService(CMSDBContext context, IBaseSession sessionInfo) : base(context, sessionInfo)
    {
    }
    public RModel<UserRole> InsertOrUpdate(UserRole model)
    {
        RModel<UserRole> res = new RModel<UserRole>();
        res.ResultType = new ResultType();
        res.ResultType.MessageList = new List<string>();

        //Duplicate Control
        var modelControl = Where(o => o.Id != model.Id && o.UserId == model.UserId && o.RoleId != model.RoleId, false).Result.FirstOrDefault();
        if (modelControl != null)
        {
            res.ResultType.RType = RType.Warning;
            res.ResultType.MessageList.Add("Duplicate");
            res.ResultRow = modelControl;
        }
        else
        {
            if (model.Id > 0)
            {
                res.ResultRow = Update(model);
            }
            else
            {
                res.ResultRow = Add(model);
            }
            SaveChanges();
            res.ResultType.RType = RType.OK;
        }
        return res;
    }






}

