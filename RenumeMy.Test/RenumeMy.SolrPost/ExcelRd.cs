using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Basement.Framework.Excels;

namespace RenumeMy.SolrPost
{
    public class ExcelRd
    {
        public IList<Resume> GetResumeList()
        {
            List<Resume> list = new List<Resume>();
            string path = ConfigurationManager.AppSettings["ExcelFilePath"];
            ExcelExtension ex = new ExcelExtension(path);
            DataTable dt = ex.ReadExcelAsDataTable(false);
            if (dt != null && dt.Rows.Count > 0)
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (i != 0)
                    {
                        list.Add(new Resume
                        {
                            UserMasterId = Convert.ToInt64(dr[0]),
                            RootCompanyId = Convert.ToInt64(dr[1])
                        });
                    }
                    i++;
                }
            }
            return list;
        }
    }
}
