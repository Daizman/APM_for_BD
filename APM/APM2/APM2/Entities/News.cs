using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace APM2.Entities
{
    public class News : INotifyPropertyChanged
    {
        public int Id { get; set; }
        string header;
        string content;
        public DateTime Release_date { get; set; }
        public int Site_id { get; set; }
        string site;

        public News(int id, string header, string content, DateTime release_date, int site_id)
        {
            Id = id;
            Header = header;
            Content = content;
            Release_date = release_date;
            Site_id = site_id;

            var tmp = from s in DGV.Sites
                       where s.Id == Site_id
                       select s.Addr;
            if (tmp.Count() > 0)
                site = tmp.Last();
            else
                site = "";
        }

        public News()
        {
            Site_id = Id = -1;
            header = content = "";
            Release_date = DateTime.Now;

            var tmp = from s in DGV.Sites
                      where s.Id == Site_id
                      select s.Addr;
            if (tmp.Count()>0)
                site = tmp.Last();
            else
                site = "";
        }

        public string Header
        {
            get => header;
            set
            {
                header = value?.Trim();
                OnPropertyChanged(nameof(Header));
            }
        }

        public string Content
        {
            get => content;
            set
            {
                content = value?.Trim();
                OnPropertyChanged(nameof(Content));
            }
        }

        public string Site
        {
            get => site;
            set
            {
                site = value;
                OnPropertyChanged(nameof(Site));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
