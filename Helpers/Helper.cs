using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Linq.Dynamic.Core;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Converters;

class ReplaceExpressionVisitor : ExpressionVisitor
{
    private readonly Expression _oldValue;
    private readonly Expression _newValue;

    public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
    {
        _oldValue = oldValue;
        _newValue = newValue;
    }

    public override Expression Visit(Expression node)
    {
        if (node == _oldValue)
        {
            return _newValue;
        }

        return base.Visit(node);
    }
}

public static class Helpers
{


    public static string strSqlColumn(this DataTable table)
    {
        string sql = "";
        foreach (DataColumn column in table.Columns)
        {
            if (sql.Length > 0)
                sql += ", ";
            sql += column.ColumnName;
        }
        return sql;
    }





    public static string toSqlInsertInto<T>(this List<T> _list)
    {
        var baseType = new BaseModel().GetType().GetProperties().Where(o => o.Name != "Id" && o.Name != "CreaDate" && o.Name != "CreaUser").ToList();
        var tablename = typeof(T).Name;
        var str = Environment.NewLine + @"------------START " + tablename + "---------------------\n";

        _list.ForEach(o =>
        {
            str += Environment.NewLine + @"insert into " + @"""" + tablename + @"""" + Environment.NewLine;
            var i = 0;
            var prop = o.GetType().GetProperties().ToList();
            str += " (";
            prop.ForEach(pp =>
            {
                var value = o.GetPropValue(pp.Name);
                if (value == null || value == "" || baseType.Any(cc => cc.Name == pp.Name)) return;
                str += i != 0 ? "," : "";
                i++;
                str += @"""" + pp.Name + @"""";
            });
            str += @" ) " + Environment.NewLine + "  values (";

            i = 0;
            prop.ForEach(pp =>
            {
                var value = o.GetPropValue(pp.Name);
                if (value == null || value == "" || baseType.Any(cc => cc.Name == pp.Name)) return;

                str += i != 0 ? "," : "";
                i++;

                switch (Type.GetTypeCode(pp.PropertyType))
                {
                    case TypeCode.Boolean:
                        {
                            value = value.ToBoolean();
                            break;
                        }
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                        {
                            value = value.ToInt();
                            break;
                        }
                    case TypeCode.Double:
                        {
                            value = value.ToDouble();
                            break;
                        }
                    case TypeCode.Decimal:
                        {
                            value = value.ToDecimal();
                            break;
                        }
                    case TypeCode.DateTime:
                        {
                            value = @"'" + value.ToStr() + @"'";
                            break;
                        }
                    case TypeCode.String:
                        {
                            value = @"'" + value.ToStr() + @"'";
                            break;
                        }
                    case TypeCode.Object:
                        {
                            //value = value.ToInt();
                            break;
                        }
                    default:
                        {
                            //value = value.ToInt();
                            break;
                        }
                }

                if (pp.Name == "CreaUser")
                    str += 1;
                else if (pp.Name == "CreaDate")
                    str += @"'" + DateTime.Now.ToString() + @"'";
                else
                    str += value;


            });
            str += " );";
        });
        str += Environment.NewLine + Environment.NewLine + @"------------END " + tablename + "---------------------" + Environment.NewLine + Environment.NewLine;
        return str;
    }


    public static Expression<Func<T, bool>> Combine<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
    {
        if (expression1 == null && expression2 == null)
        {
            return null;
        }

        if (expression1 == null)
        {
            return expression2;
        }

        if (expression2 == null)
        {
            return expression1;
        }

        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(expression1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expression1.Body);

        var rightVisitor = new ReplaceExpressionVisitor(expression2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expression2.Body);

        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
    }



    public static Dictionary<string, object> GetPropertyAttributes(PropertyInfo property)
    {
        Dictionary<string, object> attribs = new Dictionary<string, object>();
        foreach (CustomAttributeData attribData in property.GetCustomAttributesData())
        {
            string typeName = attribData.Constructor.DeclaringType.Name;
            if (typeName.EndsWith("Attribute"))
                typeName = typeName.Substring(0, typeName.Length - 9);

            if (typeName == "Required")
                attribs[typeName] = typeName.ToLower();
            else if (typeName == "DataType")
                attribs[typeName] = attribData.ConstructorArguments[0].Value;
            else
                attribs[typeName] = attribData.ConstructorArguments.Count > 0 ? attribData.ConstructorArguments[0].Value : "";
        }
        return attribs;
    }

    public static void SetValueCustom(this object container, string propertyName, object value)
    {
        container.GetType().GetProperty(propertyName).SetValue(container, value, null);
    }

    public static object GetPropValue(this object obj, string fieldName)
    { //<-- fieldName = "Details.Name"
        object value = null;
        string[] nameParts = fieldName.Split('.');
        foreach (String part in nameParts)
        {
            if (obj == null) { return ""; }

            Type type = obj.GetType();
            PropertyInfo info = type.GetProperty(part);
            if (info == null) { return ""; }

            if (info.PropertyType.Name.ToLower().Contains("collection"))
            {
                value = info.GetValue(obj, null) as IList;
                //foreach (var item in list) //<-- this list should be the "Details" property
                //{
                //    value += "," + item;
                //}
            }
            else
            {
                value = info.GetValue(obj, null);
            }

        }
        return value;
    }

