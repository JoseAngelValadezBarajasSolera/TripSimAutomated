using System;
using System.Windows.Media;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Shapes;

//*****************************************************************************************
// Filename:    Mapping.cs
// Author:      SC Yuen
// Date:        2015-09-29
// System:      Tools
//-----------------------------------------------------------------------------------------
// Description: Creates a map when user opens a file and connected to a port. It goes from
//              Start() which then initializes the map design and then it gets the current
//              coordinates from UpdateCoordinates() and passes those coordinates to
//              Mapping(). Mapping() then sets the view and presents the map. On the
//              starting position it will place a pin there from Pin(). After that it will
//              place blue dots on the map to indicate where the current position is at
//              from PlaceDot(). It will place blue dot according to the NMEATimer, which
//              is set to be 1000ms. However, it will stop printing when the NMEATimer gets
//              stopped.
//              If the user opens another file, it will stop processing the current map and
//              opens up a new map.
//*****************************************************************************************
// XRS Corporation Confidential & Proprietary
// Copyright (c) 2015-2017, XRS Corporation. All rights reserved.
//*****************************************************************************************

namespace Trip_Simulator
{
    public partial class Map : Form
    {
        public const double ZOOM_250_FEET = 18.0;
        public const double ZOOM_2500_FEET = 14.8;
        public const double ZOOM_1_MILE = 13.6;
        public const double ZOOM_2_MILES = 12.0;
        public const double ZOOM_5_MILES = 11.4;
        public const double ZOOM_2000_MILES = 3.0;

        Location loc;
        Pushpin pin = new Pushpin();
        MainForm mMainForm;
        bool cameraLock = true;
        bool defaultMap = false;

        // Constructor
        public Map(MainForm main)
        {
            mMainForm = main;
            InitializeComponent(main);
            loc = main.GetUpdateCoordinates();
            mapUserControl1.Map.Loaded += new RoutedEventHandler(MapLoaded);    // workaround for WPF Map.SetView exception
            Shown += new System.EventHandler(FormShown);                   // workaround for scroll wheel not zooming when map first shown
            Mapping();
        }

        // Overloaded constructor
        public Map(MainForm main, bool DM)
        {
            defaultMap = DM;
            mMainForm = main;
            InitializeComponent(main);
            loc = main.GetUpdateCoordinates();
            mapUserControl1.Map.Loaded += new RoutedEventHandler(MapLoaded);    // workaround for WPF Map.SetView exception
            Shown += new System.EventHandler(FormShown);                   // workaround for scroll wheel not zooming when map first shown
            Mapping();
        }

        //*****************************************************************************************
        // Name: CamLock()
        // Description: This function follows the current location in the Bing map.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-05   SC Yuen     Initial version
        //*****************************************************************************************
        public void CamLock(bool Lock)
        {
            cameraLock = Lock;
            mapUserControl1.Map.ZoomLevel = ZOOM_250_FEET;
        }

        //*****************************************************************************************
        // Name: MapLoaded()
        // Description: This function zooms in to the current location on the Bing map.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-05   SC Yuen     Initial version
        //-----------------------------------------------------------------------------------------
        // 2017-06-21   SC Yuen     If defaultMap is true, then don't zoom in too close
        //*****************************************************************************************
        private void MapLoaded(object sender, RoutedEventArgs e)
        {
            if (defaultMap)
            {
                mapUserControl1.Map.SetView(loc, ZOOM_2000_MILES);
                defaultMap = false;
            }
            else
            {
                mapUserControl1.Map.SetView(loc, mMainForm.ZoomLevel);
            }
        }

        //*****************************************************************************************
        // Name: FormShown()
        // Description: This function focuses on the current location in the Bing map.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-05   SC Yuen     Initial version
        //*****************************************************************************************
        private void FormShown(object sender, EventArgs e)
        {
            mapUserControl1.Map.Focus();
        }

