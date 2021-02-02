
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;

public static class SessionRequest
{
    private static IHttpContextAccessor _IHttpContextAccessor;

    public static void Configure(IHttpContextAccessor __IHttpContextAccessor)
    {
        _IHttpContextAccessor = __IHttpContextAccessor;
    }

    public static HttpContext _HttpContext => _IHttpContextAccessor.HttpContext;

    public static User _User
    {
        get
        {
            return _IHttpContextAccessor.HttpContext.Session.Get<User>("_user");
        }
        set { }
    }

    public static SiteConfig config
    {
        get
        {
            return _IHttpContextAccessor.HttpContext.Session.Get<SiteConfig>("config");
        }
        set { }
    }
    public static int LanguageID
    {
        get
        {
            return _IHttpContextAccessor.HttpContext.Session.Get<int>("LanguageID");
        }
        set { }
    }

    public static string copyright = $"{DateTime.Now.Year} © Yazılım&Tasarım (Software&Design)  <a target='_blank' href='http://muratkale.com.tr'>by Murat KALE 0530 511 71 27</a>";


}