    public static string DynamicInput(this object model, string controllerName, string nonProp, string orderby, string titleName,
    string PageType, string labelClass, string inputClass, string colClass, string btn,string InsertMethodName)
    {
        string str = "";
        string strScript = "";
        try
        {
            #region Configurations
            Type t = model.GetType();
            controllerName = string.IsNullOrEmpty(controllerName) ? t.Name : controllerName;

            var formname = "frm_" + t.Name;

            var props = t.GetProperties().ToList();

            var baseType = new BaseModel().GetType().GetProperties().Where(o => o.Name != "ModDate" && o.Name != "CreaDate").ToList();
            props = props.AsQueryable().Where(d => d.Name != "ModDate" && d.Name != "CreaDate" && !GetPropertyAttributes(d).Keys.Any(oo => oo == "NotMapped")).ToList();

            var IdValue = model.GetPropValue("Id");
            var idval = IdValue.ToInt();

            str += "<div formdata='" + model.GetType().Name + "'  class='kt-container  kt-grid__item kt-grid__item--fluid'>                                                                     ";
            str += "  <div class='kt-portlet kt-portlet--mobile'>                                                                                                                ";
            str += "      <div class='kt-portlet__head kt-portlet__head--lg'>                                                                                                       ";
            str += "          <div class='kt-portlet__head-label'>                                                                                                            ";
            str += $"              <span class='kt-portlet__head-icon'><i class='kt-font-brand flaticon2-line-chart'></i></span><h3 class='kt-portlet__head-title'>{titleName}</h3><br/>   ";
            str += "          </div>";
            str += idval < 1 ? "" : $"<div class='form-group row float-md-left'><div class='col-md-offset-9 col-md-9'><input dataid='{IdValue}' type='button' value='Sil' class='btnDelete btn btn-danger btn-elevate btn-icon-sm' /></div></div>";
            str += "     </div>";

            #region popup
            if (PageType == "modal")
            {
                str += "<style>.modal-dialog {width: 75% !important;} /*.form-group.row{margin-left: 0px;}*/</style>";
                str += "<div class='kt-portlet__head-toolbar'>                                                                                                       ";
                str += "              <div class='kt-portlet__head-wrapper'>                                                                                                         ";
                str += "                  <div class='kt-portlet__head-actions'>                                                                                                     ";
                str += "                      <button type='button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button>           ";
                str += "                  </div>                                                                                                                                     ";
                str += "              </div>                                                                                                                                         ";
                str += "          </div>                                                                                                                                             ";
                str += "</div>                                                                                                                                                 ";
            }
            #endregion

            str += "<div class='kt-portlet__body'>                                                                                                                         ";
            str += "    <div class='form-horizontal'>                                                                                                                      ";

            str += $"<form name='{formname}' id='frm_" + t.Name + "'>";
            str += "<div class='form-group row'>";


            baseType.ForEach(prp =>
            {

                if (prp.PropertyType.Name != "DateTime")
                {
                    str +=
                                  "<input  " +
                                  "id='" + prp.Name + "' " +
                                  "name='" + prp.Name + "' " +
                                  "value='" + model.GetPropValue(prp.Name) + "' " +
                                  "type='hidden'>  ";
                }
                else
                {
                    str +=
                                  "<input  " +
                                  "id='" + prp.Name + "' " +
                                  "name='" + prp.Name + "' " +
                                  "value='" + model.GetPropValue(prp.Name)?.ToDateTime().Value.ToShortDateString() + "' " +
                                  "type='hidden'>  ";
                }
                props = props.AsQueryable().Where(d => d.Name != prp.Name).ToList();

            });

            if (!string.IsNullOrEmpty(nonProp))
                nonProp.Split(",").ToList().ForEach(o =>
                {
                    props = props.AsQueryable().Where(d => d.Name != o.Trim()).ToList();
                });

            var orderbyProps = props.AsQueryable().OrderBy(oo => "" == "");

            if (!string.IsNullOrEmpty(orderby))
                orderby.Split(",").ToList().ForEach(o =>
                {
                    if (!string.IsNullOrEmpty(o))
                        props = orderbyProps.ThenBy(oo => oo.Name == o.Trim()).ToList();
                });

            #endregion



            #region forms


            foreach (var prp in props)
            {
                try
                {
                    if (prp.Name == "IsAdmin")
                    {

                    }
                    #region Group Label
                    if (prp.Name == "ContentPageId")
                        str += "<div class='col-md-12'><div class='form-group m-form__group row'> <div class='col-10 ml-auto'> <h3 class='m-form__section'>1. Sayfa Yapısı</h3> </div> </div> </div>";
                    //if (prp.Name == "BannerImage")
                    //    str += "<div class='col-md-12'><div class='form-group m-form__group row'> <div class='col-10 ml-auto'> <h3 class='m-form__section'>2. Sayfa İçeriği</h3> </div> </div> </div>";
                    //else if (prp.Name == "LangId")
                    //    str += "<div class='col-md-12'><div class='form-group m-form__group row'> <div class='col-10 ml-auto'> <h3 class='m-form__section'>3. Sayfa Ayarları</h3> </div> </div> </div>";
                    #endregion

                    object value = null;
                    #region Set Value Custom
                    switch (Type.GetTypeCode(prp.PropertyType))
                    {
                        case TypeCode.Boolean:
                        case TypeCode.Char:
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                        case TypeCode.DateTime:
                        case TypeCode.String:
                        case TypeCode.Object:
                            {
                                value = model.GetPropValue(prp.Name);
                                //if (prp.PropertyType.Name == "ICollection`1" || prp.PropertyType.FullName.Contains("Entity"))
                                //    if (prp.Name == "Documents" || prp.Name == "Gallery")
                                //        value = model.GetPropValue(prp.Name);
                                //    else
                                //        value = model.GetPropValue(prp.Name);

                                break;
                            }
                    }
                    #endregion

                    var dName = GetPropertyAttributes(prp);

                    #region Display Label Name
                    var DisplayName = "";
                    if (dName.Count > 0 && dName.Any(o => o.Key == "DisplayName"))
                        DisplayName = dName.FirstOrDefault(o => o.Key == "DisplayName").Value.ToStr();
                    else
                        DisplayName = prp.Name;
                    #endregion

                    #region Required
                    var Required = "";
                    var strReq = "";
                    var placeholder = "";
                    if (dName.Count > 0 && dName.Any(o => o.Key == "Required"))
                    {
                        Required = dName.FirstOrDefault(o => o.Key == "Required").Value.ToStr();
                        placeholder = DisplayName;
                        strReq = " <span style='color:red;' class='required'> * </span>";
                        DisplayName += strReq;
                    }
                    #endregion

                    switch (Type.GetTypeCode(prp.PropertyType))
                    {
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Object:
                            {
                                if (IdValue.ToInt() > 0 && (prp.PropertyType.Name == "ICollection`1" || prp.PropertyType.FullName.Contains("Entity")))
                                {
                                    if (prp.Name == "Documents" || prp.Name == "Gallery")
                                    {
                                        str += "<div class='" + "col-md-12" + "'><div class='row form-group shadow p-3 mb-5 bg-white rounded'>";
                                        str += "<div class='" + "col-md-4" + "'>";
                                        str += "<div class='" + labelClass + "'><label class='control-label ' for='" + prp.Name + "'>" +
                                            "<h3>" + DisplayName + "</h3></label></div>";
                                        str += "</div>";
                                        str += "<div class='" + "col-md-4" + "'>";
                                        str += "<input multiple class='btn-default' for='file_" + prp.Name + "' id='file_" + prp.Name + "' type='file'>    ";
                                        str += "</div>";
                                        str += "<div class='" + "col-md-4" + "'>";
                                        str += "<input value='Yükle' class='btn-default' id='btn_" + prp.Name + "' type='button'>    ";
                                        str += "</div>";
                                        str += "<div class='" + "col-md-12" + "'>";
                                        str += "<div class='row form-group'>";
                                        str += "</div>";
                                        str += "</div>";
                                        if (value != null)
                                        {
                                            str += "  <div id='filediv_" + prp.Name + "' class='filediv col-md-12' style='padding:10px;overflow-x:scroll;' >                                              ";
                                            foreach (var image in (value as IEnumerable<object>).Cast<object>().ToList())
                                            {

                                                var link = "/uploads/" + image.GetPropValue("Link");
                                                str += "             <div class='shadow p-3 mb-5 bg-white rounded' dataid='" + image.GetPropValue("Id") + "' style='float:left;margin:5px;'>                                              ";

                                                var linkStr = "<a href='" + link + "' title='" + image.GetPropValue("Title") + "' target='_blank' >" + image.GetPropValue("Name") + "</a>        ";

                                                if (image.GetPropValue("Name").ToStr().Contains(".png") || image.GetPropValue("Name").ToStr().Contains(".jpeg") || image.GetPropValue("Name").ToStr().Contains(".jpg"))
                                                {
                                                    var imageStr = "                 <img src='" + link + "' dataid='" + image.GetPropValue("Link") + "' name='" + image.GetPropValue("Name") + "' " +
                                                         "alt='" + image.GetPropValue("Alt") + "' " +
                                                         "title='" + image.GetPropValue("Title") + "' width='100' height='100' />                  ";
                                                    linkStr = "<a href='" + link + "' title='" + image.GetPropValue("Title") + "' target='_blank' >" + imageStr + "</a>        ";
                                                }
                                                str += linkStr;

                                                str += "<a href='javascript:;' dataid='" + image.GetPropValue("Id") + "' class='deleteImage'><i style='margin:5px !important; ' class='far fa-trash-alt'></i></a>        ";


                                                str += "</div>";
                                            }
                                            str += "</div>                                                                                                                                                   ";
                                        }

                                        str += "</div></div>";



                                        var gallleryOrdocumentName = prp.Name == "Gallery" ? "Gallery" : "Document";
                                        #region JS UPLOAD
                                        var strUpload = $"   $('#btn_{prp.Name}').click(function(e)                                                               ";
                                        strUpload += " {                                                                                  ";

                                        strUpload += "      var fileUpload = $('#' + 'file_" + prp.Name + "').get(0);                                                                     ";
                                        strUpload += "      var files = fileUpload.files;                                                                                             ";
                                        strUpload += "      var maxFiles = 10;                                                                                                        ";
                                        strUpload += "                                                                                                                                ";
                                        strUpload += "      if (files.length > maxFiles) {                                                                                            ";
                                        strUpload += "          alert('Lütfen Maksimum ' + maxFiles + ' görsel ekleyiniz.');                                                          ";
                                        strUpload += "          return;                                                                                                               ";
                                        strUpload += "      }                                                                                                                         ";
                                        strUpload += $"      var totalImage = ($('#filediv_{prp.Name} .deleteImage[dataid]').length + files.length);                                                       ";
                                        strUpload += "      if (totalImage > maxFiles) {                                                                                              ";
                                        strUpload += $"          alert($('#filediv_{prp.Name} .deleteImage[dataid]').length + ' adet görsel mevcut. Lütfen Maksimum ' + maxFiles + ' görsel ekleyiniz.');  ";
                                        strUpload += "          return;                                                                                                               ";
                                        strUpload += "      }                                                                                                                         ";
                                        strUpload += "                                                                                                                                ";
                                        strUpload += "      var fileData = new FormData();                                                                                            ";
                                        strUpload += "      for (var i = 0; i < files.length; i++) {                                                                                  ";
                                        strUpload += "          fileData.append('files', files[i]);                                                                                   ";
                                        strUpload += "      }                                                                                                                         ";
                                        strUpload += "   var postModel = []; var ids={Name:'" + gallleryOrdocumentName + "',Id:" + (IdValue.ToInt() > 0 ? IdValue : 0) + "};   postModel.push(ids); fileData.append('postmodel', JSON.stringify(postModel));                                                                  ";


                                        strUpload += "        $.LoadingOverlay('show');";
                                        strUpload += $"       $.ajxUpload('/{controllerName}/ImportDocuments',                                                           ";
                                        strUpload += "        fileData, function(resultData) {                                           ";

                                        strUpload += "       alerts('Yüklendi'); location.reload();                                            ";

                                        strUpload += "        $.LoadingOverlay('hide');                                                                  ";
                                        strUpload += "        }, function() { location.reload(); });                                                     ";
                                        strUpload += "        });                                                                                        ";



                                        strUpload += $" $('#filediv_{prp.Name} .deleteImage').click(function ()                                                           ";
                                        strUpload += " { var id = $(this).attr('dataid');                                                               ";
                                        strUpload += " alerts('Silmek istediğinize emin misiniz ? ', 'yesno', function (result) {              ";
                                        strUpload += "     if (result == true) {                                                                      ";
                                        strUpload += "         $.LoadingOverlay('show');                                                              ";


                                        strUpload += $"        $.ajx('/{controllerName}/DeleteImage',";
                                        strUpload += "{ id: id }, function (resultID) { ";
                                        strUpload += $@"           $(""#filediv_{prp.Name} div[dataid='"" + resultID + ""']"").remove();                                      ";
                                        strUpload += "            $.LoadingOverlay('hide');                                                          ";
                                        strUpload += "         });                                                                                    ";

                                        strUpload += "     }                                                                                          ";
                                        strUpload += " });   });                                                                                      ";



                                        str += "    <script>$(function(){ " + strUpload + " }) </script> ";
                                        #endregion


                                    }
                                }
                                else if (IdValue.ToInt() > 0 && (prp.PropertyType.Name == "Documents" || prp.PropertyType.Name == "Gallery"))
                                {

                                    str += "<div style='margin:1px;' class='" + "col-md-5" + "'><div class='row form-group'>";
                                    str += "<div class='" + "col-md-3" + "'><label class='control-label ' for='" + prp.Name + "'><h6>" + DisplayName + "</h6></label></div>";
                                    str += "<div class='" + "col-md-6" + "'>";
                                    str += "<input class='btn-default' for='file_" + prp.Name + "' id='file_" + prp.Name + "' type='file'>    ";
                                    str += "</div>";
                                    str += "<div class='" + "col-md-1" + "'>";
                                    str += "<input value='Yükle' class='btn-default' id='btn_" + prp.Name + "' type='button'>    ";
                                    str += "</div>";



                                    if (value != null)
                                    {
                                        str += "  <div text='" + prp.Name + "'  id='filediv_" + prp.Name + "' class='filediv col-md-12' style='padding:7px;' >                                              ";

                                        var link = "/uploads/" + value.GetPropValue("Link");
                                        str += "             <div class='shadow p-3 mb-5 bg-white rounded' dataid='" + value.GetPropValue("Id") + "' style='float:left;margin:5px;'>                                              ";

                                        var linkStr = "<a href='" + link + "' title='" + value.GetPropValue("Title") + "' target='_blank' >" + value.GetPropValue("Name") + "</a>        ";

                                        if (value.GetPropValue("Name").ToStr().Contains(".png") || value.GetPropValue("Name").ToStr().Contains(".jpeg") || value.GetPropValue("Name").ToStr().Contains(".jpg"))
                                        {
                                            var imageStr = "                 <img src='" + link + "' dataid='" + value.GetPropValue("Link") + "' name='" + value.GetPropValue("Name") + "' " +
                                                 "alt='" + value.GetPropValue("Alt") + "' " +
                                                 "title='" + value.GetPropValue("Title") + "' width='100' height='100' />                  ";
                                            linkStr = "<a href='" + link + "' title='" + value.GetPropValue("Title") + "' target='_blank' >" + imageStr + "</a>        ";
                                        }
                                        str += linkStr;

                                        str += "                 <a href='javascript:;' dataid='" + value.GetPropValue("Id") + "' class='deleteImage'><i style='margin:5px !important; ' class='far fa-trash-alt'></i></a>        ";
                                        str += "             </div>                                                                                                                                        ";
                                        str += "                                                                                                                                                           ";

                                        str += "</div>                                                                                                                                                   ";
                                    }

                                    str += "</div></div>";


                                    #region JS UPLOAD
                                    var strUpload = $"   $('#btn_{prp.Name}').click(function(e)                                                               ";
                                    strUpload += " {                                                                                  ";

                                    strUpload += "      var fileUpload = $('#' + 'file_" + prp.Name + "').get(0);                                                                     ";
                                    strUpload += "      var files = fileUpload.files;                                                                                             ";
                                    strUpload += "                                                                                                                                ";
                                    strUpload += "      if (files.length <1) {                                                                                            ";
                                    strUpload += "          alert('Lütfen görsel ekleyiniz.');                                                          ";
                                    strUpload += "          return;                                                                                                               ";
                                    strUpload += "      }                                                                                                                         ";

                                    strUpload += "                                                                                                                                ";
                                    strUpload += "      var fileData = new FormData();                                                                                            ";
                                    strUpload += "      for (var i = 0; i < files.length; i++) {                                                                                  ";
                                    strUpload += "          fileData.append('files', files[i]);                                                                                   ";
                                    strUpload += "      }                                                                                                                         ";
                                    strUpload += "   var postModel = []; var ids={text:'" + prp.Name + "',value:" + (IdValue.ToInt() > 0 ? IdValue : 0) + "};   postModel.push(ids); fileData.append('postmodel', JSON.stringify(postModel));                                                                  ";

                                    strUpload += "        $.LoadingOverlay('show');";
                                    strUpload += $"       $.ajxUpload('/{controllerName}/ImportDocumentsSingle',                                                           ";
                                    strUpload += "        fileData, function(resultData) {                                           ";

                                    strUpload += "        alerts('Yüklendi');    location.reload();                                          ";

                                    strUpload += "        $.LoadingOverlay('hide');                                                                  ";
                                    strUpload += "        }, function() { location.reload(); });                                                     ";
                                    strUpload += "        });                                                                                        ";



                                    #region Delete Image
                                    strUpload += $" $('#filediv_{prp.Name} .deleteImage').click(function ()                                                           ";
                                    strUpload += " { var id = $(this).attr('dataid');                                                               ";
                                    strUpload += " alerts('Silmek istediğinize emin misiniz ? ', 'yesno', function (result) {              ";
                                    strUpload += "     if (result == true) {                                                                      ";
                                    strUpload += "         $.LoadingOverlay('show');                                                              ";


                                    strUpload += $"        $.ajx('/{controllerName}/DeleteImage',";
                                    strUpload += "{ id: id }, function (resultID) { ";
                                    strUpload += $@"           $(""#filediv_{prp.Name} div[dataid='"" + resultID + ""']"").remove();                                      ";
                                    strUpload += "            $.LoadingOverlay('hide');                                                          ";
                                    strUpload += "         });                                                                                    ";

                                    strUpload += "     }                                                                                          ";
                                    strUpload += " });   });                                                                                      ";

                                    #endregion


                                    str += "    <script>$(function(){ " + strUpload + " }) </script> ";
                                    #endregion

                                }
                                else
                                {
                                    var relation = props.FirstOrDefault(o => prp.Name.Substring(prp.Name.Length - 2, 2) == "Id" && o.Name == prp.Name.Replace("Id", ""));
                                    if (relation != null || prp.Name.Substring(prp.Name.Length - 2, 2) == "Id")
                                    {
                                        var methodName = prp.Name.Replace("Id", "");
                                        if (relation != null)
                                            methodName = relation.PropertyType.Name;

                                        str += "<div class='" + colClass + "'><div class='row form-group'>";
                                        str += "<div class='" + labelClass + "'><label class='control-label ' for='" + prp.Name + "'>" + DisplayName + "</label></div>";
                                        str += "<div class='" + inputClass + "'> " +
                                            "<select " + Required + " " +
                                            "id='dp_" + prp.Name + "' " +
                                            "name='dp_" + prp.Name + "' " +
                                            "class='form-control '>" +
                                            "</select>";
                                        str += "</div></div></div>";


                                        if (prp.Name == "CityId" || prp.Name == "TownId")
                                        {
                                            if (prp.Name == "CityId")
                                            {
                                                var Town = props.FirstOrDefault(o => o.Name == "TownId");
                                                strScript += " function get" + methodName + "() { $('#dp_" + prp.Name + "').addOptionAjax('/" + methodName + "/GetSelect',null, 'value', 'text', null, getTown, '" + value + "', '', '" + placeholder + " Seçiniz'); } get" + methodName + "(); ";
                                                strScript += "$('#dp_" + "CityId" + "').change(function () { getTown(); });";
                                                if (Town != null)
                                                {
                                                    var TownValue = model.GetPropValue("TownId");
                                                    var strParam = "{id:$('#dp_CityId').val()}";
                                                    strScript += " function get" + "Town" + "() { $('#dp_" + Town.Name + "').addOptionAjax('/" + "Town" + "/GetSelect', " + strParam + ", 'value', 'text', null, null, '" + TownValue + "', '', '" + "İlçe" + " Seçiniz'); }   ";
                                                }
                                            }
                                        }
                                        else
                                            strScript += " function get" + methodName + "() { $('#dp_" + prp.Name + "').addOptionAjax('/" + methodName + "/GetSelect',null, 'value', 'text', null, null, '" + value + "', '', '" + placeholder + " Seçiniz'); } get" + methodName + "(); ";




                                    }
                                    else
                                    {
                                        if (prp.PropertyType.GenericTypeArguments.Any(o => o.Name == "Boolean"))
                                        {
                                            var boolCount = 12 / props.Count(o => o.PropertyType.GenericTypeArguments.Any(o => o.Name == "Boolean"));
                                            str += " <div class='col-md-" + boolCount + "'>                                                                                                                                               ";
                                            str += "     <div style='margin:15px;' class='custom-control custom-checkbox'>                                                                                                                     ";
                                            str += "         <input " + (value.ToBoolean() == true ? "checked='checked'" : "") + " " + Required + "  id='" + prp.Name + "' name='" + prp.Name + "' class='custom-control-input' type='checkbox'>   ";
                                            str += "         <label class='custom-control-label'  for='" + prp.Name + "'>" + DisplayName + "</label>                                                             ";
                                            str += "     </div>                                                                                                                                                           ";
                                            str += " </div>                                                                                                                                                               ";
                                        }
                                        if (prp.PropertyType.GenericTypeArguments.Any(o => o.Name == "Int32"))
                                        {
                                            str += "<div class='" + colClass + "'><div class='row form-group'>";
                                            str += "<div class='" + labelClass + "'><label class='control-label ' for='" + prp.Name + "'>" + DisplayName + "</label></div>";
                                            str += "<div class='" + inputClass + "'> " +
                                           "<input  " + Required + "  " +
                                           "id='" + prp.Name + "' " +
                                           "name='" + prp.Name + "' " +
                                           "placeholder='" + placeholder + "' " +
                                           "value='" + value + "' " +
                                           "class='form-control ' " +
                                           "type='number'>  ";
                                            str += "</div></div></div>";
                                        }
                                    }

                                }
                                break;
                            }
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            {
                                str += "<div class='" + colClass + "'><div class='row form-group'>";
                                str += "<div class='" + labelClass + "'><label class='control-label ' for='" + prp.Name + "'>" + DisplayName + "</label></div>";
                                str += "<div class='" + inputClass + "'> " +
                                    "<input " + Required + "  " +
                                    "id='" + prp.Name + "' " +
                                    "name='" + prp.Name + "' " +
                                    "placeholder='" + placeholder + "' " +
                                    "value='" + value + "' " +
                                    "class='form-control ' " +
                                    "type='text'>  ";
                                str += "</div></div></div>";
                                break;
                            }
                        case TypeCode.Boolean:
                            {
                                var boolCount = 12 / props.Count(o =>
                                o.PropertyType.Name == "Boolean"
                                 || o.PropertyType.GenericTypeArguments.Any(o => o.Name == "Boolean"));
                                str += " <div class='col-md-" + boolCount + "'>                                                                                                                                               ";
                                str += "     <div style='margin:15px;' class='custom-control custom-checkbox'>                                                                                                                     ";
                                str += "         <input " + (value.ToBoolean() == true ? "checked='checked'" : "") + " " + Required + "  id='" + prp.Name + "' name='" + prp.Name + "' class='custom-control-input' type='checkbox'>   ";
                                str += "         <label class='custom-control-label'  for='" + prp.Name + "'>" + DisplayName + "</label>                                                             ";
                                str += "     </div>                                                                                                                                                           ";
                                str += " </div>                                                                                                                                                               ";
                            }
                            break;
                        case TypeCode.Int32:
                            {
                                if (prp.PropertyType.IsEnum)
                                {
                                    str += "<div class='" + colClass + "'><div class='row form-group'>";
                                    str += "<div class='" + labelClass + "'><label class='control-label ' for='" + prp.Name + "'>" + DisplayName + "</label></div>";
                                    str += "<div class='" + inputClass + "'> " +
                                       "<select " + Required + " " +
                                        "id='dp_" + prp.Name + "' " +
                                        "name='dp_" + prp.Name + "' " +
                                        "class='form-control '>" +
                                        "</select>";
                                    str += "</div></div></div>" + Environment.NewLine;

                                    strScript += " $('#dp_" + prp.Name + "').addOptionAjax('/" + t.Name + "/Get" + prp.Name + "', null, 'value', 'text', function () { }, function () { }, '" + (value == null ? "" : ((int)value).ToStr()) + "', '' , 'Seçiniz');";
                                }
                                else
                                {
                                    var relation = props.FirstOrDefault(o => prp.Name.Substring(prp.Name.Length - 2, 2) == "Id" && o.Name == prp.Name.Replace("Id", ""));
                                    if (relation != null || prp.Name.Substring(prp.Name.Length - 2, 2) == "Id")
                                    {
                                        var methodName = prp.Name.Replace("Id", "");
                                        if (relation != null)
                                            methodName = relation.PropertyType.Name;

                                        str += "<div class='" + colClass + "'><div class='row form-group'>";
                                        str += "<div class='" + labelClass + "'><label class='control-label ' for='" + prp.Name + "'>" + DisplayName + "</label></div>";
                                        str += "<div class='" + inputClass + "'> " +
                                            "<select " + Required + " " +
                                            "id='dp_" + prp.Name + "' " +
                                            "name='dp_" + prp.Name + "' " +
                                            "class='form-control '>" +
                                            "</select>";
                                        str += "</div></div></div>";


                                        if (prp.Name == "CityId" || prp.Name == "TownId")
                                        {
                                            if (prp.Name == "CityId")
                                            {
                                                var Town = props.FirstOrDefault(o => o.Name == "TownId");
                                                strScript += " function get" + methodName + "() { $('#dp_" + prp.Name + "').addOptionAjax('/" + methodName + "/GetSelect',null, 'value', 'text', null, getTown, '" + value + "', '', '" + placeholder + " Seçiniz'); } get" + methodName + "(); ";
                                                strScript += "$('#dp_" + "CityId" + "').change(function () { getTown(); });";
                                                if (Town != null)
                                                {
                                                    var TownValue = model.GetPropValue("TownId");
                                                    var strParam = "{id:$('#dp_CityId').val()}";
                                                    strScript += " function get" + "Town" + "() { $('#dp_" + Town.Name + "').addOptionAjax('/" + "Town" + "/GetSelect', " + strParam + ", 'value', 'text', null, null, '" + TownValue + "', '', '" + "İlçe" + " Seçiniz'); }   ";
                                                }
                                            }
                                        }
                                        else
                                            strScript += " function get" + methodName + "() { $('#dp_" + prp.Name + "').addOptionAjax('/" + methodName + "/GetSelect',null, 'value', 'text', null, null, '" + value + "', '', '" + placeholder + " Seçiniz'); } get" + methodName + "(); ";


                                    }
                                    else
                                    {
                                        if (prp.PropertyType.GenericTypeArguments.Any(o => o.Name == "Boolean"))
                                        {
                                            var boolCount = 12 / props.Count(o => o.PropertyType.GenericTypeArguments.Any(o => o.Name == "Boolean"));
                                            str += " <div class='col-md-" + boolCount + "'>                                                                                                                                               ";
                                            str += "     <div style='margin:15px;' class='custom-control custom-checkbox'>                                                                                                                     ";
                                            str += "         <input " + (value.ToBoolean() == true ? "checked='checked'" : "") + " " + Required + "  id='" + prp.Name + "' name='" + prp.Name + "' class='custom-control-input' type='checkbox'>   ";
                                            str += "         <label class='custom-control-label'  for='" + prp.Name + "'>" + DisplayName + "</label>                                                             ";
                                            str += "     </div>                                                                                                                                                           ";
                                            str += " </div>                                                                                                                                                               ";
                                        }
                                        if (prp.PropertyType.GenericTypeArguments.Any(o => o.Name == "Int32"))
                                        {
                                            str += "<div class='" + colClass + "'><div class='row form-group'>";
                                            str += "<div class='" + labelClass + "'><label class='control-label ' for='" + prp.Name + "'>" + DisplayName + "</label></div>";
                                            str += "<div class='" + inputClass + "'> " +
                                           "<input  " + Required + "  " +
                                           "id='" + prp.Name + "' " +
                                           "name='" + prp.Name + "' " +
                                           "placeholder='" + placeholder + "' " +
                                           "value='" + value + "' " +
                                           "class='form-control ' " +
                                           "type='number'>  ";
                                            str += "</div></div></div>";
                                        }
                                    }

                                }
                                break;
                            }
                        case TypeCode.DateTime:
                            {
                                str += "<div class='" + colClass + "'><div class='row form-group'>";
                                str += "<div class='" + labelClass + "'><label class='control-label ' for='" + prp.Name + "'>" + DisplayName + "</label></div>";

                                str += "<div class='input-group-append input-group date " + inputClass + "'> " +
                                   "<input " + Required + "  " +
                                   "placeholder='" + DisplayName + " Seçiniz'" +
                                   "autocomplete='off'" +
                                   "id='" + prp.Name + "' " +
                                   "name='" + prp.Name + "' " +
                                   "value='" + (value.ToDateTime().Value.Year < 1900 ? "" : value?.ToDateTime().Value.ToShortDateString()) + "' " +
                                   "class='form-control ' " +
                                   "type='datetime'>  ";

                                str += "<span class='input-group-text'><i class='la la-calendar'></i></span>";
                                str += "</div></div></div>";

                                strScript += " $('#" + prp.Name + "').datepicker({format: 'dd/mm/yyyy', language: 'tr',todayBtn:'linked',clearBtn:!0,todayHighlight:!0}); ";
                                break;
                            }
                        case TypeCode.String:
                            {
                                var textName = "";
                                if (dName.Count > 0 && dName.Any(o => o.Key == "DataType"))
                                    textName = dName.FirstOrDefault(o => o.Key == "DataType").Value.ToStr();

                                if (textName != "")
                                {
                                    if (textName == "text")
                                    {
                                        str += "<div class='" + "col-md-12" + "'><div class='row form-group'>";
                                        str += "<div class='" + "col-md-2" + "'><label class='control-label ' for='" + prp.Name + "'>" + DisplayName + "</label></div>";
                                        str += "<div class='" + "col-md-10" + "'> ";
                                        str += " <textarea id='" + prp.Name + "' name='" + prp.Name + "' placeholder='" + placeholder +
                                            "' class='form-control'>" + value.ToStr().Trim() + "</textarea>";
                                        str += "</div></div></div>";
                                    }

                                }
                                else
                                {
                                    str += "<div class='" + colClass + "'><div class='row form-group'>";
                                    str += "<div class='" + labelClass + "'><label class='control-label ' for='" + prp.Name + "'>" + DisplayName + "</label></div>";
                                    str += "<div class='" + inputClass + "'> ";
                                    str +=
                                           "<input " + Required + "  " +
                                      "id='" + prp.Name + "' " +
                                      "name='" + prp.Name + "' " +
                                      "placeholder='" + placeholder + "' " +
                                      "value='" + System.Uri.UnescapeDataString(value.ToStr()) + "' " +
                                      "class='form-control ' " +
                                      "type='" + ((prp.Name == "Pass" || prp.Name == "Password" || prp.Name == "Sifre") ? "password" : "text") + "'>  ";
                                    str += "</div></div></div>";
                                }
                                break;
                            }
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                            break;
                        case TypeCode.Empty:
                            break;
                        case TypeCode.DBNull:
                            break;
                        case TypeCode.Char:
                            break;

                    }

                }
                catch (Exception ex)
                {
                    str += prp.Name + " : " + ex.Message + "\n <br/>" + ex.InnerException;
                }
            }
            str += "</div>";
            str += btn;
            #endregion

            str += "</form>";

            #region script

            #region textarea
            strScript += $"    $('#{formname} textarea').each(function()    ";
            strScript += "{       if (CKEDITOR.instances[$(this).attr('name')])                ";
            strScript += "           CKEDITOR.instances[$(this).attr('name')].destroy();      ";
            strScript += "       CKEDITOR.replace($(this).attr('name'), { }); CKEDITOR.instances[$(this).attr('name')].config.allowedContent = true;   CKEDITOR.instances[$(this).attr('name')].config.autoParagraph = false;              ";
            strScript += "   });                                                              ";
            #endregion
            #region ceoLink
            strScript += "   try {                                                    ";
            strScript += "           $('#Name').ceo({ target: '#Link' });             ";
            strScript += "       } catch (e) { }                                      ";
            strScript += "       try {                                                ";
            strScript += "           $('#Name').ceo({ target: '#MetaKeywords' });     ";
            strScript += "       } catch (e) { }                                      ";
            strScript += "       try {                                                ";
            strScript += "           $('#Name').dup({ target: '#MetaDescription' });  ";
            strScript += "       } catch (e) { }                                      ";
            #endregion

            #region Submit


            strScript += $"   $('.btnDelete').click(function ()                                                                      ";
            strScript += "{                  var dataid = $(this).attr('dataid');                                                                       ";
            strScript += "                  alerts('Silinsin mi ?', 'yesno', function (result) {                                                       ";
            strScript += "                      if (result == true) {                                                                                  ";
            strScript += $"                          $.LoadingOverlay('show');                                                          ";
            strScript += $"                          $.ajx('/{t.Name}/Delete', ";
            strScript += " { id: dataid }, function (resultData) {          ";
            strScript += "                              if (resultData && resultData.IsDeleted) {                                                      ";
            strScript += "                                  location.href='/" + t.Name + "/InsertOrUpdatePage';                                                                     ";
            strScript += "                              }                                                                                              ";
            strScript += "                              else {                                                                                         ";
            strScript += "                                  alerts(resultData.ResultType.MessageList[0]);                                              ";
            strScript += "                              }                                                                                              ";
            strScript += "                              $.LoadingOverlay('hide');                                                      ";
            strScript += "                          });                                                                                                ";
            strScript += "                      }                                                                                                      ";
            strScript += "                  });                                                                                                        ";
            strScript += "             }); ";



            strScript += $"   $('#{formname}').submit(function(e)                                                               ";
            strScript += " {      e.preventDefault();                                                                                  ";
            strScript += $"       var postModel = $.fn.toForm('#{formname}');                                                    ";
            strScript += " try{debugger;postModel.Level = parseInt(postModel.Level);} catch(ex){}  ";
            strScript += "        $.LoadingOverlay('show');";
            strScript += $"       $.ajx('/{controllerName}/{InsertMethodName}',                                                           ";
            strScript += "        { postModel: postModel }, function(resultData) {                                           ";

            strScript += "try { debugger; location.href=  '/" + t.Name + "' } catch(ex){ location.reload();}";

            strScript += "        $.LoadingOverlay('hide');                                                                  ";
            strScript += "        }, function() {location.reload(); });                                                     ";
            strScript += "        });                                                                                        ";


            #endregion

            #endregion
        }
        catch (Exception ex)
        {
            str += ex.Message + "\n <br/>" + ex.InnerException;
        }

        str += "</div></div></div></div>";

        str += "    <script>$(function(){ " + strScript + " }) </script> ";

        return str;
    }


