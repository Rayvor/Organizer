using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.ViewModel;
using System.ComponentModel;
using System.Collections;

namespace FriendOrganizer.UI.Wrapper
{
    class FriendWrapper : ViewModelBase, INotifyDataErrorInfo
    {
        public FriendWrapper(Friend model)
        {
            Model = model;
        }

        public Friend Model { get; }

        public int Id { get { return Model.Id; } }

        public string FirstName
        {
            get { return Model.FirstName; }
            set
            {
                Model.FirstName = value;
                OnPropertyChanged();
                ValidateProperty(nameof(FirstName));
            }
        }

        private void ValidateProperty(string propertyName)
        {
            ClearErrors(propertyName);
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, "Robot", StringComparison.OrdinalIgnoreCase))
                    {
                        AddError(propertyName, "Robots are not valid friends");
                    }
                    break;
            }
        }

        public string LastName
        {
            get { return Model.LastName; }
            set
            {
                Model.LastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return Model.Email; }
            set
            {
                Model.Email = value;
                OnPropertyChanged();
            }
        }

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

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void AddError(string propertyName, string error)
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

        private void ClearErrors(string propertyName)
        {
            if (_errorsByProperyname.ContainsKey(propertyName))
            {
                _errorsByProperyname.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
