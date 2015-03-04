using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZP.Campus.Framework.Core.IO
{
    public class DirectoryExtension
    {
        /// <summary>
        /// 获取一个目录下所有文件路径(包括所有子目录)
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static List<string> GetFilePathsContainAllSubDirectories(string directoryPath)
        {
            return GetFilePathsContainAllSubDirectories(directoryPath);
        }

        /// <summary>
        /// 搜索并获取一个目录下的文件路径(包括所有子目录)
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static List<string> GetFilePathsContainAllSubDirectories(string directoryPath, string searchPattern)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            return GetFilePathsContainAllSubDirectories(directoryInfo, searchPattern);
        }

        /// <summary>
        /// 获取一个目录下的文件路径(包括所有子目录)
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        public static List<string> GetFilePathsContainAllSubDirectories(DirectoryInfo directoryInfo)
        {
            return GetFilePathsContainAllSubDirectories(directoryInfo, "");
        }

        /// <summary>
        /// 搜索并获取一个目录下的文件路径(包括所有子目录)
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static List<string> GetFilePathsContainAllSubDirectories(DirectoryInfo directoryInfo, string searchPattern)
        {
            List<string> fileList = new List<string>();
            List<FileInfo> fileInfoList = GetFilesContainAllSubDirectories(directoryInfo, searchPattern);
            foreach (FileInfo fileInfo in fileInfoList)
            {
                fileList.Add(fileInfo.FullName);
            }

            return fileList;
        }

        /// <summary>
        /// 搜索并获取一个目录下的所有文件(包括所有子目录)
        /// </summary>
        /// <param name="directoryPath">当前目录路径</param>
        /// <returns></returns>
        public static List<FileInfo> GetFilesContainAllSubDirectories(string directoryPath)
        {
            return GetFilesContainAllSubDirectories(directoryPath, "");
        }

        /// <summary>
        /// 搜索并获取一个目录下的文件(包括所有子目录)
        /// </summary>
        /// <param name="directoryPath">当前目录路径</param>
        /// <param name="searchPattern">文件搜索模式</param>
        /// <returns></returns>
        public static List<FileInfo> GetFilesContainAllSubDirectories(string directoryPath, string searchPattern)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            return GetFilesContainAllSubDirectories(directoryInfo, searchPattern);
        }

        /// <summary>
        /// 搜索并获取一个目录下的所有文件(包括所有子目录)
        /// </summary>
        /// <param name="directoryInfo">当前目录</param>
        /// <returns></returns>
        public static List<FileInfo> GetFilesContainAllSubDirectories(DirectoryInfo directoryInfo)
        {
            return GetFilesContainAllSubDirectories(directoryInfo, "");
        }

        /// <summary>
        /// 搜索并获取一个目录下的文件(包括所有子目录)
        /// </summary>
        /// <param name="directoryInfo">当前目录</param>
        /// <param name="searchPattern">文件搜索模式</param>
        /// <returns></returns>
        public static List<FileInfo> GetFilesContainAllSubDirectories(DirectoryInfo directoryInfo, string searchPattern)
        {
            List<FileInfo> fileList = new List<FileInfo>();
            if (directoryInfo.Exists)
            {
                Stack<DirectoryInfo> directoryStack = new Stack<DirectoryInfo>();
                DirectoryInfo parentDir = directoryInfo;
                directoryStack.Push(parentDir);

                while (directoryStack.Count > 0)
                {
                    // 获取目录表
                    foreach (DirectoryInfo dir in parentDir.GetDirectories())
                    {
                        directoryStack.Push(dir);
                    }

                    // 出栈, 获取文件信息
                    if (directoryStack.Count > 0)
                    {
                        DirectoryInfo subDir = directoryStack.Peek();

                        // 列出当前目录文件
                        fileList.AddRange(GetFiles(parentDir, searchPattern));

                        parentDir = subDir;
                        directoryStack.Pop();
                    }
                }
            }

            return fileList;
        }

        /// <summary>
        /// 取当前目录下的所有文件
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static List<FileInfo> GetFiles(string directoryPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            return GetFiles(directoryInfo);
        }

        /// <summary>
        /// 取当前目录下的文件
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static List<FileInfo> GetFiles(string directoryPath, string searchPattern)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            return GetFiles(directoryInfo, searchPattern);
        }

        /// <summary>
        /// 取当前目录下的所有文件
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        public static List<FileInfo> GetFiles(DirectoryInfo directoryInfo)
        {
            return GetFiles(directoryInfo, "");
        }

        /// <summary>
        /// 取当前目录下的文件
        /// </summary>
        /// <param name="directoryInfo">当前目录</param>
        /// <param name="searchPattern">文件匹配模式</param>
        /// <returns></returns>
        public static List<FileInfo> GetFiles(DirectoryInfo directoryInfo, string searchPattern)
        {
            List<FileInfo> fileList = new List<FileInfo>();
            if (!directoryInfo.Exists)
               // throw new DirectoryNotFoundException(string.Format(Resources.Culture, Resources.DirectoryNotFound_DirectoryName, directoryInfo.Name));

            if (!string.IsNullOrEmpty(searchPattern))
            { // 按模式进行匹配
                foreach (FileInfo file in directoryInfo.GetFiles(searchPattern))
                {
                    fileList.Add(file);
                }
            }
            else
            { // 不按模式进行匹配
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    fileList.Add(file);
                }
            }

            return fileList;
        }
    }
}
