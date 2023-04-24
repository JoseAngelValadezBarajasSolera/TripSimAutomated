using System;
using System.Text;
using System.Windows.Forms;

namespace Trip_Simulator.NMEA
{
    class PUBX00
    {
        public static string Name = "$PUBX";
        public char NorthSouth;
        public char EastWest;
        public string checksum;
        public string[] fields;

        //*****************************************************************************************
        // Name: PUBX00(string)
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
        public PUBX00(string line, bool HACCEnabled, decimal HACCValue, DateTimePicker SetTime)
        {
            checksum = line.Split('*')[1];
            fields = line.Split('*')[0].Split(',');
            if (fields[0] != "$PUBX") return;
            if (fields[1] != "00") return;              // We only parse the $PUBX,00 message.

            StringBuilder sb = new StringBuilder();

            // Checking to see if we're using default current date/time.
            if (SetTime == null)
            {
                if (fields[2].Length > 0)                   // UTC time of fix
                {
                    sb.Append(fields[2].Substring(0, 2));
                    sb.Append(fields[2].Substring(2, 2));
                    sb.Append(fields[2].Substring(4, 2));
                    sb.Append(".");
                    sb.Append(fields[2].Substring(7, 2));

                    fields[2] = sb.ToString();     // Fix time
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
                fields[2] = sb.ToString();
                sb.Clear();
            }

            if (fields[9] == "") fields[9] = "2.1";

            if (HACCEnabled) fields[9] = HACCValue.ToString();
        }

        //*****************************************************************************************
        // Name: PUBX00(decimal, decimal, DateTime, bool)
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
        public PUBX00(decimal latitude, decimal longitude, DateTime dateTime, bool leap, DateTimePicker SetTime)
        {
            // used for converting lat and long
            decimal integral;
            decimal fraction;

            // sample: $PUBX,00,202656.00,4451.91921,N,09326.66548,W,238.968,D3,3.0,4.2,0.000,193.65,0.000,,1.11,1.70,1.13,8,0,0*7A
            StringBuilder sb = new StringBuilder();

            fields = new string[16];

            fields[0] = Name;              // u-blox Proprietary Message
            fields[1] = "00";              // u-blox Proprietary Message Number (00)

            // Checking to see if the leap timer has been activated.
            if(leap == true)
            {
                sb.Append(23);
                sb.Append(59);
                sb.Append(60);
                sb.Append(".");
                sb.Append(0);
                fields[2] = sb.ToString();     // UTC time
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
                fields[2] = sb.ToString();     // UTC time
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
                fields[2] = sb.ToString();
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
            fields[3] = sb.ToString();
            sb.Clear();

            fields[4] = NorthSouth.ToString();

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
            fields[5] = sb.ToString();
            sb.Clear();

            fields[6] = EastWest.ToString();
            fields[9] = "2.1";
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
