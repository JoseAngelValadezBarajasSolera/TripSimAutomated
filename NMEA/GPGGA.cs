using System;
using System.Text;
using System.Windows.Forms;

namespace Trip_Simulator.NMEA
{
    // GPGGA: GPS Fix Data

    class GPGGA
    {
        public static string Name = "$GPGGA";
        public char NorthSouth;
        public char EastWest;
        public string[] fields;

        //*****************************************************************************************
        // Name: GPGGA(string)
        // Description: The constructor for when there is only a single string input. It splits the
        //              input and set the value and constraints to them. i.e. Setting the date and
        //              time formats, and setting constraints of how many decimals the coordinates
        //              should have, and etc.
        //-----------------------------------------------------------------------------------------
        // Inputs: line
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public GPGGA(string line, DateTimePicker SetTime)
        {
            string checksum;
            bool isValid;

            string[] message = line.Split('*');
            if (message.Length > 1)
            {
                checksum = message[1];
                fields = message[0].Split(',');

                byte calcChecksum = 0;
                foreach (byte c in line.Substring(1))   // skip over leading '$' character
                {
                    if (c == '*') break;
                    calcChecksum ^= c;
                }
                isValid = (checksum == calcChecksum.ToString("X2"));

                if (!isValid) return;
                if (fields[0] != Name) return;          // wrong message type

                StringBuilder sb = new StringBuilder();

                // Checking to see if we're using default current date/time.
                if (SetTime == null)
                {
                    if (fields[1].Length > 0)               // UTC time of fix
                    {
                        sb.Append(fields[1].Substring(0, 2));
                        sb.Append(fields[1].Substring(2, 2));
                        sb.Append(fields[1].Substring(4, 2));
                        sb.Append(".");
                        sb.Append(fields[1].Substring(7, 2));

                        fields[1] = sb.ToString();          // Fix time
                        sb.Clear();
                    }
                }
                // Using user selected date/time.
                else
                {
                    sb.Append(SetTime.Value.Hour.ToString("D2"));
                    sb.Append(SetTime.Value.Minute.ToString("D2"));
                    sb.Append(SetTime.Value.Second.ToString("D2"));
                    sb.Append(".");
                    sb.Append(SetTime.Value.Millisecond.ToString());
                    fields[1] = sb.ToString();
                    sb.Clear();
                }
            }
        }

        //*****************************************************************************************
        // Name: GPGGA(decimal, decimal, DateTime, bool)
        // Description: The constructor for when there are multiple inputs from the KML files the
        //              Trip Simulator reads. This constructor sets the format and constraints on
        //              the inputs it was given.
        //-----------------------------------------------------------------------------------------
        // Inputs: latitude, longitude, dateTime, leap
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public GPGGA(decimal latitude, decimal longitude, DateTime dateTime, bool leap, DateTimePicker SetTime)
        {
            // used for converting lat and long
            decimal integral;
            decimal fraction;

            // sample: $GPGGA,162610.00,4331.17178,N,07940.68693,W,1,07,1.32,155.5,M,-36.0,M,,*62
            StringBuilder sb = new StringBuilder();

            fields = new string[15];

            fields[0] = Name;              // Recommended Minimum sentence C

            // Checking to see if the leap timer has been activated.
            if(leap == true)
            {
                sb.Append(23);
                sb.Append(59);
                sb.Append(60);
                sb.Append(".");
                sb.Append(0);
                fields[1] = sb.ToString();     // Fix time
                sb.Clear();
            }
            // Checking to see if we're using default current date/time.
            else if (SetTime == null)
            {
                sb.Append(dateTime.Hour.ToString("D2"));
                sb.Append(dateTime.Minute.ToString("D2"));
                sb.Append(dateTime.Second.ToString("D2"));
                sb.Append(".");
                sb.Append(dateTime.Millisecond.ToString());
                fields[1] = sb.ToString();     // Fix time
                sb.Clear();
            }
            // Using user selected date/time.
            else
            {
                sb.Append(SetTime.Value.Hour.ToString("D2"));
                sb.Append(SetTime.Value.Minute.ToString("D2"));
                sb.Append(SetTime.Value.Second.ToString("D2"));
                sb.Append(".");
                sb.Append(SetTime.Value.Millisecond.ToString());
                fields[1] = sb.ToString();
                sb.Clear();
            }

            if (latitude < 0)
            {
                NorthSouth = 'S';
                latitude *= -1;
            }
            else NorthSouth = 'N';

            integral = decimal.Truncate(latitude);
            fraction = latitude - integral;
            sb.Append(integral.ToString("00"));
            sb.Append((fraction * 60).ToString("00.00000"));
            fields[2] = sb.ToString();
            sb.Clear();

            // North or South of Latitude
            fields[3] = NorthSouth.ToString();

            if (longitude < 0)
            {
                EastWest = 'W';
                longitude *= -1;
            }
            else EastWest = 'E';

            integral = decimal.Truncate(longitude);
            fraction = longitude - integral;
            sb.Append(integral.ToString("000"));
            sb.Append((fraction * 60).ToString("00.00000"));
            fields[4] = sb.ToString();
            sb.Clear();

            // East or West of longitude
            fields[5] = EastWest.ToString();

            /* Fix quality: 0 = invalid
                            1 = GPS fix (SPS)
                            2 = DGPS fix
                            3 = PPS fix
			                4 = Real Time Kinematic
			                5 = Float RTK
                            6 = estimated (dead reckoning) (2.3 feature)
			                7 = Manual input mode
			                8 = Simulation mode
             */
            // GPS quality indicator
            fields[6] = "8";                        

            // number of satellites being tracked
            fields[7] = "08";

            // Horizontal Dilution Of Position (HDOP)
            fields[8] = "0.9";
        }

        //*****************************************************************************************
        // Name: toString()
        // Description: Overrides the system's ToString() and makes it so that when it's called, it
        //              will separate each of the strings appended to 'sb' with a ",". This also sets
        //              the value of the checksum too.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Join(",", fields));
            // calculate checksum
            byte checksum = 0;
            // skip over leading '$' character
            foreach (byte c in sb.ToString().Substring(1))
            {
                checksum ^= c;
            }
            sb.Append('*');
            sb.Append(checksum.ToString("X2"));
            return sb.ToString();
        }
    }
}
