using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApp.Models;

namespace WebApp.Helper
{
    public class XmlReader
    {
        public static List<WordModel> GetWordModelList()
        {
            string xmlData = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/data.xml"); //Path of the xml script  
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);

            return (from rows in ds.Tables[0].AsEnumerable()

                    select new WordModel
                    {
                        word = rows[0].ToString(),
                        count = Convert.ToInt32(rows[1].ToString()), //Convert row to int  
                    }).OrderByDescending(q => q.count).Take(10).ToList();
        }
    }
}