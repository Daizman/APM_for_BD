using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace APM2.Entities
{
    public class User : INotifyPropertyChanged
    {
        string name;
        string password;
        string salt;
        public int Id { get; set; }
        public byte[] Password_byte { get; set; }
        public bool Role_is_admin { get; set; }

        public User(int id, string name, string password, byte[] password_byte, string salt, bool role_is_admin)
        {
            Id = id;
            Name = name;
            Password = password;
            Password_byte = password_byte;
            Salt = salt;
            Role_is_admin = role_is_admin;
        }

        public User()
        {
            Id = -1;
            name = salt = password = "";
            Password_byte = null;
            Role_is_admin = false;
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

        public string Password
        {
            get => password;
            set
            {
                password = value?.Trim();
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Salt
        {
            get => salt;
            set
            {
                salt = value?.Trim();
                OnPropertyChanged(nameof(Salt));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
