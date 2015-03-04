using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCustomViewEngine
{
    /// <summary>
    /// 自定义视图引擎
    /// </summary>
    public class CustomViewSelectEngine : BuildManagerViewEngine
    {
        private static string _skinName = string.Empty;

        public static string SkinName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_skinName))
                {
                    //return _skinName;
                }
                string url = HttpContext.Current.Request.Url.ToString();
                string[] arr = url.Replace("http://", "").Split('/');
                if (arr.Length < 2)
                {
                    _skinName = "Default";
                }
                _skinName = arr[1];
                return _skinName;
            }
        }

        public static string ViewFolderName = "Views";

        internal static readonly string ViewtartFileName = "_ViewStart";

        public CustomViewSelectEngine() : this(null) { }
        public CustomViewSelectEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
        {
            base.AreaViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml" };
            base.AreaMasterLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml" };
            base.AreaPartialViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml" };
            switch (SkinName.ToLower())
            {
                case "default":
                    base.ViewLocationFormats = new string[] { 
                        "~/Views/{0}.cshtml", 
                        "~/Views/{1}/{0}.cshtml", 
                        "~/Views/Shared/{0}.cshtml",
                        "~/Views/Error/{0}.cshtml",
                        "~/Views/Shared/_Layout.cshtml",
                        "~/Views/Shared/_LayoutPrev.cshtml",
                    };
                    break;
                default:
                    base.ViewLocationFormats = new string[] { 
                        "~/Views/"+SkinName+"/{0}.cshtml", 
                        "~/Views/"+SkinName+"/{1}/{0}.cshtml", 
                        "~/Views/"+SkinName+"/Shared/{0}.cshtml",
                        "~/Views/"+SkinName+"/Error/{0}.cshtml",
                        "~/Views/"+SkinName+"/Shared/_Layout.cshtml",
                        "~/Views/"+SkinName+"/Shared/_LayoutPrev.cshtml",
                    };
                    break;
            }

            base.MasterLocationFormats = new string[] { "~/Views/Home/Shared/{0}.cshtml", "~/Views/Home/Layout/{0}.cshtml" };

            //部分视图路径设置
            if (SkinName.ToLower().Equals("sfcampus1.zhaopin.com"))
                base.PartialViewLocationFormats = new string[] { "~/Views/" + SkinName + "/Shared/WebControl/{0}.cshtml" };
            else
                base.PartialViewLocationFormats = new string[] { "~/Views/Home/Shared/WebControl/{0}.cshtml" };

            base.FileExtensions = new string[] { "cshtml" };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            string layoutPath = null;
            bool runViewtartPages = false;
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, partialPath, layoutPath, runViewtartPages, fileExtensions, base.ViewPageActivator);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            string layoutPath = masterPath;
            bool runViewtartPages = true;
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, viewPath, layoutPath, runViewtartPages, fileExtensions, base.ViewPageActivator);
        }

    }
}