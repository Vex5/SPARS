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
using Windows.UI.Popups;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Spark
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {


        RandomAccessStreamReference mapIconStreamReference1, mapIconStreamReference2;

        public MapPage()
        {
            this.InitializeComponent();          

            MapControl1.Loaded += MapControl1_Loaded;
            mapIconStreamReference1 = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapPin.png"));
            mapIconStreamReference2 = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/parkingPin.png"));
        }
        

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuPage));
        }        

        bool visible = true;
        
        private void button_Click(object sender, RoutedEventArgs e)
        {            
            
            if(visible == false)
            {
                visible = true;
                button.Margin = new Thickness(0, 0, 0, 0);
                image.Margin = new Thickness(0, 0, 0, 0);
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/FrontArrow.png"));
                button.Opacity = 0.8;
                MySplitView.Visibility = Visibility.Collapsed;           

            }
            else if(visible == true)
            {
                visible = false;
                button.Margin = new Thickness(200, 0, 0, 0);
                image.Margin = new Thickness(200, 0, 0, 0);
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/BackArrow.png"));
                button.Opacity = 0.8;
                MySplitView.Visibility = Visibility.Visible;
            }

            //MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            //SplitView1.Visibility = Visibility.Visible;
            
            //promjeni strelicu img nesta nesta
           
        }

        //scc parking
        double sccx = 43.855449, sccy = 18.407314;
        //bbi parking
        double bbix = 43.858002, bbiy = 18.416905;
        //Bosmal
        double bosx = 43.846577, bosy = 18.374076;
        //Skenderija
        double skendx = 43.855569, skendy = 18.413638;
        //Wog
        double wogx = 43.899570, wogy = 18.344405;
        //KCUS
        double kcusx = 43.868465, kcusy = 18.418574;        

        //globalno deklarisani da bi se mogli koristiti dole u startpos za rutu
        double x, y, putX, putY;
        BasicGeoposition endLocation;      
        BasicGeoposition startLocation;

        //FUNKCIJA ZA PRAVOLINIJSKU UDALJENOST IZMEDJU DVIJE TACKE U RAVNI
        public double racunajDist(double a, double b)
        {
            return Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2));
        }
        //VARIJABLE KOJE CUVAJU VRIJEDNOST LONG I LAT OD NAJBLIZE TACKE
        double nearestX, nearestY;

        private async void MapControl1_Loaded(object sender, RoutedEventArgs e)
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
                    x = myLocation.Position.Latitude;
                    y = myLocation.Position.Longitude;
                    // Set the map location.
                    MapControl1.Center = myLocation;
                    MapControl1.ZoomLevel = 17;
                    MapControl1.LandmarksVisible = true;
                    //OVDJE ZA PREZENTACIJU
                   /* x = bbix;
                    y = bbiy;*/
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


            Geopoint SccParking =
            new Geopoint(new BasicGeoposition()
            {
                //Geopoint for parking
                Latitude = sccx,

                Longitude = sccy

            });

            Geopoint BbiParking =
              new Geopoint(new BasicGeoposition()
              {
                  //Geopoint for parking
                  Latitude = bbix,

                  Longitude = bbiy

              });

            Geopoint BosParking =
            new Geopoint(new BasicGeoposition()
            {
                //Geopoint for parking
                Latitude = bosx,

                Longitude = bosy

            });

            Geopoint SkendParking =
            new Geopoint(new BasicGeoposition()
            {
                //Geopoint for parking
                Latitude = skendx,

                Longitude = skendy

            });

            Geopoint WogParking =
            new Geopoint(new BasicGeoposition()
            {
                //Geopoint for parking
                Latitude = wogx,

                Longitude = wogy

            });

            Geopoint KcusParking =
            new Geopoint(new BasicGeoposition()
            {
                //Geopoint for parking
                Latitude = kcusx,

                Longitude = kcusy

            });

            MapControl1.Center =
               new Geopoint(new BasicGeoposition()
               {   //trenutna lok
                   Latitude = x,

                   Longitude = y
               });           
            

            MapIcon mapIcon1 = new MapIcon();
            mapIcon1.Location = MapControl1.Center;
            mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon1.Title = "Trenutna lokacija";
            mapIcon1.Image = mapIconStreamReference1;
            mapIcon1.ZIndex = 0;
            MapControl1.MapElements.Add(mapIcon1);

            MapIcon mapIcon2 = new MapIcon();
            mapIcon2.Location = BbiParking;
            mapIcon2.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon2.Title = "BBI";
            mapIcon2.Image = mapIconStreamReference2;
            mapIcon2.ZIndex = 0;
            MapControl1.MapElements.Add(mapIcon2);

            MapIcon mapIcon3 = new MapIcon();
            mapIcon3.Location = SccParking;
            mapIcon3.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon3.Title = "SCC";
            mapIcon3.Image = mapIconStreamReference2;
            mapIcon3.ZIndex = 0;
            MapControl1.MapElements.Add(mapIcon3);

            MapIcon mapIcon4 = new MapIcon();
            mapIcon4.Location = BosParking;
            mapIcon4.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon4.Title = "Bosmal";
            mapIcon4.Image = mapIconStreamReference2;
            mapIcon4.ZIndex = 0;
            MapControl1.MapElements.Add(mapIcon4);

            MapIcon mapIcon5 = new MapIcon();
            mapIcon5.Location = SkendParking;
            mapIcon5.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon5.Title = "Skenderija";
            mapIcon5.Image = mapIconStreamReference2;
            mapIcon5.ZIndex = 0;
            MapControl1.MapElements.Add(mapIcon5);

            MapIcon mapIcon6 = new MapIcon();
            mapIcon6.Location = WogParking;
            mapIcon6.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon6.Title = "WOG";
            mapIcon6.Image = mapIconStreamReference2;
            mapIcon6.ZIndex = 0;
            MapControl1.MapElements.Add(mapIcon6);

            MapIcon mapIcon7 = new MapIcon();
            mapIcon7.Location = KcusParking;
            mapIcon7.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon7.Title = "KCUS";
            mapIcon7.Image = mapIconStreamReference2;
            mapIcon7.ZIndex = 0;
            MapControl1.MapElements.Add(mapIcon7);

            //RUTA
            // nista ovaj komentar iznad, ova pozicija je startna lokacija, x i y su globalni definisani iznad
            startLocation = new BasicGeoposition() { Latitude = x, Longitude = y };

            double minDist = 100000;


            double[] arrayX = { sccx, bbix, bosx, skendx, wogx, kcusx };

            double[] arrayY = { sccy, bbiy, bosy, skendy, wogy, kcusy };

            for (int i = 0; i < 6; i++)
            {
                if (racunajDist(arrayX[i], arrayY[i]) < minDist)
                {
                    nearestX = arrayX[i];
                    nearestY = arrayY[i];
                    minDist = racunajDist(arrayX[i], arrayY[i]);
                }
            }

            endLocation = new BasicGeoposition() { Latitude = nearestX, Longitude = nearestY };
            /*crtaj_rutu(endLocation.Latitude, endLocation.Longitude);*/           


        }

        private void button20_Click(object sender, RoutedEventArgs e)
        {
            najkraca_crtana = true;
            MySplitView.Visibility = Visibility.Collapsed;
            visible = true;
            button.Margin = new Thickness(0, 0, 0, 0);
            image.Margin = new Thickness(0, 0, 0, 0);
            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/FrontArrow.png"));
            crtaj_rutu(nearestX, nearestY);
        }

        bool najkraca_crtana = false;
        bool bosmal_crtana = false;
        bool scc_crtana = false;
        bool bbi_crtana = false;        
        bool skenderija_crtana = false;
        bool wog_crtana = false;
        bool kcus_crtana = false;
        private async void crtaj_rutu(double lat, double lng)
        {
            
            //BOSMAL BRISANJE
            if (bosmal_crtana == true && lat != bosx && lng != bosy)
            {
                endLocation = new BasicGeoposition() { Latitude = bosx, Longitude = bosy };

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
                    MapControl1.Routes.Add(viewOfRoute);

                }
                
                
            }

            //SCC BRISANJE
            if (scc_crtana == true && lat != sccx && lng != sccy)
            {
                endLocation = new BasicGeoposition() { Latitude = sccx, Longitude = sccy };

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
                    MapControl1.Routes.Add(viewOfRoute);
                    
                }
            }

            //BBI BRISANJE
            if (bbi_crtana == true && lat != bbix && lng != bbiy)
            {
                endLocation = new BasicGeoposition() { Latitude = bbix, Longitude = bbiy };

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
                    MapControl1.Routes.Add(viewOfRoute);

                }
            }

            //SKENDERIJA BRISANJE
            if (skenderija_crtana == true && lat != skendx && lng != skendy)
            {
                endLocation = new BasicGeoposition() { Latitude = skendx, Longitude = skendy };

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
                    MapControl1.Routes.Add(viewOfRoute);

                }
            }

            //WOG BRISANJE
            if (wog_crtana == true && lat != wogx && lng != wogy)
            {
                endLocation = new BasicGeoposition() { Latitude = wogx, Longitude = wogy };

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
                    MapControl1.Routes.Add(viewOfRoute);

                }
            }

            //KCUS BRISANJE
            if (kcus_crtana == true && lat != kcusx && lng != kcusy)
            {
                endLocation = new BasicGeoposition() { Latitude = kcusx, Longitude = kcusy };

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
                    MapControl1.Routes.Add(viewOfRoute);

                }
            }

            //NAJKRACA BRISANJE
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
                    MapControl1.Routes.Add(viewOfRoute);

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
                viewOfRoute.RouteColor = Colors.SkyBlue;
                viewOfRoute.OutlineColor = Colors.DarkBlue;

                // Add the new MapRouteView to the Routes collection
                // of the MapControl.
                MapControl1.Routes.Add(viewOfRoute);
                System.Text.StringBuilder routeInfo = new System.Text.StringBuilder();

                int tmp = (int)(routeResult1.Route.EstimatedDuration.TotalMinutes * 100);
                double vrijememin = tmp / 100;

                textBlock6.Text = vrijememin.ToString();
                textBlock8.Text = (routeResult1.Route.LengthInMeters/1000).ToString();


                // Fit the MapControl to the route.
                await MapControl1.TrySetViewBoundsAsync(
                      routeResult1.Route.BoundingBox,
                      null,
                      Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);
            }
            

        }
        
        
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            bosmal_crtana = true;
            MySplitView.Visibility = Visibility.Collapsed;
            visible = true;
            button.Margin = new Thickness(0, 0, 0, 0);
            image.Margin = new Thickness(0, 0, 0, 0);
            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/FrontArrow.png"));
            crtaj_rutu(bosx, bosy);
           // brisi_rutu(sccx, sccy);
        }
        
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            scc_crtana = true;
            MySplitView.Visibility = Visibility.Collapsed;
            visible = true;
            button.Margin = new Thickness(0, 0, 0, 0);
            image.Margin = new Thickness(0, 0, 0, 0);
            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/FrontArrow.png"));
            crtaj_rutu(sccx, sccy);
         //   brisi_rutu(bosx, bosy);
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            bbi_crtana = true;
            MySplitView.Visibility = Visibility.Collapsed;
            visible = true;
            button.Margin = new Thickness(0, 0, 0, 0);
            image.Margin = new Thickness(0, 0, 0, 0);
            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/FrontArrow.png"));
            crtaj_rutu(bbix, bbiy);
            //   brisi_rutu(bosx, bosy);
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            skenderija_crtana = true;
            MySplitView.Visibility = Visibility.Collapsed;
            visible = true;
            button.Margin = new Thickness(0, 0, 0, 0);
            image.Margin = new Thickness(0, 0, 0, 0);
            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/FrontArrow.png"));
            crtaj_rutu(skendx, skendy);
            //   brisi_rutu(bosx, bosy);
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            wog_crtana = true;
            MySplitView.Visibility = Visibility.Collapsed;
            visible = true;
            button.Margin = new Thickness(0, 0, 0, 0);
            image.Margin = new Thickness(0, 0, 0, 0);
            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/FrontArrow.png"));
            crtaj_rutu(wogx, wogy);
            //   brisi_rutu(bosx, bosy);
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            kcus_crtana = true;
            MySplitView.Visibility = Visibility.Collapsed;
            visible = true;
            button.Margin = new Thickness(0, 0, 0, 0);
            image.Margin = new Thickness(0, 0, 0, 0);
            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/FrontArrow.png"));
            crtaj_rutu(kcusx, kcusy);
            //   brisi_rutu(bosx, bosy);
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            MapControl1.Center =
               new Geopoint(new BasicGeoposition()
               {   //trenutna lok
                   Latitude = x,

                   Longitude =  y
               });
        }



    }
}