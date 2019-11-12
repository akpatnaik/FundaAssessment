using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using FundaAPI.Helpers;

namespace FundaAPI.Repositories
{
    public interface IPropertyRepository
    {
        Tuple<string, string> GetProperties(string type, string city, string extra, int page);
    }

    public class PropertyRepository : IPropertyRepository
    {
        public Tuple<string, string> GetProperties(string type, string city, string extra, int page = 1)
        {
            string result = string.Empty;
            string exception = null;
            using (WebClient client = new WebClient())
            {
                try
                {
                    result = client.DownloadString($"{ConfigSettings.FundaAPIURL}?type={type}&zo=/{city}/{extra}/&page={page}&pagesize=25");
                }
                catch (Exception ex)
                {
                    exception = $"Error: {ex.Message}\r\n\r\nStatckTrace: {ex.StackTrace}";
                }
            }
            return Tuple.Create<string, string>(result, exception);
        }
    }
}