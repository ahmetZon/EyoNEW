using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;


public class SpecContentTypeService : GenericRepo<CMSDBContext, SpecContentType>, ISpecContentTypeService
{


    public SpecContentTypeService(CMSDBContext context, IBaseSession sessionInfo) : base(context, sessionInfo)
    {
    }
    public RModel<SpecContentType> InsertOrUpdate(SpecContentType model)
    {
        RModel<SpecContentType> res = new RModel<SpecContentType>();
        res.ResultType = new ResultType();
        res.ResultType.MessageList = new List<string>();

        //Duplicate Control
        var modelControl = Where(o => o.Id != model.Id && o.SpecId == model.SpecId && o.ContentTypesId == model.ContentTypesId, false).Result.FirstOrDefault();
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

