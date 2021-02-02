using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;


public class DocumentsService : GenericRepo<CMSDBContext, Documents>, IDocumentsService
{


    public DocumentsService(CMSDBContext context, IBaseSession sessionInfo) : base(context, sessionInfo)
    {
    }
    public RModel<Documents> InsertOrUpdate(Documents model)
    {
        RModel<Documents> res = new RModel<Documents>();
        res.ResultType = new ResultType();
        res.ResultType.MessageList = new List<string>();

        //Duplicate Control
        //var modelControl = Where(o => o.Id != model.Id && o.Link == model.Link, false).Result.FirstOrDefault();
        var modelControl = Where(o => o.ThumbImageId == model.ThumbImageId && o.PictureId == model.PictureId && o.BannerImageId == model.BannerImageId, false).Result.FirstOrDefault();
        if (modelControl != null && modelControl.Link == model.Link)
        {
            res.ResultType.RType = RType.Warning;
            res.ResultType.MessageList.Add("Duplicate");
            res.ResultRow = modelControl;
        }
        else
        {
            if (modelControl != null && (model.ThumbImageId > 0 || model.PictureId > 0 || model.BannerImageId > 0))
            {
                modelControl.Link = model.Link;
                res.ResultRow = Update(modelControl);
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

