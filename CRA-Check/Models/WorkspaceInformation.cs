using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRA_Check.Models
{
    /// <summary>
    /// Data model for workspace information
    /// </summary>
    public class WorkspaceInformation : INotifyPropertyChanged
    {
        /// <summary>
        /// Database ID
        /// </summary>
        public int Id { get; set; } = 1;

        /// <summary>
        /// Workspace name
        /// </summary>
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
