/*
 * Authors : Lucien Camuglia & Julien Fäh T.IS-E2
 * Date : 2016-2017 1st semester (august - november)
 * Version 1.0 LCJF Base version
 * descrption : Library for USB communication with ventus W928 weather station.
 * Use : UsbLibrary from Mr. Neuhaus 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UsbLibraryCfptAdd;
using System.Globalization;

namespace Weabrary
{
    public class WeabraryConnect
    {
        #region fields
        bool Connected = false;
        bool DataRecieved = true;
        UsbHidPort usbPort;
        int debug = 0;
        string buffer;
        int count = 0;
        #endregion

        #region constants
        private const string PID = "0x6801", VID = "0x1130";
        private const sbyte DATAS_ERROR = -5;
        #endregion

        #region events
        public event EventHandler weabrary_DeviceArrived;
        public event EventHandler weabrary_DeviceRemoved;
        #endregion

        public WeabraryConnect()
        {
            #region Usb port implemenation

            usbPort = new UsbHidPort();
            usbPort.OnSpecifiedDeviceRemoved += new EventHandler(usbPort_OnSpecifiedDeviceRemoved);
            usbPort.OnDataRecieved += new DataRecievedEventHandler(usbPort_OnDataRecieved);
            usbPort.OnDeviceArrived += new EventHandler(usbPort_OnDeviceArrived);
            usbPort.OnSpecifiedDeviceArrived += new EventHandler(usbPort_OnSpecifiedDeviceArrived);
            usbPort.OnDeviceRemoved += new EventHandler(usbPort_OnDeviceRemoved);
            usbPort.OnDataSend += new EventHandler(usbPort_OnDataSend);

            #endregion
            weabrary_DeviceArrived += OnDeviceArrived;
            weabrary_DeviceRemoved += OnDeviceRemoved;
        }

        #region public methods
        public bool Connection()
        {
            try
            {
                string vid_str = VID;
                string pid_str = PID;

                //Supprime le 0x...
                vid_str = vid_str.Substring(2);
                pid_str = pid_str.Substring(2);

                usbPort.VendorId = Int32.Parse(vid_str, System.Globalization.NumberStyles.HexNumber);
                usbPort.ProductId = Int32.Parse(pid_str, System.Globalization.NumberStyles.HexNumber);



                //Test si le device est déjà connceté au démarrage de l'application
                usbPort.CheckDevicePresent();
                Connected = true;
                GetNewValues();
            }
            catch (Exception err)
            {
                Connected = false;
                if (debug > 0)
                    Console.WriteLine("Connection error : " + err.ToString());
            }

            return Connected;
        }

        public void Refresh()
        {
            GetNewValues();
            Thread.Sleep(75); //necessering for recieving datas from W928
        }

        public double GetHumidityBySensor(int idSensor)
        {
            if (DataRecieved)
                if (CRC_Is_correct())
                {
                    W928_data datas = new W928_data();
                    datas = decoder(buffer, datas);
                    if (datas.Hchar[idSensor] == 0)
                    {
                        return datas.H[idSensor];
                    }
                    else
                    {
                        return datas.Hchar[idSensor];
                    }
                }
            return DATAS_ERROR;
        }

        public double GeTemperatureBySensor(int idSensor)
        {
            if (DataRecieved)
                if (CRC_Is_correct())
                {
                    W928_data datas = new W928_data();
                    datas = decoder(buffer, datas);
                    if (datas.Tchar[idSensor] == 0)
                    {
                        return datas.T[idSensor];
                    }
                    else
                    {
                        return datas.Tchar[idSensor];
                    }
                }
            return DATAS_ERROR;
        }

        public double GetUv()
        {
            if (DataRecieved)
                if (CRC_Is_correct())
                {
                    W928_data datas = new W928_data();
                    datas = decoder(buffer, datas);
                    if (datas.UvChar == 0)
                    {
                        return datas.Uv;
                    }
                    else
                    {
                        return datas.UvChar;
                    }

                }
            return DATAS_ERROR;
        }

        public double GetPressure()
        {
            if (DataRecieved)
                if (CRC_Is_correct())
                {
                    W928_data datas = new W928_data();
                    datas = decoder(buffer, datas);
                    if (datas.PressChar == 0)
                    {
                        return datas.Press;
                    }
                    else
                    {
                        return datas.PressChar;
                    }

                }
            return DATAS_ERROR;
        }

        public double GetForecast()
        {
            if (DataRecieved)
                if (CRC_Is_correct())
                {
                    W928_data datas = new W928_data();
                    datas = decoder(buffer, datas);
                    if (datas.ForecastChar == 0)
                    {
                        return datas.Forecast;
                    }
                    else
                    {
                        return datas.ForecastChar;
                    }

                }
            return DATAS_ERROR;
        }

        public double StormWarning()
        {
            if (DataRecieved)
                if (CRC_Is_correct())
                {
                    W928_data datas = new W928_data();
                    datas = decoder(buffer, datas);
                    if (datas.StormChar == 0)
                    {
                        return datas.Storm;
                    }
                    else
                    {
                        return datas.StormChar;
                    }

                }
            return DATAS_ERROR;
        }

        public double GetWindChill()
        {
            if (DataRecieved)
                if (CRC_Is_correct())
                {
                    W928_data datas = new W928_data();
                    datas = decoder(buffer, datas);
                    if (datas.WChillChar == 0)
                    {
                        return datas.WChill;
                    }
                    else
                    {
                        return datas.WChillChar;
                    }

                }
            return DATAS_ERROR;
        }

        public double GetWindGust()
        {
            if (DataRecieved)
                if (CRC_Is_correct())
                {
                    W928_data datas = new W928_data();
                    datas = decoder(buffer, datas);
                    if (datas.WGustChar == 0)
                    {
                        return datas.WGust;
                    }
                    else
                    {
                        return datas.WGustChar;
                    }

                }
            return DATAS_ERROR;
        }

        public double GetWindSpeed()
        {
            if (DataRecieved)
                if (CRC_Is_correct())
                {
                    W928_data datas = new W928_data();
                    datas = decoder(buffer, datas);
                    if (datas.WSpeedChar == 0)
                    {
                        return datas.WSpeed;
                    }
                    else
                    {
                        return datas.WGustChar;
                    }

                }
            return DATAS_ERROR;
        }

        public double GetWindDirection()
        {
            if (DataRecieved)
                if (CRC_Is_correct())
                {
                    W928_data datas = new W928_data();
                    datas = decoder(buffer, datas);
                    if (datas.WDirChar == 0)
                    {
                        return datas.WDir;
                    }
                    else
                    {
                        return datas.WDirChar;
                    }

                }
            return DATAS_ERROR;
        }

        public double GetRainCount()
        {
            if (DataRecieved)
                if (CRC_Is_correct())
                {
                    W928_data datas = new W928_data();
                    datas = decoder(buffer, datas);
                    if (datas.RainCountChar == 0)
                    {
                        return datas.RainCount;
                    }
                    else
                    {
                        return datas.RainCountChar;
                    }

                }
            return DATAS_ERROR;
        }

        public List<string> Help()
        {
            List<string> infos = new List<string>();
            infos.Add("Library devlopped by Lucien Camuglia & Julien Fah - T.IS-E2");
            infos.Add("CFPT 2016 1st semester work");
            infos.Add("");
            infos.Add("Error codes :");
            infos.Add(" status of sensors :");
            infos.Add("     -1 : Data is invalid");
            infos.Add("     -2 : sensor is out of range");
            infos.Add("     -3 : missing link");
            infos.Add("     -4 : any other error");
            infos.Add(" -5 : communication error between pc and W928");
            infos.Add("Forecast values signification :");
            infos.Add(" 0 : Heavy snow");
            infos.Add(" 1 : snow");
            infos.Add(" 2 : heavy rain");
            infos.Add(" 3 : rain");
            infos.Add(" 4 : cloudy");
            infos.Add(" 5 : some clouds");
            infos.Add(" 6 : sunny");
            infos.Add("Storm warning is value 1 else 0");
            infos.Add("Unit of measurment : ");
            infos.Add(" Temperature of sensors : °C");
            infos.Add(" Humidity of sensors    : % rel");
            infos.Add(" Air pressure           : mBar");
            infos.Add(" Windchill              : °C");
            infos.Add(" Wind gust              : m/s");
            infos.Add(" Wind speed             : m/s");
            infos.Add(" Rain count             : mm/m^2");
            return infos;
        }
        #endregion

        #region private methods

        private void OnDeviceArrived(object sender, EventArgs e) {    }
        private void OnDeviceRemoved(object sender, EventArgs e) {    }        
        private void usbPort_OnDeviceArrived(object sender, EventArgs args)
        {
            weabrary_DeviceArrived(this, EventArgs.Empty);
        }
        private void usbPort_OnSpecifiedDeviceArrived(object sender, EventArgs args)
        {
            weabrary_DeviceArrived(sender, EventArgs.Empty);
        }
        private void usbPort_OnDeviceRemoved(object sender, EventArgs args)
        {
            weabrary_DeviceRemoved(this, EventArgs.Empty);
        }
        private void usbPort_OnSpecifiedDeviceRemoved(object sender, EventArgs args)
        {
            Connected = false;
            weabrary_DeviceRemoved(this, EventArgs.Empty);
        }
        private void usbPort_OnDataSend(object sender, EventArgs args)
        {
            // MessageBox.Show("Envoyé");
        }

        private void usbPort_OnDataRecieved(object sender, DataRecievedEventArgs args)
        {

            byte[] bdata = new byte[usbPort.SpecifiedDevice.InputReportLength];
            int pos = 0;
            W928_data data = new W928_data();
            string strData = "";

            //Traitement de données reçues
            foreach (byte byteData in args.data)
            {
                bdata[pos++] = byteData;
                strData += byteData.ToString("X2");
            }
            strData = strData.Substring(2, strData.Length - 2); //supprime le 00 de début de buffer
            if (debug > 0)
                Console.WriteLine(strData);
            buffer += strData;//remplie le buffer avec la trame reçue

            char first = buffer[0];
            int bytes = charToInt(first);
            char[] test2 = strData.ToCharArray();
            char[] test = buffer.ToArray<char>();

            //Test de traitement des données selon le projet "weewx"
            //Certainement le traitement nécéssaire pour le bon fonctionnement de notre projet
            //Malheureusement incompris
            if ((count + bytes) < 35)
            {

                char[] tbl2 = new char[test2.Length - 1];
                for (int i = 1; i < test2.Length - 1; i++)
                    tbl2[i] = test2[i];

                char[] tbl1 = new char[test.Length - 1];
                for (int i = count; i < test2.Length - count - 1; i++)
                    tbl1[i] = test[i];

                Array.Copy(tbl2, tbl1, bytes);
                strData = new string(test);

            }
            count += bytes;


        }

        private void GetNewValues()
        {
            //trame de requête des nouvelles valeurs de la station
            buffer = "";
            byte[] tbl = new byte[9] { 0xFF, 0x05, 0x0AF, 0x00, 0x00, 0x00, 0x00, 0xAF, 0xFE };
            tbl[5] = 0x0000FB / 0x10000;
            tbl[4] = (byte)((0x0000FB - (tbl[5] * 0x10000)) / 0x100);
            tbl[3] = (byte)(0x0000FB - (tbl[5] * 0x10000) - (tbl[4] * 0x100));
            tbl[6] = (byte)(tbl[2] ^ tbl[3] ^ tbl[4] ^ tbl[5]);
            SpecifiedDevice spfd = usbPort.SpecifiedDevice;
            spfd.SendData(tbl);
        }


        private int bcd2int(char bcd)
        {
            return ((int)((bcd & 0xF0) >> 4) * 10 + (int)(bcd & 0x0F));
        }
        /// <summary>
        /// Convert a char to int
        /// </summary>
        /// <param name="chr">Char in [0..9] &&  [A..F]</param>
        /// <returns></returns>
        private int charToInt(char chr)
        {
            if ("ABCDEF".IndexOf(chr) != -1)
            {
                return int.Parse(chr.ToString(), NumberStyles.HexNumber);
            }
            else
            {
                return (int)(Char.GetNumericValue(chr));
            }
        }
        private W928_data decoder(string buf, W928_data data)
        {
            //decode temperature and humidity from all sensors
            int i;
            for (i = 0; i <= 5; i++)
            {
                int offset = i * 3;
                data.Tchar[i] = 0;
                if (debug > 0)
                    Console.WriteLine("[DEBUG] TMP {0} BUF[{1}]={2} BUF[%02d]=%02x BUF[%02d]=%02x\n", i, 0 + offset, buf[0 + offset], 1 + offset, buf[1 + offset], 2 + offset, buf[2 + offset]);
                if (bcd2int((char)(buf[0 + offset] & 0x0F)) > 9)
                {
                    if (debug > 0)
                        Console.WriteLine("[DEBUG] TMP buffer 0 & 0x0F > 9\n");
                    if (((buf[0 + offset] & 0x0F) == 0x0C) || ((buf[0 + offset] & 0x0F) == 0x0B))
                    {
                        if (debug > 0)
                            Console.WriteLine("[DEBUG] TMP buffer 0 & 0x0F = 0x0C or 0x0B\n");
                        data.Tchar[i] = -2;
                    }
                    else
                    {
                        data.Tchar[i] = -1;
                        if (debug > 0)
                            Console.WriteLine("[DEBUG] TMP other error in buffer 0\n");
                    }
                }
                if (((buf[1 + offset] & 0x40) != 0x40) && i > 0)
                {
                    if (debug > 0)
                        Console.WriteLine("[DEBUG] TMP buffer 1 bit 6 set\n");
                    data.Tchar[i] = -2;
                }
                if (data.Tchar[i] == 0)
                {
                    data.T[i] = (bcd2int(buf[0 + offset]) / 10.0) + (bcd2int((char)(buf[1 + offset] & 0x0F)) * 10.0);
                    if (debug > 0)
                        Console.WriteLine("[DEBUG] TMP %d before is %0.2f\n", i, data.T[i]);
                    if ((buf[1 + offset] & 0x20) == 0x20)
                        data.T[i] += 0.05;
                    if ((buf[1 + offset] & 0x80) != 0x80)
                        data.T[i] *= -1;
                    if (debug > 0)
                        Console.WriteLine("[DEBUG] TMP %d after is %0.2f\n", i, data.T[i]);
                }
                else
                    data.T[i] = 0;

                if (data.Tchar[i] <= 2)
                {
                    data.Hchar[i] = -2;
                    data.H[i] = 0;
                }
                else if (bcd2int((char)(buf[2 + offset] & 0x0F)) > 9)
                {
                    data.Hchar[i] = -3;
                    data.H[i] = 0;
                }
                else
                {
                    data.H[i] = (short)bcd2int(buf[2 + offset]);
                    data.Hchar[i] = 0;
                }
            }

            //decode value from UV sensor
            if (debug > 0)
                Console.WriteLine("[DEBUG] UVX BUF[18]=%02x BUF[19]=%02x\n", buf[18], buf[19]);
            if ((buf[18] == 0xAA) && (buf[19] == 0x0A))
            {
                data.UvChar = -3;
                data.Uv = 0;
            }
            else if ((bcd2int(buf[18]) > 99) || (bcd2int(buf[19]) > 99))
            {
                data.UvChar = -1;
                data.Uv = 0;
            }

            else
            {
                data.Uv = bcd2int((char)(buf[18] & 0x0F)) / 10.0 + bcd2int((char)(buf[18] & 0xF0)) + bcd2int((char)(buf[19] & 0x0F)) * 10.0;
                data.UvChar = 0;
            }

            //decode Pressure
            if (debug > 0)
                Console.WriteLine("[DEBUG] PRS BUF[20]=%02x BUF[21]=%02x\n", buf[20], buf[21]);
            if ((buf[21] & 0xF0) == 0xF0)
            {
                data.Press = 0;
                data.PressChar = -1;
            }
            else
            {
                data.Press = (int)(buf[21] * 0x100 + buf[20]) * 0.0625;
                data.PressChar = 0;
            }

            //decode weather status and Storm warning
            if (debug > 0)
                Console.WriteLine("[DEBUG] STT BUF[22]=%02x\n", buf[22]);
            if ((buf[22] & 0x0F) == 0x0F)
            {
                data.StormChar = -1;
                data.ForecastChar = -1;
                data.Storm = '0';
                data.Forecast = '0';
            }
            else
            {
                data.StormChar = 0;
                data.ForecastChar = 0;
                if ((buf[22] & 0x08) == 0x08)
                    data.Storm = '1';
                else
                    data.Storm = '0';

                data.Forecast = (char)(int)(buf[22] & 0x07);
            }

            //decode windchill
            if (debug > 0)
                Console.WriteLine("[DEBUG] WCL BUF[23]=%02x BUF[24]=%02x\n", buf[23], buf[24]);
            if ((bcd2int((char)(buf[23] & 0xF0)) > 90) || (bcd2int((char)(buf[23] & 0x0F)) > 9))
            {
                if ((buf[23] == 0xAA) && (buf[24] == 0x8A))
                    data.WChillChar = -1;
                else if ((buf[23] == 0xBB) && (buf[24] == 0x8B))
                    data.WChillChar = -2;
                else if ((buf[23] == 0xEE) && (buf[24] == 0x8E))
                    data.WChillChar = -3;
                else
                    data.WChillChar = -4;
            }
            else
                data.WChillChar = 0;
            if (((buf[24] & 0x40) != 0x40))
                data.WChillChar = -2;
            if (data.WChillChar == 0)
            {
                data.WChill = (bcd2int(buf[23]) / 10.0) + (bcd2int((char)(buf[24] & 0x0F)) * 10.0);
                if ((buf[24] & 0x20) == 0x20)
                    data.WChill += 0.05;
                if ((buf[24] & 0x80) != 0x80)
                    data.WChill *= -1;
                data.WChillChar = 0;
            }
            else
                data.WChill = 0;

            //decode windgust
            if (debug > 0)
                Console.WriteLine("[DEBUG] WGS BUF[25]=%02x BUF[26]=%02x\n", buf[25], buf[26]);
            if ((bcd2int((char)(buf[25] & 0xF0)) > 90) || (bcd2int((char)(buf[25] & 0x0F)) > 9))
            {
                data.WGustChar = -1;
                if ((buf[25] == 0xBB) && (buf[26] == 0x8B))
                    data.WGustChar = -2;
                else if ((buf[25] == 0xEE) && (buf[26] == 0x8E))
                    data.WGustChar = -3;
                else
                    data.WGustChar = -4;
            }
            else
                data.WGustChar = 0;

            if (data.WGustChar == 0)
            {

                int offset = 0;
                if ((buf[26] & 0x10) == 0x10)
                    offset = 100;
                data.WGust = ((bcd2int(buf[25]) / 10.0) + (bcd2int((char)(buf[26] & 0x0F)) * 10.0) + offset) / 2.23694;
            }
            else
                data.WGust = 0;

            //decode windspeed
            if (debug > 0)
                Console.WriteLine("[DEBUG] WSP BUF[27]=%02x BUF[28]=%02x\n", buf[27], buf[28]);
            if ((bcd2int((char)(buf[27] & 0xF0)) > 90) || (bcd2int((char)(buf[27] & 0x0F)) > 9))
            {
                data.WSpeedChar = -1;
                if ((buf[27] == 0xBB) && (buf[28] == 0x8B))
                    data.WSpeedChar = -2;
                else if ((buf[27] == 0xEE) && (buf[28] == 0x8E))
                    data.WSpeedChar = -3;
                else
                    data.WSpeedChar = -4;
            }
            else
                data.WSpeedChar = 0;

            if (data.WSpeedChar == 0)
            {

                int offset = 0;
                if ((buf[28] & 0x10) == 0x10)
                    offset = 100;
                data.WSpeed = ((bcd2int(buf[27]) / 10.0) + (bcd2int((char)(buf[28] & 0x0F)) * 10.0) + offset) / 2.23694;
            }
            else
                data.WSpeed = 0;

            //decode wind direction
            if (debug > 0)
                Console.WriteLine("[DEBUG] WDR BUF[29]=%02x\n", buf[29] & 0x0F);
            if ((data.WGustChar <= -3) || (data.WSpeedChar <= -3))
            {
                data.WDirChar = -3;
                data.WDir = '0';
            }
            else
            {
                data.WDir = (char)(int)(buf[29] & 0x0F);
                data.WDirChar = 0;
            }

            //decode rain counter
            //don't know to find out it sensor link missing, but is no problem, because the counter is inside
            //the sation, not in the sensor.
            if (debug > 0)
                Console.WriteLine("[DEBUG] RNC BUF[29]=%02x BUF[30]=%02x BUF[31]=%02x\n", buf[29] & 0xF0, buf[30], buf[31]);
            data.RainCountChar = 0;
            data.RainCount = (char)(int)(buf[31] * 0x100 + buf[30]);

            return data;
        }
        private bool CRC_Is_correct()
        {
            if (debug > 0)
                Console.WriteLine("\r\nStart CRC checking");

            int crc;
            bool correct = true;
            crc = 0x00;
            int BufferValue = 0;
            for (int i = 0; i <= buffer.Length - 1; i++)
            {
                BufferValue = charToInt(buffer[i]);

                crc = (int)crc ^ BufferValue;
            }
            if (crc != (int)Char.GetNumericValue(buffer[33]))
            {
                correct = false;
                if (debug > 0)
                    Console.WriteLine("CRC ERROR !  \r\n error code : -2");
            }
            if ((int)Char.GetNumericValue(buffer[0]) != 0x5a)
            {
                correct = false;
                if (debug > 0)
                    Console.WriteLine("CRC ERROR !  \r\n error code : -3");
            }

            if (debug > 0)
                Console.WriteLine("end of CRC check \r\n");

            return correct;
        }
        #endregion

    }
}
