using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Callisto.Controls;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Sherpany_UWP_Code_Challange.Interfaces;
using Sherpany_UWP_Code_Challange.Messages;
using WinRTXamlToolkit.Controls.Extensions;

namespace Sherpany_UWP_Code_Challange.View.Controls
{
    /// <summary>
    ///     extends the Callisto CustomDialog with the behavior to align to Top, when the softkeyboard opens and reverses this
    ///     when the keyboard closes
    /// </summary>
    public class AutoAligningDialog : CustomDialog, IDialog
    {
        protected object Parameter;
        private TaskCompletionSource<bool?> _taskCompletionSource;
        private const int DefaultAnimationTime = 200;

        public static readonly DependencyProperty BackgroundImageBrushProperty = DependencyProperty.Register(
            "BackgroundImageBrush", typeof(ImageBrush), typeof(AutoAligningDialog), new PropertyMetadata(default(ImageBrush)));

        private double _lastHeight;

        public ImageBrush BackgroundImageBrush
        {
            get { return (ImageBrush)GetValue(BackgroundImageBrushProperty); }
            set { SetValue(BackgroundImageBrushProperty, value); }
        }

        public static readonly DependencyProperty EnableBottomTransitionProperty = DependencyProperty.Register(
            "EnableBottomTransition", typeof(bool), typeof(AutoAligningDialog), new PropertyMetadata(false));

        public bool EnableBottomTransition
        {
            get { return (bool)GetValue(EnableBottomTransitionProperty); }
            set { SetValue(EnableBottomTransitionProperty, value); }
        }

        public static readonly DependencyProperty EnableCancelButtonProperty = DependencyProperty.Register(
            "EnableCancelButton", typeof(bool), typeof(AutoAligningDialog), new PropertyMetadata(false));

        private Panel _rootGrid;
        private ContentPresenter _content;

        public bool EnableCancelButton
        {
            get { return (bool)GetValue(EnableCancelButtonProperty); }
            set { SetValue(EnableCancelButtonProperty, value); }
        }
        

        public static readonly DependencyProperty DroppedObjectProperty = DependencyProperty.Register(
            "DroppedObject", typeof(object), typeof(AutoAligningDialog), new PropertyMetadata(default(object)));

        public object DroppedObject
        {
            get { return (object)GetValue(DroppedObjectProperty); }
            set { SetValue(DroppedObjectProperty, value); }
        }

        public static readonly DependencyProperty FileDropEnabledProperty = DependencyProperty.Register(
            "FileDropEnabled", typeof(bool), typeof(AutoAligningDialog), new PropertyMetadata(default(bool)));

        public bool FileDropEnabled
        {
            get { return (bool)GetValue(FileDropEnabledProperty); }
            set { SetValue(FileDropEnabledProperty, value); }
        }

        public virtual async Task<bool?> ShowAsync(object parameter)
        {
            Messenger.Default.Register<DialogCloseMessage>(this, async (m) => await DoDialogClose(m));
            Parameter = parameter;
            OnApplyTemplate();
            var input = InputPane.GetForCurrentView();
            input.Showing += ShowingHandler;
            input.Hiding += HidingHandler;
            _taskCompletionSource = new TaskCompletionSource<bool?>();
            IsOpen = true;
            SetTitle();
            AddFooterAndHeader();
            AlignDialogToCenter();
            var result = await _taskCompletionSource.Task;
            IsOpen = false;
            input.Showing -= ShowingHandler;
            input.Hiding -= HidingHandler;
            return result;
        }

        private void SetTitle()
        {
            //var root = GetTemplateChild("PART_Title") as Panel;
        }

        private void AddFooterAndHeader()
        {
            var root = GetTemplateChild("PART_RootGrid") as Panel;

            if (EnableCancelButton)
            {
                var cancelButton = new Button()
                {
                    Content = "Cancel",
                    Background = new SolidColorBrush(new Color() { A = 0 }),
                    Margin = new Thickness() { Left = 15, Top = 15 },
                    Command = CloseDialogCommand,
                    VerticalAlignment = VerticalAlignment.Top
                };
                root?.Children.Add(cancelButton);
            }

            if (root != null && FileDropEnabled)
            {
                root.AllowDrop = true;
                root.Drop += DropDocument;
                root.DragOver += OnFileDragOver;
            }
            

            if (root != null && BackgroundImageBrush != null)
            {
                BackgroundImageBrush.Stretch = Stretch.UniformToFill;
                BackgroundImageBrush.AlignmentX = AlignmentX.Center;
                _rootGrid.Children.Insert(0, new Grid() { Background = BackgroundImageBrush, Name = "BackgroundGrid" });
            }
            else
            {
                _rootGrid.Children.Insert(0, new Grid() { Background = new SolidColorBrush(Color.FromArgb(153, 0, 0, 0)), Name = "BackgroundGrid" });

            }
        }

