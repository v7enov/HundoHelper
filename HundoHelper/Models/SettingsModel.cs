using System.ComponentModel;

namespace HundoHelper.Models
{
    public class SettingsModel : INotifyPropertyChanged
    {
        private bool tbdCounterEnabled;
        private bool readLastMission;

        public bool TbdCounterEnabled {
            get => tbdCounterEnabled; 
            set {
                if (tbdCounterEnabled != value)
                {
                    tbdCounterEnabled = value;
                    OnPropertyChanged(nameof(TbdCounterEnabled));
                }
            }
        }

        public bool ReadLastMission {
            get => readLastMission;
            set {
                if (readLastMission != value)
                {
                    readLastMission = value;
                    OnPropertyChanged(nameof(ReadLastMission));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
