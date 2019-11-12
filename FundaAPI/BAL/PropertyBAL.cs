using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FundaAPI.Repositories;
using Newtonsoft.Json.Linq;

namespace FundaAPI.BAL
{
    public class PropertyBAL
    {
        public IPropertyRepository propertyRepo;

        public PropertyBAL(IPropertyRepository propRepoObject)
        {
            this.propertyRepo = propRepoObject;
        }

        public object GetProperties(string type, string city, string extra, int page)
        {
            JArray allObjects = new JArray();
            string exception = null;
            GetPropertiesRecursively(type, city, extra, page, ref allObjects, ref exception);

            if (allObjects != null)
                return (from x in allObjects.Select(i => (string)i["MakelaarNaam"]) group x by x into y orderby y.Count() descending select new { Name = y.Key, Count = y.Count() }).Take(10);
            else
                return exception;
        }

        private void GetPropertiesRecursively(string type, string city, string extra, int page, ref JArray propObjects, ref string exception)
        {
            var results = propertyRepo.GetProperties(type, city, extra, page);
            if (!string.IsNullOrEmpty(results.Item2))
            {
                exception = results.Item2;
                propObjects = null;
                goto exit;
            }
            else
            {
                page++;
                JObject resultObject = JObject.Parse(results.Item1);
                JArray objects = (JArray)resultObject["Objects"];
                for (int j = 0; j < objects.Count; j++)
                {
                    propObjects.Add(objects[j]);
                }
                int totalPages = (int)resultObject["Paging"]["AantalPaginas"];

                #region delay the thread so that API won't be rejected when the number of request goes more than 100
                // commenting the below if condition will lead to exception as the number of requests reaches more than 100 within a minute 
                // and the exception will be thrown to the user
                // delaying the thread might not be the right way but this makes sure the output is shown
                if (page % 100 == 0)
                    System.Threading.Thread.Sleep(60000);
                #endregion

                if (totalPages > 0 && page <= totalPages)
                {
                    GetPropertiesRecursively(type, city, extra, page, ref propObjects, ref exception);
                }
            }
            exit:            
            return;
        }
    }
}