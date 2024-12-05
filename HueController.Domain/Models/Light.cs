using System.ComponentModel;

namespace HueController.Domain.Models
{
    public class Light : INotifyPropertyChanged
    {
        private bool _isOn;
        private int _brightness;
        private int _hue;
        private int _saturation;

        public string Id { get; set; }
        public string Name { get; set; }

        public bool IsOn
        {
            get => _isOn;
            set
            {
                if (_isOn != value)
                {
                    _isOn = value;
                    OnPropertyChanged(nameof(IsOn));
                }
            }
        }

        public int Brightness
        {
            get => _brightness;
            set
            {
                if (_brightness != value)
                {
                    _brightness = value;
                    OnPropertyChanged(nameof(Brightness));
                }
            }
        }

        public int Hue
        {
            get => _hue;
            set
            {
                if (_hue != value)
                {
                    _hue = value;
                    OnPropertyChanged(nameof(Hue));
                }
            }
        }

        public int Saturation
        {
            get => _saturation;
            set
            {
                if (_saturation != value)
                {
                    _saturation = value;
                    OnPropertyChanged(nameof(Saturation));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
