using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;


public class SpecContentValueService : GenericRepo<CMSDBContext, SpecContentValue>, ISpecContentValueService
{


    public SpecContentValueService(CMSDBContext context, IBaseSession sessionInfo) : base(context, sessionInfo)
    {
    }
    public RModel<SpecContentValue> InsertOrUpdate(SpecContentValue model)
    {
        RModel<SpecContentValue> res = new RModel<SpecContentValue>();
        res.ResultType = new ResultType();
        res.ResultType.MessageList = new List<string>();

        //Duplicate Control
        //var modelControl = Where(o => o.Id != model.Id &&  o.Link == model.Link, false).Result.FirstOrDefault();
        //if (modelControl != null)
        //{
        //    res.ResultType.RType = RType.Warning;
        //    res.ResultType.MessageList.Add("Duplicate");
        //    res.ResultRow = modelControl;
        //}
        if (false)
        {

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

