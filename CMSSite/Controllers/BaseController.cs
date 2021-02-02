using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Text.RegularExpressions;

namespace CMSSite.Controllers
{
    public class BaseController : Controller
    {
        IHttpContextAccessor _httpContextAccessor;
        IHostingEnvironment _IHostingEnvironment;
        IContentPageService _IContentPageService;
        IDocumentsService _IDocumentsService;
        ISiteConfigService _ISiteConfigService;
        IUserService _IUserService;
        

        public BaseController( 
            IHostingEnvironment _IHostingEnvironment,
          IContentPageService _IContentPageService,
        IDocumentsService _IDocumentsService,
        ISiteConfigService _ISiteConfigService,
        IHttpContextAccessor _httpContextAccessor,
         IUserService _IUserService
            )
        { 
            this._IHostingEnvironment = _IHostingEnvironment;
            this._ISiteConfigService = _ISiteConfigService;
            this._IDocumentsService = _IDocumentsService;
            this._IContentPageService = _IContentPageService;
            this._httpContextAccessor = _httpContextAccessor;
            this._IUserService = _IUserService;

        }
        public IActionResult BaseContent()
        {
            var link = HttpContext.Items["cmspage"].ToString();
            var config = _ISiteConfigService.Where().Result.FirstOrDefault();
            _httpContextAccessor.HttpContext.Session.Set("config", config);

            if (!string.IsNullOrEmpty(link))
            {
                var menu = _IContentPageService.Where(o => o.Link == link, true, false, o => o.Documents).Result.FirstOrDefault();
                if (menu != null)
                { 
                    ViewBag.content = menu;
                    return View();
                }
                else
                {
                    return Redirect(SessionRequest.config.BaseUrl);
                }
            }
            else
            {
                return Redirect(SessionRequest.config.BaseUrl);
            }
        }

