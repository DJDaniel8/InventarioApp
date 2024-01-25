using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Services.Alerts
{
    public static class ThrowMessage
    {
        public static void ShowErrorMessage(string message)
        {
            App.Current.MainPage.DisplayAlert("Error", message, "Ok");
        }
    }
}