    private static Dictionary<Type, PropertyInfo[]> _TypesWithWriteableProperties = new Dictionary<Type, PropertyInfo[]>();


    public static Expression<Func<T, bool>> filter<T>(IList<Expression<Func<T, bool>>> predicateExpressions,
    IList<Func<Expression, Expression, BinaryExpression>> logicalFunctions)
    {
        Expression<Func<T, bool>> filter = null;

        if (predicateExpressions.Count > 0)
        {
            Expression<Func<T, bool>> firstPredicate = predicateExpressions[0];
            Expression body = firstPredicate.Body;
            for (int i = 1; i < predicateExpressions.Count; i++)
            {
                body = logicalFunctions[i - 1](body, predicateExpressions[i].Body);
            }
            filter = Expression.Lambda<Func<T, bool>>(body, firstPredicate.Parameters);
        }

        return filter;
    }

    //public static IEnumerable<T> filter<T>(IEnumerable<T> source, string columnName, string propertyValue)
    //{
    //    return source.Where(m => { return m.GetType().GetProperty(columnName).GetValue(m, null).ToString().StartsWith(propertyValue); });
    //}

    //public static Func<T, bool> filter<T>(this T model)
    //{

    //    PropertyInfo[] props = t.GetProperties();
    //    foreach (PropertyInfo o in props)
    //    {
    //        var param = Expression.Parameter(typeof(T));


