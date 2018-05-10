using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
namespace DeviceBatteryState
{
    public partial class Form1 : Form
    {

        //yapılacaklar 
        //1 ->System.Management.dll referanslara eklenir

        public Form1()
        {
            InitializeComponent();
            
        }
        private enum Availability : UInt16
        {
            NotSet = 0,
            Other = 1,
            Unknown = 2,
            Running_FullPower = 3, //Running or Full Power
            Warning = 4,
            InTest = 5,
            NotApplicable = 6,
            PowerOff = 7,
            OffLine = 8,
            OffDuty = 9,
            Degraded = 10,
            NotInstalled = 11,
            InstallError = 12,
            PowerSave_Unknown = 13,      //The device is known to be in a power save mode, but its exact status is unknown.
            PowerSave_LowPowerMode = 14, //The device is in a power save state but still functioning, and may exhibit degraded performance.
            PowerSave_Standby = 15,      //The device is not functioning, but could be brought to full power quickly.
            PowerCycle = 16,
            PowerSave_Warning = 17,      //The device is in a warning state, though also in a power save mode.
            Paused = 18,                 //The device is paused.
            NotReady = 19,               //The device is not ready.
            NotConfigured = 20,          //The device is not configured.
            Quiesced = 21
        }

        public enum BatteryProperties : int
        {
            NotSet = 0,
            Availability = 1,
            BatteryRechargeTime = 2,
            BatteryStatus = 3,
            Caption = 4,
            Chemistry = 5,
            ConfigManagerErrorCode = 6,
            ConfigManagerUserConfig = 7,
            CreationClassName = 8,
            Description = 9,
            DesignCapacity = 10,
            DesignVoltage = 11,
            DeviceID = 12,
            ErrorCleared = 13,
            ErrorDescription = 14,
            EstimatedChargeRemaining = 15,
            EstimatedRunTime = 16,
            ExpectedBatteryLife = 17,
            ExpectedLife = 18,
            FullChargeCapacity = 19,
            InstallDate = 20,
            LastErrorCode = 21,
            MaxRechargeTime = 22,
            Name = 23,
            PNPDeviceID = 24,
            PowerManagementCapabilities = 25,
            PowerManagementSupported = 26,
            SmartBatteryVersion = 27,
            Status = 28,
            StatusInfo = 29,
            SystemCreationClassName = 30,
            SystemName = 31,
            TimeOnBattery = 32,
            TimeToFullCharge = 33
        }

        public static string bataryasiz_bilgisayar = "BATARYASIZ BİLGİSAYAR";
        public static string GetBatteryStatus(BatteryProperties prop = BatteryProperties.NotSet)
        {
            string tekDeger = "";
            string result = "";
            System.Management.ObjectQuery query = new ObjectQuery("Select * FROM Win32_Battery");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection collection = searcher.Get();
            if (collection.Count == 0) //bir desktop makine ise:
                result = bataryasiz_bilgisayar;
            else //bilgisayara bagli bir batarya varsa:
                foreach (ManagementObject mo in collection)
                    foreach (PropertyData property in mo.Properties)
                    {
                        string value = "0";
                        if (property.Value != null) value = property.Value.ToString();
                        if (property.Name == "Availability")
                        {
                            try
                            {
                                Availability tmp = Availability.NotSet;
                                int deger = 0;
                                int.TryParse(value, out deger);
                                tmp = (Availability)deger;
                                value = tmp.ToString();
                            }
                            catch //(Exception ex)
                            {
                                MessageBox.Show("'" + property.Value.ToString() + "'");
                            }
                        }
                        if (prop != BatteryProperties.NotSet && prop.ToString() == property.Name) tekDeger = value;
                        result += string.Format("{0}: {1}" + Environment.NewLine, property.Name, value);
                    }
            if (tekDeger != "") result = tekDeger;
            return result;
            /*  05:08 - %51 => 50/225
             *  05:11 - %48 => 45/211
             *  04:20 - %43 => 43/168
             *  02:27 - %27 => 26/147
             *  00:39 - %05 => 4/40
                --Availability: 3 //bkz.enum
                    BatteryRechargeTime: 0 //bos ta donebiliyor. minutes
                BatteryStatus: 1 //sarjdayken 2, pili harcarken 1 oluyor. bkz.enum
                    Caption: Internal Battery
                --Chemistry: 6 //bkz.enum. 6=LithiumIon
                    ConfigManagerErrorCode: 0 //bos ta donebiliyor. bkz.enum
                    ConfigManagerUserConfig: 0 //bos ta donebiliyor.
                    CreationClassName: Win32_Battery
                    Description: Internal Battery
                    DesignCapacity: 0 //bos ta donebiliyor.
                --DesignVoltage: 3590 //3590 3608 3819 3907 4030 millivolts
                    DeviceID: BattMiniery0000
                    ErrorCleared: 0 //bos ta donebiliyor.
                    ErrorDescription: 0 //bos ta donebiliyor.
                EstimatedChargeRemaining: 50 //1 2 3 6 9 27 46 50 61 kalan tahmini sarj yuzdesi
                EstimatedRunTime: 225 //0-500 arasi pildeyken 71582788 sarjdayken: kalan tahmini sarj dakikasi
                    ExpectedBatteryLife: 0 //bos ta donebiliyor.
                    ExpectedLife: 0 //bos ta donebiliyor.
                    FullChargeCapacity: 0 //bos ta donebiliyor.
                    InstallDate: 0 //bos ta donebiliyor.
                    LastErrorCode: 0 //bos ta donebiliyor.
                    MaxRechargeTime: 0 //bos ta donebiliyor.
                    Name: Intel Battery
                    PNPDeviceID: 0 //bos ta donebiliyor.
                    PowerManagementCapabilities: System.UInt16[] //bkz.enum
                    PowerManagementSupported: False
                    SmartBatteryVersion: 0 //bos ta donebiliyor.
                Status: OK //bkz.enum
                    StatusInfo: 0 //bos ta donebiliyor. bkz.enum
                    SystemCreationClassName: Win32_ComputerSystem
                    SystemName: TANIR-SERVER
                    TimeOnBattery: 0 //bos ta donebiliyor.
                    TimeToFullCharge: 0 //bos ta donebiliyor.
             */
        }


        public void BataryaGoster()
        {
            string pilDurumu = GetBatteryStatus(BatteryProperties.BatteryStatus);
            if (pilDurumu == bataryasiz_bilgisayar)
            {

            }
            else
            {
                bool sarjOluyor = (pilDurumu == "1" ? false : true);
                double bataryaYuzdesi = Convert.ToDouble(GetBatteryStatus(BatteryProperties.EstimatedChargeRemaining));

                string sarj = "";

                if (sarjOluyor)
                {
                    sarj = "Şarj oluyor...";

                }
            

                lblPilYuzdesi.Text = "%   " + bataryaYuzdesi +sarj;
                lblPilYuzdesi.Update();


            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            BataryaGoster();
        }
    }
}
