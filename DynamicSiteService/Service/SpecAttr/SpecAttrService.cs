using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;


public class SpecAttrService : GenericRepo<CMSDBContext, SpecAttr>, ISpecAttrService
{


    public SpecAttrService(CMSDBContext context, IBaseSession sessionInfo) : base(context, sessionInfo)
    {
    }
    public RModel<SpecAttr> InsertOrUpdate(SpecAttr model)
    {
        RModel<SpecAttr> res = new RModel<SpecAttr>();
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

