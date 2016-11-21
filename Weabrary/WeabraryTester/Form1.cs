using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Weabrary;

namespace WeabraryTester
{
    public partial class Form1 : Form
    {
         WeabraryConnect wb = new WeabraryConnect();
        public Form1()
        {
            InitializeComponent();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wb.Refresh();          
            textBox1.Text += String.Join(Environment.NewLine, wb.Help());
            textBox1.Text += Environment.NewLine;            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (wb.Connection())
            {
              //  MessageBox.Show("connected");
            }
            else
            {
                MessageBox.Show("Not connected");
                OnDeviceRemoved(this, EventArgs.Empty);
            }

            wb.weabrary_DeviceArrived += this.OnDeviceArrived;
            wb.weabrary_DeviceRemoved += this.OnDeviceRemoved;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("Sensor 0 Temperature : " + wb.GeTemperatureBySensor(0).ToString()+Environment.NewLine);
            textBox1.AppendText("Sensor 1 Temperature : " + wb.GeTemperatureBySensor(1).ToString() + Environment.NewLine);
            textBox1.AppendText("Sensor 2 Temperature : " + wb.GeTemperatureBySensor(2).ToString() + Environment.NewLine);
            textBox1.AppendText("Sensor 3 Temperature : " + wb.GeTemperatureBySensor(3).ToString() + Environment.NewLine);
            textBox1.AppendText("Sensor 4 Temperature : " + wb.GeTemperatureBySensor(4).ToString() + Environment.NewLine);
            textBox1.AppendText("Sensor 5 Temperature : " + wb.GeTemperatureBySensor(5).ToString() + Environment.NewLine);


            textBox1.AppendText("Sensor 0 Humidity : " + wb.GetHumidityBySensor(0).ToString() + Environment.NewLine);
            textBox1.AppendText("Sensor 1 Humidity : " + wb.GetHumidityBySensor(1).ToString() + Environment.NewLine);
            textBox1.AppendText("Sensor 2 Humidity : " + wb.GetHumidityBySensor(2).ToString() + Environment.NewLine);
            textBox1.AppendText("Sensor 3 Humidity : " + wb.GetHumidityBySensor(3).ToString() + Environment.NewLine);
            textBox1.AppendText("Sensor 4 Humidity : " + wb.GetHumidityBySensor(4).ToString() + Environment.NewLine);
            textBox1.AppendText("Sensor 5 Humidity : " + wb.GetHumidityBySensor(5).ToString() + Environment.NewLine);

            textBox1.AppendText("UV: " + wb.GetUv().ToString() + Environment.NewLine);
            textBox1.AppendText("Pressure: " + wb.GetPressure().ToString() + Environment.NewLine);
            textBox1.AppendText("Forecast: " + wb.GetForecast().ToString() + Environment.NewLine);
            textBox1.AppendText("Wind chill: " + wb.GetWindChill().ToString() + Environment.NewLine);
            textBox1.AppendText("Wind gust: " + wb.GetWindGust().ToString() + Environment.NewLine);
            textBox1.AppendText("wind speed: " + wb.GetWindSpeed().ToString() + Environment.NewLine);
            textBox1.AppendText("Win direction: " + wb.GetWindDirection().ToString() + Environment.NewLine);
            textBox1.AppendText("Rain count: " + wb.GetRainCount().ToString() + Environment.NewLine);
            
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            wb.Refresh();
        }

        private void OnDeviceArrived(object sender, EventArgs e) {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            textBox1.AppendText("*************************************************************" +Environment.NewLine);
            textBox1.AppendText("                 Device connected" + Environment.NewLine);
            textBox1.AppendText("*************************************************************" + Environment.NewLine);


        }
        private void OnDeviceRemoved(object sender, EventArgs e) {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            textBox1.AppendText("*************************************************************" + Environment.NewLine);
            textBox1.AppendText("                 Device disconnected" + Environment.NewLine);
            textBox1.AppendText("*************************************************************" + Environment.NewLine);
        }
    }
}
