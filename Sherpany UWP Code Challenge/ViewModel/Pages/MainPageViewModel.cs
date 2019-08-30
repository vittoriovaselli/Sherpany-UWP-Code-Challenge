using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Sherpany_UWP_Code_Challange.Services;
using Sherpany_UWP_Code_Challenge.Messages;


namespace Sherpany_UWP_Code_Challenge.ViewModel.Pages
{
    public class MainPageViewModel: ViewModelBase
    {
        private readonly int _pinLenght = 6;

        private string _password;

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                Set(ref _password, value);
                ((RelayCommand)SetPasswordAndNavigateCommand).RaiseCanExecuteChanged();
            }
        }

        private readonly INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public ICommand ButtonTappedCommand => new RelayCommand(ButtonTapped);

        private async void ButtonTapped()
        {
            Messenger.Default.Send(new BeginCloseAnimationMessage());
            await Task.Delay(2000);
            Messenger.Default.Send(new CloseAppMessage());
        }

        //TODO If no passcode is set in the vault, the user can enter one and will then be navigated to the DetailPageView
        private ICommand _setPasswordAndNavigateCommand;
        public ICommand SetPasswordAndNavigateCommand
        {
            get
            {
                if(_setPasswordAndNavigateCommand == null)
                {
                    _setPasswordAndNavigateCommand = new RelayCommand(SetPasswordAndNavigate, IsPasswordValid);
                }
                return _setPasswordAndNavigateCommand;

            }
        }

        private void SetPasswordAndNavigate()
        {
            //store password
            var keyManager = new KeyManager();
            keyManager.SetEncryptionKey(_password);

            //navigate   
            _navigationService.NavigateTo("SherpanyValuesPageView");
        }

        private bool IsPasswordValid()
        {
            return String.IsNullOrEmpty(_password) ? false : (_password.All(c => Char.IsDigit(c)) && _password.Length == _pinLenght);
        }

        //TODO If a passcode has already been stored, use this to validate and navigate
        public ICommand ValidatePasswordAndNavigateCommand => new RelayCommand<string>(ValidatePasswordAndNavigate);

        private void ValidatePasswordAndNavigate(string password)
        {
            throw new NotImplementedException();
        }
    }
}
