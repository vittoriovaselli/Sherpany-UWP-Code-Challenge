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
using Sherpany_UWP_Code_Challenge.Interfaces;
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
                ((RelayCommand)SelectedCommand).RaiseCanExecuteChanged();
                
            }
        }

        public string ErrorMessage { get; set; }

        public string TextBoxMessage { get; set; } = "Set a six-digit passcode:";

        public ICommand SelectedCommand { get; set; }

        private readonly INavigationService _navigationService;

        private readonly IDialogServiceEx _dialogService;

        public MainPageViewModel(INavigationService navigationService, IDialogServiceEx dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            CheckIfPinIsSet();

            Messenger.Default.Register<EnterPressedMessage>(this, m =>
            {
                if (SelectedCommand.CanExecute(null))
                {
                    SelectedCommand.Execute(null);
                }
            });
        }

        private void CheckIfPinIsSet()
        {
            var keyManager = new KeyManager();
            if (keyManager.IsKeySet())
            {
                TextBoxMessage = "Please confirm your pin:";
                RaisePropertyChanged(String.Empty);
                SelectedCommand = ValidatePasswordAndNavigateCommand;
            }
            else
            {
                SelectedCommand = SetPasswordAndNavigateCommand;
            }
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
            bool containsChar = String.IsNullOrEmpty(_password) ? false : !_password.All(c => Char.IsDigit(c));

            if (containsChar)
            {
                ErrorMessage = "Pin must contain digits only";
            }
            else
            {
                ErrorMessage = String.Empty;
            }
           
            RaisePropertyChanged(String.Empty);

            return String.IsNullOrEmpty(_password) ? false : (!containsChar && _password.Length == _pinLenght);
        }

        //TODO If a passcode has already been stored, use this to validate and navigate
        private ICommand _validatePasswordAndNavigateCommand;
        public ICommand ValidatePasswordAndNavigateCommand
        {
            get
            {
                if (_validatePasswordAndNavigateCommand == null)
                {
                    _validatePasswordAndNavigateCommand = new RelayCommand(ValidatePasswordAndNavigate,IsPasswordValid);
                }

                return _validatePasswordAndNavigateCommand;
            }
        }

        private void ValidatePasswordAndNavigate()
        {
            var keyManager = new KeyManager();

           
            
            if(keyManager.Hash(_password) == keyManager.GetEncryptionKey(true))
            {
                _navigationService.NavigateTo("SherpanyValuesPageView");
            }
            else
            {
                _dialogService.ShowError("Invalid PIN", "Pin is incorrect", "ok", null);
            }

        }
    }
}
