using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interest
{
    internal class ConfigurationManagerReaderWriter
    {
        private Configuration _config;

        public ConfigurationManagerReaderWriter()
        {
            var configurationUserLevel = ConfigurationUserLevel.None;
            _config = ConfigurationManager.OpenExeConfiguration(configurationUserLevel);

            var now = DateTime.Now;
            var startMonth = new DateTime(now.Year, now.Month, 1);
            AddUpdateAppSettings("StartMonth", startMonth.ToString(CultureInfo.InvariantCulture));
            //_config.AppSettings.Settings.Add("Years", "15".ToString(CultureInfo.InvariantCulture));
            //_config.AppSettings.Settings.Add("UnscheduledRepaymentPercentage", "5".ToString(CultureInfo.InvariantCulture));
            //_config.AppSettings.Settings.Add("BorrowingPercentage", ".99".ToString(CultureInfo.InvariantCulture));
            //_config.AppSettings.Settings.Add("RedemptionPercentage", "2.5".ToString(CultureInfo.InvariantCulture));
            //_config.AppSettings.Settings.Add("LoanAmount", "25000".ToString(CultureInfo.InvariantCulture));
            //_config.Save(ConfigurationSaveMode.Modified);
            //conf.SaveAs(fi.FullName, ConfigurationSaveMode.Modified);
            //_config = ConfigurationManager.OpenExeConfiguration(configurationUserLevel);

            //fi.Directory.Create();
            //var configXml = new ConfigXmlDocument();
            //configXml.CreateXmlDeclaration(version: "1.0", encoding: "utf-8", standalone: "yes");
            //configXml.CreateNode(type: System.Xml.XmlNodeType.Element, name: "configuration", namespaceURI: "");
            //configXml.Save(fi.FullName);
            //fi.Create();
            //ConfigurationManager.AppSettings.
            //_configFile.Save(ConfigurationSaveMode.Full);
            ReadAllSettings();
        }

        internal void GenerateDefaultValues(string key, string value)
        {
            try
            {
                var settings = _config.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                _config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(_config.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException e)
            {
                System.Diagnostics.Debug.WriteLine("Error writing app settings: " + e.Message);
            }
        }

        static void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("AppSettings is empty.");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        System.Diagnostics.Debug.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException e)
            {
                System.Diagnostics.Debug.WriteLine("Error writing app settings: " + e.Message);
            }
        }

        static void ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                System.Diagnostics.Debug.WriteLine(result);
            }
            catch (ConfigurationErrorsException e)
            {
                System.Diagnostics.Debug.WriteLine("Error writing app settings: " + e.Message);
            }
        }

        internal string GetValue(string key)
        {
            var ret = string.Empty;
            try
            {
                var settings = _config.AppSettings.Settings;
                ret = settings[key].Value;
            }
            catch (ConfigurationErrorsException e)
            {
                System.Diagnostics.Debug.WriteLine("Error writing app settings: " + e.Message);
            }

            return ret;
        }

        internal void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var settings = _config.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                _config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(_config.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException e)
            {
                System.Diagnostics.Debug.WriteLine("Error writing app settings: " + e.Message);
            }
        }
    }
}
