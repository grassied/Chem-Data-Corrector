using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace DataCorrectionGui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OpenFileDialog oFile = new OpenFileDialog();                //open file
        SaveFileDialog sFile = new SaveFileDialog();                //saving file

        double startRange = 0;
        double endRange = 0;
        double average = 0;
        double number = 0;

        int startKey = 0;
        int endKey = 0;

        string[] incorrectData;
        string[] correctData = new string[] {};
        List<string> correctDataList = new List<string>() { };
        List<double> wavelengths = new List<double>(){  };
        List<double> absorbances = new List<double>() { };


        private void Form1_Load(object sender, EventArgs e)         //form thing?

        {
            

        }

        private void button1_Click(object sender, EventArgs e)          //open file button
        {
            oFile.Filter = "Text Files|*.txt";
            if(oFile.ShowDialog() == DialogResult.OK)
            {
                
                openFileName.Text = oFile.FileName;
                incorrectData = File.ReadAllLines(oFile.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)          //save as button
        {
            sFile.Filter = "Text Files|*.txt";
            
            if (sFile.ShowDialog() == DialogResult.OK)
            {
                sFile.InitialDirectory = oFile.InitialDirectory;
                saveFileName.Text = sFile.FileName;
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)   //save file location
        {

        }

        private void label1_Click(object sender, EventArgs e)           //wavelength range label
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)   //start range text box
        {
            startRange = double.Parse(startText.Text, System.Globalization.CultureInfo.InvariantCulture);  //string to double
            
        }

        private void openFileName_TextChanged(object sender, EventArgs e)     //display open file name
        {

        }

        private void endText_TextChanged(object sender, EventArgs e)       //end range text box
        {
            endRange = double.Parse(endText.Text, System.Globalization.CultureInfo.InvariantCulture);  //string to double
        }

        private void START_Click(object sender, EventArgs e)            //start button
        {


            //Converts string lines into 2 lists of doubles
            for (int i = 2; i < incorrectData.Length; i++)
            {
               

                wavelengths.Add(Convert.ToDouble(incorrectData.ElementAt(i).Substring(0, 5)));  //absorbs
                  absorbances.Add(Convert.ToDouble(incorrectData.ElementAt(i).Substring(6, 7)));   //waves

            }
            
            //Seaches for starting range and ending range in Wavelength List
            for (int i = 0; i < wavelengths.Count; i++)
            {
                if (wavelengths.ElementAt(i) == startRange)
                {
                    startKey = i;
                }

                if (wavelengths.ElementAt(i) == endRange)
                {
                    endKey = i;
                }

            }

            //Sums absorbances from range 
            for (int i = startKey; i <= endKey; i++)
            {
                average += absorbances.ElementAt(i);

            }

            //Calculates average
            number = (endRange - startRange) + 1;
            average = average / number;
            
            //Subtracts average from abosrbances
    
            for (int i = 0; i < absorbances.Count; i++)
            {
                absorbances[i] = absorbances.ElementAt(i) - average;
            }

            //Merges Lists back into a single string array for printing
            for (int i = 0; i < wavelengths.Count; i++)
            {
                string wave = Math.Round(wavelengths.ElementAt(i), 1).ToString();
                string abs = Math.Round(absorbances.ElementAt(i), 5).ToString();


                correctDataList.Add(wave + "," + abs);
                //correctData[i] = wave + "," + abs;

            }

            correctData = correctDataList.ToArray();
            File.WriteAllLines(sFile.FileName, correctData);
            DialogResult result =  MessageBox.Show("Data Has Been Corrected", "Success!", MessageBoxButtons.OK);
        }
    }
}
