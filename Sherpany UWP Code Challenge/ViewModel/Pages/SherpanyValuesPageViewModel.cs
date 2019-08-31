using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Sherpany_UWP_Code_Challange.Common;
using Sherpany_UWP_Code_Challange.Interfaces;
using Sherpany_UWP_Code_Challange.Model;
using Sherpany_UWP_Code_Challange.Services;

namespace Sherpany_UWP_Code_Challenge.ViewModel.Pages
{
    public class SherpanyValuesPageViewModel: ViewModelBase
    {
        private readonly IDummyApiService _apiService;

        private bool _isBusy = false;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                Set(ref _isBusy, value);
                ((RelayCommand)GetValuesCommand).RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<SherpanyValueModel> _values;

        public ObservableCollection<SherpanyValueModel> Values
        {
            get
            {
                return _values;
            }
            set
            {
                Set(ref _values, value);
            }
        }

        private int _selectedValueIndex =-1;

        public int SelectedValueIndex
        {
            get
            {
                return _selectedValueIndex;
            }
            set
            {
                Set(ref _selectedValueIndex, value);
                //do not update title, description, claim if no item is selected
                if (value != -1)
                {
                    RaisePropertyChanged("SelectedTitle");
                    RaisePropertyChanged("SelectedClaim");
                    RaisePropertyChanged("SelectedDescription");
                }             
            }
        }

        public string SelectedTitle
        {
            get
            {
                return _values?[_selectedValueIndex].Title;
            }
        }

        public string SelectedClaim
        {
            get
            {
                return _values?[_selectedValueIndex].Claim;
            }
        }

        public string SelectedDescription
        {
            get
            {
                return _values?[_selectedValueIndex].Description;
            }
        }

        public SherpanyValuesPageViewModel(IDummyApiService apiService)
        {
            _apiService = apiService;
        }

        private ICommand _getValuesCommand;

        public ICommand GetValuesCommand
        {
            get
            {
                if(_getValuesCommand == null)
                {
                    _getValuesCommand = new RelayCommand(GetValues, ()=> !IsBusy );
                }

                return _getValuesCommand;
            }
        }

        private async void GetValues()
        {
            IsBusy = true;

            var valuesCacheService = new ValuesCacheService();
            
            if (await valuesCacheService.IsListSaved())
            {
                //get the cached list
                Values = (await valuesCacheService.GetData()).ToObservableCollection();
            }
            else
            {
                //get the list from apis
                Values = (await _apiService.GetValueModelsAsync()).OrderBy(e => e.Order).ToObservableCollection();
                //save data in cache
                valuesCacheService.SetData(Values);
            }

            IsBusy = false;
        }

    }
}
