using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.IO;
using System.Linq;

namespace Trip_Simulator
{
    public class Route
    {
        public CoordinateCollection Coordinates;
        public CoordinateCollection lastCoordinates;
        int CoordinateIndex;
        decimal Displacement;

        // Constructor
        public Route(string fileName)
        {
            KmlFile file = KmlFile.Load(File.OpenRead(fileName));
            Kml kml = file.Root as Kml;
            Coordinates = kml.Flatten().OfType<LineString>().First().Coordinates;
            lastCoordinates = kml.Flatten().OfType<LineString>().Last().Coordinates;
            CoordinateIndex = 0;
            Displacement = 0;
        }


        //*****************************************************************************************
        // Name: GetListSize()
        // Description: Returns the length of the list of coordinates in the KML file.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: length
        //*****************************************************************************************
        // 2015-11-20 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public long GetListSize()
        {
            long length = lastCoordinates.LongCount() - 1;
            return length;
        }

        //*****************************************************************************************
        // Name: GetLastCoordinates()
        // Description: Returns the last set of coordinates.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: lastCoordinates
        //*****************************************************************************************
        // 2015-11-23 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public CoordinateCollection GetLastCoordinates()
        {
            return lastCoordinates;
        }

        //*****************************************************************************************
        // Name: printRoute()
        // Description: Prints the coordinates into console.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-24 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void printRoute()
        {
            foreach (var coorindate in Coordinates)
            {
                Console.WriteLine(coorindate.Latitude + ", " + coorindate.Longitude);
            }
        }

        //*****************************************************************************************
        // Name: getCurrenctPosition(decimal)
        // Description: Gets the current position of the file and returns the coordinates.
        //-----------------------------------------------------------------------------------------
        // Inputs: distance
        // Outputs: none
        // Returns: Vector currentPosition
        //*****************************************************************************************
        // 2015-09-24 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public Vector getCurrentPosition(decimal distance)
        {
            Displacement += distance;

            if (CoordinateIndex >= Coordinates.Count-1)
            {
                return Coordinates.Last();  // all waypoints have been visited
            }

            decimal segment = Distance(Coordinates.ElementAt(CoordinateIndex), Coordinates.ElementAt(CoordinateIndex + 1));

            while (Displacement >= segment)
            {
                Displacement = Displacement - segment;

                if (++CoordinateIndex >= Coordinates.Count-1)
                {
                    return Coordinates.Last();  // all waypoints have been visited
                }

                segment = Distance(Coordinates.ElementAt(CoordinateIndex), Coordinates.ElementAt(CoordinateIndex + 1));
            }

            // interpolate position between waypoints
            Vector prevWaypoint = Coordinates.ElementAt(CoordinateIndex);
            Vector nextWaypoint = Coordinates.ElementAt(CoordinateIndex + 1);
            Vector currentPosition = new Vector();
            currentPosition.Latitude = prevWaypoint.Latitude + (nextWaypoint.Latitude - prevWaypoint.Latitude) * Convert.ToDouble(Displacement / segment);
            currentPosition.Longitude = prevWaypoint.Longitude + (nextWaypoint.Longitude - prevWaypoint.Longitude) * Convert.ToDouble(Displacement / segment);

            return currentPosition;
        }

        //*****************************************************************************************
        // Name: getCoordinateIndex()
        // Description: Gets the CoordinateIndex
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: int CoordinateIndex
        //*****************************************************************************************
        // 2017-05-11   SC Yuen     Initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public int getCoordinateIndex()
        {
            return CoordinateIndex;
        }

        /// <summary>
        /// This will reset the coordinate index and the current displacement.
        /// </summary>
        public void resetCoordinateIndex()
        {
            CoordinateIndex = 0;
            Displacement = 0;
        }

        double ToRadians(double degrees) { return degrees * Math.PI / 180; }

        double ToDegrees(double radians) { return radians * 180 / Math.PI; }

        /// <summary>
        /// Uses the Haversine formula to calculate the distance between two positions.
        /// </summary>
        /// <param name="Pos1">First Position</param>
        /// <param name="Pos2">Second Position</param>
        /// <returns>distance between positions, in miles</returns>
        private decimal Distance(Vector Pos1, Vector Pos2)
        {
            // Haversine Formula (from R.W. Sinnott, "Virtues of the Haversine", Sky and Telescope, vol. 68, no. 2, 1984, p. 159):
            // dlon = lon2 - lon1
            // dlat = lat2 - lat1
            // a = sin^2(dlat/2) + cos(lat1) * cos(lat2) * sin^2(dlon/2)
            // c = 2 * arcsin(min(1,sqrt(a)))
            // d = R * c

            double R = 3959; // mean radius of Earth in miles
            double dlon = ToRadians((Pos1.Longitude - Pos2.Longitude));
            double dlat = ToRadians((Pos1.Latitude - Pos2.Latitude));
            double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                Math.Cos(ToRadians(Pos1.Latitude)) * 
                Math.Cos(ToRadians(Pos2.Latitude)) *
                Math.Sin(dlon / 2) * Math.Sin(dlon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
            return Convert.ToDecimal(d);
        }
    }
}
