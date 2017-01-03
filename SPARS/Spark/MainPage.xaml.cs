using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Spark
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

        }

        ////new

        static void Sleep(int ms)
        {
            new System.Threading.ManualResetEvent(false).WaitOne(ms);
        }

        public void Konekcija()
        {

            var profile = NetworkInformation.GetInternetConnectionProfile();
            // TODO: complete check
            if (profile != null)
            {

                this.Frame.Navigate(typeof(MenuPage));

            }
            else {

                new System.Threading.AutoResetEvent(false).WaitOne(100);
                Konekcija();
            }

        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Konekcija();
        }
    }




}