        //*****************************************************************************************
        // Name: PlaceDot(double, double, Color)
        // Description: Places "blue" dots as it records the position and the speed at that
        //              location of the dot in the simulated trip.
        //-----------------------------------------------------------------------------------------
        // Inputs: lat, lon, color
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void PlaceDot(double lat, double lon, Color color, decimal speed)
        {
            Location location = new Location(lat, lon);
            Ellipse dot = new Ellipse();
            dot.Fill = new SolidColorBrush(color);
            double radius = 4.0;
            dot.Width = radius * 2;
            dot.Height = radius * 2;
            System.Windows.Controls.ToolTip theTip = new System.Windows.Controls.ToolTip();
            theTip.Content = "Speed: " + speed.ToString(MainFunctions.SPEED_FORMAT) + " mph" + "\n" + lat.ToString("0.000000") + "," + lon.ToString("0.000000");
            theTip.FontFamily = new FontFamily("Courier New");
            dot.ToolTip = theTip;
            MapLayer.SetPosition(dot, location);
            MapLayer.SetPositionOrigin(dot, PositionOrigin.Center);
            mapUserControl1.Map.Children.Add(dot);
            if(cameraLock)
            {
                mapUserControl1.Map.Center = location;
            }
        }

        //*****************************************************************************************
        // Name: Mapping()
        // Description: Shows the map.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        // 2017-06-21 SC Yuen Got rid of the unnecessary setting of the zoom level
        //*****************************************************************************************
        private void Mapping()
        {
            Show();
        }

        //*****************************************************************************************
        // Name: Pin(double, double)
        // Description: Places a "pin" at the initial starting point, zoom and centers into it too.
        //-----------------------------------------------------------------------------------------
        // Inputs: lat, lon
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void Pin(double lat, double lon)
        {
            // Setup starting pin on the bing map
            pin.Background = new SolidColorBrush(Colors.Green);
            pin.Location = new Location(lat, lon);
            mapUserControl1.Map.Children.Add(pin);
            mapUserControl1.Map.Center = pin.Location;
        }

        //*****************************************************************************************
        // Name: FinishPin(double, double)
        // Description: Places a black and white icon at the destination.
        //-----------------------------------------------------------------------------------------
        // Inputs: lat, lon
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-11-20 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void FinishPin(double lat, double lon)
        {
            MapPolygon finishObj1 = new MapPolygon();
            finishObj1.Stroke = new SolidColorBrush(Colors.Black);
            finishObj1.Fill = new SolidColorBrush(Colors.Black);
            finishObj1.StrokeThickness = 5;
            finishObj1.Locations = new LocationCollection()
            {
                new Microsoft.Maps.MapControl.WPF.Location(lat, lon-.0001),
                new Microsoft.Maps.MapControl.WPF.Location(lat+.0001, lon),
                new Microsoft.Maps.MapControl.WPF.Location(lat+.00005, lon)
            };
            MapPolygon finishObj2 = new MapPolygon();
            finishObj2.Stroke = new SolidColorBrush(Colors.White);
            finishObj2.Fill = new SolidColorBrush(Colors.White);
            finishObj2.StrokeThickness = 5;
            finishObj2.Locations = new LocationCollection()
            {
                new Microsoft.Maps.MapControl.WPF.Location(lat, lon+.0001),
                new Microsoft.Maps.MapControl.WPF.Location(lat+.0001, lon),
                new Microsoft.Maps.MapControl.WPF.Location(lat+.00005, lon)
            };
            mapUserControl1.Map.Children.Add(finishObj1);
            mapUserControl1.Map.Children.Add(finishObj2);
        }

        //*****************************************************************************************
        // Name: HighlightRoute()
        // Description: The function that highlights the entire trip of the file that it's reading.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-11-05 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void HighlightRoute(MapPolyline polyline)
        {
            mapUserControl1.Map.Children.Add(polyline);
        }

        /// <summary>
        /// This gets invoked when the form is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Map_FormClosed(object sender, FormClosedEventArgs e)
        {
            // enable the Map button since this map has been closed
            mMainForm.GetFunctions().SetMapBtnEnabled();
        }
    }
}
