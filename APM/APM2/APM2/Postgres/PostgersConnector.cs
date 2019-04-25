using APM2.Postgres.Tables;

namespace APM2.Postgres
{
    class PostgersConnector
    {
        string strCon;
        public PostgersConnector(string strCon)
        {
            this.strCon = strCon;

            SitesTable = new SitesTable(strCon);
            NewsTable = new NewsTable(strCon);
            ContentTable = new ContentTable(strCon);
            ContentForSiteTable = new ContentForSiteTable(strCon);
            UsersTable = new UsersTable(strCon);
        }

        public SitesTable SitesTable { get; set; }
        public NewsTable NewsTable { get; set; }
        public ContentTable ContentTable { get; set; }
        public ContentForSiteTable ContentForSiteTable { get; set; }
        public UsersTable UsersTable { get; set; }
    }
}
