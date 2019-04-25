using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace APM2.Entities
{
    public class Content : INotifyPropertyChanged
    {
        public int Id { get; set; }
        string name;
        string c_type;
        string country;
        public DateTime Release_date { get; set; }
        public int Budget { get; set; }
        public TimeSpan Duration { get; set; }
        string description;
        string dub;

        public Content(int id, string name, string c_type, string country, DateTime release_date, int budget, TimeSpan duration, string description, string dub)
        {
            Id = id;
            Name = name;
            C_type = c_type;
            Country = country;
            Release_date = release_date;
            Budget = budget;
            Duration = duration;
            Description = description;
            Dub = dub;
        }

        public Content()
        {
            Budget = Id = -1;
            name = c_type = country = description = dub = "";
            Release_date = DateTime.Now;
            Duration = TimeSpan.MinValue;
        }

        public string Name
        {
            get => name;
            set
            {
                name = value?.Trim();
                OnPropertyChanged(nameof(Name));
            }
        }

        public string C_type
        {
            get => c_type;
            set
            {
                c_type = value?.Trim();
                OnPropertyChanged(nameof(C_type));
            }
        }

        public string Country
        {
            get => country;
            set
            {
                country = value?.Trim();
                OnPropertyChanged(nameof(Country));
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value?.Trim();
                OnPropertyChanged(nameof(Description));
            }
        }

        public string Dub
        {
            get => dub;
            set
            {
                dub = value?.Trim();
                OnPropertyChanged(nameof(Dub));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
