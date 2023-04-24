using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Maps.MapControl.WPF;
using System.Windows;

namespace Trip_Simulator
{
    /// <summary>
    /// Interaction logic for MapUserControl.xaml
    /// </summary>
    public partial class MapUserControl : UserControl
    {
        Point mousePosition;
        Location pinLocation;
        Pushpin pin;
        MainForm TheMain;
        MainFunctions TheFunctions;

        // Constructor
        public MapUserControl(MainForm main)
        {
            TheMain = main;
            TheFunctions = main.GetFunctions();
            InitializeComponent();
        }

        //*****************************************************************************************
        // Name: ButtonZoomIn_Click(object, RoutedEventArgs)
        // Description: Zooms in when this button gets clicked.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-05-21      SC Yuen     Initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void ButtonZoomIn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Map.ZoomLevel < 20)
            {
                Map.ZoomLevel += 0.2;
            }
        }

        ///*****************************************************************************************
        // Name: ButtonZoomOut_Click(object, RoutedEventArgs)
        // Description: Zooms out when this button gets clicked.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-05-21      SC Yuen     Initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void ButtonZoomOut_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Map.ZoomLevel > 1)
            {
                Map.ZoomLevel -= 0.2;
            }
        }

        //*****************************************************************************************
        // Name: MapWithPushpins_MouseDoubleClick(object, MouseButtonEventArgs)
        // Description: Deletes the pin if it exists, and create a new pin when a double click
        //              occurs on the mouse.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-06-21      SC Yuen     Initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (pin != null)
            {
                Map.Children.Remove(pin);
            }

            // If the map button is enabled, that means we can add pins to the map
            if (TheFunctions.GetMapBtnEnable())
            {
                // Disables the default mouse double-click action.
                e.Handled = true;

                // Determine the location to place the pushpin at on the map.

                //Get the mouse click coordinates
                mousePosition = e.GetPosition(this);
                //Convert the mouse coordinates to a location on the map
                pinLocation = Map.ViewportPointToLocation(mousePosition);
                TheFunctions.SetPinCoordinates(pinLocation);

                // The pushpin to add to the map.
                pin = new Pushpin();
                pin.Location = pinLocation;

                // Adds the pushpin to the map.
                Map.Children.Add(pin);
            }
        }

        //*****************************************************************************************
        // Name: MapWithPushpins_MouseRightClick(object, MouseButtonEventArgs)
        // Description: Deletes the pin if it exists when a right-clicked occurred on the mouse.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-06-22      SC Yuen     Initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void MapWithPushpins_MouseRightClick(object sender, MouseButtonEventArgs e)
        {
            if (pin != null)
            {
                Map.Children.Remove(pin);
                pinLocation.Latitude = 0xFFFFFFFF;
                pinLocation.Longitude = 0xFFFFFFFF;
                TheFunctions.SetPinCoordinates(pinLocation);
            }
        }
    }
}
