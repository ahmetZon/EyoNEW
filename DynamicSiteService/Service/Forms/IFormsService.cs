using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;


public interface IFormsService : IGenericRepo<Forms>
{
    RModel<Forms> InsertOrUpdate(Forms model);

}

