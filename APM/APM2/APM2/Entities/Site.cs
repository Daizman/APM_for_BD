using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace APM2.Entities
{
    public class Site : INotifyPropertyChanged
    {
        public int Id { get; set; }
        string name;
        string addr;
        public bool Access { get; set; }
        string description;

        public Site(int id, string name, string addr, bool access, string description)
        {
            Id = id;
            Name = name;
            Addr = addr;
            Access = access;
            Description = description;
        }

        public Site()
        {
            Id = -1;
            name = addr = description = "";
            Access = false;
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

        public string Addr
        {
            get => addr;
            set
            {
                addr = value?.Trim();
                OnPropertyChanged(nameof(Addr));
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