    //        object value = o.GetValue(model, new object[] { });
    //        if (value != null && !o.Name.Contains("Date"))
    //        {

    //            var condition =
    //                             Expression.Lambda<Func<T, bool>>(
    //                                 Expression.Equal(
    //                                     Expression.Property(param, o.Name),
    //                                     Expression.Constant(value, typeof(int))
    //                                 ),
    //                                 param
    //                             ).Compile(); // for LINQ to SQl/Entities skip Compile() call

    //            var body = condition.GetPredicateExpression(param);
    //            var dd = Expression.AndAlso();

    //        }
    //    }



    //    return propList;
    //}






    public static List<string> validControl<T>(this T table, string prop, string errorText)
    {
        List<string> list = new List<string>();

        var p = prop.Split(',').ToList();
        var sourceProperties = table.GetType().GetProperties().ToList();

        p.ForEach(o =>
        {
            var item = sourceProperties.FirstOrDefault(i => i.Name == o);
            if (item != null)
            {
                string cont = item.GetValue(table, null).ToStr();
                if (cont == null || string.IsNullOrEmpty(cont) || cont == "-1")
                {
                    list.Add(item.Name + "  : " + errorText);
                }
            }
        });

        return list;
    }


    //public static string deleteImage(string dosya_yolu, string filename, string ImageW, string ImageH)
    //{
    //    try
    //    {
    //        var boyutList = ConfigurationManager.AppSettings["resim-boyut"].Split(',').ToList();
    //        var controlBoyut = boyutList.Any(o => o == ImageW + "_" + ImageH);
    //        if (!controlBoyut)
    //            boyutList.Add(ImageW + "_" + ImageH);

