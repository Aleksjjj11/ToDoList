using ToDoList.ViewModel;
using Xamarin.Forms;

namespace ToDoList
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            BindingContext = new MainViewModel();
            InitializeComponent();
        }
    }
}
