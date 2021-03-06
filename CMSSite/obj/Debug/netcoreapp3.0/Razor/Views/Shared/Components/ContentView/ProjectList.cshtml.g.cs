#pragma checksum "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f94c2f34bcf937fdcd829cb869287a9f8fc48f9d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Components_ContentView_ProjectList), @"mvc.1.0.view", @"/Views/Shared/Components/ContentView/ProjectList.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\_ViewImports.cshtml"
using CMSSite;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f94c2f34bcf937fdcd829cb869287a9f8fc48f9d", @"/Views/Shared/Components/ContentView/ProjectList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f74a5ef60aca8ccb0793e472a7a82919353f59fa", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Components_ContentView_ProjectList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
  
    var content = ViewBag.content as ContentPage;
    var contents = ViewBag.contents as List<ContentPage>;
    var filters = ViewBag.filters as List<ContentPage>;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
Write(await Component.InvokeAsync("MenuView", new { type = "SubHeader" }));

#line default
#line hidden
#nullable disable
            WriteLiteral(" \r\n<div id=\"content\">\r\n    <div class=\"container\"> \r\n        <div class=\"spacer-single\"></div> \r\n        <div class=\"row\">\r\n            <div class=\"col-md-12\">\r\n                <ul id=\"filters\" class=\"wow fadeInUp\" data-wow-delay=\"0s\">\r\n");
#nullable restore
#line 13 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
                             foreach (var item in filters)
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                               <li><a href=\"#\" data-filter=\".");
#nullable restore
#line 15 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
                                                        Write(item.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("\"");
            BeginWriteAttribute("class", " class=", 642, "", 689, 1);
#nullable restore
#line 15 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
WriteAttributeValue("", 649, item.Link==content.Link?"selected":"", 649, 40, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 15 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
                                                                                                                   Write(item.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a></li>\r\n");
#nullable restore
#line 16 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <li class=\"pull-right\"><a href=\"#\" data-filter=\"*\"");
            BeginWriteAttribute("class", " class=", 812, "", 850, 1);
#nullable restore
#line 17 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
WriteAttributeValue("", 819, content.Id==21?"selected":"", 819, 31, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Tüm Projeler</a></li>\r\n                </ul> \r\n            </div>\r\n        </div> \r\n        <div id=\"gallery\" class=\"row gallery full-gallery de-gallery pf_4_cols hover-1 wow fadeInUp\" data-wow-delay=\".3s\">\r\n\r\n");
#nullable restore
#line 23 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
             foreach (var item in contents)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div");
            BeginWriteAttribute("class", " class=\"", 1137, "\"", 1200, 6);
            WriteAttributeValue("", 1145, "col-md-4", 1145, 8, true);
            WriteAttributeValue(" ", 1153, "col-sm-6", 1154, 9, true);
            WriteAttributeValue(" ", 1162, "col-xs-12", 1163, 10, true);
            WriteAttributeValue(" ", 1172, "item", 1173, 5, true);
            WriteAttributeValue(" ", 1177, "mb30", 1178, 5, true);
#nullable restore
#line 25 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
WriteAttributeValue(" ", 1182, item.Parent.Name, 1183, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                <div class=\"picframe\">\r\n                    <a class=\"simple-ajax-popup-align-top\"");
            BeginWriteAttribute("href", " href=\"", 1302, "\"", 1322, 2);
            WriteAttributeValue("", 1309, "/", 1309, 1, true);
#nullable restore
#line 27 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
WriteAttributeValue("", 1310, item.Link, 1310, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                        <span class=\"overlay-1\">\r\n                            <span class=\"pf_text\">\r\n                                <span class=\"project-name\">");
#nullable restore
#line 30 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
                                                      Write(item.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n                            </span>\r\n                        </span>\r\n                    </a>\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "f94c2f34bcf937fdcd829cb869287a9f8fc48f9d8475", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 1632, "~/uploads/", 1632, 10, true);
#nullable restore
#line 34 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
AddHtmlAttributeValue("", 1642, item.Image, 1642, 11, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "alt", 1, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
#nullable restore
#line 34 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
AddHtmlAttributeValue("", 1660, item.Image, 1660, 11, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n");
#nullable restore
#line 37 "E:\projeler\bitbucketRepos\cuhadaroglu\CMSSite\Views\Shared\Components\ContentView\ProjectList.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("               \r\n\r\n            </div>\r\n    </div>\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
