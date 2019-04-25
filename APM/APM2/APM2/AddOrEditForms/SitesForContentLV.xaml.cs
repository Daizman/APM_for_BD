using APM2.Entities;
using APM2.Postgres;
using APM2.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace APM2.AddOrEditForms
{
    public partial class SitesForContentLV : Window
    {
        public ObservableCollection<Site> Sites { get; set; }
        public List<ContentForSite> CFS { get; set; }

        public RelayCommand AddSiteCommand { get; set; }

        public Content content;

        PostgersConnector postgersConnector;

        bool flag = false;

        public SitesForContentLV(Content content)
        {
            postgersConnector = new PostgersConnector("Host=localhost;Port=5432;Database=APM;Username=postgres;Password=1234");
            InitializeComponent();
            Sites = DGV.Sites;

            CFS = new List<ContentForSite>();

            this.content = content;


            foreach (var cfs in DGV.ContentForSites)
                if (cfs.Content_id == content.Id)
                {
                    var sel = (from s in DGV.Sites
                              where cfs.Site_id == s.Id
                              select s).Last();
                    Owner.SelectedItems.Add(sel);
                }
            if (content.Id == -1 || DGV.ContentForSites.Where((s)=>s.Content_id==content.Id).Count()==0)
                flag = true;
            AddSiteCommand = new RelayCommand(AddSite);

            DataContext = this;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(flag)
                foreach (var it in e.RemovedItems)
                {
                    var tmp = (from c in DGV.ContentForSites
                               where c.Site_id == ((Site)it).Id && c.Content_id == content.Id
                               select c);
                    if (tmp.Count() > 0)
                    {
                        var cfsD = tmp.Last();
                        if (content.Id != -1)
                            postgersConnector.ContentForSiteTable.Delete(cfsD);
                        else
                            CFS.Remove(cfsD);
                        DGV.ContentForSites.Remove(cfsD);
                    }
                }
            if (flag)
                foreach (var it in e.AddedItems)
                {
                    var tmp = new ContentForSite() { Add_date = DateTime.Now, Site_id = ((Site)it).Id, Content_id = content.Id };
                    if (content.Id != -1)
                        postgersConnector.ContentForSiteTable.Insert(tmp);
                    else
                        CFS.Add(tmp);
                    DGV.ContentForSites.Add(tmp);
                }
            flag = true;
        }

        public void AddSite()
        {
            var adder = new AddOrEditSite(Mode.Add);
            adder.ShowDialog();
            if (adder.DialogResult == true)
            {
                DGV.Sites.Add(adder.site);
                postgersConnector.SitesTable.Insert(adder.site);
            }
        }
    }
}
