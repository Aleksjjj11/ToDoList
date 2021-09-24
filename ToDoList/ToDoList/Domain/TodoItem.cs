using Newtonsoft.Json;
using Xamarin.Forms;

namespace ToDoList.Domain
{
    public class TodoItem : BindableObject
    {
        private string _description;
        private bool _isDone;
        private string _title;

        [JsonProperty("description")]
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        [JsonProperty("isDone")]
        public bool IsDone
        {
            get => _isDone;
            set
            {
                _isDone = value;
                OnPropertyChanged(nameof(IsDone));
            }
        }

        [JsonProperty("title")]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }
}