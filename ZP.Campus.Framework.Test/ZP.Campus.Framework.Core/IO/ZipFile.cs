using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace ZP.Campus.Framework.Core.IO
{
    class ZipFile
    {
        public ZipFile(string fileToZip, string sourceDirectory)
        {
            //如果文件没有找到，则报错   
            if (!System.IO.Directory.Exists(sourceDirectory))
            {
                throw new System.IO.FileNotFoundException("The specified file " + sourceDirectory + " could not be found. Zipping aborderd");
            }
            ZipConstants.DefaultCodePage = 936;
            var zip = new FastZip();
            zip.CreateZip(fileToZip, sourceDirectory, true, "");
        }
    }
}
