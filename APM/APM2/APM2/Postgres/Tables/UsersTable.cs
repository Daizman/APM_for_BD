using APM2.Entities;
using Npgsql;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM2.Postgres.Tables
{
    public class UsersTable
    {
        string strCon;

        public UsersTable(string strCon)
        {
            this.strCon = strCon;
        }

        public List<User> GetUsers()
        {
            var res = new List<User>();

            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"select id, name, password, password_byte, salt, role_is_admin from users;";
                var sCommand = new NpgsqlCommand(query, sCon);

                var reader = sCommand.ExecuteReader();

                while(reader.Read())
                    res.Add(new User((int)reader["id"],(string)reader["name"],(string)reader["password"],(byte[])reader["password_byte"],(string)reader["salt"],(bool)reader["role_is_admin"]));
            }

            return res;
        }

        public void Insert(User user)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"insert into users (name, password, password_byte, salt, role_is_admin) values (@name, @password, @password_byte, @salt, @role_is_admin) returning id;";
                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@name", user.Name);
                sCommand.Parameters.AddWithValue("@password", user.Password);
                sCommand.Parameters.AddWithValue("@password_byte", user.Password_byte);
                sCommand.Parameters.AddWithValue("@salt", user.Salt);
                sCommand.Parameters.AddWithValue("@role_is_admin", user.Role_is_admin);

                var id = (int)sCommand.ExecuteScalar();

                user.Id = id;
            }
        }

        public void Update(User user)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"update users set name = @name, password = @password, password_byte = @password_byte, salt = @salt, role_is_admin = @role_is_admin where id = @id;";
                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@id", user.Id);
                sCommand.Parameters.AddWithValue("@name", user.Name);
                sCommand.Parameters.AddWithValue("@password", user.Password);
                sCommand.Parameters.AddWithValue("@password_byte", user.Password_byte);
                sCommand.Parameters.AddWithValue("@salt", user.Salt);
                sCommand.Parameters.AddWithValue("@role_is_admin", user.Role_is_admin);

                sCommand.ExecuteNonQuery();
            }
        }

        public void Delete(User user)
        {
            using (var sCon = new NpgsqlConnection(strCon))
            {
                sCon.Open();
                var query = @"delete from users where id = @id;";

                var sCommand = new NpgsqlCommand(query, sCon);

                sCommand.Parameters.AddWithValue("@id", user.Id);

                sCommand.ExecuteNonQuery();
            }
        }
    }
}