    //        foreach (var boyut in boyutList)
    //        {
    //            string fullpath = dosya_yolu + (boyut + "_" + filename);
    //            var ff = new FileInfo(fullpath);
    //            if (ff.Exists)
    //                ff.Delete();
    //        }
    //        return "true";
    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.Message;
    //    }

    //}

    //public static Bitmap saveImage(HttpPostedFileBase fu, int width, int height, string dosya_yolu, string filename, string fileType, string filigran)
    //{
    //    string[] kes;
    //    var img = new Bitmap(fu.InputStream);

    //    var boyutList = ConfigurationManager.AppSettings["resim-boyut"].Split(',').ToList();
    //    var controlBoyut = boyutList.Any(o => o == img.Width + "_" + img.Height);
    //    if (!controlBoyut)
    //        boyutList.Add(img.Width + "_" + img.Height);
    //    foreach (var boyut in boyutList)
    //    {
    //        kes = boyut.Split('_');
    //        width = Convert.ToInt32(kes[0]);
    //        height = Convert.ToInt32(kes[1]);
    //        string fullpath = (dosya_yolu + (boyut + "_" + filename + "." + fileType)).Trim();
    //        var imgResize = ResizeImage(img, new Size(width, height), filigran);


    //        imgResize.Save(fullpath, img.RawFormat);
    //    }
    //    return img;
    //}


    //public static Bitmap ResizeImage(Bitmap imgToResize, Size size, string filigran)
    //{
    //    //Image'in Aspect Ratio Oranını Bul
    //    double dblRatio = (double)imgToResize.Width / (double)imgToResize.Height;

    //    //1-) Gelen Width ve Height Değerlerinden "0" Olan Var ise, buna karşılık gelen değer, Image'in Aspect Ratio Oranına Göre Bulunur.
    //    size.Width = size.Width == 0 ? (int)(size.Height * dblRatio) : size.Width;
    //    size.Height = size.Height == 0 ? (int)(size.Width / dblRatio) : size.Height;

    //    //2-) Eğer olması istenen boyutlardan biri, image'in gerçek boyutundan büyük ise, image'in orjinal boyutuna göre işleme devam edilir.
    //    size.Width = size.Width > imgToResize.Width ? imgToResize.Width : size.Width;
    //    size.Height = size.Height > imgToResize.Height ? imgToResize.Height : size.Height;

    //    //3-) Eğer belirtilen yeni boyutlar image'in yeni aspect ratio'suna uymuyor ise, büyük olan boyut sabit alınarak, diğeri aspect ratio oranına göre bulunur.
    //    double dblResizeRatio = (double)size.Width / (double)size.Height;
    //    if (Math.Abs(dblResizeRatio - dblRatio) > 0.01)
    //    {
    //        //Büyük olan oranı bul ve diğerini Aspect ratio oranını koruyarak değiştirilir.
    //        if (size.Width > size.Height)
    //        {
    //            size.Height = (int)(size.Width / dblRatio);
    //        }
    //        else
    //        {
    //            size.Width = (int)(size.Height * dblRatio);
    //        }
    //    }

    //    Bitmap imgResize = new Bitmap(size.Width, size.Height);
    //    using (Graphics g = Graphics.FromImage((Image)imgResize))
    //    {
    //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
    //        g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);

    //        Bitmap watermark = new Bitmap(filigran);
    //        watermark = ResizeImageFiligran(watermark, new Size() { Width = imgResize.Width, Height = imgResize.Height }, filigran);

    //        var destX = (imgResize.Width - watermark.Width) / 2;
    //        var destY = (imgResize.Height - watermark.Height) / 2;

    //        g.DrawImage(watermark, new Rectangle(destX,
    //                    destY,
    //                    watermark.Width,
    //                    watermark.Height));

    //        //using (TextureBrush br = new TextureBrush(imgToResize,
    //        //new Rectangle(0, 0, imgToResize.Width - 1, imgToResize.Height - 1)))
    //        //{
    //        //    g.FillRectangle(br, 0, 0, watermark.Width, watermark.Height);
    //        //}

    //        // display a clone for demo purposes
    //    }

    //    //Bitmap filigranImg = new Bitmap(filigran);
    //    //var lastImg = WatermarkImage(imgResize, filigranImg);
    //    return imgResize;

    //}

    //public static Bitmap ResizeImageFiligran(Bitmap imgToResize, Size size, string filigran)
    //{
    //    //Image'in Aspect Ratio Oranını Bul
    //    double dblRatio = (double)imgToResize.Width / (double)imgToResize.Height;

    //    //1-) Gelen Width ve Height Değerlerinden "0" Olan Var ise, buna karşılık gelen değer, Image'in Aspect Ratio Oranına Göre Bulunur.
    //    size.Width = size.Width == 0 ? (int)(size.Height * dblRatio) : size.Width;
    //    size.Height = size.Height == 0 ? (int)(size.Width / dblRatio) : size.Height;

    //    //2-) Eğer olması istenen boyutlardan biri, image'in gerçek boyutundan büyük ise, image'in orjinal boyutuna göre işleme devam edilir.
    //    //size.Width = size.Width > imgToResize.Width ? imgToResize.Width : size.Width;
    //    //size.Height = size.Height > imgToResize.Height ? imgToResize.Height : size.Height;

    //    //3-) Eğer belirtilen yeni boyutlar image'in yeni aspect ratio'suna uymuyor ise, büyük olan boyut sabit alınarak, diğeri aspect ratio oranına göre bulunur.
    //    double dblResizeRatio = (double)size.Width / (double)size.Height;
    //    if (Math.Abs(dblResizeRatio - dblRatio) > 0.01)
    //    {
    //        //Büyük olan oranı bul ve diğerini Aspect ratio oranını koruyarak değiştirilir.
    //        if (size.Width > size.Height)
    //        {
    //            size.Height = (int)(size.Width / dblRatio);
    //        }
    //        else
    //        {
    //            size.Width = (int)(size.Height * dblRatio);
    //        }
    //    }

    //    Bitmap imgResize = new Bitmap(size.Width, size.Height);
    //    using (Graphics g = Graphics.FromImage((Image)imgResize))
    //    {
    //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
    //        g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);

    //    }
    //    return imgResize;

    //}


    //public static Bitmap WatermarkImage(Bitmap image, Bitmap watermark)
    //{

    //    using (Graphics imageGraphics = Graphics.FromImage(image))
    //    {
    //        watermark.SetResolution(imageGraphics.DpiX, imageGraphics.DpiY);

    //        int x = (image.Width - watermark.Width) / 2;
    //        int y = (image.Height - watermark.Height) / 2;

    //        imageGraphics.DrawImage(watermark, x, y, watermark.Width, watermark.Height);
    //    }

    //    return image;
    //}


    //public static string saveFiles2(HttpPostedFileBase fu, int genislik, int yukseklik, string dosya_yolu, string filename, string fileType, string filigran)
    //{
    //    try
    //    {
    //        string[] kes;

    //        foreach (var boyut in ConfigurationManager.AppSettings["resim-boyut"].Split(','))
    //        {
    //            //if (Directory.Exists(HttpContext.Current.Server.MapPath(dosya_yolu + boyut)) == false)
    //            //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(dosya_yolu + boyut));
    //            kes = boyut.Split('_');
    //            genislik = Convert.ToInt32(kes[0]);
    //            yukseklik = Convert.ToInt32(kes[1]);

    //            string fullpath = dosya_yolu + ("\\" + boyut + "_" + filename + "." + fileType);

    //            System.Drawing.Image orjinalFoto = null;
    //            orjinalFoto = System.Drawing.Image.FromStream(fu.InputStream);

    //            var w = orjinalFoto.Width;
    //            var h = orjinalFoto.Height;
    //            if (genislik != 0)
    //            {
    //                w = genislik;
    //            }
    //            if (yukseklik != 0)
    //            {
    //                h = yukseklik;
    //            }

    //            Bitmap bitmap = orjinalFoto as Bitmap;



    //            boyutlandir2(w, h, bitmap, fullpath, filigran);
    //        }



    //        return "true";
    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.Message;
    //    }
    //}

    //public static void boyutlandir2(int width, int height, Bitmap bitmap, string fullPath, string filigran)
    //{

