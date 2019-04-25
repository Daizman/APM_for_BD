using APM2.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM2.Postgres.Tables
{
    public class NewsTable
    {
        string strCon;

        public NewsTable(string strCon)
        {
            this.strCon = strCon;
        }

        public List<News> GetNews()
        {
            var res = new List<News>();

            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"select id, header, content, release_date, site_id from news;";
                var sCommand = new NpgsqlCommand(query, sCon);

                var reader = sCommand.ExecuteReader();

                while (reader.Read())
                    res.Add(new News((int)reader["id"], (string)reader["header"], (string)reader["content"], (DateTime)reader["release_date"], (int)reader["site_id"]));
            }

            return res;
        }

        public void Insert(News news)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"insert into news (header, content, release_date, site_id) values (@header, @content, @release_date, @site_id) returning id;";
                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@header", news.Header);
                sCommand.Parameters.AddWithValue("@content", news.Content);
                sCommand.Parameters.AddWithValue("@release_date", news.Release_date);
                sCommand.Parameters.AddWithValue("@site_id", news.Site_id);

                var id = (int)sCommand.ExecuteScalar();
                news.Id = id;
            }
        }

        public void Update(News news)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"update news set header = @header, content = @content, release_date = @release_date, site_id = @site_id where id = @id;";
                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@id", news.Id);
                sCommand.Parameters.AddWithValue("@header", news.Header);
                sCommand.Parameters.AddWithValue("@content", news.Content);
                sCommand.Parameters.AddWithValue("@release_date", news.Release_date);
                sCommand.Parameters.AddWithValue("@site_id", news.Site_id);

                sCommand.ExecuteNonQuery();
            }
        }

        public void Delete(News news)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"delete from news where id = @id;";

                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@id", news.Id);

                sCommand.ExecuteNonQuery();
            }
        }
    }
}
