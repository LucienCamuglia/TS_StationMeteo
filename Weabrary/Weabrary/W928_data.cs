using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weabrary
{
    public class W928_data
    {
        #region fields
        // this is the values from station
        private uint timestamp; // timestamp for this dataset
        private double[] t = new double[6];  // temperature of the sensor in °C
        private short[] h = new short[6]; // humidity of the sensors in % rel
        private double uv; // UV index
        private double press; // air pressure in mBar
        private char forecast; // weather forecast from station
        //                                      0 => heavy snow
        //                                      1 => snow
        //                                      2 => heavy rain
        //                                      3 => rain
        //                                      4 => cloudy
        //                                      5 => some clouds
        //                                      6 => sunny
        private char storm;                    // storm warning if value = 1 else 0
        private double wChill;                           // windchill in °C
        private double wGust;                            // wind gusts in m/s
        private double wSpeed;                           // wind speed in m/s
        private char wDir;                     // wind direction in x*22.5°; 0 => north
        private uint rainCount;                 // Raincounter of station as number up to 65535 * 0.7 mm/m²
        // status of sensors, names are _<sensor>; if status is <> 0, the value should be 0
        // 0  => value of sensor should be ok
        // -1 => data is invalid
        // -2 => sensor is out of range
        // -3 => missing link
        // -4 => any other error
        private sbyte[] _t = new sbyte[6];
        private sbyte[] _h = new sbyte[6];
        private sbyte _press, _uv, _wDir, _wSpeed, _wGust, _wChill, _RainCount, _storm, _forecast;
        private uint __src;                     // source adress of dataset (needed for dump)
        #endregion


        #region properties
        public uint Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }
        public double[] T
        {
            get { return t; }
            set { t = value; }
        }
        public short[] H
        {
            get { return h; }
            set { h = value; }
        }
        public double Uv
        {
            get { return uv; }
            set { uv = value; }
        }
        public double Press
        {
            get { return press; }
            set { press = value; }
        }
        public char Forecast
        {
            get { return forecast; }
            set { forecast = value; }
        }
        public char Storm
        {
            get { return storm; }
            set { storm = value; }
        }
        public double WChill
        {
            get { return wChill; }
            set { wChill = value; }
        }
        public double WGust
        {
            get { return wGust; }
            set { wGust = value; }
        }
        public double WSpeed
        {
            get { return wSpeed; }
            set { wSpeed = value; }
        }
        public char WDir
        {
            get { return wDir; }
            set { wDir = value; }
        }
        public uint RainCount
        {
            get { return rainCount; }
            set { rainCount = value; }
        }
        
        public uint _src
        {
            get { return __src; }
            set { __src = value; }
        }
        public sbyte ForecastChar
        {
            get { return _forecast; }
            set { _forecast = value; }
        }

        public sbyte StormChar
        {
            get { return _storm; }
            set { _storm = value; }
        }

        public sbyte RainCountChar
        {
            get { return _RainCount; }
            set { _RainCount = value; }
        }

        public sbyte WChillChar
        {
            get { return _wChill; }
            set { _wChill = value; }
        }

        public sbyte WGustChar
        {
            get { return _wGust; }
            set { _wGust = value; }
        }

        public sbyte WSpeedChar
        {
            get { return _wSpeed; }
            set { _wSpeed = value; }
        }

        public sbyte WDirChar
        {
            get { return _wDir; }
            set { _wDir = value; }
        }

        public sbyte UvChar
        {
            get { return _uv; }
            set { _uv = value; }
        }

        public sbyte PressChar
        {
            get { return _press; }
            set { _press = value; }
        }
        public sbyte[] Tchar
        {
            get { return _t; }
            set { _t = value; }
        }
        public sbyte[] Hchar
        {
            get { return _h; }
            set { _h = value; }
        }
        #endregion
        
    }
}