    //    double oran = 0;
    //    double genislik = bitmap.Width;
    //    double yukseklik = bitmap.Height;
    //    if (genislik < yukseklik)
    //    {
    //        oran = genislik / yukseklik;
    //        genislik = width;
    //        yukseklik = width / oran;
    //    }
    //    else if (genislik > yukseklik)
    //    {
    //        oran = yukseklik / genislik;
    //        genislik = height / oran;
    //        yukseklik = height;
    //        if (genislik < width)
    //        {
    //            oran = genislik / yukseklik;
    //            genislik = width;
    //            yukseklik = width / oran;
    //        }
    //    }
    //    System.Drawing.Size yeniboyut = new System.Drawing.Size(Convert.ToInt32(genislik), Convert.ToInt32(yukseklik));
    //    Bitmap image = new Bitmap(bitmap, yeniboyut);
    //    int x = (image.Width - width) / 2;
    //    int y = (image.Height - height) / 2;
    //    Bitmap bmp = new Bitmap(width, height, image.PixelFormat);
    //    Graphics grf = Graphics.FromImage(bmp);
    //    grf.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
    //    grf.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
    //    grf.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
    //    grf.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
    //    grf.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
    //    StringFormat Format = new StringFormat();
    //    Format.Alignment = StringAlignment.Center;
    //    Format.FormatFlags = StringFormatFlags.NoWrap;
    //    if (!string.IsNullOrEmpty(filigran))
    //    {
    //        grf.DrawString(filigran, new Font("Verdana", width / 15, FontStyle.Bold), new SolidBrush(Color.FromArgb(80, 255, 255, 255)), bmp.Width / 2, bmp.Height / 2, Format);
    //    }
    //    //if (width < 250)
    //    //    bmp.Save(trgImagePath, ImageFormat.Png);
    //    //else
    //    bmp.Save(fullPath, System.Drawing.Imaging.ImageFormat.Jpeg);
    //    bmp.Dispose();
    //    image.Dispose();
    //    bitmap.Dispose();
    //}


    public static string ExQuote(this string value)
    {
        return "'" + value.ToStr() + "'";
    }

    public static int GetNumberDigits(int value)
    {
        if (value < 10) return 10;
        int deger = 1;
        int basamak = 1;
        do
        {
            value /= 10;
            basamak++;

        } while (value > 10);

        for (int i = 0; i < basamak; i++)
        {
            deger *= 10;
        }

        return deger;
    }

