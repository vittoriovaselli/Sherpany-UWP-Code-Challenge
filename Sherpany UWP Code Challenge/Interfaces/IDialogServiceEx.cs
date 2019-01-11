using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;

namespace Sherpany_UWP_Code_Challenge.Interfaces
{
    public interface IDialogServiceEx : IDialogService
    {
        Task<bool?> ShowDialogAsync(string key, object parameter);
        void Configure(string key, Type viewType);
    }

    public interface IDialog
    {
        Task<bool?> ShowAsync(object parameter);
    }
}
