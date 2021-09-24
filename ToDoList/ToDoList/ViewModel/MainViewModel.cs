using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToDoList.Domain;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ToDoList.ViewModel
{
    public class MainViewModel : BindableObject
    {
        public MainViewModel()
        {
            TodoItems = new ObservableCollection<TodoItem>();
            LoadTodoListFromPreferences();
        }

        public ObservableCollection<TodoItem> TodoItems { get; set; }

        private string _newTodoText;
        public string NewTodoText
        {
            get => _newTodoText;
            set
            {
                _newTodoText = value;
                OnPropertyChanged(nameof(NewTodoText));
            }
        }

        private Command _deleteTodoItemCommand;
        public Command DeleteTodoItemCommand => _deleteTodoItemCommand ?? (_deleteTodoItemCommand = new Command<TodoItem>(async item =>
        {
            await Task.Delay(710);
            TodoItems.Remove(item);
        }));

        private Command _addTodoItemCommand;

        public Command AddTodoItemCommand => _addTodoItemCommand ?? (_addTodoItemCommand = new Command(() =>
        {
            TodoItems.Add(new TodoItem
            {
                Description = NewTodoText,
                IsDone = false,
            });

            NewTodoText = string.Empty;
        }));

        public void SaveTodoListInPreferences()
        {
            Preferences.Set("todoList", JsonConvert.SerializeObject(TodoItems));
        }

        public void LoadTodoListFromPreferences()
        {
            var itemsInJson = Preferences.Get("todoList", "[]");
            TodoItems = JsonConvert.DeserializeObject<ObservableCollection<TodoItem>>(itemsInJson);
            OnPropertyChanged(nameof(TodoItems));
        }
    }
}