using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Interest.ViewModels
{
    public class DataErrorViewModelBase : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        public bool HasErrors { get { return _validationErrors.Count > 0; } }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            IEnumerable ret = default;
            if (_validationErrors.ContainsKey(propertyName))
            {
                ret = _validationErrors[propertyName];
            }
            return ret;
        }

        protected override bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            var ret = base.SetProperty(ref backingField, value, propertyName);
            ret = ret && ValidateProperty(propertyName, value);
            return ret;
        }

        private bool ValidateProperty<T>(string propertyName, T value)
        {
            var validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new(this)
            {
                MemberName = propertyName
            };
            var success = Validator.TryValidateProperty(value, validationContext, validationResults);

            if (!success)
            {
                _validationErrors[propertyName] = validationResults.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                _ = _validationErrors.Remove(propertyName);
            }
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            return success;
        }
    }
}
