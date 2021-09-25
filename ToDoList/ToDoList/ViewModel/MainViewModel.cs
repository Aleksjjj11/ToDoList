using System.Collections.ObjectModel;
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
            SelectedTodoItem = new TodoItem();
            LoadTodoListFromPreferences();
        }

        public ObservableCollection<TodoItem> TodoItems { get; set; }

        private TodoItem _selectedTodoItem;
        public TodoItem SelectedTodoItem
        {
            get => _selectedTodoItem;
            set
            {
                _selectedTodoItem = value;
                OnPropertyChanged(nameof(SelectedTodoItem));
            }
        }

        private int _selectedTodoItemNumber;

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
            await Task.Delay(650);
            TodoItems.Remove(item);
        }));

        private Command _addTodoItemCommand;

        public Command AddTodoItemCommand => _addTodoItemCommand ?? (_addTodoItemCommand = new Command(() =>
        {
            if (string.IsNullOrWhiteSpace(NewTodoText)) return;

            TodoItems.Add(new TodoItem
            {
                Title = NewTodoText,
                Description = string.Empty,
                IsDone = false,
            });

            NewTodoText = string.Empty;
        }));

        private Command _selectTodoItemCommand;

        public Command SelectTodoItemCommand => _selectTodoItemCommand ?? (_selectTodoItemCommand =
            new Command<TodoItem>(
                item =>
                {
                    SelectedTodoItem.Title = item.Title;
                    SelectedTodoItem.Description = item.Description;
                    _selectedTodoItemNumber = TodoItems.IndexOf(item);
                }));

        private Command _resetSelectedTodoItemCommand;

        public Command ResetSelectedTodoItemCommand => _resetSelectedTodoItemCommand ?? (_resetSelectedTodoItemCommand =
            new Command(
                () =>
                {
                    SelectedTodoItem.Title = string.Empty;
                    SelectedTodoItem.Description = string.Empty;
                }));

        private Command _submitChangesCommand;
        public Command SubmitChangesCommand => _submitChangesCommand ?? (_submitChangesCommand = new Command(() =>
        {
            var itemToSubmitChanges = TodoItems[_selectedTodoItemNumber];
            itemToSubmitChanges.Title = SelectedTodoItem.Title;
            itemToSubmitChanges.Description = SelectedTodoItem.Description;
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