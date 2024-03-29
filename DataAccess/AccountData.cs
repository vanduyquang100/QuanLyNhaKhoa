﻿using QuanLyNhaKhoa.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace QuanLyNhaKhoa.DataAccess
{
    public class AccountData
    {
        string connectionString;
        public Interfaces.Account StoredAccount { get; set; } = null;
        public AccountData(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public bool Login(Interfaces.Account account)
        {
            string accountType = "UNKNOWN";
            if (account is AdministratorAccount)
            {
                accountType = "QTV";
            }
            else if (account is CustomerAccount)
            {
                accountType = "KHACH_HANG";
            }
            else if (account is DentistAccount)
            {
                accountType = "NHA_SI";
            }
            else if (account is ReceptionistAccount)
            {
                accountType = "NHAN_VIEN";
            }
            else
            {
                return false;
            }
            string query = $"SELECT COUNT(*) FROM {accountType} WHERE SDT = '{account.PhoneNumber}' AND MATKHAU = '{account.Password}' AND TRANGTHAI != 0";
            Debug.WriteLine("Login " + query);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int count = (int)command.ExecuteScalar();

                    if (StoredAccount is null)
                    {
                        StoredAccount = GetAccountInfo(account);
                    }
                    return count > 0;
                }
            }

        }

        public Interfaces.Account GetAccountInfo(Interfaces.Account account)
        {
            if (account is AdministratorAccount)
            {
                return GetAdministratorAccount(account.PhoneNumber);
            }
            else if (account is CustomerAccount)
            {
                return GetCustomerAccount(account.PhoneNumber);
            }
            else if (account is DentistAccount)
            {
                return GetDentistAccount(account.PhoneNumber);
            }

            return null;
        }

        private AdministratorAccount GetAdministratorAccount(string PhoneNumber)
        {
            AdministratorAccount account = new AdministratorAccount();
            string query = $"SELECT * FROM QTV WHERE SDT={PhoneNumber}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                account.Id = reader.GetString(reader.GetOrdinal("MAQTV"));
                                account.PhoneNumber = reader.GetString(reader.GetOrdinal("SDT"));
                                account.Name = reader.GetString(reader.GetOrdinal("HOTEN"));
                                account.Birthday = reader.GetDateTime(reader.GetOrdinal("NGAYSINH"));
                                account.Address = reader.GetString(reader.GetOrdinal("DIACHI"));
                                account.Password = reader.GetString(reader.GetOrdinal("MATKHAU"));
                                account.Status = reader.GetInt32(reader.GetOrdinal("TRANGTHAI"));
                                return account;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        private CustomerAccount GetCustomerAccount(string PhoneNumber)
        {
            CustomerAccount account = new CustomerAccount();
            string query = $"SELECT * FROM KHACH_HANG WHERE SDT={PhoneNumber}";
            Debug.WriteLine("customer: " + query);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                account.Id = reader.GetString(reader.GetOrdinal("MAKH"));
                                account.PhoneNumber = reader.GetString(reader.GetOrdinal("SDT"));
                                account.Name = reader.GetString(reader.GetOrdinal("HOTEN"));
                                account.Birthday = reader.GetDateTime(reader.GetOrdinal("NGAYSINH"));
                                account.Address = reader.GetString(reader.GetOrdinal("DIACHI"));
                                account.Password = reader.GetString(reader.GetOrdinal("MATKHAU"));
                                account.Status = reader.GetInt32(reader.GetOrdinal("TRANGTHAI"));
                                return account;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        private DentistAccount GetDentistAccount(string PhoneNumber)
        {
            DentistAccount account = new DentistAccount();
            string query = $"SELECT * FROM NHA_SI WHERE SDT={PhoneNumber}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                account.Id = reader.GetString(reader.GetOrdinal("MANS"));
                                account.PhoneNumber = reader.GetString(reader.GetOrdinal("SDT"));
                                account.Name = reader.GetString(reader.GetOrdinal("HOTEN"));
                                account.Birthday = reader.GetDateTime(reader.GetOrdinal("NGAYSINH"));
                                account.Address = reader.GetString(reader.GetOrdinal("DIACHI"));
                                account.Password = reader.GetString(reader.GetOrdinal("MATKHAU"));
                                //account.Chuyenmon = reader.GetString(reader.GetOrdinal("CHUYENMON"));
                                account.Status = reader.GetInt32(reader.GetOrdinal("TRANGTHAI"));
                                return account;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        private ReceptionistAccount GetReceptionistAccount(string PhoneNumber)
        {
            ReceptionistAccount account = new ReceptionistAccount();
            string query = $"SELECT * FROM NHAN_VIEN WHERE SDT={PhoneNumber}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                account.Id = reader.GetString(reader.GetOrdinal("MANV"));
                                account.PhoneNumber = reader.GetString(reader.GetOrdinal("SDT"));
                                account.Name = reader.GetString(reader.GetOrdinal("HOTEN"));
                                account.Birthday = reader.GetDateTime(reader.GetOrdinal("NGAYSINH"));
                                account.Address = reader.GetString(reader.GetOrdinal("DIACHI"));
                                account.Password = reader.GetString(reader.GetOrdinal("MATKHAU"));
                                account.Status = reader.GetInt32(reader.GetOrdinal("TRANGTHAI"));
                                return account;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Receptionist");

                        return null;
                    }
                }
            }
            return null;
        }

        bool SignUpCustomer(Interfaces.Account account)
        {
            string query = "INSERT INTO KHACH_HANG (MAKH, SDT, HOTEN, NGAYSINH, DIACHI, MATKHAU, TRANGTHAI)"
                + "VALUES (@MAKH, @SDT, @HOTEN, @NGAYSINH, @DIACHI, @MATKHAU, @TRANGTHAI)";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MAKH", account.Id);
                        command.Parameters.AddWithValue("@SDT", account.PhoneNumber);
                        command.Parameters.AddWithValue("@HOTEN", account.Name);
                        command.Parameters.AddWithValue("@NGAYSINH", account.Birthday);
                        command.Parameters.AddWithValue("@DIACHI", account.Address);
                        command.Parameters.AddWithValue("@MATKHAU", account.Password);
                        command.Parameters.AddWithValue("@TRANGTHAI", account.Status);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        bool SignUpDentist(Interfaces.Account account)
        {
            string query = "INSERT INTO NHA_SI (MANS, SDT, HOTEN, NGAYSINH, DIACHI, MATKHAU, TRANGTHAI)"
                + "VALUES (@MAKH, @SDT, @HOTEN, @NGAYSINH, @DIACHI, @MATKHAU, @TRANGTHAI)";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MANS", account.Id);
                        command.Parameters.AddWithValue("@SDT", account.PhoneNumber);
                        command.Parameters.AddWithValue("@HOTEN", account.Name);
                        command.Parameters.AddWithValue("@NGAYSINH", account.Birthday);
                        command.Parameters.AddWithValue("@DIACHI", account.Address);
                        command.Parameters.AddWithValue("@MATKHAU", account.Password);
                        command.Parameters.AddWithValue("@TRANGTHAI", account.Status);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        bool SignUpReceptionist(Interfaces.Account account)
        {
            throw new NotImplementedException();
        }

        bool SignUpAdministrator(Interfaces.Account account)
        {
            throw new NotImplementedException();
        }

        public List<ReceptionistAccount> GetReceptionists(string byName = null, int offset = 0, int fetchSize = 50)
        {
            List<ReceptionistAccount> receps = new();
            // query the database to get all orders
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM NHAN_VIEN ";
                if (byName is not null)
                {
                    query += $"WHERE HOTEN LIKE N'%{byName}%' ";
                }
                query += $"ORDER BY MANV OFFSET {offset} ROWS FETCH NEXT {fetchSize} ROWS ONLY";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            // check if there are result
                            if (!reader.HasRows)
                            {
                                return null;
                            }
                            // Access columns from the result set
                            string id = reader.GetString(reader.GetOrdinal("MANV"));
                            string name = reader.GetString(reader.GetOrdinal("HOTEN"));
                            int status = reader.GetInt32(reader.GetOrdinal("TRANGTHAI"));
                            receps.Add(new ReceptionistAccount()
                            {
                                Id = id,
                                Name = name,
                                Status = status,
                            });
                        }
                    }
                }
            }
            return receps;
        }

        public bool UpdateAccount(Interfaces.Account account)
        {
            string accountType;
            string accountIdName;
            if (account is AdministratorAccount)
            {
                accountType = "QTV";
                accountIdName = "MAQTV";
            }
            else if (account is ReceptionistAccount)
            {
                accountType = "NHAN_VIEN";
                accountIdName = "MANV";
            }
            else if (account is DentistAccount)
            {
                accountType = "NHA_SI";
                accountIdName = "MANS";
            }
            else if (account is CustomerAccount)
            {
                accountType = "KHACH_HANG";
                accountIdName = "MAKH";
            }
            else
            {
                return false;
            }

            string query = $"UPDATE {accountType} SET SDT = @SDT, HOTEN = @HOTEN, NGAYSINH = @NGAYSINH, DIACHI = @DIACHI, MATKHAU = @MATKHAU WHERE {accountIdName} = '{account.Id}'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SDT", account.PhoneNumber);
                        command.Parameters.AddWithValue("@HOTEN", account.Name);
                        command.Parameters.AddWithValue("@NGAYSINH", account.Birthday.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@DIACHI", account.Address);
                        command.Parameters.AddWithValue("@MATKHAU", account.Password);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool LockOrUnlockAccount(Interfaces.Account account, bool isLocked)
        {
            string accountType;
            string accountIdName;
            if (account is AdministratorAccount)
            {
                accountType = "QTV";
                accountIdName = "MAQTV";
            }
            else if (account is ReceptionistAccount)
            {
                accountType = "NHAN_VIEN";
                accountIdName = "MANV";
            }
            else if (account is DentistAccount)
            {
                accountType = "NHA_SI";
                accountIdName = "MANS";
            }
            else if (account is CustomerAccount)
            {
                accountType = "KHACH_HANG";
                accountIdName = "MAKH";
            }
            else
            {
                return false;
            }

            string query = $"UPDATE {accountType} SET TRANGTHAI = @TRANGTHAI WHERE {accountIdName} = '{account.Id}'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TRANGTHAI", isLocked ? 0 : 1);

                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddAccount(Interfaces.Account account)
        {
            string accountType;
            string accountIdName;
            if (account is AdministratorAccount)
            {
                accountType = "QTV";
                accountIdName = "MAQTV";
            }
            else if (account is ReceptionistAccount)
            {
                accountType = "NHAN_VIEN";
                accountIdName = "MANV";
            }
            else if (account is DentistAccount)
            {
                accountType = "NHA_SI";
                accountIdName = "MANS";
            }
            else if (account is CustomerAccount)
            {
                accountType = "KHACH_HANG";
                accountIdName = "MAKH";
            }
            else
            {
                return false;
            }

            string query = $"INSERT INTO {accountType} ({accountIdName}, SDT, HOTEN, NGAYSINH, DIACHI, MATKHAU, TRANGTHAI) VALUES (@ID, @SDT, @HOTEN, @NGAYSINH, @DIACHI, @MATKHAU, @TRANGTHAI)";
            string latestId = LatestId(account).Substring(2, 4);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", $"{accountType.Substring(0, 2)}" + $"{int.Parse(latestId) + 1}".PadLeft(4, '0'));
                        command.Parameters.AddWithValue("@SDT", account.PhoneNumber);
                        command.Parameters.AddWithValue("@HOTEN", account.Name);
                        command.Parameters.AddWithValue("@NGAYSINH", account.Birthday.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@DIACHI", account.Address);
                        command.Parameters.AddWithValue("@MATKHAU", account.Password);
                        command.Parameters.AddWithValue("@TRANGTHAI", account.Status);

                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Ooopsss");
                return false;
            }
        }

        public string LatestId(Interfaces.Account account)
        {
            string accountType = "UNKNOWN";
            string accountIdName;
            if (account is AdministratorAccount)
            {
                accountType = "QTV";
                accountIdName = "MAQTV";
            }
            else if (account is ReceptionistAccount)
            {
                accountType = "NHAN_VIEN";
                accountIdName = "MANV";
            }
            else if (account is DentistAccount)
            {
                accountType = "NHA_SI";
                accountIdName = "MANS";
            }
            else if (account is CustomerAccount)
            {
                accountType = "KHACH_HANG";
                accountIdName = "MAKH";
            }
            else
            {
                return "";
            }

            string query = $"SELECT {accountIdName} FROM {accountType}";
            string latestId = "";
            int latestIdParse = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ;
                        while (reader.Read())
                        {
                            // check if there are result
                            if (!reader.HasRows)
                            {
                                return "";
                            }
                            // Access columns from the result set
                            string tempIdString = reader.GetString(reader.GetOrdinal($"{accountIdName}"));
                            int tempId = int.Parse(tempIdString.Substring(2, 4));
                            if (tempId > latestIdParse)
                            {
                                latestIdParse = tempId;
                                latestId = tempIdString;
                            }
                        }
                    }
                }
            }
            return latestId;
        }

        public List<CustomerAccount> GetCustomers(string byName = null, int offset = 0, int fetchSize = 50)
        {
            List<CustomerAccount> customers = new();
            // query the database to get all orders
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM KHACH_HANG ";
                if (byName is not null)
                {
                    query += $"WHERE HOTEN LIKE N'%{byName}%' ";
                }
                query += $"ORDER BY MAKH OFFSET {offset} ROWS FETCH NEXT {fetchSize} ROWS ONLY";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            // check if there are result
                            if (!reader.HasRows)
                            {
                                return null;
                            }
                            // Access columns from the result set
                            string id = reader.GetString(reader.GetOrdinal("MAKH"));
                            string name = reader.GetString(reader.GetOrdinal("HOTEN"));
                            int status = reader.GetInt32(reader.GetOrdinal("TRANGTHAI"));
                            string phone = reader.GetString(reader.GetOrdinal("SDT"));
                            customers.Add(new CustomerAccount()
                            {
                                Id = id,
                                Name = name,
                                Status = status,
                                PhoneNumber = phone
                            });
                        }
                    }
                }
            }
            return customers;
        }

        public List<DentistAccount> GetDentists(string byName = null, int offset = 0, int fetchSize = 50)
        {
            List<DentistAccount> dentists = new();
            // query the database to get all orders
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM NHA_SI ";
                if (byName is not null)
                {
                    query += $"WHERE HOTEN LIKE N'%{byName}%' ";
                }
                query += $"ORDER BY MANS OFFSET {offset} ROWS FETCH NEXT {fetchSize} ROWS ONLY";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            // check if there are result
                            if (!reader.HasRows)
                            {
                                return null;
                            }
                            // Access columns from the result set
                            string id = reader.GetString(reader.GetOrdinal("MANS"));
                            string name = reader.GetString(reader.GetOrdinal("HOTEN"));
                            int status = reader.GetInt32(reader.GetOrdinal("TRANGTHAI"));
                            dentists.Add(new DentistAccount()
                            {
                                Id = id,
                                Name = name,
                                Status = status,
                            });
                        }
                    }
                }
            }
            return dentists;
        }

        public List<AdministratorAccount> GetAdministrators(string byName = null, int offset = 0, int fetchSize = 50)
        {
            List<AdministratorAccount> admins = new();
            // query the database to get all orders
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM QTV ";
                if (byName is not null)
                {
                    query += $"WHERE HOTEN LIKE N'%{byName}%' ";
                }
                query += $"ORDER BY MAQTV OFFSET {offset} ROWS FETCH NEXT {fetchSize} ROWS ONLY";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            // check if there are result
                            if (!reader.HasRows)
                            {
                                return null;
                            }
                            // Access columns from the result set
                            string id = reader.GetString(reader.GetOrdinal("MAQTV"));
                            string name = reader.GetString(reader.GetOrdinal("HOTEN"));
                            int status = reader.GetInt32(reader.GetOrdinal("TRANGTHAI"));
                            admins.Add(new AdministratorAccount()
                            {
                                Id = id,
                                Name = name,
                                Status = status,
                            });
                        }
                    }
                }
            }
            return admins;
        }

        public bool ResetPassword(Interfaces.Account account)
        {
            string accountType;
            string accountIdName;
            if (account is AdministratorAccount)
            {
                accountType = "QTV";
                accountIdName = "MAQTV";
            }
            else if (account is ReceptionistAccount)
            {
                accountType = "NHAN_VIEN";
                accountIdName = "MANV";
            }
            else if (account is DentistAccount)
            {
                accountType = "NHA_SI";
                accountIdName = "MANS";
            }
            else if (account is CustomerAccount)
            {
                accountType = "KHACH_HANG";
                accountIdName = "MAKH";
            }
            else
            {
                return false;
            }

            string query = $"UPDATE {accountType} SET MATKHAU = @MATKHAU WHERE {accountIdName} = '{account.Id}'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MATKHAU", "123456789".PadRight(50, ' '));

                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
