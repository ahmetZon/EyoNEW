using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace CMS.Controllers
{
    public class ContentPageController : Controller
    {
#pragma warning disable CS0618 // 'IHostingEnvironment' artık kullanılmıyor: 'This type is obsolete and will be removed in a future version. The recommended alternative is Microsoft.AspNetCore.Hosting.IWebHostEnvironment.'
        IHostingEnvironment _IHostingEnvironment;
#pragma warning restore CS0618 // 'IHostingEnvironment' artık kullanılmıyor: 'This type is obsolete and will be removed in a future version. The recommended alternative is Microsoft.AspNetCore.Hosting.IWebHostEnvironment.'
        IContentPageService _IContentPageService;
        IDocumentsService _IDocumentsService;

#pragma warning disable CS0618 // 'IHostingEnvironment' artık kullanılmıyor: 'This type is obsolete and will be removed in a future version. The recommended alternative is Microsoft.AspNetCore.Hosting.IWebHostEnvironment.'
        public ContentPageController(IHostingEnvironment _IHostingEnvironment, IContentPageService _IContentPageService, IDocumentsService _IDocumentsService)
#pragma warning restore CS0618 // 'IHostingEnvironment' artık kullanılmıyor: 'This type is obsolete and will be removed in a future version. The recommended alternative is Microsoft.AspNetCore.Hosting.IWebHostEnvironment.'
        {
            this._IContentPageService = _IContentPageService;
            this._IDocumentsService = _IDocumentsService;
            this._IHostingEnvironment = _IHostingEnvironment;

        }


        [HttpPost]
        public IActionResult UpdateOrder(List<OrderUpdateModel> postModel)
        {
            var rows = _IContentPageService.Where().Result.ToList();
            postModel.ForEach(o =>
            {
                var row = rows.FirstOrDefault(r => r.Id == o.Id);
                if (row != null)
                {
                    row.OrderNo = o.OrderNo;
                    _IContentPageService.Update(row);
                }
            });
            _IContentPageService.SaveChanges();
            return Json("ok");
        }


        [HttpPost]
        public JsonResult SaveSingleImage(Documents postmodel)
        {
            try
            {
                var result = _IDocumentsService.InsertOrUpdate(postmodel);
                return Json(result.ResultRow);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [HttpPost]
        public JsonResult SaveMultiDoc(List<Documents> DocList)
        {
            try
            {
                var isErr = false;
                List<string> errMsg = new List<string>();
                DocList.ForEach(o =>
                {
                    RModel<Documents> result = _IDocumentsService.InsertOrUpdate(o);
                    if(result.ResultType.RType == RType.Warning)
                    {
                        isErr = true;
                        errMsg = result.ResultType.MessageList;
                     
                    }
                });
                if(isErr)
                {
                    return Json("Err-duplicate");
                }
                else
                {
                    return Json(DocList);
                }
               
            }
            catch (Exception ex)
            {
                return Json("Err-try");
                throw ex;
            }

        }

        [HttpPost]
        public JsonResult GetPaging(DTParameters<ContentPage> param, int ContentTypesId)
        {
            var result = _IContentPageService.GetPaging(o => o.ContentTypesId == ContentTypesId, true, param, false, o => o.Lang, o => o.Documents, o => o.ContentPageChilds, o => o.Parent);
            result.data = result.data.OrderBy(o=>o.OrderNo).ToList();
            return Json(result);
        }

        //public ActionResult GetPaging([DataSourceRequest] DataSourceRequest request, int selecttype)
        //{
        //    var orders = _IContentPageService.Where(o => o.ContentTypesId == selecttype, true, false, o => o.ContentTypes).Result;
        //    orders = orders.ApplyOrdersFiltering(request.Filters);
        //    var total = orders.Count();
        //    orders = orders.ApplyOrdersSorting(request.Groups, request.Sorts);
        //    if (!request.Sorts.Any() && !request.Groups.Any())
        //        orders = orders.OrderBy(o => o.Id);
        //    orders = orders.ApplyOrdersPaging(request.Page, request.PageSize);
        //    var data = orders.ApplyOrdersGrouping(request.Groups);
        //    var result = new DataSourceResult()
        //    {
        //        Data = data,
        //        Total = total
        //    };
        //    return Json(result);
        //}

        public ActionResult GetGallery([DataSourceRequest] DataSourceRequest request, int id)
        {
            var orders = _IDocumentsService.Where(o => o.GalleryId == id).Result;
            orders = orders.ApplyOrdersFiltering(request.Filters);
            var total = orders.Count();
            orders = orders.ApplyOrdersSorting(request.Groups, request.Sorts);
            if (!request.Sorts.Any() && !request.Groups.Any())
                orders = orders.OrderByDescending(o => o.CreaDate);
            orders = orders.ApplyOrdersPaging(request.Page, request.PageSize);
            var data = orders.ApplyOrdersGrouping(request.Groups);
            var result = new DataSourceResult()
            {
                Data = data,
                Total = total
            };
            return Json(result);
        }

        public ActionResult GetDocuments([DataSourceRequest] DataSourceRequest request, int id)
        {
            var orders = _IDocumentsService.Where(o => o.DocumentId == id).Result;
            orders = orders.ApplyOrdersFiltering(request.Filters);
            var total = orders.Count();
            orders = orders.ApplyOrdersSorting(request.Groups, request.Sorts);
            if (!request.Sorts.Any() && !request.Groups.Any())
                orders = orders.OrderByDescending(o => o.CreaDate);
            orders = orders.ApplyOrdersPaging(request.Page, request.PageSize);
            var data = orders.ApplyOrdersGrouping(request.Groups);
            var result = new DataSourceResult()
            {
                Data = data,
                Total = total
            };
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetSelect()
        {
            var result = _IContentPageService.Where(null, true, false, o => o.ContentTypes).Result.Select(o => new { value = o.Id, text = o.Name }).ToList();
            return Json(result);
        }

        //public JsonResult GetContentType()
        //{
        //    var list = Enum.GetValues(typeof(ContentType)).Cast<int>().Select(x => new { name = ((ContentType)x).ToStr(), value = x.ToString(), text = ((ContentType)x).ExGetDescription() }).ToArray();
        //    return Json(list);
        //}

        public JsonResult GetTemplateType()
        {
            var list = Enum.GetValues(typeof(TemplateType)).Cast<int>().Select(x => new { name = ((TemplateType)x).ToStr(), value = x.ToString(), text = ((TemplateType)x).ExGetDescription() }).ToArray();
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetContentPage()
        {
            var result = _IContentPageService.Where(null, true, false, o => o.ContentTypes).Result.Select(o => new { value = o.Id, text = o.Name }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult InsertOrUpdate(ContentPage postmodel)
        {
            try
            {
                postmodel.Description = HttpUtility.HtmlDecode(postmodel.Description);
                postmodel.ContentData = HttpUtility.HtmlDecode(postmodel.ContentData);
                postmodel.ContentShort = HttpUtility.HtmlDecode(postmodel.ContentShort);
                var result = _IContentPageService.InsertOrUpdate(postmodel);
                return Json(result.ResultRow);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public JsonResult ImportDocumentsSingle()
        {
            try
            {
                var aa = HttpContext.Request.Form["postmodel"];
                var bb = aa[0].Replace("[", "").Replace("]", "");
                var idValue = bb.Deserialize<TextValue>().value.ToInt();
                var text = bb.Deserialize<TextValue>().text.ToStr();

                var row = new Documents();

                switch (text)
                {
                    case "ThumbImage":
                        {
                            row = _IDocumentsService.Where(o => o.ThumbImageId == idValue, false, true).Result.FirstOrDefault();
                            if (row == null) { row = new Documents(); row.Guid = Guid.NewGuid().ToString(); }
                            else
                            {
                                row.IsDeleted = null;
                            }
                            row.ThumbImageId = idValue;
                            break;
                        }
                    case "BannerImage":
                        {
                            row = _IDocumentsService.Where(o => o.BannerImageId == idValue, false, true).Result.FirstOrDefault();
                            if (row == null) { row = new Documents(); row.Guid = Guid.NewGuid().ToString(); }
                            else
                            {
                                row.IsDeleted = null;
                            }
                            row.BannerImageId = idValue;
                            break;
                        }
                    case "Picture":
                        {
                            row = _IDocumentsService.Where(o => o.PictureId == idValue, false, true).Result.FirstOrDefault();

                            if (row == null) { row = new Documents(); row.Guid = Guid.NewGuid().ToString(); }
                            else
                            {
                                row.IsDeleted = null;
                            }
                            row.PictureId = idValue;
                            break;
                        }

                }



                var files = HttpContext.Request.Form.Files.FirstOrDefault();
                string filename = ContentDispositionHeaderValue.Parse(files.ContentDisposition).FileName.ToString().Trim('"');
                row.Name = filename;
                var newfilename = Guid.NewGuid().ToString() + "." + filename.Split('.').LastOrDefault();
                var path = this.GetPathAndFilename(newfilename);
                using (FileStream output = System.IO.File.Create(path)) files.CopyTo(output);

                row.Link = newfilename;
                row.Types = "ContentPage";

                //if (row != null)
                //{
                //    if (System.IO.File.Exists(path + row.Link))
                //        System.IO.File.Delete(path + row.Link);

                //}
                //else
                //{

                //}
                var rs = _IDocumentsService.InsertOrUpdate(row);
                return Json(row);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public JsonResult ImportDocuments()
        {
            try
            {
                var aa = HttpContext.Request.Form["postmodel"];
                var bb = aa[0].Replace("[", "").Replace("]", "");
                var postModel = bb.Deserialize<ContentPage>();

                var files = HttpContext.Request.Form.Files.ToList();
                var Documents = new List<Documents>();
                var guid = Guid.NewGuid().ToString();

                var gallleryOrdocumentName = postModel.Name == "Gallery" ? "Gallery" : "Document";

                files.ForEach(source =>
                {
                    string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.ToString().Trim('"');
                    var orjinalFileName = filename;
                    filename = Guid.NewGuid().ToString() + "." + filename.Split('.').LastOrDefault();
                    var path = this.GetPathAndFilename(filename);
                    using (FileStream output = System.IO.File.Create(path))
                        source.CopyTo(output);

                    var row = new Documents
                    {
                        DocumentId = gallleryOrdocumentName == "Document" ? postModel?.Id : null,
                        GalleryId = gallleryOrdocumentName == "Gallery" ? postModel?.Id : null,
                        Link = filename,
                        Guid = guid,
                        Name = orjinalFileName,
                        Types = "ContentPage"
                    };
                    _IDocumentsService.InsertOrUpdate(row);
                    Documents.Add(row);
                    _IDocumentsService.SaveChanges();
                });

                return Json(Documents);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public JsonResult DeleteImage(int id)
        {
            var result = _IDocumentsService.Where(o => o.Types == "ContentPage" && o.Id == id).Result.FirstOrDefault();

            var path = this.GetPathAndFilename(result.Link);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _IDocumentsService.Delete(result);
            var res = _IDocumentsService.SaveChanges();
            return Json(result.Id);
        }

        public JsonResult DeleteImageAll(int id)
        {
            var resultList = _IDocumentsService.Where(o => o.Types == "ContentPage" && o.DocumentId == id).Result.ToList();

            resultList.ForEach(result =>
            {

                var path = this.GetPathAndFilename(result.Link);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                _IDocumentsService.Delete(result);
                var res = _IDocumentsService.SaveChanges();
            });

            return Json("ok");
        }


        public ContentPage Get(int id)
        {
            var result = _IContentPageService.Where(o => o.Id == id, true, false,
                o => o.ContentTypes, o => o.Documents, o => o.ThumbImage, o => o.Picture, o => o.BannerImage, o => o.Gallery)
                .Result.FirstOrDefault();
            if (result != null)
            {
                var dc = _IDocumentsService.Where().Result.ToList();
                result.Documents = dc.Where(o => o.DocumentId == result.Id).ToList();
                result.Gallery = dc.Where(o => o.GalleryId == result.Id).ToList();
            }
            return (result);
        }

        public JsonResult Delete(int id)
        {
            var result = _IContentPageService.Delete(id);
            _IContentPageService.SaveChanges();

            DeleteImageAll(id);

            return Json(result);
        }

        [AcceptVerbs("Post")]
        public ActionResult EditingInline_Destroy([DataSourceRequest] DataSourceRequest request, ContentPage postModel)
        {
            if (postModel != null)
            {
                var result = _IContentPageService.Delete(postModel);
                _IContentPageService.SaveChanges();

            }

            return Json(new[] { postModel }.ToDataSourceResult(request, ModelState));
        }

        public IActionResult Index()
        {
            //var selecttype = HttpContext.Request.Query["selecttype"].ToInt();
            //var rs = _IContentPageService.Where(o => o.ContentTypesId == selecttype, true, false,
            //    o => o.Lang, o => o.Documents, o => o.ContentPageChilds, o => o.Parent);
            //rs.Result = rs.Result.OrderByDescending(o => o.CreaDate).ThenByDescending(o => o.Name);
            //var result = rs.Result.ToList();
            //ViewBag.list = result;
            return View();
        }

        public IActionResult InsertOrUpdatePage()
        {
            var row = Get(Request.Query["id"].ToInt());
            ViewBag.postModel = row;
            return View();
        }




        private string GetPathAndFilename(string filename)
        {
            string path = this._IHostingEnvironment.WebRootPath + "\\uploads\\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path + filename;
        }

    }
}
