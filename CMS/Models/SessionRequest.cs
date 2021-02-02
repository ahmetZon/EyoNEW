

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;


public static class SessionRequest
{
    private static IHttpContextAccessor _IHttpContextAccessor;

    public static void Configure(IHttpContextAccessor __IHttpContextAccessor)
    {
        _IHttpContextAccessor = __IHttpContextAccessor;
    }

    public static HttpContext _HttpContext => _IHttpContextAccessor.HttpContext;

    public static string Title = "CMS";
    public static string StartPage = "Base";
    public static string StartAction = "Index";
    public static string version = DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(".", "").Replace(" ", "");
    public static string copyright = $"{DateTime.Now.Year} © Yazılım&Tasarım (Software&Design)  <a target='_blank' href='#'> by Hybrid</a>";
    public static string layoutID = "4";
    public static string layoutUrlBase = $"";
    public static string layoutUrl = $"";
    public static string logo = "~/img/logo.svg";
    public static string defaultImage = "~/img/default.png";

    public static string jokerPass = "123_*1";



    public static User _User
    {
        get
        {
            return _IHttpContextAccessor.HttpContext.Session.Get<User>("_user");
        }
        set { }
    }

    public static List<ContentTypes> ctypes
    {
        get
        {
            return _IHttpContextAccessor.HttpContext.Session.GetList<ContentTypes>("ctypes");
        }
        set { }
    }

    public static List<FormType> formtypes
    {
        get
        {
            return _IHttpContextAccessor.HttpContext.Session.GetList<FormType>("formtypes");
        }
        set { }
    }

    public static string Trans(this string keyword)
    {
        return keyword;
    }
    //public static Kullanici _User { get; set; }




}


