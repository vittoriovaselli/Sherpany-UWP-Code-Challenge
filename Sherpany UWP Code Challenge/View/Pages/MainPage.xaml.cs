using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Messaging;
using Sherpany_UWP_Code_Challenge.Messages;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sherpany_UWP_Code_Challenge
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPageView : Page
    {
        public MainPageView()
        {
            this.InitializeComponent();

            Messenger.Default.Register<BeginCloseAnimationMessage>(this, m => CloseAppAnimation.Begin());

            KeyUp += MainPageViewKeyUp;
        }

        private void MainPageViewKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                Messenger.Default.Send(new EnterPressedMessage());
            }
        }

        private void OnDragableGridManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            this.DragableGridTransform.TranslateX += e.Delta.Translation.X;
            this.DragableGridTransform.TranslateY += e.Delta.Translation.Y;
        }

        private void OnDragableGridManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            Button.IsEnabled = false;
        }

        private void OnDragableGridManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Button.IsEnabled = true;
        }
    }
}
