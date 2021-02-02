using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;


using System;



public class ServiceConfigService : GenericRepo<CMSDBContext,ServiceConfig>, IServiceConfigService
{


    public ServiceConfigService(CMSDBContext context, IBaseSession sessionInfo) : base(context, sessionInfo)
    {
    }
    public RModel<ServiceConfig> InsertOrUpdate(ServiceConfig model)
    {
        RModel<ServiceConfig> res = new RModel<ServiceConfig>();
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

