using APM2.Entities;
using APM2.Postgres;
using APM2.Tools;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace APM2.AddOrEditForms
{
    public partial class AddOrEditNews : Window
    {
        public News news;

        Mode curMode;

        string header;

        public AddOrEditNews(Mode mode, News news = null)
        {
            InitializeComponent();

            postgersConnector = new PostgersConnector("Host=localhost;Port=5432;Database=andrey_bd2;Username=postgres;Password=postgres");

            curMode = mode;

            SitesLst = new ObservableCollection<string>();

            foreach (var s in DGV.Sites)
                SitesLst.Add(s.Addr);

            CBNews.SelectedIndex = 0;

            if (mode == Mode.Add)
            {
                ModeOpen = "Add news";
                CurModeNews = "Add";
                this.news = new News();

                Header = string.Empty;
                NContent = string.Empty;
                NewsReleaseDate = DateTime.Now ;
            }
            else
            {
                ModeOpen = "Edit news";
                CurModeNews = "Edit";
                InitializeForm(news);
                this.news = news;
                header = news.Header;
            }

            AddOrEditNewsCommand = new RelayCommand(AddOrEdit);
            AddSiteCommand = new RelayCommand(AddSite);

            DataContext = this;
        }

        private void InitializeForm(News news)
        {
            Header = news.Header;
            NContent = news.Content;
            NewsReleaseDate = news.Release_date;
            int i = 0;

            var newSiteAddr = (from n in DGV.Newss
                               where n.Id == news.Id
                               select n.Site_id).Last();

            var addr = "";
            foreach (var s in DGV.Sites)
                if (s.Id == newSiteAddr)
                {
                    addr = s.Addr;
                    break;
                }

            foreach (var s in SitesLst)
            {
                if (s == addr)
                {
                    CBNews.SelectedIndex = i;
                    break;
                }
                i++;
            }
        }

        public string ModeOpen { get; set; }
        public string CurModeNews { get; set; }

        public string Header { get; set; }
        public string NContent { get; set; }
        public DateTime NewsReleaseDate { get; set; }
        public ObservableCollection<string> SitesLst { get; set; }

        public RelayCommand AddOrEditNewsCommand { get; set; }
        public RelayCommand AddSiteCommand { get; set; }

        PostgersConnector postgersConnector;

        public void AddSite()
        {
            var adder = new AddOrEditSite(Mode.Add);
            adder.ShowDialog();
            if (adder.DialogResult == true)
            {
                DGV.Sites.Add(adder.site);
                postgersConnector.SitesTable.Insert(adder.site);
                SitesLst.Add(adder.Addr);
            }
        }

        public void AddOrEdit()
        {
            foreach (var n in DGV.Newss)
                if (Header.Trim() == n.Header && (curMode == Mode.Add || curMode == Mode.Edit && Header.Trim() != header))
                {
                    MessageBox.Show("Новость с таким заголовком уже существует");
                    return;
                }
            if (Header.Trim() == string.Empty || NContent.Trim() == string.Empty)
            {
                MessageBox.Show("Введите данные полностью");
                return;
            }

            if (CBNews.Text==string.Empty)
            {
                MessageBox.Show("Нельзя добавить новость не на сайт");
                return;
            }

            news.Header = Header.Trim();
            news.Content = NContent.Trim();
            news.Release_date = NewsReleaseDate;
            news.Site = CBNews.Text;

            var site_id = (from s in DGV.Sites
                          where s.Addr == CBNews.Text
                          select s.Id).Last();

            news.Site_id = site_id;

            DialogResult = true;

            Close();
        }
    }
}
