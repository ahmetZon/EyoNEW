using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CMSSite.Components
{
    public class MenuView : ViewComponent
    {
        IHttpContextAccessor _httpContextAccessor;
        IContentPageService _IContentPageService;
        IDocumentsService _IDocumentsService;
        public MenuView(
            IContentPageService _IContentPageService,
            IDocumentsService _IDocumentsService,
        IHttpContextAccessor httpContextAccessor
            )
        {
            this._IContentPageService = _IContentPageService;
            this._httpContextAccessor = httpContextAccessor;
            this._IDocumentsService = _IDocumentsService;
        }

        public IViewComponentResult Invoke(string type)
        {
            #region dynamicContent 
            var link = HttpContext.Request.Path.Value.Trim().Trim('/');
            var content = _IContentPageService.Where(o => o.Link.ToLower() == link.ToLower() , true, false, o => o.Gallery, o => o.Documents, o => o.ThumbImage, o => o.Picture, o => o.BannerImage).Result.FirstOrDefault();
            int langID = 1;
            if (content != null && _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID") != content.LangId)
            {
                _httpContextAccessor.HttpContext.Session.SetInt32("LanguageID", content.LangId);
                langID = content.LangId;
            }

            List<ContentPage> contentPages = new List<ContentPage>();
            if (_httpContextAccessor.HttpContext.Session.Get("contentPages") == null)
            {
                contentPages = _IContentPageService.Where(x => x.LangId == content.LangId, true, false).Result.ToList();
                _httpContextAccessor.HttpContext.Session.Set("contentPages", contentPages);
            }
            else
            { 
                contentPages =
                _httpContextAccessor.HttpContext.Session.Get<List<ContentPage>>("contentPages");

                if(content!=null&&content.LangId!=contentPages.FirstOrDefault().LangId)
                {
                    contentPages = _IContentPageService.Where(o => o.LangId == content.LangId, true, false, o => o.ContentPageChilds, o => o.Parent, o => o.Gallery, o => o.Documents, o => o.ThumbImage, o => o.Picture, o => o.BannerImage).Result.ToList();

                }

                _httpContextAccessor.HttpContext.Session.Set("contentPages", contentPages);
            }
 
            ViewBag.contentPages = contentPages;
            ViewBag.content = content;
            ViewBag.LanguageID = _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID");
            if (type == "SideMenu")
            {
                int parentID = content.ContentPageId ?? 0;
                ContentPage parentContent = new ContentPage();
                if (parentID != 0)
                {
                    while (contentPages.Any(p => p.Id == parentID))
                    {
                        parentContent = contentPages.FirstOrDefault(p => p.Id == parentID);

                        if (parentContent.ContentPageId == null)
                        {
                            parentID = parentContent.Id;
                            break;
                        }
                        else
                        {
                            parentID = parentContent.ContentPageId ?? 0;
                        }
                    }
                    ViewBag.SideMenu = contentPages.Where(x => x.ContentPageId == parentID && x.IsSideMenu == true && x.LangId == _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID")).OrderBy(o => o.ContentOrderNo).ToList();
                    List<ContentPage> sideMenus = new List<ContentPage>();
                    List<ContentPage> tempList = contentPages.Where(x => x.ContentPageId == parentID).OrderBy(o=>o.ContentOrderNo).ToList();
                    while (true)
                    {
                        foreach (var item in tempList)
                        {
                            if (item.IsSideMenu == true)
                            {
                                sideMenus.Add(item);
                            }
                        }
                        tempList = contentPages.Where(x => x.IsDeleted == null && x.LangId == content.LangId && tempList.Select(x => x.Id).ToList().Contains(x.ContentPageId ?? 0)).OrderBy(o => o.ContentOrderNo).ThenBy(m=>m.Id).ToList();
                        if (tempList.SelectMany(x => x.ContentPageChilds).Count() == 0)
                        {
                            foreach (var item in tempList)
                            {
                                if (item.IsSideMenu == true)
                                    sideMenus.Add(item);
                            }
                            break;
                        }
                    }
                    if (sideMenus.Any(x => x.Id == (x.LangId == 1 ? 307 : 593)))
                    {
                        ContentPage itemTah = sideMenus.FirstOrDefault(x => x.Id == (x.LangId == 1 ? 307 : 593));
                        sideMenus.Remove(itemTah);
                        sideMenus.Add(itemTah);
                        //sideMenus = Helpers.Swap(sideMenus, 18, 7).ToList();
                    }
                    ViewBag.SideMenu = sideMenus;
                }
                else
                {
                    ViewBag.SideMenu = content;
                }
            }
            #endregion
            return View(type);
        }
    }
}