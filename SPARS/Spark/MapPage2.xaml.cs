using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Spark
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage2 : Page
    {

        RandomAccessStreamReference mapIconStreamReference3, mapIconStreamReference4;

        public MapPage2()
        {
            this.InitializeComponent();

            MapControl2.Loaded += MapControl2_Loaded;
            mapIconStreamReference3 = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapPinRed.png"));
            mapIconStreamReference4 = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/hitnaPin.png"));
        }
        double x2, y2;
        //KUM
        double kumx = 43.869310, kumy = 18.413738;
        //VRAZOVA
        double vrazx = 43.856877, vrazy = 18.411144;
        BasicGeoposition endLocation;
        BasicGeoposition startLocation;

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuPage));
        }

        bool visible2 = true;

        static void Sleep(int ms)
        {
            new System.Threading.ManualResetEvent(false).WaitOne(ms);
        }

        private void buttonSplit_Click(object sender, RoutedEventArgs e)
        {

            if (visible2 == false)
            {
                visible2 = true;
                buttonSplit.Margin = new Thickness(0, 0, 0, 0);
                image.Margin = new Thickness(0, 0, 0, 0);
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/FrontArrow.png"));
                buttonSplit.Opacity = 0.8;
                MySplitView.Visibility = Visibility.Collapsed;


            }
            else if (visible2 == true)
            {
                visible2 = false;
                buttonSplit.Margin = new Thickness(200, 0, 0, 0);
                image.Margin = new Thickness(200, 0, 0, 0);
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/BackArrow.png"));
                buttonSplit.Opacity = 0.8;
                MySplitView.Visibility = Visibility.Visible;
            }

            //MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            //SplitView1.Visibility = Visibility.Visible;

            //promjeni strelicu img nesta nesta

        }

        public double racunajDist(double a, double b)
        {
            return Math.Sqrt(Math.Pow(a - x2, 2) + Math.Pow(b - y2, 2));
        }

        double nearestX, nearestY;

        private async void MapControl2_Loaded(object sender, RoutedEventArgs e)
        {

            // Set your current location.
            var accessStatus = await Geolocator.RequestAccessAsync();
            //ovo sam izbacio iz switcha da bi se moglo koristiti dalje u funkciji
            Geolocator geolocator = new Geolocator();
            Geoposition pos;
            Geopoint myLocation;

            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:

                    // Get the current location.
                    //Geolocator geolocator = new Geolocator();
                    pos = await geolocator.GetGeopositionAsync();
                    myLocation = pos.Coordinate.Point;
                    //stavljam vrijednosti x i y da se koriste dalje
                    x2 = myLocation.Position.Latitude;
                    y2 = myLocation.Position.Longitude;
                    // Set the map location.
                    MapControl2.Center = myLocation;
                    MapControl2.ZoomLevel = 17;
                    MapControl2.LandmarksVisible = true;
                    break;

                case GeolocationAccessStatus.Denied:
                    this.Frame.Navigate(typeof(MenuPage));
                    // Handle the case  if access to location is denied.
                    break;

                case GeolocationAccessStatus.Unspecified:
                    this.Frame.Navigate(typeof(MenuPage));
                    // Handle the case if  an unspecified error occurs.
                    break;
            }




            MapControl2.Center =
               new Geopoint(new BasicGeoposition()
               {   //trenutna lok
                   Latitude = x2,

                   Longitude = y2
               });

            /*
            Geopoint Parking =
              new Geopoint(new BasicGeoposition()
              {
                  //Geopoint for parking
                  Latitude = putX,

                  Longitude = putY

              });*/

            Geopoint KUM =
              new Geopoint(new BasicGeoposition()
              {
                  //Geopoint for parking
                  Latitude = kumx,

                  Longitude = kumy

              });
            Geopoint Vrazova =
              new Geopoint(new BasicGeoposition()
              {
                  //Geopoint for parking
                  Latitude = vrazx,

                  Longitude = vrazy

              });


            MapIcon mapIcon1 = new MapIcon();
            mapIcon1.Location = MapControl2.Center;
            mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon1.Title = "Trenutna lokacija";
            mapIcon1.Image = mapIconStreamReference3;
            mapIcon1.ZIndex = 0;
            MapControl2.MapElements.Add(mapIcon1);

            MapIcon mapIcon2 = new MapIcon();
            mapIcon2.Location = KUM;
            mapIcon2.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon2.Title = "KUM";
            mapIcon2.Image = mapIconStreamReference4;
            mapIcon2.ZIndex = 0;
            MapControl2.MapElements.Add(mapIcon2);

            MapIcon mapIcon3 = new MapIcon();
            mapIcon3.Location = Vrazova;
            mapIcon3.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon3.Title = "Vrazova";
            mapIcon3.Image = mapIconStreamReference4;
            mapIcon3.ZIndex = 0;
            MapControl2.MapElements.Add(mapIcon3);


            /*MapIcon mapIcon1 = new MapIcon();
            mapIcon1.Location = MapControl1.Center;
            mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon1.Title = "Trenutna lokacija";
            mapIcon1.Image = mapIconStreamReference1;
            mapIcon1.ZIndex = 0;
            MapControl1.MapElements.Add(mapIcon1);
            */

            // nista ovaj komentar iznad, ova pozicija je startna lokacija, x i y su globalni definisani iznad
            startLocation = new BasicGeoposition() { Latitude = x2, Longitude = y2 };

            //endLocation = new BasicGeoposition() { Latitude = kumx, Longitude = kumy };


            // Get the route between the points.
            /*
            MapRouteFinderResult routeResult =
                  await MapRouteFinder.GetDrivingRouteAsync(
                  new Geopoint(startLocation),
                  new Geopoint(endLocation),
                  MapRouteOptimization.Time,
                  MapRouteRestrictions.None);

            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                // Use the route to initialize a MapRouteView.
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.PaleVioletRed;
                viewOfRoute.OutlineColor = Colors.DarkRed;

                // Add the new MapRouteView to the Routes collection
                // of the MapControl.
                MapControl2.Routes.Add(viewOfRoute);

                // Fit the MapControl to the route.
                await MapControl2.TrySetViewBoundsAsync(
                      routeResult.Route.BoundingBox,
                      null,
                      Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);
            }*/

            double minDist = 100000;


            double[] arrayX = { vrazx, kumx};

            double[] arrayY = { vrazy, kumy};

            for (int i = 0; i < 2; i++)
            {
                if (racunajDist(arrayX[i], arrayY[i]) < minDist)
                {
                    nearestX = arrayX[i];
                    nearestY = arrayY[i];
                    minDist = racunajDist(arrayX[i], arrayY[i]);
                }
            }
            

        }


        bool vrazova_crtana = false;
        bool kum_crtana = false;
        bool najkraca_crtana = false;

        private void button20_Click(object sender, RoutedEventArgs e)
        {
            najkraca_crtana = true;
            visible2 = true;
            MySplitView.Visibility = Visibility.Collapsed;        
        buttonSplit.Margin = new Thickness(0, 0, 0, 0);
            image.Margin = new Thickness(0, 0, 0, 0);
            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/FrontArrow.png"));
            crtaj_rutu(nearestX, nearestY);
        }

        private async void crtaj_rutu(double lat, double lng)
        {
            //BRISANJE NAJBLIZE
            if (najkraca_crtana == true && lat != nearestX && lng != nearestY)
            {
                endLocation = new BasicGeoposition() { Latitude = nearestX, Longitude = nearestY };

                MapRouteFinderResult routeResult =
                  await MapRouteFinder.GetDrivingRouteAsync(
                  new Geopoint(startLocation),
                  new Geopoint(endLocation),
                  MapRouteOptimization.Time,
                  MapRouteRestrictions.None);

                if (routeResult.Status == MapRouteFinderStatus.Success)
                {
                    // Use the route to initialize a MapRouteView.
                    MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                    viewOfRoute.RouteColor = Colors.White;
                    viewOfRoute.OutlineColor = Colors.White;

                    // Add the new MapRouteView to the Routes collection
                    // of the MapControl.
                    MapControl2.Routes.Add(viewOfRoute);

                }
            }

            //KUM BRISANJE
            if (kum_crtana == true && lat != kumx && lng != kumy)
            {
                endLocation = new BasicGeoposition() { Latitude = kumx, Longitude = kumy };

                MapRouteFinderResult routeResult =
                  await MapRouteFinder.GetDrivingRouteAsync(
                  new Geopoint(startLocation),
                  new Geopoint(endLocation),
                  MapRouteOptimization.Time,
                  MapRouteRestrictions.None);

                if (routeResult.Status == MapRouteFinderStatus.Success)
                {
                    // Use the route to initialize a MapRouteView.
                    MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                    viewOfRoute.RouteColor = Colors.White;
                    viewOfRoute.OutlineColor = Colors.White;

                    // Add the new MapRouteView to the Routes collection
                    // of the MapControl.
                    MapControl2.Routes.Add(viewOfRoute);

                }
            }

            //VRAZOVA BRISANJE
            if (vrazova_crtana == true && lat != vrazx && lng != vrazy)
            {
                endLocation = new BasicGeoposition() { Latitude = vrazx, Longitude = vrazy };

                MapRouteFinderResult routeResult =
                  await MapRouteFinder.GetDrivingRouteAsync(
                  new Geopoint(startLocation),
                  new Geopoint(endLocation),
                  MapRouteOptimization.Time,
                  MapRouteRestrictions.None);

                if (routeResult.Status == MapRouteFinderStatus.Success)
                {
                    // Use the route to initialize a MapRouteView.
                    MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                    viewOfRoute.RouteColor = Colors.White;
                    viewOfRoute.OutlineColor = Colors.White;

                    // Add the new MapRouteView to the Routes collection
                    // of the MapControl.
                    MapControl2.Routes.Add(viewOfRoute);

                }
            }

            endLocation = new BasicGeoposition() { Latitude = lat, Longitude = lng };

            // Get the route between the points.

            MapRouteFinderResult routeResult1 =
                  await MapRouteFinder.GetDrivingRouteAsync(
                  new Geopoint(startLocation),
                  new Geopoint(endLocation),
                  MapRouteOptimization.Time,
                  MapRouteRestrictions.None);

            if (routeResult1.Status == MapRouteFinderStatus.Success)
            {
                // Use the route to initialize a MapRouteView.
                MapRouteView viewOfRoute = new MapRouteView(routeResult1.Route);
                viewOfRoute.RouteColor = Colors.OrangeRed;
                viewOfRoute.OutlineColor = Colors.DarkRed;

                // Add the new MapRouteView to the Routes collection
                // of the MapControl.
                MapControl2.Routes.Add(viewOfRoute);
                System.Text.StringBuilder routeInfo = new System.Text.StringBuilder();

                int tmp = (int)(routeResult1.Route.EstimatedDuration.TotalMinutes * 100);
                double vrijememin = tmp / 100;

                textBlock6.Text = vrijememin.ToString();
                textBlock8.Text = (routeResult1.Route.LengthInMeters / 1000).ToString();


                // Fit the MapControl to the route.
                await MapControl2.TrySetViewBoundsAsync(
                      routeResult1.Route.BoundingBox,
                      null,
                      Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);
            }

        }


        private void button4_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.Visibility = Visibility.Collapsed;
            kum_crtana = true;
            visible2 = true;
            buttonSplit.Margin = new Thickness(0, 0, 0, 0);
            image.Margin = new Thickness(0, 0, 0, 0);
            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/FrontArrow.png"));
            crtaj_rutu(kumx, kumy);
            // brisi_rutu(sccx, sccy);
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.Visibility = Visibility.Collapsed;
            vrazova_crtana = true;
            visible2 = true;
            buttonSplit.Margin = new Thickness(0, 0, 0, 0);
            image.Margin = new Thickness(0, 0, 0, 0);
            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/FrontArrow.png"));
            crtaj_rutu(vrazx, vrazy);
            //   brisi_rutu(bosx, bosy);
        }


        private void button3_Click(object sender, RoutedEventArgs e)
        {
            MapControl2.Center =
               new Geopoint(new BasicGeoposition()
               {   //trenutna lok
                   Latitude = x2,

                   Longitude = y2
               });
        }



    }
}