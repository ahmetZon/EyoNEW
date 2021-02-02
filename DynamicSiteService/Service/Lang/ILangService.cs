using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;


using System;



public interface ILangService : IGenericRepo<Lang>
{
    RModel<Lang> InsertOrUpdate(Lang model);

}