        public ICommand CloseDialogCommand => new RelayCommand<string>(async (b) => await DoDialogClose(new DialogCloseMessage(b == "True")));

        private async Task DoDialogClose(DialogCloseMessage msg)
        {
            Messenger.Default.Unregister<DialogCloseMessage>(this);
            if (EnableBottomTransition)
            {
                await _rootGrid.Offset(offsetX: 0, offsetY: (float)_rootGrid.Height, duration: DefaultAnimationTime, delay: 0, easingType: EasingType.Cubic).StartAsync();
            }
            else
            {
                await _rootGrid.Fade(value: 0f, duration: DefaultAnimationTime, delay: 0, easingType: EasingType.Default).StartAsync();
            }
            if (GetTemplateChild("PART_RootGrid") is Panel root && FileDropEnabled)
            {
                root.AllowDrop = false;
                root.Drop -= DropDocument;
                root.DragOver -= OnFileDragOver;
            }

            //Remove background again, otherwise it will be added on top of the old one OBR-5261
            if (_rootGrid?.GetChildren()?.First() is Grid background && background.Name == "BackgroundGrid")
            {
                _rootGrid.Children.Remove(background);
            }

            _taskCompletionSource.SetResult(msg.Result);
        }


        private void ResizeContainers()
        {
            if (_rootGrid != null)
            {
                _content.MaxWidth = Window.Current.Bounds.Width - 40;
                _content.MaxHeight = Window.Current.Bounds.Height - 100;
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _rootGrid = GetTemplateChild("PART_RootGrid") as Panel;
            _content = GetTemplateChild("PART_Content") as ContentPresenter;
            if (_rootGrid != null)
            {
                if (EnableBottomTransition)
                {
                    _rootGrid.Offset(offsetX: 0, offsetY: (float)_rootGrid.Height, duration: 0, delay: 0, easingType: EasingType.Cubic).Then().Offset(offsetX: 0, offsetY: 0, duration: 200, delay: 0, easingType: EasingType.Cubic).Start();
                }
                else
                {
                    _rootGrid.Fade(value: 0.0f, duration: 0, delay: 0, easingType: EasingType.Default).Then().Fade(value: 1f, duration: DefaultAnimationTime, delay: 0, easingType: EasingType.Default).Start();
                }


                Window.Current.SizeChanged += ResizeContainers;
                ResizeContainers();
            }
        }

        private void ResizeContainers(object sender, WindowSizeChangedEventArgs e)
        {
            ResizeContainers();
        }

        private void HidingHandler(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            var root = GetTemplateChild("PART_RootGrid") as FrameworkElement;
            if (root != null)
            {
                var newHeight = Window.Current.Bounds.Height;
                root.Height = newHeight > 0 ? newHeight : 0;
            }
            AlignDialogToCenter();
        }

        private void AlignDialogToCenter()
        {
            var element = GetTemplateChild("PART_BannerBorder");
            if (element != null)
                ((FrameworkElement)element).VerticalAlignment = VerticalAlignment.Center;
        }

        private void ShowingHandler(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            var root = GetTemplateChild("PART_RootGrid") as FrameworkElement;
            if (root != null)
            {
                _lastHeight = root.ActualHeight;
                var newHeight = root.ActualHeight - args.OccludedRect.Height;
                root.Height = newHeight > 0 ? newHeight : 0;
            }


            var element = GetTemplateChild("PART_BannerBorder") as FrameworkElement;
            if (element != null)
                ((FrameworkElement)element).VerticalAlignment = VerticalAlignment.Center;
        }


        private void DropDocument(object sender, DragEventArgs e)
        {
            Task.Run(() => DispatcherHelper.RunAsync(async () =>
            {
                if (e.DataView.Contains(StandardDataFormats.StorageItems))
                {
                    var items = await e.DataView.GetStorageItemsAsync();
                    if (items.Count == 1)
                    {
                        DroppedObject = items.First();
                    }
                }
            }));
        }

        private void OnFileDragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;

        }
    }
}