using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using uint32_t = System.UInt32;
using uint16_t = System.UInt16;
using uint8_t = System.Byte;

//*****************************************************************************************
// Filename:    DATFileReader.cs
// Author:      SC Yuen
// Date:        2015-09-29
// System:      Tools
//-----------------------------------------------------------------------------------------
// Description: Reads the DAT file and counts how many lines are in the file.
//*****************************************************************************************
// XRS Corporation Confidential & Proprietary
// Copyright (c) 2015-2017, XRS Corporation. All rights reserved.
//*****************************************************************************************

namespace Trip_Simulator
{
    class DATFileReader : Form
    {
        int case1Counter = 0;               // STD1 messages gets called 5 times per seconds and we only need one message per second.
                                            // We only gets the messages when the case1Counter == 0.
        int lineCount = 0;                  // Counting the amount of NMEA sentences the DAT file contains. 
        int STD1Count = 0;                  // Counting the amount of STD1 messages the DAT file contains.
        int STD2Count = 0;                  // Counting the amount of STD2 messages the DAT file contains.
        sGPS GPS = new sGPS();
        SerialPort _serialPort;
        List<string> theList = new List<string> { };
        List<uint> STD1List = new List<uint> { };
        List<uint> STD2List = new List<uint> { };
        sECM1 ECM1 = new sECM1();
        sECM2 ECM2 = new sECM2();
        const UInt32 RelayRecorderGPSTypeV0 = 0x80000000;
        const UInt32 RelayRecorderGPSTypeV1 = 0x80000001;

        // Contains variables for the desired ECM1 vehicle data.
        private class sECM1
        {
            public uint32_t TotalVehicleDistance;
            public uint16_t VehicleSpeed;
            public uint16_t EngineSpeed;
            public uint16_t InstantaneousFuelRate;
            public uint8_t PercentEngineLoad;
        }

        // Contains variables for the desired ECM2 vehicle data.
        private class sECM2
        {
            public uint32_t TotalFuel;
            public uint32_t TotalPTOHours;
            public uint32_t TotalEngineHours;
            public uint16_t PTOStatus;
        }

        // Contains all the variables necessary to record the data from the file.
        public class sGPS
        {
            public UInt32 RelayRecorderGPSType;
            public UInt32 MessageLength;
            public float Latitude = 0xFFFFFFFF;
            public float Longitude = 0xFFFFFFFF;
            public UInt16 Speed = 0xFFFF;
            public byte hour = 0xFF;
            public byte minute = 0xFF;
            public byte second = 0xFF;
            public byte year = 0xFF;
            public byte month = 0xFF;
            public byte day = 0xFF;
            public float Hacc = 0xFFFFFFFF;
            public byte fix = 0XFF;
        }

