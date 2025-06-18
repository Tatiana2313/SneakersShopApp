using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SneakersShopApp.Models;
using System.Windows;
using System.Collections.ObjectModel;

namespace SneakersShopApp.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString = @"Data Source=DESKTOP-K260DFM; Initial Catalog = Sneakers;Integrated Security=True";

        public User AuthenticateUser(string login, string password)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM Users WHERE login_user = @login", connection);
                    command.Parameters.AddWithValue("@login", login);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hashedPassword = reader.GetString(3); 
                            if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                            {
                                return new User
                                {
                                    IdUser = reader.GetInt16(0),
                                    NameRole = reader.GetString(1),
                                    LoginUser = reader.GetString(2),
                                    PasswordUser = null 
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }

        public List<KindSport> GetKindOfSport()
        {
            var sports = new List<KindSport>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT cod_kind_of_sport, name_sport FROM Kind_of_sport", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    sports.Add(new KindSport
                    {
                        CodKindSport = reader.GetInt16(0),
                        NameSport = reader.GetString(1)
                    });
                }
            }
            return sports;
        }

        public KindSport GetSportByCod(int sportCod)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT cod_kind_of_sport, name_sport FROM Kind_of_sport WHERE cod_kind_of_sport = @sportCod", connection);
                command.Parameters.AddWithValue("@sportCod", sportCod);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var sport = new KindSport
                    {
                        CodKindSport = reader.GetInt16(0),
                        NameSport = reader.GetString(1)
                    };

                    return sport;
                }
                return null;
            }
        }

        public List<Sneaker> GetSneakers()
        {
            var sneakers = new List<Sneaker>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Sneakers", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sneakers.Add(new Sneaker
                        {
                            CodSneakers = reader.GetInt16(0),
                            SneakersName = reader.GetString(1),
                            VendorCode = reader.GetString(2),
                            Brand = reader.GetString(3),
                            ProducingCountry = reader.GetString(4),
                            KindOfSport = GetSportByCod(reader.GetInt16(5)),
                            Material = reader.GetString(6),
                            Gender = reader.GetString(7),
                            Size = reader.GetInt16(8),
                            Color = reader.GetString(9),
                            UnitPrice = reader.GetDecimal(10),
                            Photo = reader.GetString(11)
                        });
                    }
                }
            }
            return sneakers;
        }

        public void AddSneakers(Sneaker sneakers)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("AddSneakers", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Sneakers_name", sneakers.SneakersName);
                command.Parameters.AddWithValue("@Vendor_code", sneakers.VendorCode);
                command.Parameters.AddWithValue("@brand", sneakers.Brand);
                command.Parameters.AddWithValue("@Producing_country", sneakers.ProducingCountry);
                command.Parameters.AddWithValue("@cod_kind_of_sport", sneakers.KindOfSport.CodKindSport);
                command.Parameters.AddWithValue("@material", sneakers.Material);
                command.Parameters.AddWithValue("@gender", sneakers.Gender); 
                command.Parameters.AddWithValue("@size", sneakers.Size);
                command.Parameters.AddWithValue("@color", sneakers.Color);
                command.Parameters.AddWithValue("@unit_price", sneakers.UnitPrice);
                command.Parameters.AddWithValue("@photo", sneakers.Photo);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateSneakers(Sneaker sneakers)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UpdateSneakers", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@cod_sneakers", sneakers.CodSneakers);
                command.Parameters.AddWithValue("@Sneakers_name", sneakers.SneakersName);
                command.Parameters.AddWithValue("@Vendor_code", sneakers.VendorCode);
                command.Parameters.AddWithValue("@brand", sneakers.Brand);
                command.Parameters.AddWithValue("@Producing_country", sneakers.ProducingCountry);
                command.Parameters.AddWithValue("@cod_kind_of_sport", sneakers.KindOfSport.CodKindSport);
                command.Parameters.AddWithValue("@material", sneakers.Material);
                command.Parameters.AddWithValue("@gender", sneakers.Gender);
                command.Parameters.AddWithValue("@size", sneakers.Size);
                command.Parameters.AddWithValue("@color", sneakers.Color);
                command.Parameters.AddWithValue("@unit_price", sneakers.UnitPrice);
                command.Parameters.AddWithValue("@photo", sneakers.Photo);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteSneakers(int sneakersCod)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DeleteRecordsSneakers", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@recordID", sneakersCod);
                command.ExecuteNonQuery(); 
            }
        }

        public List<Provider> GetProviders()
        {
            var providers = new List<Provider>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Providerr", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        providers.Add(new Provider
                        {
                            CodProdiver = reader.GetInt16(0),
                            ProviderName = reader.GetString(1),
                            Addres = reader.GetString(2),
                            PhoneFax = reader.GetString(3),
                            RascetScet = reader.GetString(4)
                        });
                    }
                }
            }
            return providers;
        }

        public void AddProvider(Provider provider)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("AddProviders", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@provider_name", provider.ProviderName);
                command.Parameters.AddWithValue("@addres", provider.Addres);
                command.Parameters.AddWithValue("@phone_fax", provider.PhoneFax);
                command.Parameters.AddWithValue("@rascet_scet", provider.RascetScet);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateProvider(Provider provider)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UpdateProvider", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@provider_cod", provider.CodProdiver);
                command.Parameters.AddWithValue("@provider_name", provider.ProviderName);
                command.Parameters.AddWithValue("@addres", provider.Addres);
                command.Parameters.AddWithValue("@phone_fax", provider.PhoneFax);
                command.Parameters.AddWithValue("@rascet_scet", provider.RascetScet);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteProvider(int providerCod)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DeleteRecordsProvider", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@recordID", providerCod);
                command.ExecuteNonQuery();
            }
        }

        public Provider GetProviderByCod(int providerCod)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT cod_provider, provider_name, addres, phone_fax, rascet_scet FROM Providerr WHERE cod_provider = @providerCod", connection);
                command.Parameters.AddWithValue("@providerCod", providerCod);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Provider
                    {
                        CodProdiver = reader.GetInt16(0),
                        ProviderName = reader.GetString(1),
                        Addres = reader.IsDBNull(2) ? null : reader.GetString(2),
                        PhoneFax = reader.IsDBNull(3) ? null : reader.GetString(3),
                        RascetScet = reader.IsDBNull(4) ? null : reader.GetString(4)
                    };
                }
                return null;
            }
        }

        public Sneaker GetSneakersByCod(int sneakersCod)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT cod_sneakers, Sneakers_name, Vendor_code, brand, Producing_country, cod_kind_of_sport, material, gender, size, color, unit_price, photo FROM Sneakers WHERE cod_sneakers = @sneakersCod", connection);
                command.Parameters.AddWithValue("@sneakersCod", sneakersCod);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var sneakers = new Sneaker
                    {
                        CodSneakers = reader.GetInt16(0),
                        SneakersName = reader.GetString(1),
                        VendorCode = reader.GetString(2),
                        Brand = reader.GetString(3),
                        ProducingCountry = reader.GetString(4),
                        KindOfSport = GetSportByCod(reader.GetInt16(5)),
                        Material = reader.GetString(6),
                        Gender = reader.GetString(7),
                        Size = reader.GetInt16(8),
                        Color = reader.GetString(9),
                        UnitPrice = reader.GetDecimal(10),
                        Photo = reader.GetString(11)
                    };

                    return sneakers;
                }
                return null;
            }
        }

        public List<TTN> GetTTNs()
        {
            var ttns = new List<TTN>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT num_doc, date_post, cod_provider, cod_sneakers, number_of_sneakers FROM TTN", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ttns.Add(new TTN
                        {
                            NumDoc = reader.GetInt16(0),
                            DatePost = reader.GetDateTime(1),
                            Provider = GetProviderByCod(reader.GetInt16(2)),
                            Sneakers = GetSneakersByCod(reader.GetInt16(3)),
                            NumberOfSneakers = reader.GetInt16(4)
                        });
                    }
                }
            }
            return ttns;
        }

        public void AddTTN(TTN ttn)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("AddTTN", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@date_post", ttn.DatePost);
                command.Parameters.AddWithValue("@cod_provider", ttn.Provider.CodProdiver);
                command.Parameters.AddWithValue("@cod_sneakers", ttn.Sneakers.CodSneakers);
                command.Parameters.AddWithValue("@number_of_sneakers", ttn.NumberOfSneakers);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateTTN(TTN ttn)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UpdateTTN", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@num_doc", ttn.NumDoc);
                command.Parameters.AddWithValue("@cod_provider", ttn.Provider.CodProdiver);
                command.Parameters.AddWithValue("@cod_sneakers", ttn.Sneakers.CodSneakers);
                command.Parameters.AddWithValue("@number_of_sneakers", ttn.NumberOfSneakers);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteTTN(int numDoc)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DeleteRecordsTTN", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@recordID", numDoc);
                command.ExecuteNonQuery();
            }
        }

        public List<Chek> GetCheks()
        {
            var cheks = new List<Chek>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT check_number, date_of_sale, sale_time, cod_sneakers, number_of_sneakers FROM Chek", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cheks.Add(new Chek
                        {
                            CheckNumber = reader.GetInt16(0),
                            DateOfSale = reader.GetDateTime(1),
                            SaleTime = reader.GetTimeSpan(2),
                            Sneakers = GetSneakersByCod(reader.GetInt16(3)),
                            NumberOfSneakers = reader.GetInt16(4)
                        });
                    }
                }
            }
            return cheks;
        }

        public void AddChek(Chek chek)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("AddCheck", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@cod_sneakers", chek.Sneakers.CodSneakers);
                command.Parameters.AddWithValue("@number_of_sneakers", chek.NumberOfSneakers);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateChek(Chek chek)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UpdateCheck", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@check_number", chek.CheckNumber);
                command.Parameters.AddWithValue("@cod_sneakers", chek.Sneakers.CodSneakers);
                command.Parameters.AddWithValue("@number_of_sneakers", chek.NumberOfSneakers);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteChek(int checkNumber)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DeleteRecordsCheck", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@recordID", checkNumber);
                command.ExecuteNonQuery();
            }
        }

        public List<JurnalUceta> GetJurnalUceta()
        {
            var jurnals = new List<JurnalUceta>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT month_name, cod_sneakers, NumbeOfDeliveredSneakers, NumberOfSneakersSold FROM Jurnal_uceta", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        jurnals.Add(new JurnalUceta
                        {
                            MonthName = reader.GetString(0),
                            Sneakers = GetSneakersByCod(reader.GetInt16(1)),
                            NumberOfDeliveredSneakers = reader.GetInt16(2),
                            NumberOfSneakersSold = reader.GetInt16(3)
                        });
                    }
                }
            }
            return jurnals;
        }

        public void AddJurnalUceta(JurnalUceta jurnal)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("AddJurnalInfo", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@month_name", jurnal.MonthName);
                command.Parameters.AddWithValue("@cod_sneakers", jurnal.Sneakers.CodSneakers);
                command.Parameters.AddWithValue("@NumbeOfDeliveredSneakers", jurnal.NumberOfDeliveredSneakers);
                command.Parameters.AddWithValue("@NumberOfSneakersSold", jurnal.NumberOfSneakersSold);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateJurnalUceta(JurnalUceta jurnal)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UpdateJurnalUceta", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@month_name", jurnal.MonthName);
                command.Parameters.AddWithValue("@cod_sneakers", jurnal.Sneakers.CodSneakers);
                command.Parameters.AddWithValue("@NumbeOfDeliveredSneakers", jurnal.NumberOfDeliveredSneakers);
                command.Parameters.AddWithValue("@NumberOfSneakersSold", jurnal.NumberOfSneakersSold);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteJurnalUceta(string monthName, string sneakersName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DeleteRecordsUcet", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@month", monthName);
                command.Parameters.AddWithValue("@name", sneakersName);
                command.ExecuteNonQuery();
            }
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT id_user, name_role, login_user, password_user FROM Users", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            IdUser = reader.GetInt16(0),
                            NameRole = reader.GetString(1),
                            LoginUser = reader.GetString(2),
                            PasswordUser = reader.GetString(3)
                        });
                    }
                }
            }
            return users;
        }

        public void AddUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("AddUser", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@name_role", user.NameRole);
                command.Parameters.AddWithValue("@login_user", user.LoginUser);
                command.Parameters.AddWithValue("@password_user", user.PasswordUser);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UpdateUser", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@id_user", user.IdUser);
                command.Parameters.AddWithValue("@name_role", user.NameRole);
                command.Parameters.AddWithValue("@login_user", user.LoginUser);
                command.Parameters.AddWithValue("@password_user", user.PasswordUser);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteUser(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DeleteUser", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserID", userId);
                command.ExecuteNonQuery();
            }
        }

        public void CreateBackupDB(string FilePath)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "BACKUP DATABASE Sneakers TO DISK = N'" + FilePath + "' WITH NOFORMAT, NOINIT, NAME = N'Sneakers-Full Database Backup', SKIP, NOREWIND, NOUNLOAD";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RestoreBackupDB(string FilePath)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                bool isDatabaseInUse = IsDatabaseInUse(connection, "Sneakers");
                if (isDatabaseInUse) DisconnectDatabase(connection, "Sneakers");
                string query = "USE master RESTORE DATABASE Sneakers FROM DISK = N'" + FilePath + "' WITH REPLACE, FILE = 1,  NOUNLOAD";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private bool IsDatabaseInUse(SqlConnection connection, string databaseName)
        {
            string query = $"SELECT COUNT(*) FROM sys.dm_exec_sessions WHERE database_id = DB_ID('{databaseName}')";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                int sessionCount = (int)cmd.ExecuteScalar();
                return sessionCount > 0;
            }
        }

        private void DisconnectDatabase(SqlConnection connection, string databaseName)
        {
            string query = $"ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
