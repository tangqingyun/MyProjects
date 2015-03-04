using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ZP.Campus.Framework.Core.IO
{
    public class BaseFile
    {
        /// <summary>
        /// 导出到文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void ExportToFile(string filePath)
        {
            //string fileType = GetFileType(filePath);
            //string content = ReadFileContent(filePath);

            var fileName = HttpUtility.UrlEncode(System.IO.Path.GetFileName(filePath), Encoding.UTF8);
            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.Buffer = true;
            //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            //HttpContext.Current.Response.Charset = "utf-8";
            ////HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;//设置输出流为简体中文
            //HttpContext.Current.Response.ContentType = fileType;
            //HttpContext.Current.Response.Write(content);
            //HttpContext.Current.Response.End();

            FileInfo fileInfo = new FileInfo(filePath);

            HttpContext.Current.Response.Clear();

            HttpContext.Current.Response.ClearContent();

            HttpContext.Current.Response.ClearHeaders();

            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());

            HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");

            HttpContext.Current.Response.ContentType = "application/octet-stream";

            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;

            HttpContext.Current.Response.WriteFile(fileInfo.FullName);

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();

        }

        /// <summary>
        /// 导出到文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        /// <param name="fileType">文件类型</param>
        public static void ExportToFile(string fileName, string content)
        {
            string fileType = GetFileType(fileName);

            fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.Charset = "utf-8";
            //HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;//设置输出流为简体中文
            HttpContext.Current.Response.ContentType = fileType;
            HttpContext.Current.Response.Write(content);
            HttpContext.Current.Response.End();
        }

        private static string GetFileType(string fileName)
        {
            var ext = System.IO.Path.GetExtension(fileName);

            switch (ext)
            {
                case ".xls":
                case ".xlsx":

                    return "application/ms-excel";
                case ".doc":
                case ".docx":
                    return "application/ms-word";

                default:
                    return "application/ms-excel";
            }
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="file">返回文件内容</param>
        /// <returns></returns>
        public static string ReadFileContent(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                StreamReader sr = new StreamReader(fs);

                return sr.ReadToEnd();
            }
        }
    }
}
