using System;
using System.Threading.Tasks;
using ToDoList.ViewModel;
using Xamarin.Forms;

namespace ToDoList
{
    public partial class MainPage : ContentPage
    {
        private readonly uint _duration = 400;
        private readonly double _openY = (Device.RuntimePlatform == "Android") ? 20 : 60;

        public MainPage()
        {
            BindingContext = new MainViewModel();
            InitializeComponent();
        }

        private async void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Completed && EditFormDrawer.TranslationY > 0)
            {
                await CloseEditForm();
            }
            else if (e.StatusType == GestureStatus.Completed && EditFormDrawer.TranslationY < 0)
            {
                await OpenEditForm();
            }
            else if (EditFormDrawer.TranslationY > 0)
            {
                EditFormDrawer.TranslationY += e.TotalY;
            }
        }

        private async Task CloseEditForm()
        {
            await Task.WhenAll
            (
                Backdrop.FadeTo(0, length: _duration),
                EditFormDrawer.TranslateTo(0, 460, length: _duration, easing: Easing.Linear)
            );

            Backdrop.InputTransparent = true;
        }

        private async Task OpenEditForm()
        {
            await Task.WhenAll
            (
                Backdrop.FadeTo(1, length: _duration),
                EditFormDrawer.TranslateTo(0, _openY, length: _duration, easing: Easing.Linear)
            );

            Backdrop.InputTransparent = false;
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            await CloseEditForm();
        }

        private async void FrameTapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            if (Backdrop.Opacity == 0)
            {
                await OpenEditForm();
            }
            else
            {
                await CloseEditForm();
            }
        }

        private async void SwipeGestureRecognizer_OnSwiped(object sender, SwipedEventArgs e)
        {
            if (e.Direction == SwipeDirection.Down)
            {
                await CloseEditForm();
            }
        }
        
        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await CloseEditForm();
        }
    }
}
