using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using APM2.Entities;
using APM2.Postgres;
using APM2.Tools;
using Org.BouncyCastle.Security;

namespace APM2
{
    public enum Role
    {
        Guest,
        Operator,
        Admin
    }

    public enum Mode
    {
        Add,
        Edit
    }

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        Random rndSalt;

        public MainWindow()
        {
            InitializeComponent();

            postgersConnector = new PostgersConnector("Host=localhost;Port=5432;Database=APM;Username=postgres;Password=1234");

            Users = new ObservableCollection<User>(postgersConnector.UsersTable.GetUsers());

            LogInCommand = new RelayCommand(LogIn);
            SignUpCommand = new RelayCommand(SignUp);
            GuestEnterCommand = new RelayCommand(GuestEnter);

            Login = "";
            Password = "";

            rndSalt = new Random();

            DataContext = this;
        }

        PostgersConnector postgersConnector;

        public static ObservableCollection<User> Users { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public RelayCommand LogInCommand { get; set; }
        public RelayCommand SignUpCommand { get; set; }
        public RelayCommand GuestEnterCommand { get; set; }

        public void LogIn()
        {
            if(Login == string.Empty || Password == string.Empty)
            {
                MessageBox.Show("Введите данные полностью");
                return;
            }
            foreach(var user in Users)
                if(user.Name == Login.Trim() && user.Password == Password)
                {
                    if(user.Role_is_admin)
                    {
                        var adminAPM = new DGV(Role.Admin);
                        adminAPM.Show();
                        Close();
                    }
                    else
                    {
                        var moderatorAPM = new DGV(Role.Operator);
                        moderatorAPM.Show();
                        Close();
                    }
                    return;
                }
            MessageBox.Show("Пользователь с таким логином и паролем не найден");
        }

        public void SignUp()
        {
            if (Login == string.Empty || Password == string.Empty)
            {
                MessageBox.Show("Введите данные полностью");
                return;
            }
            foreach (var user in Users)
                if (user.Name == Login.Trim())
                {
                    MessageBox.Show("Пользователь с таким логином уже есть");
                    return;
                }
            if (!CheckPass())
                return;


            var newUser = new User() {Name = Login, Password = Password, Role_is_admin = false };

            newUser.Salt = TakeSalt();
            newUser.Password_byte = CalcHash(newUser.Salt);

            postgersConnector.UsersTable.Insert(newUser);

            Users.Add(newUser);

            MessageBox.Show("Регистрация прошла успешно");

            Login = Password = string.Empty;
            OnPropertyChanged(nameof(Login));
            OnPropertyChanged(nameof(Password));
        }

        public byte[] CalcHash(string salt)
        {
            byte[] input = Encoding.Default.GetBytes(Password + salt);
            return DigestUtilities.CalculateDigest("GOST3411-2012-256", input);
        }

        public string TakeSalt()
        {
            var salt = new StringBuilder();
            for (int i = 0; i < 25; i++)
            {
                salt.Append(rndSalt.Next() % 10);
                salt.Append('A' + rndSalt.Next() % 10);
            }
            return salt.ToString();
        }

        public bool CheckPass()
        {
            var spaceC = from symb in Password
                         where (symb == ' ' || symb == '\n' || symb == '\t')
                         select symb;
            int s = 0;
            foreach (var el in spaceC)
                s++;
            if(Password.Length - s < 8)
            {
                MessageBox.Show("Пароль должен содержать 8 не пробельных символов");
                return false;
            }
            return true;
        }

        public void GuestEnter()
        {
            var guestAPM = new DGV(Role.Guest);
            guestAPM.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
