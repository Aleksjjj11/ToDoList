using ToDoList.ViewModel;
using Xamarin.Forms;

namespace ToDoList
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            if (MainPage.BindingContext is MainViewModel viewModel)
            {
                viewModel.SaveTodoListInPreferences();
            }
        }

        protected override void OnResume()
        {
            if (MainPage.BindingContext is MainViewModel viewModel)
            {
                viewModel.SaveTodoListInPreferences();
            }
        }
    }
}
