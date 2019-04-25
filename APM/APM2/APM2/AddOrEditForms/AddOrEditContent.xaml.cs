using APM2.Entities;
using APM2.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace APM2.AddOrEditForms
{
    public partial class AddOrEditContent : Window
    {
        public Content content;

        public List<ContentForSite> CFS;

        Mode curMode;

        string name;
        string description;

        public AddOrEditContent(Mode mode, Content content = null)
        {
            InitializeComponent();

            curMode = mode;

            CFS = new List<ContentForSite>();

            Sites = new ObservableCollection<string>();

            if (mode == Mode.Add)
            {
                TitleName = "Add content";
                CurModeAccept = "Add";
                this.content = new Content();
                ContentType = string.Empty;
                ContentName = string.Empty;
                ContentCountry = string.Empty;
                ContentReleaseDate = DateTime.Now;
                ContentBudget = 0;
                ContentDuration = new TimeSpan(0);
                ContentDescription = string.Empty;
                ContentDub = string.Empty;
            }
            else
            {
                TitleName = "Edit content";
                CurModeAccept = "Edit";
                InitializeForm(content);
                this.content = content;
                name = content.Name;
                description = content.Description;
            }

            AcceptCommand = new RelayCommand(AddOrEdit);
            AddSitesCommand = new RelayCommand(AddSites);

            DataContext = this;
        }

        private void InitializeForm(Content content)
        {
            ContentType = content.C_type;
            ContentName = content.Name;
            ContentCountry = content.Country;
            ContentReleaseDate = content.Release_date;
            ContentBudget = (uint)content.Budget;
            ContentDuration = content.Duration;
            ContentDescription = content.Description;
            ContentDub = content.Dub;

            var sites = (from cfs in DGV.ContentForSites
                         where cfs.Content_id == content.Id
                         select cfs.Site_id).ToList();

            foreach (var s in DGV.Sites)
                if (sites.Contains(s.Id))
                    Sites.Add(s.Addr);
        }

        public string TitleName { get; private set; }
        public string CurModeAccept { get; private set; }

        public string ContentType { get; set; }
        public string ContentName { get; set; }
        public string ContentCountry { get; set; }
        public DateTime ContentReleaseDate { get; set; }
        public uint ContentBudget { get; set; }
        public TimeSpan ContentDuration { get; set; }
        public string ContentDescription { get; set; }
        public string ContentDub { get; set; }
        public ObservableCollection<string> Sites { get; set; }

        public RelayCommand AcceptCommand { get; set; }
        public RelayCommand AddSitesCommand { get; set; }

        public void AddSites()
        {
            var sitesAdder = new SitesForContentLV(content);
            sitesAdder.ShowDialog();

            CFS = sitesAdder.CFS;

            Sites.Clear();

            List<int> sites;

            if (content.Id != -1)
            {
                sites = (from cfs in DGV.ContentForSites
                        where cfs.Content_id == content.Id
                        select cfs.Site_id).ToList();
            }
            else
            {
                sites = (from cfs in sitesAdder.CFS
                        where cfs.Content_id == content.Id
                        select cfs.Site_id).ToList();
            }

            foreach (var s in DGV.Sites)
                if (sites.Contains(s.Id))
                    Sites.Add(s.Addr);

            CBSites.SelectedIndex = 0;
        }

        public void AddOrEdit()
        {
            foreach (var c in DGV.Contents)
                if (ContentName.Trim() == c.Name && ContentDescription.Trim() == c.Description && (curMode == Mode.Add || curMode == Mode.Edit && ContentName.Trim() != name && ContentDescription.Trim() != description))
                {
                    MessageBox.Show("Контент с таким названием и описанием уже существует");
                    return;
                }
            if (ContentType.Trim() == string.Empty 
                || ContentName.Trim() == string.Empty
                || ContentCountry.Trim() == string.Empty
                || ContentDescription.Trim() == string.Empty
                || ContentDub.Trim() == string.Empty)
            {
                MessageBox.Show("Введите данные полностью");
                return;
            }

            content.C_type = ContentType.Trim();
            content.Name = ContentName.Trim();
            content.Country = ContentCountry.Trim();
            content.Release_date = ContentReleaseDate;
            content.Budget = (int)ContentBudget;
            content.Duration = ContentDuration;
            content.Description = ContentDescription.Trim();
            content.Dub = ContentDub.Trim();

            DialogResult = true;

            Close();
        }
    }
}
