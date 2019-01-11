using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Sherpany_UWP_Code_Challenge.Model;

namespace Sherpany_UWP_Code_Challenge.View.Dialog
{
    public sealed partial class ConfirmActionDialog
    {
        private bool? _result;
        public ConfirmActionDialog()
        {
            InitializeComponent();
            DataContext = this as FrameworkElement;
        }

        public override async Task<bool?> ShowAsync(object parameter)
        {
            //_viewModel.Init((RoomViewModel) parameter);
            if (parameter is ConfirmActionParameters confirmActionParameters)
            {
                TitleTextBox.Text = confirmActionParameters.Title;
                ContentTextBox.Text = confirmActionParameters.Text;
                ActionButtonText.Text = confirmActionParameters.ActionButtonText;
                CancelButton.Content = confirmActionParameters.CancelButtonText;
                RedActionButtonText.Text = confirmActionParameters.ActionButtonText;
                if (confirmActionParameters.RedCancelButton)
                {
                    RedActionButton.Visibility = Visibility.Visible;
                    ActionButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    RedActionButton.Visibility = Visibility.Collapsed;
                    ActionButton.Visibility = Visibility.Visible;
                }

                _result = await base.ShowAsync(parameter);
                return _result;
            }
            return _result;
        }


        private void TextBoxOnFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text.Length > 0)
                {
                    textBox.SelectionStart = textBox.Text.Length;
                    textBox.SelectionLength = 0;
                }
            }
        }
    }
}