using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Spark
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BrainMap : Page
    {
        double zomx = 43.859565, zomy = 18.416069;

        RandomAccessStreamReference mapIconStreamReference5, mapIconStreamReference6;

        private void button21_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MenuPage));
        }

        public BrainMap()
        {
            this.InitializeComponent();

            MapControl3.Loaded += MapControl3_Loaded;

            mapIconStreamReference5 = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/ZombiePin.png"));
            mapIconStreamReference6 = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/brainPin.png"));

        }
        private async void MapControl3_Loaded(object sender, RoutedEventArgs e)
        {
            Geopoint Zombie =
             new Geopoint(new BasicGeoposition()
             {
                 //Geopoint zombie
                 Latitude = zomx,

                 Longitude = zomy

             });
            Geopoint Mozak =
              new Geopoint(new BasicGeoposition()
              {
                  //Geopoint for parking
                  Latitude = 43.846286,


                  Longitude = 18.374677

              });

            MapControl3.Center =
               new Geopoint(new BasicGeoposition()
               {   //trenutna lok
                   Latitude = zomx,

                   Longitude = zomy
               });
            MapControl3.ZoomLevel = 17;
            MapControl3.LandmarksVisible = true;


            MapIcon mapIcon1 = new MapIcon();
            mapIcon1.Location = Zombie;
            mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon1.Title = "Zombie";
            mapIcon1.Image = mapIconStreamReference5;
            mapIcon1.ZIndex = 0;
            MapControl3.MapElements.Add(mapIcon1);

            MapIcon mapIcon2 = new MapIcon();
            mapIcon2.Location = Mozak;
            mapIcon2.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon2.Title = "Mozak";
            mapIcon2.Image = mapIconStreamReference6;
            mapIcon2.ZIndex = 0;
            MapControl3.MapElements.Add(mapIcon2);

            BasicGeoposition startLocation = new BasicGeoposition() { Latitude = zomx, Longitude = zomy };

            BasicGeoposition endLocation = new BasicGeoposition() { Latitude = 43.847039, Longitude = 18.373636 };

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
                viewOfRoute.RouteColor = Colors.DarkRed;
                viewOfRoute.OutlineColor = Colors.DarkRed;

                // Add the new MapRouteView to the Routes collection
                // of the MapControl.
                MapControl3.Routes.Add(viewOfRoute);

                // Fit the MapControl to the route.
                await MapControl3.TrySetViewBoundsAsync(
                      routeResult.Route.BoundingBox,
                      null,
                      Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);
            }
        }


    }
}
