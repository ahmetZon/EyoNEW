using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;


using System;



public class ServiceConfigAuthService : GenericRepo<CMSDBContext,ServiceConfigAuth>, IServiceConfigAuthService
{


    public ServiceConfigAuthService(CMSDBContext context, IBaseSession sessionInfo) : base(context, sessionInfo)
    {
    }
    public RModel<ServiceConfigAuth> InsertOrUpdate(ServiceConfigAuth model)
    {
        RModel<ServiceConfigAuth> res = new RModel<ServiceConfigAuth>();
        res.ResultType = new ResultType();
        res.ResultType.MessageList = new List<string>();

        //Duplicate Control
        //var modelControl = Where(o => o.Id != model.Id &&  o.ServiceName == model.Tc, false).Result.FirstOrDefault();
        if (false)
        {
            //res.ResultType.RType = RType.Warning;
            //res.ResultType.MessageList.Add("Duplicate");
            //res.ResultRow = modelControl;
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

