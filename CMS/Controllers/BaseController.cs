using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Controllers
{
    public class BaseController : Controller
    {
        ILangService _ILangService;
        IUserService _IUserService;
#pragma warning disable CS0618 // 'IHostingEnvironment' artık kullanılmıyor: 'This type is obsolete and will be removed in a future version. The recommended alternative is Microsoft.AspNetCore.Hosting.IWebHostEnvironment.'
        IHostingEnvironment _IHostingEnvironment;
#pragma warning restore CS0618 // 'IHostingEnvironment' artık kullanılmıyor: 'This type is obsolete and will be removed in a future version. The recommended alternative is Microsoft.AspNetCore.Hosting.IWebHostEnvironment.'
        IServiceConfigService _IServiceConfigService;
        IHttpContextAccessor _IHttpContextAccessor;

        public BaseController(
#pragma warning disable CS0618 // 'IHostingEnvironment' artık kullanılmıyor: 'This type is obsolete and will be removed in a future version. The recommended alternative is Microsoft.AspNetCore.Hosting.IWebHostEnvironment.'
            IHostingEnvironment _IHostingEnvironment,
#pragma warning restore CS0618 // 'IHostingEnvironment' artık kullanılmıyor: 'This type is obsolete and will be removed in a future version. The recommended alternative is Microsoft.AspNetCore.Hosting.IWebHostEnvironment.'
            IServiceConfigService _IServiceConfigService,
            IHttpContextAccessor _IHttpContextAccessor,
            ILangService _ILangService,
            IUserService _IUserService
            )
        {
            this._ILangService = _ILangService;
            this._IHostingEnvironment = _IHostingEnvironment;
            this._IServiceConfigService = _IServiceConfigService;
            this._IHttpContextAccessor = _IHttpContextAccessor;
            this._IUserService = _IUserService;

        }
  
    
        public IActionResult Index()
        {
            if (SessionRequest._User == null)
            {

                return RedirectToAction("Login1", "Login");
            }

            ViewBag.pageTitle = "Dashboard";




            var menus = _IServiceConfigService.Where().Result.ToList();

            var menuler = new List<string>();

            try
            {
                var filePath = _IHostingEnvironment.ContentRootPath + @"\Views";
                //menuler = Directory.EnumerateFiles(filePath, "*", SearchOption.AllDirectories).Select(o =>
                //o.Split("\\")[8].ToStr()

                //).Where(o =>
                //!o.ToStr().Contains("Base")
                //&& !o.ToStr().Contains("Shared")
                //&& !o.ToStr().Contains("Login")
                //&& !o.ToStr().Contains("_")
                //).Distinct().OrderBy(o => o).ToList();
            }
#pragma warning disable CS0168 // ex' değişkeni ifade edilir ancak hiçbir zaman kullanılmaz
            catch (Exception ex)
#pragma warning restore CS0168 // ex' değişkeni ifade edilir ancak hiçbir zaman kullanılmaz
            {
                //throw ex;
            }



            //var fark = menuler.Where(oo => !menus.Select(o => o.Name).Contains(oo)).ToList();

            //if (fark.Count > 0)
            //{

            //    fark.ForEach(o =>
            //    {
            //        menus.Add(new ServiceConfig()
            //        {
            //            Name = o,
            //            Description = o,
            //            Url = "/" + o,
            //            ServiceName = o
            //        });

            //    });
            //    _IServiceConfigService.AddBulk(menus);
            //    _IServiceConfigService.SaveChanges();

            //}

            _IHttpContextAccessor.HttpContext.Session.Set("menus", menus);

            return View();
        }

        public IActionResult Logout()
        {
            _IHttpContextAccessor.HttpContext.Session.Clear();
            return RedirectToAction("Login1", "Login");
        }

        public IActionResult Report()
        {
            return View();
        }


    }
}