        public IActionResult Index()
        {

            setData();
            return View();
        }
        public IActionResult Content()
        {
            var link = HttpContext.Items["cmspage"].ToString();
            var config = _ISiteConfigService.Where().Result.FirstOrDefault();
            _httpContextAccessor.HttpContext.Session.Set("config", config);
            if (!string.IsNullOrEmpty(link))
            {
                var menu = _IContentPageService.Where(o => o.Link == link, true, false, o => o.Documents, o => o.TemplateType).Result.FirstOrDefault();
                if (menu != null)
                {
                    _httpContextAccessor.HttpContext.Session.SetInt32("LanguageID", menu.LangId);
                    ViewBag.LanguageID = menu.LangId;
                    ViewBag.page = menu;
                    return View();
                }
                else
                {
                    int langID = 0;
                    if (_httpContextAccessor.HttpContext.Session.GetString("LanguageID") != null)
                    {
                        langID = _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID") ?? 1;
                    }
                    if (_httpContextAccessor.HttpContext.Session.GetInt32("LanguageID") == null || langID == 0)
                    {
                        _httpContextAccessor.HttpContext.Session.SetInt32("LanguageID", 1);
                    }
                    ViewBag.LanguageID = _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID");
                    return Redirect(SessionRequest.config.BaseUrl);
                }
            }
            else
            {
                return Redirect(SessionRequest.config.BaseUrl);
            }
        }
        public IActionResult SideContent(string link)
        {
            getPageData(link);
            return View();
        }
        private List<ContentPage> getPageData(string link)
        {
            var fullLink = HttpContext.Request.Path.Value.Trim().ToStr();
            int langID = 0;
            if (_httpContextAccessor.HttpContext.Session.GetString("LanguageID") != null)
            {
                langID = _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID") ?? 1;
            }
            if (_httpContextAccessor.HttpContext.Session.GetInt32("LanguageID") == null || langID == 0)
            {
                langID = 1;
                _httpContextAccessor.HttpContext.Session.SetInt32("LanguageID", 1);
            }

            var contentPages = _IContentPageService.Where(o => o.LangId == langID, true, false).Result.ToList();

            ViewBag.contentPages = contentPages.ToList();
            ViewBag.Pages = contentPages.ToList();
            var config = _ISiteConfigService.Where().Result.FirstOrDefault();
            _httpContextAccessor.HttpContext.Session.Set("config", config);
            if (!string.IsNullOrEmpty(link))
            {
                var menu = _IContentPageService.Where(o => o.Link == link, true, false).Result.FirstOrDefault();
                if (menu != null)
                {
                    _httpContextAccessor.HttpContext.Session.SetInt32("LanguageID", menu.LangId);
                    ViewBag.page = menu;
                }
                else
                {
                    _httpContextAccessor.HttpContext.Session.SetInt32("LanguageID", 1);
                    Redirect(SessionRequest.config.BaseUrl);
                }
                ViewBag.LanguageID = _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID");
            }
            else
            {
                Redirect(SessionRequest.config.BaseUrl);
            }
            return contentPages;
        }
        public IActionResult Header()
        {
            #region dynamicContent
            int langID = 0;
            if (_httpContextAccessor.HttpContext.Session.GetString("LanguageID") != null)
            {
                langID = _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID") ?? 1;
            }
            if (_httpContextAccessor.HttpContext.Session.GetInt32("LanguageID") == null || langID == 0)
            {
                langID = 1;
                _httpContextAccessor.HttpContext.Session.SetInt32("LanguageID", 1);
            }
            var contentPages = _IContentPageService.Where(o => o.LangId == langID, true, false, o => o.ContentPageChilds, o => o.Documents).Result.ToList();
            contentPages.ForEach(o =>
            {
                o.ContentPageChilds = contentPages.Where(oo => oo.ContentPageId == o.Id).ToList();
            });
            ViewBag.contentPages = contentPages;
            //   ViewBag.fikirler = _IFikirService.Where(o => o.FikirStatus != FikirDurumu.Ondegerlendirme,
            //      true, false,
            //      o => o.AtananDepartman, o => o.AtananKullanici, o => o.FikirBegen).Result.ToList();
            var config = _ISiteConfigService.Where().Result.FirstOrDefault();
            _httpContextAccessor.HttpContext.Session.Set("config", config);

            var _user = _IUserService.Where(o => (o.Name == "admin"), true, false).Result.FirstOrDefault();

            _httpContextAccessor.HttpContext.Session.Set("_user", _user);

            #endregion
            return View();
        }
        public IActionResult Footer()
        {
            return View();
        }
        public void setData()
        {
            #region dynamicContent
            var link = HttpContext.Request.Path.Value.Trim().ToStr();
            List<ContentPage> contentPages = new List<ContentPage>();
        
            int langID = 0;
            if (_httpContextAccessor.HttpContext.Session.GetInt32("LanguageID") != null)
            {
                langID = _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID") ?? 1;
            }

            if (_httpContextAccessor.HttpContext.Session.GetInt32("LanguageID") == null || langID == 0)
            {
                langID = 1;
                _httpContextAccessor.HttpContext.Session.SetInt32("LanguageID", 1);
            }

            var menu = _IContentPageService.Where(o => o.Link == link, true, false, null).Result.FirstOrDefault();
            if (menu != null)
            {
                _httpContextAccessor.HttpContext.Session.SetInt32("LanguageID", menu.LangId);
                ViewBag.LanguageID = _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID");
            }
            else
            {
                if (link == "/" || link == "/Base/Index")
                {
                    if (_httpContextAccessor.HttpContext.Session.GetInt32("LanguageID") != null)
                    {
                        ViewBag.LanguageID = _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID") ?? 1;
                    }
                    else
                    {
                        ViewBag.LanguageID = 1;
                    }

                }
                else
                {
                    _httpContextAccessor.HttpContext.Session.SetInt32("LanguageID", 1);
                    ViewBag.LanguageID = _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID");
                }
            }
            var currState = _httpContextAccessor.HttpContext.Session.GetString("currState"); 
            
            ViewBag.currState = currState;
            bool isBayi = false;
            bool isEndustri = false;
            bool isMimar = false;
            bool isBireysel = false;
            switch (currState)
            {
                case "Bayi":
                    isBayi = true;
                    break;
                case "Endustri":
                    isEndustri = true;
                    break;
                case "Mimar":
                    isMimar = true;
                    break;
                case "Bireysel":
                    isBireysel = true;
                    break;
                default:
                    isBayi = true;
                    break;
            }
         

            contentPages = _IContentPageService.Where(x => x.LangId == langID&& x.IsDeleted == null && (x.IsBayi == isBayi || x.IsEndustri == isEndustri || x.IsMimar == isMimar || x.IsBireysel == isBireysel) && x.IsInteral==true, true, false, o => o.ContentPageChilds, o => o.Parent, o => o.Gallery, o => o.Documents, o => o.ThumbImage, o => o.Picture, o => o.BannerImage).Result.ToList(); 
            _httpContextAccessor.HttpContext.Session.Set("contentPages", contentPages);
            //if (_httpContextAccessor.HttpContext.Session.Get("contentPages") == null)
            //{
            //    contentPages = _IContentPageService.Where(null, true, false, o => o.ContentPageChilds, o => o.Parent, o => o.Gallery, o => o.Documents, o => o.ThumbImage, o => o.Image, o => o.BannerImage).Result.ToList();
            //    _httpContextAccessor.HttpContext.Session.Set("contentPages", contentPages);
            //}
            //else
            //{
            //    contentPages =
            //    _httpContextAccessor.HttpContext.Session.Get<List<ContentPage>>("contentPages");
            //    _httpContextAccessor.HttpContext.Session.Set("contentPages", contentPages);
            //}
            // ViewBag.IsHeaderMenu = contentPages.Where(o => o.IsHeaderMenu == true).OrderBy(o => o.ContentOrderNo).ThenBy(o => o.Name).ToList();
            // ViewBag.IsFooterMenu = contentPages.Where(o => o.IsFooterMenu == true).OrderBy(o => o.ContentOrderNo).ThenBy(o => o.Name).ToList();
            ViewBag.contentPages = contentPages.ToList();
            ViewBag.Pages = contentPages.ToList();
            //   ViewBag.fikirler = _IFikirService.Where(o => o.FikirStatus != FikirDurumu.Ondegerlendirme,
            //      true, false,
            //      o => o.AtananDepartman, o => o.AtananKullanici, o => o.FikirBegen).Result.ToList();



            var config = _ISiteConfigService.Where().Result.FirstOrDefault();
            _httpContextAccessor.HttpContext.Session.Set("config", config);
            var _user = _IUserService.Where(o => (o.Name == "admin"), true, false).Result.FirstOrDefault();
            _httpContextAccessor.HttpContext.Session.Set("_user", _user);
            #endregion
        }
        public IActionResult GetTemplateType()
        {
            var list = Enum.GetValues(typeof(TemplateType)).Cast<int>().Select(x => new { value = x.ToString(), text = ((TemplateType)x).ExGetDescription() }).ToArray();
            return Json(list);
        }

