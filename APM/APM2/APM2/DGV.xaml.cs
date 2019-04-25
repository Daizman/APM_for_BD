using APM2.AddOrEditForms;
using APM2.Entities;
using APM2.Postgres;
using APM2.Tools;
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
using System.Windows.Shapes;

namespace APM2
{
    public partial class DGV : Window, INotifyPropertyChanged
    {
        public DGV(Role role)
        {
            InitializeComponent();

            if (role == Role.Guest)
                UserMode = Visibility.Hidden;
            else
                UserMode = Visibility.Visible;
            if (role == Role.Admin)
                AdminRole = Visibility.Visible;
            else
                AdminRole = Visibility.Hidden;

            postgersConnector = new PostgersConnector("Host=localhost;Port=5432;Database=APM;Username=postgres;Password=1234");

            Sites = new ObservableCollection<Site>(postgersConnector.SitesTable.GetSites());
            Contents = new ObservableCollection<Content>(postgersConnector.ContentTable.GetContents());
            Newss = new ObservableCollection<News>(postgersConnector.NewsTable.GetNews());
            ContentForSites = new ObservableCollection<ContentForSite>(postgersConnector.ContentForSiteTable.GetContentForSites());
            Users = new ObservableCollection<User>(MainWindow.Users);

            InitializeCommands();

            DataContext = this;
        }

        private void InitializeCommands()
        {
            AddSiteCommand = new RelayCommand(AddSite);
            EditSiteCommand = new RelayCommand(EditSite, () => SelectedSite != null);
            DeleteSiteCommand = new RelayCommand(DeleteSite, () => SelectedSite != null);

            AddNewsCommand = new RelayCommand(AddNews);
            EditNewsCommand = new RelayCommand(EditNews, () => SelectedNews != null);
            DeleteNewsCommand = new RelayCommand(DeleteNews, () => SelectedNews != null);

            AddContentCommand = new RelayCommand(AddContent);
            EditContentCommand = new RelayCommand(EditContent, () => SelectedContent != null);
            DeleteContentCommand = new RelayCommand(DeleteContent, () => SelectedContent != null);

            AddUserCommand = new RelayCommand(AddUser);
            EditUserCommand = new RelayCommand(EditUser, () => SelectedUser != null);
            DeleteUserCommand = new RelayCommand(DeleteUser, () => SelectedUser != null);

            SiteContentCommand = new RelayCommand(SiteContent, () => SelectedSite != null);
            SiteNewsCommand = new RelayCommand(SiteNews, () => SelectedSite != null);
        }

        public Visibility UserMode { get; set; }
        public Visibility AdminRole { get; set; }
        PostgersConnector postgersConnector;

        public static ObservableCollection<Site> Sites { get; private set; }
        public static ObservableCollection<Content> Contents { get; private set; }
        public static ObservableCollection<News> Newss { get; private set; }
        public static ObservableCollection<ContentForSite> ContentForSites { get; private set; }
        public static ObservableCollection<User> Users { get; private set; }

        public Site SelectedSite { get; set; }
        public News SelectedNews { get; set; }
        public Content SelectedContent { get; set; }
        public User SelectedUser { get; set; }

        public RelayCommand AddSiteCommand { get; set; }
        public RelayCommand EditSiteCommand { get; set; }
        public RelayCommand DeleteSiteCommand { get; set; }

        public RelayCommand AddNewsCommand { get; set; }
        public RelayCommand EditNewsCommand { get; set; }
        public RelayCommand DeleteNewsCommand { get; set; }

        public RelayCommand AddContentCommand { get; set; }
        public RelayCommand EditContentCommand { get; set; }
        public RelayCommand DeleteContentCommand { get; set; }

        public RelayCommand AddUserCommand { get; set; }
        public RelayCommand EditUserCommand { get; set; }
        public RelayCommand DeleteUserCommand { get; set; }

        public RelayCommand SiteContentCommand { get; set; }
        public RelayCommand SiteNewsCommand { get; set; }

        public void AddSite()
        {
            var adder = new AddOrEditSite(Mode.Add);
            adder.ShowDialog();
            if(adder.DialogResult == true)
            {
                Sites.Add(adder.site);
                postgersConnector.SitesTable.Insert(adder.site);
            }
        }

        public void EditSite()
        {
            var editor = new AddOrEditSite(Mode.Edit, SelectedSite);
            editor.ShowDialog();
            if (editor.DialogResult == true)
            {
                Sites.Remove(SelectedSite);
                Sites.Add(editor.site);
                postgersConnector.SitesTable.Update(editor.site);
                foreach (var n in Newss)
                    if (n.Site_id == editor.site.Id)
                        n.Site = editor.Addr;
            }
        }

