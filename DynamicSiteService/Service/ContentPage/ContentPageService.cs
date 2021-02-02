using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Web;

public class ContentPageService : GenericRepo<CMSDBContext, ContentPage>, IContentPageService
{


    public ContentPageService(CMSDBContext context, IBaseSession sessionInfo) : base(context, sessionInfo)
    {
    }
    public RModel<ContentPage> InsertOrUpdate(ContentPage model)
    {
        RModel<ContentPage> res = new RModel<ContentPage>();
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
        model.Description = HttpUtility.HtmlDecode(model.Description);
        model.ContentData = HttpUtility.HtmlDecode(model.ContentData);
        model.ContentShort = HttpUtility.HtmlDecode(model.ContentShort);
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

