using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;


using System;



public interface ISiteConfigService : IGenericRepo<SiteConfig>
{
    RModel<SiteConfig> InsertOrUpdate(SiteConfig model);

}

