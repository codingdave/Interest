using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interest.ViewModels
{
    public class AboutViewModel : BindableBase
    {
        public AboutViewModel()
        {
            _text = "About this application";
            _productName = "Interest Calculator";
            _version = "1.0.0.0";
            _copyright = "All rights reserved";
            _companyName = "David Schaefer";
            _email = "codingdave@gmail.com";
            _description = "This application helps understanding costs for interests and helps in creating payment plans";
        }

        #region Text
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _ = SetProperty(ref _text, value); }
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

        #region CompanyName
        private string _companyName;

        public string CompanyName
        {
            get { return _companyName; }
            set { _ = SetProperty(ref _companyName, value); }
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

    }
}