        [Route("profil")]
        public IActionResult profil()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangeType(string ID)
        { 
            string currState = "Bayi";
            if (_httpContextAccessor.HttpContext.Session.GetString("currState") != null)
            {
                currState = _httpContextAccessor.HttpContext.Session.GetString("currState"); 
            }
            //_httpContextAccessor.HttpContext.Session.Set("contentPages", contentPages);   
            switch (ID)
            {
                case "Bayi":
                    currState = "Bayi";
                    break;
                case "Endustri":
                    currState = "Endustri";
                    break;
                case "Mimar":
                    currState = "Mimar";
                    break;
                case "Bireysel":
                    currState = "Bireysel";
                    break;
                default:
                    currState= "Bayi";
                    break;
            }
            ViewBag.currState = currState;
            _httpContextAccessor.HttpContext.Session.Set("currState", currState);
            return Json("");
        } 
        [HttpPost]
        public IActionResult ChangeLanguage(string LanguageID, string PageUrl, string PageData)
        {
            int SiteLanguage = Convert.ToInt32(LanguageID);
            _httpContextAccessor.HttpContext.Session.SetInt32("LanguageID", SiteLanguage);
            var contentPages = _IContentPageService.Where(x => x.LangId == SiteLanguage, true, false).Result.ToList();
            _httpContextAccessor.HttpContext.Session.Set("contentPages", contentPages);

            ContentPage OldPage = _IContentPageService.Where(x => x.IsDeleted == null && x.Link == PageUrl, true, false, null).Result.FirstOrDefault();
            if (OldPage != null)
            {
                int orgID = OldPage.OrjId ?? 0;
                ContentPage NewPage = _IContentPageService.Where(x => x.IsDeleted == null && x.LangId == SiteLanguage && x.OrjId == orgID, true, false, null).Result.FirstOrDefault();
                if (NewPage == null)
                {
                    return Json("/");
                }
                else
                {
                    return Json(NewPage.Link);
                }
            }
            else
            {
                return Json("");
            } 
            //_IContentPageService.Where(o => o.Link == link, true, false, null).Result.FirstOrDefault(); 
        }
 
      
    }
}
//web@cuhadaroglu.com