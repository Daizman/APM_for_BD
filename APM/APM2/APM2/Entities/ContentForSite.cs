using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace APM2.Entities
{
    public class ContentForSite : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public DateTime Add_date { get; set; }
        public int Site_id { get; set; }
        public int Content_id { get; set; }

        public ContentForSite(DateTime add_date, int site_id, int content_id, int id)
        {
            Add_date = add_date;
            Site_id = site_id;
            Content_id = content_id;
            Id = id;
        }

        public ContentForSite()
        {
            Add_date = DateTime.Now;
            Site_id = Content_id = Id = -1;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
