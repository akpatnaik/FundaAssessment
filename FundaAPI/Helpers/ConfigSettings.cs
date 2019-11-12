using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace FundaAPI.Helpers
{
    public static class ConfigSettings
    {
        private static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string FundaAPIURL
        {
            get
            {
                return GetValue("funda.api");
            }
        }
    }
}