using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Interest.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
            //string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //string fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            string productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

            _productName = "Interest Calculator";
            _aboutText = $"About";
            _version = $"Version {productVersion}";
            _copyright = "All rights reserved";
            _author = "David Schaefer";
            _email = "codingdave@gmail.com";
            _description = $"The {_productName} helps understanding costs for interests and helps in creating payment plans";
        }

        #region AboutText
        private string _aboutText;

        public string AboutText
        {
            get { return _aboutText; }
            set { _ = SetProperty(ref _aboutText, value); }
        }
        #endregion

        #region ProductName
        private string _productName;

        public string ProductName
        {
            get { return _productName; }
            set { _ = SetProperty(ref _productName, value); }
        }
        #endregion

        #region Version
        private string _version;

        public string Version
        {
            get { return _version; }
            set { _ = SetProperty(ref _version, value); }
        }
        #endregion

        #region Copyright
        private string _copyright;

        public string Copyright
        {
            get { return _copyright; }
            set { _ = SetProperty(ref _copyright, value); }
        }
        #endregion

        #region Author
        private string _author;

        public string Author
        {
            get { return _author; }
            set { _ = SetProperty(ref _author, value); }
        }
        #endregion

        #region Email
        private string _email;

        public string Email
        {
            get { return _email; }
            set { _ = SetProperty(ref _email, value); }
        }
        #endregion

        #region Description
        private string _description;

        public string Description
        {
            get { return _description; }
            set { _ = SetProperty(ref _description, value); }
        }
        #endregion

        #region AuthorInfo
        public string AuthorInfo
        {
            get { return $"{_author}, {_email}"; }
        }
        #endregion
    }
}