        //*****************************************************************************************
        // Name: DATReader(SerialPort, string)
        // Description: Reads the DAT file and counts how many lines are in the file. It will
        //              determine what GPS type it is and select the appropriate measures to get
        //              the necessary data in order to build the NMEA sentences. At the end, it
        //              will return the lineCount which contains the amount of lines in the file.
        //-----------------------------------------------------------------------------------------
        // Inputs: _serialPort, TheFileName
        // Outputs: none
        // Returns: int lineCount
        //*****************************************************************************************
        // 2015-09-29 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        // 2017-04-21 SC Yuen   Added the ability to read TEH from DAT file
        //*****************************************************************************************
        public int DATReader(SerialPort _serialPort, string TheFileName)
        {
            int nRecords = 0;
            this._serialPort = _serialPort;

            FileStream fs = null;
            BinaryReader br = null;

            try
            {
                fs = File.OpenRead(TheFileName);
                br = new BinaryReader(fs);

                long flength = fs.Length;
                int gpsseq = 0;

                while (fs.Position < (flength - 2 * sizeof(UInt32)))
                {
                    if ((nRecords++ % 1000) == 0) Application.DoEvents();

                    UInt32 signature = br.ReadUInt32();
                    UInt32 length = br.ReadUInt32();

                    switch (signature)
                    {
                        // For GPS type 0, read the DAT file this way.
                        case RelayRecorderGPSTypeV0:    // GPS v0
                            gpsseq++;
                            GPS.RelayRecorderGPSType = RelayRecorderGPSTypeV0;
                            GPS.Latitude = (float)br.ReadInt32() * 0.000001F;     // 0
                            GPS.Longitude = (float)br.ReadInt32() * 0.000001F;    // 4
                            br.ReadBytes(2);
                            GPS.Speed = br.ReadUInt16();        // 10
                            br.ReadBytes(2);
                            GPS.fix = br.ReadByte();            // 14
                            br.ReadBytes(2);
                            GPS.hour = br.ReadByte();           // 17
                            GPS.minute = br.ReadByte();         // 18
                            GPS.second = br.ReadByte();         // 19
                            GPS.year = br.ReadByte();           // 20
                            GPS.month = br.ReadByte();          // 21
                            GPS.day = br.ReadByte();            // 22
                                                                // some GPS v0 have Hacc data, so check if next field is valid message id or Hacc
                            var signatureOrHacc = br.ReadBytes(4);

                            switch (BitConverter.ToUInt32(signatureOrHacc, 0))
                            {
                                case 1:                         // STD1
                                case 2:                         // STD2
                                case 4:                         // DSRC
                                case 6:                         // DTC1
                                case 7:                         // BOOT
                                case 8:                         // PWR
                                case 9:                         // VEH_ID
                                case 10:                        // DTC2
                                case RelayRecorderGPSTypeV0:    // GPS v1
                                case RelayRecorderGPSTypeV1:    // GPS v2
                                                                // valid signature, no Hacc data present, revert back to previous position
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    GPS.MessageLength = 23; // hard-coded length, GPS v0 length is broken
                                    break;

                                default:
                                    GPS.MessageLength = 27; // hard-coded length, GPS v0 length is broken
                                    break;
                            }
                            char NorS;
                            char EorW;
                            if (GPS.Latitude < 0)
                            {
                                NorS = 'S';
                                GPS.Latitude *= -1;
                            }
                            else NorS = 'N';

                            if (GPS.Longitude < 0)
                            {
                                EorW = 'W';
                                GPS.Longitude *= -1;
                            }
                            else EorW = 'E';

                            // Piecing the sentences together.
                            string theRMC = "$GPRMC," + GPS.hour + GPS.minute + GPS.second + ".00," + "A,"
                                + GPS.Latitude + "," + NorS.ToString() + "," + GPS.Longitude + "," + EorW.ToString()
                                + "," + GPS.Speed + ",," + GPS.day + GPS.month + GPS.year + ",,,S,";

                            theList.Add(theRMC);
                            lineCount++;

                            string theGGA = "$GPGGA," + GPS.hour + GPS.minute + GPS.second + ".00," + GPS.Latitude + ","
                                + NorS.ToString() + "," + GPS.Longitude + "," + EorW.ToString() + "," + GPS.fix.ToString() + ",,,,,,,,";

                            theList.Add(theGGA);
                            lineCount++;

                            string thePUB = "$PUBX,00," + GPS.hour + GPS.minute + GPS.second + ".00," + GPS.Latitude + ","
                                + NorS.ToString() + "," + GPS.Longitude + "," + EorW.ToString() + ",,,,,,,,,,";

                            theList.Add(thePUB);
                            lineCount++;

                            System.Threading.Thread.Sleep(1000);        // Wait for 1 second after a set of NMEA sentences
                            continue;                                   // has been printed out.

                        // For GPS type 1, read DAT file this way.
                        case RelayRecorderGPSTypeV1:    // GPS v1                                            
                            gpsseq++;
                            GPS.RelayRecorderGPSType = RelayRecorderGPSTypeV1;
                            GPS.MessageLength = length;
                            GPS.Latitude = (float)br.ReadInt32() * 0.000001F;     // 0
                            GPS.Longitude = (float)br.ReadInt32() * 0.000001F;    // 4
                            br.ReadBytes(2);
                            GPS.Speed = br.ReadUInt16();        // 10
                            br.ReadBytes(2);
                            GPS.fix = br.ReadByte();            // 14
                            br.ReadBytes(2);
                            GPS.hour = br.ReadByte();           // 17
                            GPS.minute = br.ReadByte();         // 18
                            GPS.second = br.ReadByte();         // 19
                            GPS.year = br.ReadByte();           // 20
                            GPS.month = br.ReadByte();          // 21
                            GPS.day = br.ReadByte();            // 22
                            GPS.Hacc = br.ReadSingle();         // 23                                            

                            if (GPS.Latitude < 0)
                            {
                                NorS = 'S';
                                GPS.Latitude *= -1;
                            }
                            else NorS = 'N';

                            if (GPS.Longitude < 0)
                            {
                                EorW = 'W';
                                GPS.Longitude *= -1;
                            }
                            else EorW = 'E';

                            // Piecing the sentences together.
                            theRMC = "$GPRMC," + GPS.hour.ToString("00") + GPS.minute.ToString("00") + GPS.second.ToString("00") + ".00," + "A,"
                                + GPS.Latitude.ToString("0000.00000") + "," + NorS.ToString() + "," + GPS.Longitude.ToString("00000.00000") + "," + EorW.ToString()
                                + "," + GPS.Speed + ",," + GPS.day.ToString("00") + GPS.month.ToString("00") + GPS.year.ToString("##") + ",,,S,";

                            theList.Add(theRMC);
                            lineCount++;

                            theGGA = "$GPGGA," + GPS.hour.ToString("00") + GPS.minute.ToString("00") + GPS.second.ToString("00") + ".00," + GPS.Latitude.ToString("0000.00000") + ","
                                + NorS.ToString() + "," + GPS.Longitude.ToString("00000.00000") + "," + EorW.ToString() + "," + GPS.fix.ToString() + ",,,,,,,,";

                            theList.Add(theGGA);
                            lineCount++;

                            thePUB = "$PUBX,00," + GPS.hour.ToString("00") + GPS.minute.ToString("00") + GPS.second.ToString("00") + ".00," + GPS.Latitude.ToString("0000.00000") + ","
                                + NorS.ToString() + "," + GPS.Longitude.ToString("00000.00000") + "," + EorW.ToString() + ",,," + GPS.Hacc + ",,,,,,";

                            theList.Add(thePUB);
                            lineCount++;
                            case1Counter = 0;

                            continue;

                        // Reads the odometer, vehicle speed, engine speed, fuel rate, and the engine load.
                        case 0x00000001:
                            br.ReadBytes(8);

                            for (int i = 0; i < 5; i++)
                            {
                                br.ReadBytes(4);
                                ECM1.TotalVehicleDistance = br.ReadUInt32();
                                if (case1Counter == 0)
                                {
                                    STD1List.Add(ECM1.TotalVehicleDistance); // STD1 .125 km
                                    STD1Count++;
                                }

                                ECM1.VehicleSpeed = br.ReadUInt16();
                                if (case1Counter == 0)
                                {
                                    STD1List.Add(ECM1.VehicleSpeed); // STD1 1/256 km/hr
                                    STD1Count++;
                                }

                                ECM1.EngineSpeed = br.ReadUInt16();
                                if (case1Counter == 0)
                                {
                                    STD1List.Add(ECM1.EngineSpeed); // STD1 .125 rpm
                                    STD1Count++;
                                }

                                br.ReadBytes(4);
                                ECM1.InstantaneousFuelRate = br.ReadUInt16(); // STD1 .05 L/hr
                                if (case1Counter == 0)
                                {
                                    STD1List.Add(ECM1.InstantaneousFuelRate);
                                    STD1Count++;
                                }

                                br.ReadBytes(3);
                                ECM1.PercentEngineLoad = br.ReadByte(); // STD1
                                if (case1Counter == 0)
                                {
                                    STD1List.Add(ECM1.PercentEngineLoad);
                                    STD1Count++;
                                }

                                br.ReadBytes(2);
                                case1Counter++;
                            }
                            continue;

                        // Reads the total fuel, total PTO hours, and the PTO status.
                        case 0x00000002:
                            br.ReadBytes(24);
                            ECM2.TotalFuel = br.ReadUInt32(); // STD2 .05 L
                            STD2List.Add(ECM2.TotalFuel);
                            STD2Count++;

                            ECM2.TotalPTOHours = br.ReadUInt32(); // STD2 .05 hr
                            STD2List.Add(ECM2.TotalPTOHours);
                            STD2Count++;

                            ECM2.TotalEngineHours = br.ReadUInt32(); // STD .05 hr
                            STD2List.Add(ECM2.TotalEngineHours);
                            STD2Count++;

                            br.ReadBytes(18);
                            ECM2.PTOStatus = br.ReadUInt16(); // STD2
                            STD2List.Add(ECM2.PTOStatus);
                            STD2Count++;

                            br.ReadBytes(10);
                            case1Counter = 0;
                            continue;

                        default:                        
                            fs.Seek(length, SeekOrigin.Current);
                            continue;
                    }   // End of the switch case.
                }   // End of the while loop.
            }   // End of try block.
            finally
            {
                if (br != null) br.Close();
                if (fs != null) fs.Close();
            }
            return lineCount;
        }

