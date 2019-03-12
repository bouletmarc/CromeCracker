using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CromeCracker
{
    public partial class Form1 : Form
    {
        public string Version = "V1.0.0";
        public byte[] Buffer;

        public List<string> LocationIndex;
        public List<int> LocationMode;

        public List<string> LocationIndexCracked;
        public List<int> LocationModeCracked;

        public byte[] Find1 = new byte[7] { 0x72, 0x0F, 0x74, 0x2F, 0xFE, 0xC8, 0x74 };     // -5
        public byte[] Find2 = new byte[7] { 0x74, 0x0C, 0xEB, 0x5D, 0xC6, 0x80, 0x65 };     // -2
        public byte[] Find3 = new byte[7] { 0x75, 0x54, 0xA1, 0xA0, 0x17, 0x5A, 0x00 };     // -1
        public byte[] Find4 = new byte[7] { 0x75, 0x14, 0x8D, 0x55, 0xF4, 0x8B, 0x83 };     // -1
        public byte[] Find5 = new byte[7] { 0x0F, 0x84, 0x98, 0x00, 0x00, 0x00, 0x8D };     // -1

        public byte[] Crack1 = new byte[7] { 0x90, 0x90, 0x90, 0x90, 0xFE, 0xC8, 0xEB };    // +5
        public byte[] Crack2 = new byte[7] { 0x90, 0x90, 0xEB, 0x5D, 0xC6, 0x80, 0x65 };    // +2
        public byte[] Crack3 = new byte[7] { 0xEB, 0x54, 0xA1, 0xA0, 0x17, 0x5A, 0x00 };    // +1
        public byte[] Crack4 = new byte[7] { 0x74, 0x14, 0x8D, 0x55, 0xF4, 0x8B, 0x83 };    // +1
        //public byte[] Crack4 = new byte[7] { 0xEB, 0x14, 0x8D, 0x55, 0xF4, 0x8B, 0x83 };    // +1
        public byte[] Crack5 = new byte[7] { 0x0F, 0x85, 0x98, 0x00, 0x00, 0x00, 0x8D };     // +1

        public Form1()
        {
            InitializeComponent();

            button2.Enabled = false;
            button3.Enabled = false;

            Log_This("Crome Cracker Interface Loaded " + Version);
            Log_This("Made by BM Devs (Bouletmarc)");
        }

        public void Log_This(string Message)
        {
            textBox_Logs.AppendText(Message);
            textBox_Logs.AppendText(Environment.NewLine);
        }

        private string GetHexLocation(int ThisNumber)
        {
            string XNumber = "2";

            if (ThisNumber > 255)
                XNumber = "2";
            if (ThisNumber > 4095)
                XNumber = "3";
            if (ThisNumber > 65535)
                XNumber = "4";
            if (ThisNumber > 1048575)
                XNumber = "5";
            if (ThisNumber > 16777215)
                XNumber = "6";
            if (ThisNumber > 268435455)
                XNumber = "7";

            return ThisNumber.ToString("X" + XNumber);
        }

        private void Crack()
        {
            Log_This("Cracking...");

            for (int index = 0; index < LocationIndex.Count; index++)
            {
                int Location = int.Parse(LocationIndex[index]);
                int Mode = LocationMode[index];

                byte[] LookupArray = new byte[0];

                if (Mode == 0) LookupArray = Crack1;
                if (Mode == 1) LookupArray = Crack2;
                if (Mode == 2) LookupArray = Crack3;
                if (Mode == 3) LookupArray = Crack4;
                if (Mode == 4) LookupArray = Crack5;

                for (int i = 0; i < LookupArray.Length; i++)
                    Buffer[Location + i] = LookupArray[i];

                if (LocationIndex.Count > 1)
                    progressBar1.Value = (index * 100) / (LocationIndex.Count - 1);
                else
                    progressBar1.Value = 50;
            }

            Log_This("Cracking DONE");

            progressBar1.Value = 0;

            Save();
        }

        private void UnCrack()
        {
            Log_This("Uncracking...");

            for (int index = 0; index < LocationIndexCracked.Count; index++)
            {
                int Location = int.Parse(LocationIndexCracked[index]);
                int Mode = LocationModeCracked[index];

                byte[] LookupArray = new byte[0];

                if (Mode == 0) LookupArray = Find1;
                if (Mode == 1) LookupArray = Find2;
                if (Mode == 2) LookupArray = Find3;
                if (Mode == 3) LookupArray = Find4;
                if (Mode == 4) LookupArray = Find5;

                for (int i = 0; i < LookupArray.Length; i++)
                    Buffer[Location + i] = LookupArray[i];

                if (LocationIndexCracked.Count > 1)
                    progressBar1.Value = (index * 100) / (LocationIndexCracked.Count - 1);
                else
                    progressBar1.Value = 50;
            }

            Log_This("Cracking DONE");

            progressBar1.Value = 0;

            Save();
        }

        private void CheckCompatible()
        {
            LocationIndex = new List<string>();
            LocationMode = new List<int>();

            LocationIndexCracked = new List<string>();
            LocationModeCracked = new List<int>();

            bool CanBeCracked = false;
            bool AlreadyCracked = false;

            button2.Enabled = false;
            button3.Enabled = false;

            Log_This("Checking Compatibility");

            progressBar1.Value = 0;

            for (int Index = 0; Index < 5; Index++)
            {
                byte[] LookupArray = new byte[0];
                byte[] LookupArrayCracked = new byte[0];

                if (Index == 0)
                {
                    LookupArray = Find1;
                    LookupArrayCracked = Crack1;
                }
                if (Index == 1)
                {
                    LookupArray = Find2;
                    LookupArrayCracked = Crack2;
                }
                if (Index == 2)
                {
                    LookupArray = Find3;
                    LookupArrayCracked = Crack3;
                }
                if (Index == 3)
                {
                    LookupArray = Find4;
                    LookupArrayCracked = Crack4;
                }
                if (Index == 4)
                {
                    LookupArray = Find5;
                    LookupArrayCracked = Crack5;
                }

                for (int i = 0; i < Buffer.Length; i++)
                {

                    progressBar1.Value = (i * 100) / (Buffer.Length - 1);

                    if (Buffer[i] == LookupArray[0])
                    {
                        bool IsMatchingLookup = true;

                        for (int i2 = 0; i2 < LookupArray.Length; i2++)
                            if (Buffer[i + i2] != LookupArray[i2])
                                IsMatchingLookup = false;

                        if (IsMatchingLookup)
                        {
                            LocationIndex.Add(i.ToString());
                            LocationMode.Add(Index);
                            CanBeCracked = true;
                        }
                    }

                    if (Buffer[i] == LookupArrayCracked[0])
                    {
                        bool IsMatchingLookup = true;

                        for (int i2 = 0; i2 < LookupArrayCracked.Length; i2++)
                            if (Buffer[i + i2] != LookupArrayCracked[i2])
                                IsMatchingLookup = false;

                        if (IsMatchingLookup)
                        {
                            LocationIndexCracked.Add(i.ToString());
                            LocationModeCracked.Add(Index);
                            AlreadyCracked = true;
                        }
                    }
                }
            }

            progressBar1.Value = 0;

            if (CanBeCracked)
            {
                button2.Enabled = true;

                if (AlreadyCracked)
                {
                    Log_This("This file is partially cracked");
                    button3.Enabled = true;
                }
                else
                    Log_This("This file can be cracked");
            }
            else
            {
                if (AlreadyCracked)
                {
                    Log_This("This file is already cracked");
                    button3.Enabled = true;
                }
                else
                    Log_This("This file CAN'T be cracked");
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                Buffer = File.ReadAllBytes(openFileDialog1.FileName);
                Log_This("File : " + Path.GetFileName(openFileDialog1.FileName) + " loaded");
                Log_This("File Size (integer/hex) : " + Buffer.Length + "/0x" + GetHexLocation(Buffer.Length));
                CheckCompatible();
            }
        }

        private void Save()
        {
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    File.Create(saveFileDialog1.FileName).Dispose();
                    File.WriteAllBytes(saveFileDialog1.FileName, Buffer);

                    Log_This("File : " + Path.GetFileName(saveFileDialog1.FileName) + " saved");
                }
                catch
                {
                    Log_This("ERROR : CAN'T write file at the specified location");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Crack();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UnCrack();
        }
    }
}
