using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using Sherpany_UWP_Code_Challange.Interfaces;

namespace Sherpany_UWP_Code_Challange.Services
{
    class DialogServiceEx : GalaSoft.MvvmLight.Views.DialogService, IDialogServiceEx
    {
        private readonly Dictionary<string, Type> _classDictionary = new Dictionary<string, Type>();
        public async Task<bool?> ShowDialogAsync(string key, object parameter)
        {
            if (!_classDictionary.ContainsKey(key))
                throw new ArgumentException($"Key {key} not registered");
            var dialog = ServiceLocator.Current.GetInstance(_classDictionary[key]) as IDialog;
            if (dialog == null)
                return null;
            return await dialog.ShowAsync(parameter);
        }

        public void Configure(string key, Type viewType)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be empty");
            if (!viewType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IDialog)))
                throw new ArgumentException($"Type {viewType} must implement IDialog");
            if (_classDictionary.ContainsKey(key))
                _classDictionary[key] = viewType;
            else
                _classDictionary.Add(key, viewType);
        }
    }
}
