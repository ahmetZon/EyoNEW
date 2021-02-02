using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;


public interface ISpecAttrService : IGenericRepo<SpecAttr>
{
    RModel<SpecAttr> InsertOrUpdate(SpecAttr model);

}

