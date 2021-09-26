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
            InitEventHandlers();
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
        
        private void ImageButtonStartEvent_OnClicked(object sender, EventArgs e)
        {
            StartEventDatePicker.Focus();
        }

        private void ImageButtonEndEvent_OnClicked(object sender, EventArgs e)
        {
            EndEventDatePicker.Focus();
        }

        private void InitEventHandlers()
        {
            var viewModel = (MainViewModel)BindingContext;

            StartEventDatePicker.DateSelected += (sender, args) =>
            {
                viewModel.SelectedTodoItem.StartEventDateTime = args.NewDate;
                StartEventTimePicker.Focus();
            };

            EndEventDatePicker.DateSelected += (sender, args) =>
            {
                viewModel.SelectedTodoItem.EndEventDateTime = args.NewDate;
                EndEventTimePicker.Focus();
            };

            StartEventTimePicker.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TimePicker.Time))
                {
                    var time = StartEventTimePicker.Time;

                    if (viewModel.SelectedTodoItem.StartEventDateTime != null)
                    {
                        var startEventDateTime = viewModel.SelectedTodoItem.StartEventDateTime.Value;

                        startEventDateTime = startEventDateTime.AddHours(-startEventDateTime.Hour);
                        startEventDateTime = startEventDateTime.AddMinutes(-startEventDateTime.Minute);

                        startEventDateTime = startEventDateTime.AddHours(time.Hours);
                        startEventDateTime = startEventDateTime.AddMinutes(time.Minutes);

                        viewModel.SelectedTodoItem.StartEventDateTime = startEventDateTime;
                    }
                }
            };

            EndEventTimePicker.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TimePicker.Time))
                {
                    var time = EndEventTimePicker.Time;

                    if (viewModel.SelectedTodoItem.EndEventDateTime != null)
                    {
                        var endEventDateTime = viewModel.SelectedTodoItem.EndEventDateTime.Value;

                        endEventDateTime = endEventDateTime.AddHours(-endEventDateTime.Hour);
                        endEventDateTime = endEventDateTime.AddMinutes(-endEventDateTime.Minute);

                        endEventDateTime = endEventDateTime.AddHours(time.Hours);
                        endEventDateTime = endEventDateTime.AddMinutes(time.Minutes);

                        viewModel.SelectedTodoItem.EndEventDateTime = endEventDateTime;
                    }
                }
            };
        }
    }
}
