using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace App7.Logics
{
    public class Methoden
    {
        private static string ConnectionString = "";

        public static string GetSetConnectionString()
        {
            return ConnectionString;
        }

        public static void GetSetConnectionString(string value)
        {
            ConnectionString = value;
        }

        //Berechnen der Downloadzeit
        public static double BerechneDownloadZeit(int einheit, double datensize, double internetspeed)
        {
            switch (einheit)
            {
                case 0: // Byte
                    datensize = datensize / 1000000;
                    break;
                case 1: // KB
                    datensize = datensize / 1000;
                    break;
                case 2: // MB
                    break;
                case 3: // GB
                    datensize = datensize * 1000;
                    break;
                case 4: // TB
                    datensize = datensize * 1000000;
                    break;
            }

            //Dividiert die Dateigröße durch die Downloadzeit
            return datensize / internetspeed;
        }

        public static string Speedcheck()
        {
            //Ermittelt die Internetgeschwindigkeit anhand einer Zeitmessung, des Download von genau 25 Millionen Bytes
            Stopwatch watch = new Stopwatch();

            using (var client = new WebClient())
            {
                watch.Start();
                Uri downloadlink = new Uri("https://firl.online/download25");
                client.DownloadData(downloadlink);
                watch.Stop();
            }

            var speed = Math.Round(25000000 / watch.Elapsed.TotalSeconds / (1000 * 1000), 2);
            return speed.ToString();
        }

        public static string CheckDBSize(string cs)
        {
            //Erfassen der Datenbank Größe durch die Query sp_saceused
            SqlConnection Conn = new SqlConnection(cs);
            SqlCommand testCMD = new SqlCommand("sp_spaceused", Conn);

            testCMD.CommandType = CommandType.StoredProcedure;

            Conn.Open();

            SqlDataReader reader = testCMD.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string sum = reader["database_size"].ToString();
                    return sum.Replace(".", ",");
                }
            }


            return "0";
        }

        public static string CheckDBConnection(string cs)
        {
            StringBuilder errorMessages = new StringBuilder();
            SqlConnection conn = new SqlConnection(cs);
            string resulttext = "";

            //try
            //{
            //    conn.open();
            //}
            //catch (sqlexception ex)
            //{
            //    for (int i = 0; i < ex.errors.count; i++)
            //    {
            //        errormessages.append("index #" + i + "\n" +
            //            "nachricht: " + ex.errors[i].message + "\n" +
            //            "linenumber: " + ex.errors[i].linenumber + "\n" +
            //            "source: " + ex.errors[i].source);
            //    }
            //    return errormessages.tostring();
            //}
            //finally
            //{
            //    resulttext = "ok";
            //}

            return resulttext;
        }
    }
}
