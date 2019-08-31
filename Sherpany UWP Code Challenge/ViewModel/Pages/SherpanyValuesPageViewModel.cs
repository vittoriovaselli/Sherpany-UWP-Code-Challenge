using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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

        private readonly IValuesCacheService _valuesCacheService;

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

        public SherpanyValuesPageViewModel(IDummyApiService apiService, IValuesCacheService valuesCacheService)
        {
            _apiService = apiService;
            _valuesCacheService = valuesCacheService;

        }

        private void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                int order = ((SherpanyValueModel)e.NewItems[0]).Order;
                int newPosition = e.NewStartingIndex;
                int from = Math.Min(order, newPosition);
                int to = Math.Max(order, newPosition);
                while (from <= to)
                {
                    Values[from].Order = from;
                    from++;
                }
                _valuesCacheService.SetData(Values);
            }
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
            
            if (await _valuesCacheService.IsListStored())
            {
                //get the cached list
                Values = (await _valuesCacheService.GetData()).ToObservableCollection();
            }
            else
            {
                //get the list from apis
                Values = (await _apiService.GetValueModelsAsync()).OrderBy(e => e.Order).ToObservableCollection();
                //save data in cache
                _valuesCacheService.SetData(Values);
            }

            Values.CollectionChanged += ContentCollectionChanged;


            IsBusy = false;
        }

    }
}
