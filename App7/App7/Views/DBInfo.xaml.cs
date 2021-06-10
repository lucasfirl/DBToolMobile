using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App7.Logics;

namespace App7.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DBInfo : ContentPage
    {
        public DBInfo()
        {
            InitializeComponent();
        }

        private async void Button_Safedbinfo(object sender, EventArgs e)
        {
            string cs = @"Data Source = " + dbadresse.Text + "; Initial Catalog = " + dbname.Text + "; User ID = " + dbuser.Text + "; Password = " + dbpw.Text + ";";

            loading.IsRunning = true;
            safebutton.IsEnabled = false;

            string result = await Task.Run(() => Methoden.CheckDBConnection(cs));

            loading.IsRunning = false;
            safebutton.IsEnabled = true;


            if (result == "ok")
            {
                Methoden.GetSetConnectionString(cs);
                await DisplayAlert("Info", "Verbindung erfolgreich!", "OK");
            }
            else
            {
                await DisplayAlert("Alert", result, "OK");
            }
        }
    }
}