    public static string Base64Encode(this string plainText)
    {
        if (plainText == null)
            return plainText;

        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    public static string Base64Decode(this string base64EncodedData)
    {
        if (base64EncodedData == null)
            return base64EncodedData;

        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static DataTable ToDataTable<T>(this List<T> data)
    {
        PropertyDescriptorCollection properties =
            TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable();
        foreach (PropertyDescriptor prop in properties)
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        foreach (T item in data)
        {
            DataRow row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            table.Rows.Add(row);
        }
        return table;
    }

    public static DataTable ToDataTable<T>(this IList<T> data)
    {
        PropertyDescriptorCollection properties =
            TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable();
        foreach (PropertyDescriptor prop in properties)
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        foreach (T item in data)
        {
            DataRow row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            table.Rows.Add(row);
        }
        return table;
    }

    public static bool tccontrol1(this string kimlikno)
    {
        kimlikno = kimlikno.Trim();
        if (kimlikno.Length != 11)
        {
            return false;
        }
        int[] sayilar = new int[11];
        for (int i = 0; i < kimlikno.Length; i++)
        {
            sayilar[i] = Int32.Parse(kimlikno[i].ToString());
        }
        int toplam = 0;
        for (int i = 0; i < kimlikno.Length - 1; i++)
        {
            toplam += sayilar[i];
        }
        if (toplam.ToString()[1].ToString() == sayilar[10].ToString() & sayilar[10] % 2 == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool tccontrol2(this string kimlikno)
    {
        bool returnvalue = false;
        if (kimlikno.Length == 11)
        {
            Int64 ATCNO, BTCNO, TcNo;
            long C1, C2, C3, C4, C5, C6, C7, C8, C9, Q1, Q2;

            TcNo = Int64.Parse(kimlikno);

            ATCNO = TcNo / 100;
            BTCNO = TcNo / 100;

            C1 = ATCNO % 10; ATCNO = ATCNO / 10;
            C2 = ATCNO % 10; ATCNO = ATCNO / 10;
            C3 = ATCNO % 10; ATCNO = ATCNO / 10;
            C4 = ATCNO % 10; ATCNO = ATCNO / 10;
            C5 = ATCNO % 10; ATCNO = ATCNO / 10;
            C6 = ATCNO % 10; ATCNO = ATCNO / 10;
            C7 = ATCNO % 10; ATCNO = ATCNO / 10;
            C8 = ATCNO % 10; ATCNO = ATCNO / 10;
            C9 = ATCNO % 10; ATCNO = ATCNO / 10;
            Q1 = ((10 - ((((C1 + C3 + C5 + C7 + C9) * 3) + (C2 + C4 + C6 + C8)) % 10)) % 10);
            Q2 = ((10 - (((((C2 + C4 + C6 + C8) + Q1) * 3) + (C1 + C3 + C5 + C7 + C9)) % 10)) % 10);

            returnvalue = ((BTCNO * 100) + (Q1 * 10) + Q2 == TcNo);
        }
        return returnvalue;
    }


    /// <summary>
    /// Add Quote to string value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ExToUpper(this string value)
    {
        string deger = string.Empty;
        for (int i = 0; i < value.Length; i++)
        {
            deger += GetUpperChar(value[i]);
        }
        return deger.ToUpper();

    }

    public static string ExToLower(this string value)
    {
        string deger = string.Empty;
        for (int i = 0; i < value.Length; i++)
        {
            deger += GetLowerChar(value[i]);
        }
        return deger;
    }

    private static string GetUpperChar(char value)
    {
        string deger = value.ToString();
        switch (value)
        {
            case 'i': deger = "I"; break;
            default:
                break;
        }
        return deger;
    }

    private static string GetLowerChar(char value)
    {
        string deger = value.ToString();
        switch (value)
        {
            case 'I': deger = "i"; break;
            default:
                break;
        }

        return deger.ToLower();
    }


    public static string ToStr(this object key)
    {
        return key == null ? string.Empty : key.ToString();
    }

    public static char ToChar(this object key)
    {
        return Convert.ToChar(key);
    }

    public static string ToStrDate(this DateTime key, string format)
    {
        return key == null ? string.Empty : key.ToString("dd" + format + "MM" + format + "yyyy");
    }

    public static bool ToBoolean(this object key)
    {
        bool deger = false;
        if (key != null)
        {
            if (key.ToString().Contains("True") || key.ToString().Contains("False"))
            {
                bool.TryParse(key.ToString(), out deger);
            }
            else
            {
                deger = Convert.ToBoolean(key.ToInt());
            }
        }
        return deger;
    }

    public static int ToInt(this object key)
    {
        int value = 0;
        if (key != null)
            int.TryParse(key.ToString(), out value);
        return value;
    }

    public static long ToLong(this object key)
    {
        long value = 0;
        if (key != null)
            long.TryParse(key.ToString(), out value);
        return value;
    }

    public static int ToInt(this object key, int value)
    {
        int ret = value;
        if (key != null)
        {
            if (!int.TryParse(key.ToString(), out value))
                value = ret;
        }
        return value;
    }

    public static decimal ToDecimal(this object key, CultureInfo Culture)
    {
        decimal value = 0;
        if (key != null)
            decimal.TryParse(key.ToString(), NumberStyles.Any, Culture, out value);
        return value;
    }

    public static decimal ToDecimal(this object key)
    {
        decimal value = 0;
        if (key != null)
            decimal.TryParse(key.ToString(), NumberStyles.Any, new CultureInfo("tr-TR"), out value);
        return value;
    }

    public static decimal ToDecimal(this object key, decimal value, CultureInfo Culture)
    {
        decimal ret = value;
        if (key != null)
        {
            if (!decimal.TryParse(key.ToString(), NumberStyles.Any, Culture, out value))
                value = ret;
        }
        return value;
    }

    public static decimal ToDecimal(this object key, decimal value)
    {
        decimal ret = value;
        if (key != null)
        {
            if (!decimal.TryParse(key.ToString(), NumberStyles.Any, new CultureInfo("tr-TR"), out value))
                value = ret;
        }
        return value;
    }
    public static string toFixed(this double number, uint decimals)
    {
        return number.ToString("N" + decimals);
    }

    public static double ToDouble(this object key)
    {
        double value = 0;
        if (key != null)
            double.TryParse(key.ToString(), NumberStyles.Any, new CultureInfo("tr-TR"), out value);
        return value;
    }

    public static double ToDouble(this object key, CultureInfo Culture)
    {
        double value = 0;
        if (key != null)
            double.TryParse(key.ToString(), NumberStyles.Any, Culture, out value);
        return value;
    }

    public static double ToDouble(this object key, double value, CultureInfo Culture)
    {
        double ret = value;
        if (key != null)
        {
            if (!double.TryParse(key.ToString(), NumberStyles.Any, Culture, out value))
                value = ret;
        }
        return value;
    }

    public static DateTime? ToDateTime(this object key)
    {
        DateTime? deger = null;
        if (key != null)
        {
            try
            {
                var date = new DateTime();
                if (DateTime.TryParse(key.ToString(), out date))
                    return date;
                else
                    return null;
            }
            catch
            {
                Console.WriteLine(@"AIop81F9y0ORvI5v98QT");
            }
        }
        return deger;
    }
    public static string ToYMD(this DateTime theDate)
    {
        return theDate.ToString("yyyyMMdd");
    }

    public static string ToYMD(this DateTime? theDate)
    {
        return theDate.HasValue ? theDate.Value.ToYMD() : string.Empty;
    }

    public static int TryParseToInt(this string Deger, int value)
    {
        int.TryParse(Deger, out value);
        return value;
    }

    public static double TryParseToDouble(this string Deger, double value)
    {
        double.TryParse(Deger, out value);
        return value;
    }

    public static decimal TryParseToDecimal(this string deger, decimal value)
    {
        decimal.TryParse(deger, out value);
        return decimal.Round(value, 2);
    }

    public static decimal TryParseToDecimal(this string deger, decimal value, bool round)
    {
        decimal.TryParse(deger, out value);
        if (round) return decimal.Round(value, 2);
        else return value;
    }

    public static string QuotedStr(this object columnValue)
    {
        if (columnValue == null) columnValue = "";
        switch (Type.GetTypeCode(columnValue.GetType()))
        {
            case TypeCode.String:
                return "'" + EscapeText(columnValue.ToString()) + "'";
            default:
                return EscapeText(columnValue.ToString());
        }
    }

    public static string EscapeText(string textToEscape)
    {
        string backslashesEscaped = textToEscape.Replace(@"\", @"\\");
        string backslashAndSingleQuoteEscaped = backslashesEscaped.Replace(@"'", @"\'");
        return backslashAndSingleQuoteEscaped;
    }

    public static int ExGetWeekIndex(this DayOfWeek week)
    {
        int value = -1;

        switch (week)
        {
            case DayOfWeek.Friday:
                value = 4;
                break;
            case DayOfWeek.Monday:
                value = 0;
                break;
            case DayOfWeek.Saturday:
                value = 5;
                break;
            case DayOfWeek.Sunday:
                value = 6;
                break;
            case DayOfWeek.Thursday:
                value = 3;
                break;
            case DayOfWeek.Tuesday:
                value = 1;
                break;
            case DayOfWeek.Wednesday:
                value = 2;
                break;
            default:
                break;
        }

        return value;
    }

    public static bool IsDate(String str)
    {
        bool res = false;
        try
        {
            System.DateTime dt = System.DateTime.Parse(str);
            res = true;
        }
        catch
        {
            Console.WriteLine(@"rKrZpQ7cKEdfARKM7swZ");
            // Not a date, handle appropriately
            res = false;
        }
        return res;
    }

    public static bool IsBetween<T>(this T value, T min, T max) where T : IComparable
    {
        //return Comparer<T>.Default.Compare(value, min) >= 0
        //    && Comparer<T>.Default.Compare(value, max) <= 0;
        return (min.CompareTo(value) <= 0) && (value.CompareTo(max) <= 0);
    }

    public static bool InRange<T>(this T value, params T[] values)
    {
        // Should be even number of items
        Debug.Assert(values.Length % 2 == 0);

        for (int i = 0; i < values.Length; i += 2)
            if (!value.InRange(values[i], values[i + 1]))
                return false;

        return true;
    }

    public static Task<List<T>> ToListAsync<T>(this IQueryable<T> list)
    {
        return Task.Run(() => list.ToList());
    }

    /// <summary>
    /// Verilen doğum tarihine göre yaş bilgisi döner
    /// </summary>
    /// <param name="BirthDate"></param>
    /// <returns></returns>
    public static int ExGetAge(this DateTime? BirthDate)
    {
        if (BirthDate.HasValue)
            return DateTime.Now.Date.Subtract(BirthDate.Value.Date).TotalDays.ToInt() / 365;
        else
            return 0;
    }

    public static int ExGetAge(this DateTime BirthDate)
    {
        return DateTime.Now.Date.Subtract(BirthDate.Date).TotalDays.ToInt() / 365;
    }

    public static int ExGetAge(this DateTime BirthDate, DateTime RegisterDate)
    {
        return RegisterDate.Date.Subtract(BirthDate.Date).TotalDays.ToInt() / 365;
    }

    #region WEBSERVICE

    public static void RemoveTimezoneForDataSet(this DataSet ds)
    {
        foreach (DataTable dt in ds.Tables)
        {
            foreach (DataColumn dc in dt.Columns)
            {

                if (dc.DataType == typeof(DateTime))
                {
                    dc.DateTimeMode = DataSetDateTime.Unspecified;
                }
            }
        }
    }

    #endregion

    #region ENUMS

    public static List<KeyValuePair<string, int>> GetEnumList<T>()
    {
        var list = new List<KeyValuePair<string, int>>();
        foreach (var e in Enum.GetValues(typeof(T)))
        {
            list.Add(new KeyValuePair<string, int>(e.ToString(), (int)e));
        }
        return list;
    }


    public static string ExGetEnumDescription(this string Text, Enum value)
    {
        if (value == null)
            Text = string.Empty;

        FieldInfo fi = value.GetType().GetField(value.ToString());

        if (fi == null)
            Text = string.Empty;

        DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(
            typeof(DescriptionAttribute),
            false);

        if (attributes != null &&
            attributes.Length > 0)
            Text = attributes[0].Description;
        else
            Text = value.ToStr();

        return Text;
    }

    public static string ExGetDescription(this Enum value)
    {
        if (value == null)
            return string.Empty;

        FieldInfo fi = value.GetType().GetField(value.ToString());

        if (fi == null)
            return string.Empty;

        DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(
            typeof(DescriptionAttribute),
            false);

        if (attributes != null &&
            attributes.Length > 0)
            return attributes[0].Description;
        else
            return value.ToStr();
    }

    public static string ExGetDescription(this PropertyInfo property)
    {
        if (property == null)
            return string.Empty;

        DescriptionAttribute[] attributes =
            (DescriptionAttribute[])property.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null &&
            attributes.Length > 0)
            return attributes[0].Description;
        else
            return property.Name.ToStr();
    }

    public static string ExGetDescription(this Type property)
    {
        if (property == null)
            return string.Empty;

        DescriptionAttribute[] attributes =
            (DescriptionAttribute[])property.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null &&
            attributes.Length > 0)
            return attributes[0].Description;
        else
            return property.Name.ToStr();
    }
    #endregion

    #region Data Extension

    public static T NewClass<T>(this T source) where T : class, new()
    {
        var destination = new T();
        PropertyInfo[] properties;
        Type type = typeof(T);
        lock (_TypesWithWriteableProperties)
        {
            if (!_TypesWithWriteableProperties.TryGetValue(type, out properties))
            {
                List<PropertyInfo> props = new List<PropertyInfo>();
                PropertyInfo[] classProps = type.GetProperties();
                foreach (var prop in classProps)
                {
                    if (prop.CanWrite)
                    {
                        props.Add(prop);
                    }
                }
                properties = props.ToArray();
                _TypesWithWriteableProperties[type] = properties;
            }
        }

        foreach (var prop in properties)
        {
            object value = prop.GetValue(source);
            try
            {
                prop.SetValue(destination, value);
            }
            catch
            {
                Console.WriteLine(@"kNWvMLUwguPX9gSnB70n");
            }
        }
        return destination;
    }

    public static T Copy<T, U>(this U source) where T : class, new()
    {
        var destination = new T();
        var destinationProperties = destination.GetType().GetProperties().ToList();
        var sourceProperties = source.GetType().GetProperties().ToList();

        foreach (var destinationProperty in destinationProperties)
        {
            var sourceProperty = sourceProperties.Find(item => item.Name == destinationProperty.Name);

            if (sourceProperty != null && destinationProperty.CanWrite)
            {
                try
                {
                    destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                }
                catch (Exception)
                {
                    Console.WriteLine(@"QAZlpQtSbuxuePR89Znk");
                }
            }
        }
        return destination;
    }
    #endregion

    public static List<Dictionary<string, object>> Read(DbDataReader reader)
    {
        List<Dictionary<string, object>> expandolist = new List<Dictionary<string, object>>();
        foreach (var item in reader)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(item))
            {
                var obj = propertyDescriptor.GetValue(item);
                expando.Add(propertyDescriptor.Name, obj);
            }
            expandolist.Add(new Dictionary<string, object>(expando));
        }
        return expandolist;
    }

    public static Dictionary<string, object> DictionaryFromType(this object atype)
    {
        if (atype == null) return new Dictionary<string, object>();
        Type t = atype.GetType();
        PropertyInfo[] props = t.GetProperties();
        Dictionary<string, object> dict = new Dictionary<string, object>();
        foreach (PropertyInfo prp in props)
        {
            object value = prp.GetValue(atype, new object[] { });
            dict.Add(prp.Name, value);
        }
        return dict;
    }

    public static string[] PropertiesFromType(this object atype)
    {
        if (atype == null) return new string[] { };
        Type t = atype.GetType();
        PropertyInfo[] props = t.GetProperties();
        List<string> propNames = new List<string>();
        foreach (PropertyInfo prp in props)
        {
            propNames.Add(prp.Name);
        }
        return propNames.ToArray();
    }



    public static string GetMyTable<T>(this IEnumerable<T> list, params Expression<Func<T, object>>[] fxns)
    {

        StringBuilder sb = new StringBuilder();
        sb.Append("<TABLE>\n");

        sb.Append("<TR>\n");
        foreach (var fxn in fxns)
        {
            sb.Append("<TD>");
            sb.Append(GetName(fxn));
            sb.Append("</TD>");
        }
        sb.Append("</TR> <!-- HEADER -->\n");


        foreach (var item in list)
        {
            sb.Append("<TR>\n");
            foreach (var fxn in fxns)
            {
                sb.Append("<TD>");
                sb.Append(fxn.Compile()(item));
                sb.Append("</TD>");
            }
            sb.Append("</TR>\n");
        }
        sb.Append("</TABLE>");

        return sb.ToString();
    }

    public static string GetName<T>(Expression<Func<T, object>> expr)
    {
        var member = expr.Body as MemberExpression;
        if (member != null)
            return Helpers.GetName2(member);

        var unary = expr.Body as UnaryExpression;
        if (unary != null)
            return GetName2((MemberExpression)unary.Operand);

        return "?+?";
    }

    public static string GetName2(MemberExpression member)
    {
        var fieldInfo = member.Member as FieldInfo;
        if (fieldInfo != null)
        {
            var d = fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (d != null) return d.Description;
            return fieldInfo.Name;
        }

        var propertInfo = member.Member as PropertyInfo;
        if (propertInfo != null)
        {
            var d = propertInfo.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (d != null) return d.Description;
            return propertInfo.Name;
        }

        return "?-?";
    }

    public static string ToEn(this string text)
    {
        return String.Join("", text.Normalize(NormalizationForm.FormD)
        .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
    }

    public static string toTr(this string str)
    {
        string once, sonra;
        once = str;
        sonra = once.Replace('ı', 'i');
        once = sonra.Replace('ö', 'o');
        sonra = once.Replace('ü', 'u');
        once = sonra.Replace('ş', 's');
        sonra = once.Replace('ğ', 'g');
        once = sonra.Replace('ç', 'c');
        sonra = once.Replace('İ', 'I');
        once = sonra.Replace('Ö', 'O');
        sonra = once.Replace('Ü', 'U');
        once = sonra.Replace('Ş', 'S');
        sonra = once.Replace('Ğ', 'G');
        once = sonra.Replace('Ç', 'C');
        str = once;
        return str;

    }

    public static string toCustomTr(this string str)
    {
        str = str.Trim().Replace(".", "").Replace(" ", "").ToLower(new CultureInfo("tr-TR", false)).toTr();
        return str;
    }

    public static void setUrl(this string url)
    {
        System.Diagnostics.Process.Start(url);
    }

    public static string getStr(this string str, string start, string end)
    {
        int first = str.IndexOf(start);
        int last = str.LastIndexOf(end);
        string str2 = str.Substring(first + 1, last - first - 1);
        return str2;
    }

    public static double ToMaxMin(bool durum, params double[] args)
    {
        return durum ? args.ToList().Max() : args.ToList().Min();
    }

    public static List<KeyValuePair<string, string>> GetEnumValuesAndDescriptions<T>()
    {
        Type enumType = typeof(T);

        if (enumType.BaseType != typeof(Enum))
            throw new ArgumentException("T is not System.Enum");

        List<KeyValuePair<string, string>> enumValList = new List<KeyValuePair<string, string>>();

        foreach (var e in Enum.GetValues(typeof(T)))
        {
            var fi = e.GetType().GetField(e.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            enumValList.Add(new KeyValuePair<string, string>((attributes.Length > 0) ? e.ToString() : e.ToString(), attributes[0].Description));
        }

        return enumValList;
    }

    public static List<string> GetEnumDescriptions<T>()
    {
        Type enumType = typeof(T);

        if (enumType.BaseType != typeof(Enum))
            throw new ArgumentException("T is not System.Enum");

        List<string> enumValList = new List<string>();

        foreach (var e in Enum.GetValues(typeof(T)))
        {
            var fi = e.GetType().GetField(e.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            enumValList.Add(attributes[0].Description);
        }

        return enumValList;
    }




    //public static Object GetPropValue(this Object obj, String name)
    //{
    //    foreach (String part in name.Split('.'))
    //    {
    //        if (obj == null) { return null; }

    //        Type type = obj.GetType();
    //        PropertyInfo info = type.GetProperty(part);
    //        if (info == null) { return null; }

    //        obj = info.GetValue(obj, null);
    //    }
    //    return obj;
    //}



    public static void noSetValueCustom<T>(this object container, string nopropertyName, object value, object total)
    {
        var prop = container.GetType().GetProperties().ToList();
        prop.ForEach(o =>
        {
            if (!nopropertyName.Contains(o.Name) && o.CanWrite)
            {
                var val = container.GetPropValue(o.Name);
                if (val != null)
                    container.SetValueCustom(o.Name, (((val.ToDouble() * value.ToDouble())) / total.ToDouble()).ToString("0.##"));
            }
            else
            {
                var val = container.GetPropValue(o.Name);
                if (val != null)
                    container.SetValueCustom(o.Name, val);
            }
        });
    }

    public static void noSetValueCustom2<T>(this object container2, List<T> containerList, string nopropertyName, string propertyName)
    {

        container2.GetType().GetProperties().ToList().ForEach(o =>
        {
            if (!nopropertyName.Contains(o.Name) && (!string.IsNullOrEmpty(propertyName) ? propertyName.Contains(o.Name) : true) && o.CanWrite)
            {
                var total = new List<double>();
                containerList.Where(l => l.GetType().GetProperty(o.Name).CanWrite).ToList().ForEach(oo =>
                {
                    var vals = oo.GetPropValue(o.Name).ToDouble();
                    total.Add(vals);
                });
                var val = container2.GetPropValue(o.Name).ToDouble();
                total.Add(val);
                if (total.Count > 0)
                    container2.SetValueCustom(o.Name, (total.Average()).ToString("0.##"));
            }
            else
            {
                containerList.ForEach(oo =>
                {
                    var vals = oo.GetPropValue(o.Name);
                    container2.SetValueCustom(o.Name, vals);
                });
            }
        });
    }




    public static string GetCleanText(this string text)
    {
        string outtext = text.ToLower(System.Globalization.CultureInfo.GetCultureInfo("en"));
        outtext = ClearTurkish(outtext);
        string validchars = "abcdefghijklmnopqrstuvwxyz0123456789";
        string outvalidtext = "";
        for (int x = 0; x < outtext.Length; x++)
            if (validchars.IndexOf(outtext.Substring(x, 1)) != -1)
                outvalidtext += outtext.Substring(x, 1);
            else
                outvalidtext += "-";
        return outvalidtext.Replace("---", "-").Replace("--", "-").Replace("+", "_arti_");
    }
    public static string ClearTurkish(this string input)
    {
        return
            StripHtml(input)
            .Replace("Ç", "C")
            .Replace("ç", "c")
            .Replace("Ğ", "G")
            .Replace("ğ", "g")
            .Replace("İ", "I")
            .Replace("ı", "i")
            .Replace("Ö", "ö")
            .Replace("ö", "o")
            .Replace("Ş", "S")
            .Replace("ş", "s")
            .Replace("Ü", "U")
            .Replace("ü", "u")
            .Replace("â", "a")
            .Replace("?", "-")
            .Replace("=", "-")
            .Replace("!", "-")
            .Replace(" ", "-")
            .Replace("&", "-")
            .Replace("%", "-")
            .Replace("\\", "-")
            .Replace("(", "-")
            .Replace(")", "-")
            .Replace("<", "-")
            .Replace(">", "-")
            .Replace("'", "-")
            .Replace("\"", "-")
            .Replace("\n", "-")
            .Replace(".", "-")
            .Replace(",", "-")
            .Replace("@", "-")
            .Replace(":", "-")
            .Replace(";", "-")
            .Replace("“", "-")
            .Replace("”", "-")
            .Replace("/", "-")
            .Replace("+", "_arti_");
    }

    public static string GetCleanTextSearch(this string text)
    {
        string outtext = text.ToLower(System.Globalization.CultureInfo.GetCultureInfo("en"));
        string validchars = "abcçdefgğhıijklmnoöpqrsştuüvwxyz0123456789 ";
        string outvalidtext = "";
        for (int x = 0; x < outtext.Length; x++)
            if (validchars.IndexOf(outtext.Substring(x, 1)) != -1)
                outvalidtext += text.Substring(x, 1);
            else
                outvalidtext += "";

        string removedless2chars = "";
        foreach (string holder in outvalidtext.Trim().Split(' '))
        {
            if (holder.Trim().Length > 2)
            {
                removedless2chars += holder;
                removedless2chars += " ";
            }
        }
        return removedless2chars.Trim();
    }

    public static string ReplaceIllegalCharacters(this string val, string replaceCharacter = "")
    {
        replaceCharacter = replaceCharacter ?? "";
        val = val.Replace(" ", "")
                    .Replace("!", replaceCharacter)
                    .Replace("'", replaceCharacter)
                    .Replace("^", replaceCharacter)
                    .Replace("%", replaceCharacter)
                    .Replace("&", replaceCharacter)
                    .Replace("/", replaceCharacter)
                    .Replace("(", replaceCharacter)
                    .Replace(")", replaceCharacter)
                    .Replace("=", replaceCharacter)
                    .Replace("?", replaceCharacter)
                    .Replace("<", replaceCharacter)
                    .Replace(">", replaceCharacter)
                    .Replace("£", replaceCharacter)
                    .Replace("#", replaceCharacter)
                    .Replace("½", replaceCharacter)
                    .Replace("{", replaceCharacter)
                    .Replace("[", replaceCharacter)
                    .Replace("]", replaceCharacter)
                    .Replace("}", replaceCharacter)
                    .Replace("\\", replaceCharacter)
                    .Replace("|", replaceCharacter)
                    .Replace("*", replaceCharacter)
                    .Replace("é", replaceCharacter)
                    .Replace("¨", replaceCharacter)
                    .Replace("~", replaceCharacter)
                    .Replace("`", replaceCharacter)
                    .Replace(";", replaceCharacter)
                    .Replace(":", replaceCharacter)
                    .Replace(" ", replaceCharacter);
        return val;
    }

    public static string StripHtml(this string text)
    {
        return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
    }


    public static string ceoUrl(this string url)
    {
        if (string.IsNullOrEmpty(url)) return "";
        url = url.ToLower();
        url = url.Trim();
        //if (url.Length > 100)
        //{
        //    url = url.Substring(0, 100);
        //}
        url = url.Replace("İ", "I");
        url = url.Replace("ı", "i");
        url = url.Replace("ğ", "g");
        url = url.Replace("Ğ", "G");
        url = url.Replace("ç", "c");
        url = url.Replace("Ç", "C");
        url = url.Replace("ö", "o");
        url = url.Replace("Ö", "O");
        url = url.Replace("ş", "s");
        url = url.Replace("Ş", "S");
        url = url.Replace("ü", "u");
        url = url.Replace("Ü", "U");
        url = url.Replace("'", "");
        url = url.Replace("\"", "");
        char[] replacerList = @"$%#@!*?;:~`+=()[]{}|\'<>,/^&"".".ToCharArray();
        for (int i = 0; i < replacerList.Length; i++)
        {
            string strChr = replacerList[i].ToString();
            if (url.Contains(strChr))
            {
                url = url.Replace(strChr, string.Empty);
            }
        }
        Regex r = new Regex("[^a-zA-Z0-9_-]");
        url = r.Replace(url, "-");
        while (url.IndexOf("--") > -1)
            url = url.Replace("--", "-");

        return url;
    }

    public static byte[] Serialize(this object item)
    {
        var jsonString = JsonConvert.SerializeObject(item);
        return Encoding.UTF8.GetBytes(jsonString);
    }

    public static T Deserialize<T>(this string serializedObject, string format)
    {
        if (serializedObject == null)
            return default(T);

        var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

        return JsonConvert.DeserializeObject<T>(serializedObject, dateTimeConverter);
    }

    public static T Deserialize<T>(this string serializedObject)
    {
        if (serializedObject == null)
            return default(T);

        return JsonConvert.DeserializeObject<T>(serializedObject);
    }

    public static string ToJson(this object model)
    {
        var jsonString = JsonConvert.SerializeObject(model);
        return jsonString;
    }


    public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
    {
        foreach (var value in list)
        {
            await func(value);
        }
    }


    public static string ToQueryString(this object request, string separator = ",")
    {
        if (request == null)
            throw new ArgumentNullException("request");

        // Get all properties on the object
        var properties = request.GetType().GetProperties()
            .Where(x => x.CanRead)
            .Where(x => x.GetValue(request, null) != null)
            .ToDictionary(x => x.Name, x => x.GetValue(request, null));

        // Get names for all IEnumerable properties (excl. string)
        var propertyNames = properties
            .Where(x => !(x.Value is string) && x.Value is IEnumerable)
            .Select(x => x.Key)
            .ToList();

        // Concat all IEnumerable properties into a comma separated string
        foreach (var key in propertyNames)
        {
            var valueType = properties[key].GetType();
            var valueElemType = valueType.IsGenericType
                                    ? valueType.GetGenericArguments()[0]
                                    : valueType.GetElementType();
            if (valueElemType.IsPrimitive || valueElemType == typeof(string))
            {
                var enumerable = properties[key] as IEnumerable;
                properties[key] = string.Join(separator, enumerable.Cast<object>());
            }
        }

        // Concat all key/value pairs into a string separated by ampersand
        return string.Join("&", properties
            .Select(x => string.Concat(
                Uri.EscapeDataString(x.Key), "=",
                Uri.EscapeDataString(x.Value.ToString()))));
    }



    public static bool isMail(this string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static string limit(this string source, int maxLength)
    {
        if (source.Length <= maxLength)
        {
            return source;
        }

        return source.Substring(0, maxLength);
    }

}


public class ParameterBinder : ExpressionVisitor
{
    public ParameterExpression value;
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node.Type == value.Type ? value : base.VisitParameter(node);
    }
}
