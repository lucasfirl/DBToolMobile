using App7.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App7.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Calculating : ContentPage
    {
        public Calculating()
        {
            InitializeComponent();
            internetspeed.Text = "";
            dbsize.Text = "";
        }
        private bool accept = false;


        private async void Button_Dbsizeauto(object sender, EventArgs e)
        {
            if (Methoden.GetSetConnectionString() == "")
            {
                await DisplayAlert("Fehler", "Sie müssen zuerst Ihre SQL Verbindungsdaten eingeben.", "OK");
            }
            else
            {
                sizebutton.IsEnabled = false;
                loading.IsRunning = true;
                dbsize.Text = await Task.Run(() => Methoden.CheckDBSize(Methoden.GetSetConnectionString()));
                einheitpicker.SelectedIndex = 2;
                sizebutton.IsEnabled = true;
                loading.IsRunning = false;
            }
        }

        private async Task RunSpeedcheck()
        {
            speedbutton.IsEnabled = false;
            loading.IsRunning = true;
            internetspeed.Text = await Task.Run(() => Methoden.Speedcheck());
            speedbutton.IsEnabled = true;
            loading.IsRunning = false;
        }

        private async void Button_Speedauto(object sender, EventArgs e)
        {
            //bool displayok = await DisplayAlert("Warnung", "Die Automatische Downloadspeed ermittlung verbraucht in der Regel 25Mb Datenvolume. \n\n Möchten Sie fortfahren?", "Ja", "Nein");

            if (accept)
            {
                await RunSpeedcheck();
            }
            else
            {
                bool displayok = await DisplayAlert("Warnung", "Die automatische Downloadspeed Ermittlung verbraucht in der Regel 25Mb Datenvolume. \n\n Möchten Sie fortfahren?", "Ja", "Nein");

                if (displayok)
                {
                    accept = true;
                    await RunSpeedcheck();
                }
            }
        }



        private async void Button_Check(object sender, EventArgs e)
        {
            if (internetspeed.Text != "" && dbsize.Text != "" && Regex.Matches(internetspeed.Text, @"[a-zA-Z]").Count < 1 && Regex.Matches(dbsize.Text, @"[a-zA-Z]").Count < 1)
            {
                double dbsizenumber = Convert.ToDouble(dbsize.Text);
                double internetspeednumber = Convert.ToDouble(internetspeed.Text) / 8;

                if (einheitpicker.SelectedIndex == -1)
                {
                    await DisplayAlert("Fehler", "Bitte geben Sie eine Einheit an.", "OK");
                }

                double result = Methoden.BerechneDownloadZeit(einheitpicker.SelectedIndex, dbsizenumber, internetspeednumber);

                double hours = Math.Round(result / 3600);
                double minutes = Math.Round(result / 60 % 60);
                resultlabel.Text = "Geschätze Wartezeit: " + hours + "h " + minutes + "m";
            }
            else
            {
                await DisplayAlert("Fehler", "Bitte geben Sie in beiden Boxen einen Wert an.\n\nOder Überprüfen Sie auf Buchstaben.", "OK");
            }
        }
    }
}