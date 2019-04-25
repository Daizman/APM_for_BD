using APM2.Entities;
using Npgsql;
using System;
using System.Collections.Generic;

namespace APM2.Postgres.Tables
{
    public class ContentTable
    {
        string strCon;

        public ContentTable(string strCon)
        {
            this.strCon = strCon;
        }

        public List<Content> GetContents()
        {
            var res = new List<Content>();

            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"select id, c_type, name, country, release_date, budget, duration, description, dub from content;";
                var sCommand = new NpgsqlCommand(query, sCon);

                var reader = sCommand.ExecuteReader();

                while (reader.Read())
                    res.Add(new Content((int)reader["id"], (string)reader["c_type"], (string)reader["name"],(string)reader["country"] , (DateTime)reader["release_date"], (int)reader["budget"],
                        (TimeSpan)reader["duration"],(string)reader["description"],(string)reader["dub"]));
            }

            return res;
        }

        public void Insert(Content content)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"insert into content (c_type, name, country, release_date, budget, duration, description, dub) 
                              values (@c_type, @name, @country, @release_date, @budget, @duration, @description, @dub) returning id;";
                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@c_type", content.C_type);
                sCommand.Parameters.AddWithValue("@name", content.Name);
                sCommand.Parameters.AddWithValue("@country", content.Country);
                sCommand.Parameters.AddWithValue("@release_date", content.Release_date);
                sCommand.Parameters.AddWithValue("@budget", content.Budget);
                sCommand.Parameters.AddWithValue("@duration", content.Duration);
                sCommand.Parameters.AddWithValue("@description", content.Description);
                sCommand.Parameters.AddWithValue("@dub", content.Dub);

                var id = (int)sCommand.ExecuteScalar();
                content.Id = id;
            }
        }

        public void Update(Content content)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"update content set c_type = @c_type, name = @name, country = @country, release_date = @release_date, budget = @budget, duration = @duration, 
                              description = @description, dub = @dub where id = @id;";
                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@id", content.Id);
                sCommand.Parameters.AddWithValue("@c_type", content.C_type);
                sCommand.Parameters.AddWithValue("@name", content.Name);
                sCommand.Parameters.AddWithValue("@country", content.Country);
                sCommand.Parameters.AddWithValue("@release_date", content.Release_date);
                sCommand.Parameters.AddWithValue("@budget", content.Budget);
                sCommand.Parameters.AddWithValue("@duration", content.Duration);
                sCommand.Parameters.AddWithValue("@description", content.Description);
                sCommand.Parameters.AddWithValue("@dub", content.Dub);

                sCommand.ExecuteNonQuery();
            }
        }

        public void Delete(Content content)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"delete from content where id = @id;";

                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@id", content.Id);

                sCommand.ExecuteNonQuery();
            }
        }
    }
}