        public void DeleteSite()
        {
            var toDelC = (from cfs in ContentForSites
                         where cfs.Site_id == SelectedSite.Id
                         select cfs).ToArray();
            foreach (var d in toDelC)
                ContentForSites.Remove(d);

            var toDelN = (from n in Newss
                          where n.Site_id == SelectedSite.Id
                          select n).ToArray();
            foreach (var n in toDelN)
                Newss.Remove(n);

            postgersConnector.SitesTable.Delete(SelectedSite);
            Sites.Remove(SelectedSite);
        }

        public void AddNews()
        {
            var adder = new AddOrEditNews(Mode.Add);
            adder.ShowDialog();
            if (adder.DialogResult == true)
            {
                Newss.Add(adder.news);
                postgersConnector.NewsTable.Insert(adder.news);
            }
        }

        public void EditNews()
        {
            var editor = new AddOrEditNews(Mode.Edit, SelectedNews);
            editor.ShowDialog();
            if (editor.DialogResult == true)
            {
                Newss.Remove(SelectedNews);
                Newss.Add(editor.news);
                postgersConnector.NewsTable.Update(editor.news);
            }
        }

        public void DeleteNews()
        {
            postgersConnector.NewsTable.Delete(SelectedNews);
            Newss.Remove(SelectedNews);
        }

        public void AddContent()
        {
            var adder = new AddOrEditContent(Mode.Add);
            adder.ShowDialog();
            if (adder.DialogResult == true)
            {
                Contents.Add(adder.content);
                postgersConnector.ContentTable.Insert(adder.content);

                foreach (var cfs in adder.CFS)
                {
                    cfs.Content_id = adder.content.Id;
                    postgersConnector.ContentForSiteTable.Insert(cfs);
                }
            }
        }

        public void EditContent()
        {
            var editor = new AddOrEditContent(Mode.Edit, SelectedContent);
            editor.ShowDialog();
            if (editor.DialogResult == true)
            {
                Contents.Remove(SelectedContent);
                Contents.Add(editor.content);
                postgersConnector.ContentTable.Update(editor.content);
            }
        }

        public void DeleteContent()
        {
            var toDel = (from cfs in ContentForSites
                         where cfs.Content_id == SelectedContent.Id
                         select cfs).ToArray();
            foreach (var d in toDel)
                ContentForSites.Remove(d);
            postgersConnector.ContentTable.Delete(SelectedContent);
            Contents.Remove(SelectedContent);
        }

        public void AddUser()
        {
            var adder = new AddOrEditUser(Mode.Add);
            adder.ShowDialog();
            if (adder.DialogResult == true)
            {
                Users.Add(adder.user);
                postgersConnector.UsersTable.Insert(adder.user);
                MainWindow.Users = Users;
            }
        }

        public void EditUser()
        {
            var editor = new AddOrEditUser(Mode.Edit, SelectedUser);
            editor.ShowDialog();
            if (editor.DialogResult == true)
            {
                Users.Remove(SelectedUser);
                Users.Add(editor.user);
                postgersConnector.UsersTable.Update(editor.user);
                MainWindow.Users = Users;
            }
        }

        public void DeleteUser()
        {
            var adminCount = (from u in Users
                              where u.Role_is_admin == true
                              select u).Count();
            if(SelectedUser.Role_is_admin && adminCount==1)
            {
                MessageBox.Show("Нельзя удалять последнего админа");
                return;
            }

            postgersConnector.UsersTable.Delete(SelectedUser);
            Users.Remove(SelectedUser);

            MainWindow.Users = Users;
        }

        public void SiteContent()
        {
            var answ = new StringBuilder();
            var takeId = (from cfs in ContentForSites
                         where cfs.Site_id == SelectedSite.Id
                         select cfs.Content_id).ToArray();
            foreach (var c in Contents)
                if (takeId.Contains(c.Id))
                    answ.Append(c.Name + " - " + c.Description + '\n');
            MessageBox.Show(answ.ToString());
        }

        public void SiteNews()
        {
            var answ = new StringBuilder();
            var takeId = (from n in Newss
                          where n.Site_id == SelectedSite.Id
                          select n.Id).ToArray();
            foreach (var n in Newss)
                if (takeId.Contains(n.Id))
                    answ.Append(n.Header + " - " + n.Content + '\n');
            MessageBox.Show(answ.ToString());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
