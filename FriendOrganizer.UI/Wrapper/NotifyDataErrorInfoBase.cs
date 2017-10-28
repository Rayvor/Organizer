using System;
using System.Collections.Generic;
using System.Linq;
using FriendOrganizer.UI.ViewModel;
using System.ComponentModel;
using System.Collections;

namespace FriendOrganizer.UI.Wrapper
{
    public class NotifyDataErrorInfoBase : ViewModelBase, INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> _errorsByProperyname
       = new Dictionary<string, List<string>>();

        public bool HasErrors => _errorsByProperyname.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByProperyname.ContainsKey(propertyName)
                ? _errorsByProperyname[propertyName]
                : null;
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            base.OnPropertyChanged(nameof(HasErrors));
        }

        protected void AddError(string propertyName, string error)
        {
            if (!_errorsByProperyname.ContainsKey(propertyName))
            {
                _errorsByProperyname[propertyName] = new List<string>();
            }
            if (!_errorsByProperyname[propertyName].Contains(error))
            {
                _errorsByProperyname[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearErrors(string propertyName)
        {
            if (_errorsByProperyname.ContainsKey(propertyName))
            {
                _errorsByProperyname.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
