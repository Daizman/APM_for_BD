using APM2.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM2.Postgres.Tables
{
    public class ContentForSiteTable
    {
        string strCon;

        public ContentForSiteTable(string strCon)
        {
            this.strCon = strCon;
        }

        public List<ContentForSite> GetContentForSites()
        {
            var res = new List<ContentForSite>();

            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"select site_id, content_id, add_date, id from content_for_site;";
                var sCommand = new NpgsqlCommand(query, sCon);

                var reader = sCommand.ExecuteReader();

                while (reader.Read())
                    res.Add(new ContentForSite((DateTime)reader["add_date"],(int)reader["site_id"], (int)reader["content_id"],(int)reader["id"]));
            }

            return res;
        }

        public void Insert(ContentForSite CFS)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"insert into content_for_site (site_id, content_id, add_date) values (@site_id, @content_id, @add_date) returning id;";
                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@site_id", CFS.Site_id);
                sCommand.Parameters.AddWithValue("@content_id", CFS.Content_id);
                sCommand.Parameters.AddWithValue("@add_date", CFS.Add_date);

                var id = (int)sCommand.ExecuteScalar();
                CFS.Id = id;
            }
        }

        public void Update(ContentForSite CFS)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"update content_for_site set site_id = @site_id, content_id = @content_id, add_date = @add_date where id = @id;";
                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@id", CFS.Id);
                sCommand.Parameters.AddWithValue("@site_id", CFS.Site_id);
                sCommand.Parameters.AddWithValue("@content_id", CFS.Content_id);
                sCommand.Parameters.AddWithValue("@add_date", CFS.Add_date);

                sCommand.ExecuteNonQuery();
            }
        }

        public void Delete(ContentForSite CFS)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"delete from content_for_site where site_id = @site_id and content_id = @content_id;";

                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@site_id", CFS.Site_id);
                sCommand.Parameters.AddWithValue("@content_id", CFS.Content_id);

                sCommand.ExecuteNonQuery();
            }
        }
    }
}
