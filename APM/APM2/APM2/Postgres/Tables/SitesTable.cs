using APM2.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM2.Postgres.Tables
{
    public class SitesTable
    {
        string strCon;

        public SitesTable(string strCon)
        {
            this.strCon = strCon;
        }

        public List<Site> GetSites()
        {
            var res = new List<Site>();

            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"select id, name, addr, access, description from site;";
                var sCommand = new NpgsqlCommand(query, sCon);

                var reader = sCommand.ExecuteReader();

                while (reader.Read())
                    res.Add(new Site((int)reader["id"], (string)reader["name"], (string)reader["addr"], (bool)reader["access"], (string)reader["description"]));
            }

            return res;
        }

        public void Insert(Site site)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"insert into site (name, addr, access, description) values (@name, @addr, @access, @description) returning id;";
                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@name", site.Name);
                sCommand.Parameters.AddWithValue("@addr", site.Addr);
                sCommand.Parameters.AddWithValue("@access", site.Access);
                sCommand.Parameters.AddWithValue("@description", site.Description);

                var id = (int)sCommand.ExecuteScalar();
                site.Id = id;
            }
        }

        public void Update(Site site)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"update site set name = @name, addr = @addr, access = @access, description = @description where id = @id;";
                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@id", site.Id);
                sCommand.Parameters.AddWithValue("@name", site.Name);
                sCommand.Parameters.AddWithValue("@addr", site.Addr);
                sCommand.Parameters.AddWithValue("@access", site.Access);
                sCommand.Parameters.AddWithValue("@description", site.Description);

                sCommand.ExecuteNonQuery();
            }
        }

        public void Delete(Site site)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"delete from site where id = @id;";

                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@id", site.Id);

                sCommand.ExecuteNonQuery();
            }
        }
    }
}
