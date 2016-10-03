using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UsbLibraryCfptAdd;

namespace POC_Weebrary
{
    public partial class Form1 : Form
    {
        UsbHidPort usbPort;

        private const string PID = "0x6801", VID = "0x1130";
        public Form1()
        {
            InitializeComponent();

            #region Implémentation du port usb

            //Implémentation du composant usb
            usbPort = new UsbHidPort();

            //Implémentation des procédures événementielles de connexion-déconnexion
            usbPort.OnDeviceArrived += new EventHandler(usbPort_OnDeviceArrived);
            usbPort.OnSpecifiedDeviceArrived += new EventHandler(usbPort_OnSpecifiedDeviceArrived);
            usbPort.OnDeviceRemoved += new EventHandler(usbPort_OnDeviceRemoved);
            usbPort.OnSpecifiedDeviceRemoved += new EventHandler(usbPort_OnSpecifiedDeviceRemoved);

            //Implémentation des procédures événementielles de transmission
            usbPort.OnDataRecieved += new DataRecievedEventHandler(usbPort_OnDataRecieved);
            /*usbPort.OnDataSend += new EventHandler(usbPort_OnDataSend);*/

            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] tbl = new byte[9] { 0x05, 0x0AF, 0x00, 0x00, 0x00, 0x00, 0xAF, 0xFE, 0x00 };
            tbl[4] = 0x0000FB / 0x10000;
            tbl[3] = (byte)((0x0000FB - (tbl[4] * 0x10000)) / 0x100);
            tbl[2] = (byte)(0x0000FB - (tbl[4] * 0x10000) - (tbl[3] * 0x100));
            tbl[5] = (byte)(tbl[1] ^ tbl[2] ^ tbl[3] ^ tbl[4]);
            SpecifiedDevice spfd = usbPort.SpecifiedDevice;
            spfd.SendData(tbl);
            for (int i = 0; i < 9; i++)
                Console.WriteLine(tbl[i].ToString());

            //(usbPort as SpecifiedDevice).SendData(tbl);
            // usbPort.CheckDevicePresent();
            //usbPort.
        }

        private void usbPort_OnDeviceArrived(object sender, EventArgs args)
        {
            MessageBox.Show("USB Connecté");
        }

        private void usbPort_OnSpecifiedDeviceArrived(object sender, EventArgs args)
        {
            MessageBox.Show("USB spécifié Connecté");
        }

        private void usbPort_OnDeviceRemoved(object sender, EventArgs args)
        {
            MessageBox.Show("USB Retiré");
        }

        private void usbPort_OnSpecifiedDeviceRemoved(object sender, EventArgs args)
        {
            MessageBox.Show("USB Spécifié Retiré");
        }

        private void Form1_Load(object sender, EventArgs e)
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
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Configuration VID/PID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void usbPort_OnDataRecieved(object sender, DataRecievedEventArgs args)
        {
            MessageBox.Show("Réception");
        }

    }
}
