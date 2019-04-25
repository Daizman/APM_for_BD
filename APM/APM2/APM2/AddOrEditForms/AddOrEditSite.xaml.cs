using APM2.Entities;
using APM2.Tools;
using System.Windows;

namespace APM2.AddOrEditForms
{
    public partial class AddOrEditSite : Window
    {
        public Site site;

        Mode curMode;
        string addr;

        public AddOrEditSite(Mode mode, Site site = null)
        {
            InitializeComponent();

            curMode = mode;

            if (mode == Mode.Add)
            {
                ModeOpen = "Add site";
                CurModeSite = "Add";
                this.site = new Site();
                SName = string.Empty;
                Addr = string.Empty;
                Access = false;
                Description = string.Empty;
            }
            else
            {
                ModeOpen = "Edit site";
                CurModeSite = "Edit";
                InitializeForm(site);
                this.site = site;
                addr = site.Addr;
            }

            AddOrEditSiteCommand = new RelayCommand(AddOrEdit);

            DataContext = this;
        }

        private void InitializeForm(Site site)
        {
            SName = site.Name;
            Addr = site.Addr;
            Access = site.Access;
            Description = site.Description;
        }

        public string ModeOpen { get; set; }
        public string CurModeSite { get; set; }

        public string SName { get; set; }
        public string Addr { get; set; }
        public bool Access { get; set; }
        public string Description { get; set; }

        public RelayCommand AddOrEditSiteCommand { get; set; }

        public void AddOrEdit()
        {
            foreach(var s in DGV.Sites)
                if(Addr.Trim() == s.Addr && (curMode == Mode.Add || curMode == Mode.Edit && Addr.Trim() != addr))
                {
                    MessageBox.Show("Сайт с таким адресом уже существует");
                    return;
                }
            if(SName.Trim() == string.Empty || Addr.Trim() == string.Empty || Description.Trim() == string.Empty)
            {
                MessageBox.Show("Введите данные полностью");
                return;
            }

            site.Name = SName.Trim();
            site.Addr = Addr.Trim();
            site.Access = Access;
            site.Description = Description.Trim();

            DialogResult = true;

            Close();
        }

    }
}
