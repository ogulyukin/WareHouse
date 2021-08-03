using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Warehouse
{
    public class DbWork
    {
        public static Dictionary<int, string> getProviders(String connection)
        {
            var result = new Dictionary<int, string>();

            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = "SELECT * FROM 'ProviderTab'";
            using var query = new SqliteCommand(sql, db);
            using var res = query.ExecuteReader();
            if (res.HasRows)
            {
                while (res.Read())
                {
                    result.Add(res.GetInt32("id"), res.GetString("name"));
                }
            }
            return result;
        }
        public static Dictionary<int, string> getGoodTypes(String connection)
        {
            var result = new Dictionary<int, string>();

            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = "SELECT * FROM 'GoodsTypesTab'";
            using var query = new SqliteCommand(sql, db);
            using var res = query.ExecuteReader();
            if (res.HasRows)
            {
                while (res.Read())
                {
                    result.Add(res.GetInt32("id"), res.GetString("name"));
                }
            }
            return result;
        }
        public static List<Good> getGood(String connection)
        {
            var result = new List<Good>();

            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = "SELECT * FROM 'GoodsTab'";
            using var query = new SqliteCommand(sql, db);
            using var res = query.ExecuteReader();
            if (res.HasRows)
            {
                while (res.Read())
                {
                    var good = new Good();
                    good.ID = res.GetInt32("id");
                    good.Name = res.GetString("name");
                    good.TypeId = res.GetInt32("typeId");
                    good.ProviderId = res.GetInt32("providerId");
                    good.Price = res.GetDouble("price");
                    good.Quantity = res.GetDouble("quantity");
                    result.Add(good);
                }
            }
            return result;
        }
        public static int AddProvider(string name, string connection)
        {
            int result = 0;
            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = $"SELECT id FROM 'ProviderTab' WHERE name = '{name}'";
            using var query01 = new SqliteCommand(sql, db);
            using var res01 = query01.ExecuteReader();
            if (res01.HasRows)
                return result;
            sql = $"INSERT INTO 'ProviderTab'(name) VALUES ('{name}');";
            var query02 = new SqliteCommand(sql, db);
            query02.ExecuteNonQuery();
            sql = $"SELECT id FROM 'ProviderTab' WHERE name = '{name}'";
            var query03 = new SqliteCommand(sql, db);
            using var res02 = query03.ExecuteReader();
            if (res02.HasRows)
            {
                res02.Read();
                result = res02.GetInt32("id");
            }
            return result;
        }
        public static int AddType(string name, string connection)
        {
            int result = 0;
            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = $"SELECT id FROM 'GoodsTypesTab' WHERE name = '{name}'";
            using var query01 = new SqliteCommand(sql, db);
            using var res01 = query01.ExecuteReader();
            if (res01.HasRows)
                return result;
            sql = $"INSERT INTO 'GoodsTypesTab'(name) VALUES ('{name}');";
            var query02 = new SqliteCommand(sql, db);
            query02.ExecuteNonQuery();
            sql = $"SELECT id FROM 'GoodsTypesTab' WHERE name = '{name}'";
            var query03 = new SqliteCommand(sql, db);
            using var res02 = query03.ExecuteReader();
            if (res02.HasRows)
            {
                res02.Read();
                result = res02.GetInt32("id");
            }
            return result;
        }
        public static int AddGood(string name, int typeId, int providerId, double price, double quantity, string connection)
        {
            int result = 0;
            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = $"SELECT id FROM 'GoodsTab' WHERE name = '{name}'";
            using var query01 = new SqliteCommand(sql, db);
            using var res01 = query01.ExecuteReader();
            if (res01.HasRows)
                return result;
            sql = $"INSERT INTO 'GoodsTab'(name, typeId, providerId, price, quantity) VALUES ('{name}','{typeId}','{providerId}','{price}','{quantity}');";
            var query02 = new SqliteCommand(sql, db);
            query02.ExecuteNonQuery();
            sql = $"SELECT id FROM 'GoodsTab' WHERE name = '{name}'";
            var query03 = new SqliteCommand(sql, db);
            using var res02 = query03.ExecuteReader();
            if (res02.HasRows)
            {
                res02.Read();
                result = res02.GetInt32("id");
            }
            return result;
        }
        
        public static void updateTypes(int id, string name, string connection)
        {
            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = $"UPDATE 'GoodsTypesTab' SET name = '{name}' WHERE id = '{id}';";
            var query01 = new SqliteCommand(sql, db);
            query01.ExecuteNonQuery();
        }
        
        public static void updateProviders(int id, string name, string connection)
        {
            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = $"UPDATE 'ProviderTab' SET name = '{name}' WHERE id = '{id}';";
            var query01 = new SqliteCommand(sql, db);
            query01.ExecuteNonQuery();
        }

        public static void updateGoods(int id, string name, int typeId, int providerId, double price, double quantity, string connection)
        {
            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = $"UPDATE 'GoodsTab' SET name = '{name}', providerId = '{providerId}', typeId = '{typeId}', price = '{price}', quantity = '{quantity}' WHERE id = '{id}';";
            var query01 = new SqliteCommand(sql, db);
            query01.ExecuteNonQuery();
        }

        public static void deleteTypes(int id, string connection)
        {
            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = $"DELETE  FROM 'GoodsTypesTab' WHERE id = '{id}';";
            var query01 = new SqliteCommand(sql, db);
            query01.ExecuteNonQuery();
        }
        
        public static void deleteProvider(int id, string connection)
        {
            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = $"DELETE  FROM 'ProviderTab' WHERE id = '{id}';";
            var query01 = new SqliteCommand(sql, db);
            query01.ExecuteNonQuery();
        }

        public static void deleteGood(int id, string connection)
        {
            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = $"DELETE  FROM 'GoodsTab' WHERE id = '{id}';";
            var query01 = new SqliteCommand(sql, db);
            query01.ExecuteNonQuery();
        }
    }
}
