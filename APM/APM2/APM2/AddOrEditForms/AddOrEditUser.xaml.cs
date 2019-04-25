using APM2.Entities;
using APM2.Tools;
using Org.BouncyCastle.Security;
using System;
using System.Linq;
using System.Text;
using System.Windows;

namespace APM2.AddOrEditForms
{
    public partial class AddOrEditUser : Window
    {
        public User user;

        Mode curMode;
        string login;

        Random rndSalt;

        public AddOrEditUser(Mode mode, User user = null)
        {
            InitializeComponent();

            curMode = mode;

            if (mode == Mode.Add)
            {
                ModeOpen = "Add user";
                CurModeUser = "Add";
                this.user = new User();
                SName = "";
                Password = "";
                Admin = false;
                Salt = "";
            }
            else
            {
                ModeOpen = "Edit user";
                CurModeUser = "Edit";
                InitializeForm(user);
                this.user = user;
                login = user.Name;
            }

            rndSalt = new Random();
            AddOrEditUserCommand = new RelayCommand(AddOrEdit);

            DataContext = this;
        }

        private void InitializeForm(User user)
        {
            SName = user.Name;
            Password = user.Password;
            Admin = user.Role_is_admin;
            Salt = user.Salt;
        }

        public string ModeOpen { get; set; }
        public string CurModeUser { get; set; }

        public string SName { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }
        public string Salt { get; set; }


        public RelayCommand AddOrEditUserCommand { get; set; }

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

        public void AddOrEdit()
        {
            var adminCount = (from u in DGV.Users
                              where u.Role_is_admin == true
                              select u).Count();
            if (!Admin && adminCount == 1 && user.Id != -1)
            {
                MessageBox.Show("Нельзя удалять последнего админа");
                return;
            }

            foreach (var u in DGV.Users)
                if (SName.Trim() == u.Name && (curMode == Mode.Add || curMode == Mode.Edit && SName.Trim() != login))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует");
                    return;
                }
            if (SName.Trim() == string.Empty || Password.Trim() == string.Empty)
            {
                MessageBox.Show("Введите данные полностью");
                return;
            }

            user.Name = SName.Trim();
            user.Password = Password.Trim();
            user.Role_is_admin = Admin;
            if (user.Salt == string.Empty)
                user.Salt = TakeSalt();
            else
                user.Salt = Salt.Trim();
            user.Password_byte = CalcHash(user.Salt);

            DialogResult = true;

            Close();
        }
    }
}
