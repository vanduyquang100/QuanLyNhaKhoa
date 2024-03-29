﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System;

namespace QuanLyNhaKhoa.ViewModels.Dentist
{
    public class DentistAppointmentViewModel : INotifyPropertyChanged
    {
        private string _appoID;
        private string _cusID;
        private string _cusName;
        private string _mrID;
        private DateOnly _appoDate;
        private TimeOnly _appoTime;

        public string AppoID
        {
            get; set;
        }
        public string CusID
        {
            get
            {
                return _cusID;
            }
            set
            {
                _cusID = value;
                NotifyPropertyChanged(nameof(CusID));
            }
        }

        public string CusName
        {
            get
            {
                return _cusName;
            }
            set
            {
                _cusName = value;
                NotifyPropertyChanged(nameof(CusName));
            }
        }

        public string _MrID
        {
            set
            {
                _mrID = value;
                NotifyPropertyChanged(nameof(_mrID));

            }
            get
            {
                return _mrID;
            }
        }

        public DateOnly AppoDate
        {
            get
            {
                return _appoDate;
            }
            set
            {
                _appoDate = value;
                NotifyPropertyChanged(nameof(AppoDate));
            }
        }

        public TimeOnly AppoTime
        {
            get
            {
                return _appoTime;
            }
            set
            {
                _appoTime = value;
                NotifyPropertyChanged(nameof(AppoTime));
            }
        }

        public ObservableCollection<DentistAppointmentViewModel> GetAppointments(string connectionString, string denID)
        {
            var GetAppointmentQuery = "select LH.MALICHHEN, KH.HOTEN, KH.MAKH, LH.GIOKHAM, LH.NGAYKHAM from LICH_HEN LH " +
                             "join KHACH_HANG KH on LH.MAKH = KH.MAKH " +
                             "join NHA_SI NS on LH.NHASI = NS.MANS " +
                             "where NS.MANS = '" + denID + "' " +
                             "order by LH.NGAYKHAM asc, LH.GIOKHAM asc";

            var appointments = new ObservableCollection<DentistAppointmentViewModel>();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = GetAppointmentQuery;
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var DentistAppointment = new DentistAppointmentViewModel();
                                    DentistAppointment.AppoID = reader.GetString(0);
                                    DentistAppointment.CusName = reader.GetString(1);
                                    DentistAppointment.CusID = reader.GetString(2);
                                    var time = reader.GetTimeSpan(3);
                                    DentistAppointment.AppoTime = TimeOnly.FromTimeSpan(time);
                                    var date = reader.GetDateTime(4);
                                    DentistAppointment.AppoDate = DateOnly.FromDateTime(date);


                                    appointments.Add(DentistAppointment);
                                }
                            }
                        }
                    }
                }
                return appointments;
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
