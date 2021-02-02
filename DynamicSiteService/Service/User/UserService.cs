using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

using System;



    public class UserService : GenericRepo<CMSDBContext, User>, IUserService
    {


        public UserService(CMSDBContext context, IBaseSession sessionInfo) : base(context, sessionInfo)
        {
        }
        public RModel<UserModel> InsertOrUpdate(User model)
        {
            RModel<UserModel> res = new RModel<UserModel>();
            res.ResultType = new ResultType();
            res.ResultType.MessageList = new List<string>();

            //Duplicate Control
            var modelControl = Where(o => o.Id != model.Id &&  o.UserName == model.UserName, false).Result.FirstOrDefault();
            if (modelControl != null)
            {
                res.ResultType.RType = RType.Warning;
                res.ResultType.MessageList.Add("Duplicate");
                res.ResultRow = modelControl as UserModel;
            }
            else
            {
                if (model.Id > 0)
                {
                    res.ResultRow = Update(model) as UserModel;
                }
                else
                {
                    res.ResultRow = Add(model) as UserModel;
                }
                SaveChanges();
                res.ResultType.RType = RType.OK;
            }
            return res;
        }






    }

