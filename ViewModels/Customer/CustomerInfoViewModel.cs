﻿using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;

namespace QuanLyNhaKhoa.Models
{
    public class CustomerInfoViewModel : INotifyPropertyChanged
    {
        private string _cusID;
        private string _cusName;
        private DateOnly _dateOfBirth;
        private string _phoneNum;
        private string _addr;

        public string CusID
        {
            get; set;
        }
        public string CusName
        {
            get; set;
        }

        public DateOnly DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                _dateOfBirth = value;
                NotifyPropertyChanged(nameof(DateOfBirth));
            }
        }
        public string PhoneNum
        {
            get
            {
                return _phoneNum;
            }
            set
            {
                _phoneNum = value;
                NotifyPropertyChanged(nameof(PhoneNum));
            }
        }
        public string Addr
        {
            get
            {
                return _addr;
            }
            set
            {
                _addr = value;
                NotifyPropertyChanged(nameof(Addr));
            }
        }

        public CustomerInfoViewModel()
        {

        }


        public CustomerInfoViewModel(CustomerAccount customerAccount)
        {
            CusID = customerAccount.Id;
            CusName = customerAccount.Name;
            DateTime date = customerAccount.Birthday;
            DateOfBirth = DateOnly.FromDateTime(date);
            PhoneNum = customerAccount.PhoneNumber;
            Addr = customerAccount.Address;
        }



        public CustomerInfoViewModel GetCustomerInfo(string connectionString, CustomerInfoViewModel customerInfo, string CusId)
        {
            string GetCustomerInfoQuery = "select MAKH, HOTEN, NGAYSINH, SDT, DIACHI from KHACH_HANG " +
                                                "where MAKH = " + "'" + CusId + "'";

            try
            {
                using (var conn = new SqlConnection(@connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = GetCustomerInfoQuery;
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                //var customerInfo = new CustomerInfoViewModel();

                                while (reader.Read())
                                {
                                    customerInfo.CusID = reader.GetString(0);
                                    customerInfo.CusName = reader.GetString(1);
                                    DateTime date = reader.GetDateTime(2);
                                    customerInfo.DateOfBirth = DateOnly.FromDateTime(date);
                                    customerInfo.PhoneNum = reader.GetString(3);
                                    customerInfo.Addr = reader.GetString(4);
                                }

                            }
                        }
                    }
                }
                return customerInfo;

            }
            catch (Exception eSql)
            {
                Debug.WriteLine($"Exception: {eSql.Message}");
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