        //*****************************************************************************************
        // Name: TheListOfSentences()
        // Description: Gets the list of NMEA sentences that was gathered from the DAT file.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: List<string> theList
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public List<string> TheListOfSentences()
        {
            return theList;
        }

        //*****************************************************************************************
        // Name: GetSTD1List()
        // Description: Gets the list STD1 messages from the DAT file.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: List<uint> STD1List
        //*****************************************************************************************
        // 2016-01-26 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public List<uint> GetSTD1List()
        {
            return STD1List;
        }

        //*****************************************************************************************
        // Name: GetSTD2List()
        // Description: Gets the list STD2 messages from the DAT file.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: List<uint> STD2List
        //*****************************************************************************************
        // 2016-01-26 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public List<uint> GetSTD2List()
        {
            return STD2List;
        }

        //*****************************************************************************************
        // Name: GetSTD1Count()
        // Description: Gets the STD1Count to let MainFunctions know how big the array should be.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: int STD1Count
        //*****************************************************************************************
        // 2016-01-26 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public int GetSTD1Count()
        {
            return STD1Count;
        }

        //*****************************************************************************************
        // Name: GetSTD2Count()
        // Description: Gets the STD2Count to let MainFunctions know how big the array should be.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: int STD2Count
        //*****************************************************************************************
        // 2016-01-26 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public int GetSTD2Count()
        {
            return STD2Count;
        }
    }
}
