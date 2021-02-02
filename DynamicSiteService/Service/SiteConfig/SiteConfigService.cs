using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;


using System;


public class SiteConfigService : GenericRepo<CMSDBContext, SiteConfig>, ISiteConfigService
{


    public SiteConfigService(CMSDBContext context, IBaseSession sessionInfo) : base(context, sessionInfo)
    {
    }
    public RModel<SiteConfig> InsertOrUpdate(SiteConfig model)
    {
        RModel<SiteConfig> res = new RModel<SiteConfig>();
        res.ResultType = new ResultType();
        res.ResultType.MessageList = new List<string>();

        //Duplicate Control
        //var modelControl = Where(o => o.Id != model.Id && o.Name == model.Name, false).Result.FirstOrDefault();
        //if (modelControl != null)
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
            var result = SaveChanges();
            res.ResultType.RType = RType.OK;
        }
        return res;
    }






}

