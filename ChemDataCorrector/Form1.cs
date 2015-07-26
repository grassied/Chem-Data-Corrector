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


namespace ChemDataCorrector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OpenFileDialog oFile = new OpenFileDialog();                //open file
        SaveFileDialog sFile = new SaveFileDialog();                //saving file

        double startRange = 0;                                      //Start Range
        double endRange = 0;                                        //End Range
        double average = 0;                                         //Average of absorbances
        double number = 0;                                          //Number of values in range
        int startKey = 0;                                           //Starting key (index) of range
        int endKey = 0;                                             //Ending key (index) of range
        bool open = false;                                          //Is open file valid?
        bool save = false;                                          //Is save file valid?


        //Function for Form
        private void Form1_Load(object sender, EventArgs e)

        {


        }
        //open file button
        private void button1_Click(object sender, EventArgs e)
        {
            oFile.Filter = "All Files (*.txt, *.IFX)|*.txt;*.IFX|Text Files (*.txt)|*.txt|IFX Files (*.IFX)|*.IFX";
            if (oFile.ShowDialog() == DialogResult.OK)
            {

                openFileName.Text = oFile.FileName;
                open = true;
            }
            else
            {
                open = false;
            }
        }

        //save as button
        private void button2_Click(object sender, EventArgs e)
        {
            sFile.Filter = "Text Files|*.txt";
            sFile.FileName = oFile.SafeFileName;

            if (sFile.ShowDialog() == DialogResult.OK)
            {
                sFile.InitialDirectory = oFile.InitialDirectory;
                saveFileName.Text = sFile.FileName;
                save = true;
            }
            else
            {
                save = false;
            }
        }
        //save file location Text box
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        //wavelength range label
        private void label1_Click(object sender, EventArgs e)
        {

        }
        //start text box key presses (not used)
        private void startText_KeyDown(object sender, KeyEventArgs e)
        {




        }
        //start range text box
        private void textBox3_TextChanged(object sender, EventArgs e)
        {




        }
        //open file location Text box
        private void openFileName_TextChanged(object sender, EventArgs e)
        {

        }
        //end range text box
        private void endText_TextChanged(object sender, EventArgs e)
        {

        }
        //Start Button Function
        private void START_Click(object sender, EventArgs e)
        {


            string[] correctData = new string[] { };                    //String array of corrected data    
            List<string> correctDataList = new List<string>() { };      //List of corrected data
            List<double> wavelengths = new List<double>() { };          //List of wavelengths
            List<double> absorbances = new List<double>() { };          //List of absorbances
            string[] incorrectData = new string[] { };                  //String array of original data
            string[] splitString = new string[] { };                    //String array to seperate each line by comma
            string temp = "";                                           //Temporary string 
            int count = 0;                                              //Start of data entries (after header)
            int j = 0;                                                  //While loop counter
            double garbage = 0;                                         //Garbage double variable to test for header lines
            bool startValid = false;                                    //if start is valid
            bool endValid = false;                                      //if end is valid
            bool data = false;                                          //line has valid data
            bool input = false;                                         //valid input
            //Makes sure files have been selected/opened

            if (open && save)
            {

                incorrectData = File.ReadAllLines(oFile.FileName);

                while (!data)
                {
                    try
                    {
                        garbage = Convert.ToDouble(incorrectData.ElementAt(j).Substring(0, 1));
                        data = true;
                    }
                    catch (Exception)
                    {
                        data = false;
                        j++;
                        count++;
                    }

                }

                //Converts string lines into 2 lists of doubles
                for (int i = count; i < incorrectData.Length; i++)
                {
                    temp = incorrectData.ElementAt(i);
                  //Tries to seperate by Comma
                    try
                    {
                        splitString = temp.Split(',');

                        wavelengths.Add(Convert.ToDouble(splitString.ElementAt(0)));
                        absorbances.Add(Convert.ToDouble(splitString.ElementAt(1)));

                        Array.Clear(splitString, 0, splitString.Length);
                        input = true;
                    }
                    catch (Exception)
                    {
                       //If comma fails, try by space/tab
                        try
                        {
                            splitString = temp.Split((char[])null, StringSplitOptions.RemoveEmptyEntries); ;

                            wavelengths.Add(Convert.ToDouble(splitString.ElementAt(0)));
                            absorbances.Add(Convert.ToDouble(splitString.ElementAt(1)));

                            Array.Clear(splitString, 0, splitString.Length);
                            input = true;
                        }
                        //input is not in valid format
                        catch (Exception)
                        {

                            DialogResult error = MessageBox.Show("Input is invalid", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            input = false;
                            break;
                        }


                    }

                }

                if (input)
                {
                    // Tries to convert start range to double (catches exceptions to prevent crash)
                    try
                    {
                        startRange = double.Parse(startText.Text, System.Globalization.CultureInfo.InvariantCulture);  //string to double
                        startValid = true;

                    }
                    catch (Exception)
                    {

                        DialogResult error = MessageBox.Show("Start Range is not valid", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        startValid = false;
                    }
                    // Tries to convert end range to double (catches exceptions to prevent crash)

                    try
                    {

                        endRange = double.Parse(endText.Text, System.Globalization.CultureInfo.InvariantCulture);  //string to double
                        endValid = true;

                    }
                    catch (Exception)
                    {

                        DialogResult error = MessageBox.Show("End Range is not valid", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        endValid = false;
                    }
                    //Checks for negative start range
                    if (startRange < 0)
                    {

                        DialogResult error = MessageBox.Show("Start Range can't be negative", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        startValid = false;
                    }
                    //Checks to make sure start is not larger than end            
                    if (startRange > endRange)
                    {

                        DialogResult error = MessageBox.Show("Start Range should be LESS than End Range", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        startValid = false;

                    }
                    //Checks to make sure start and end are in range 
                    else if (startRange < (wavelengths.ElementAt(0)) || (endRange > wavelengths.Max()))
                    {
                        DialogResult error = MessageBox.Show("Values are not in range \n Range is: " + wavelengths.ElementAt(0) + " to " + wavelengths.Max(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        startValid = false;
                        endValid = false;
                    }

                }
                else
                {

                    DialogResult error = MessageBox.Show("Make sure Valid files have been selected", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                if (startValid && endValid)
                {



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

                    ///////////////////////////////////////////////////////////////////////////
                    
                    if (advCheckBox1.Checked == true)
                    {
                        List<double> xWaveLog = new List<double>() { };
                        List<double> yAbsLog = new List<double>() { };
                        for (int i = startKey; i <= endKey; i++)
                        {
                            xWaveLog.Add(Math.Log10(wavelengths.ElementAt(i))); //Converts wavelengths to log10
                            yAbsLog.Add(Math.Log10(absorbances.ElementAt(i)));  //Converts absorbances to log10
                        }

                        //Calculates slope and y intercept of linear fit
                        
                        var ds = new CommonLib.Numerical.XYDataSet(xWaveLog, yAbsLog);
                        double mSlope = ds.Slope;              
                        double bIntercept = ds.YIntercept;    

                        //Calculates new absorbances for each wavelength using linear fit
                        List<double> scatterLine = new List<double> { };
                        for (int i = 0; i < wavelengths.Count; i++)
                        {
                            scatterLine.Add(mSlope*Math.Log10(wavelengths.ElementAt(i))+bIntercept);
                        }

                        //Calculates corrected absorbances
                        for (int i = 0; i < absorbances.Count; i++)
                        {
                            absorbances[i] = absorbances.ElementAt(i) - scatterLine.ElementAt(i);
                        }

                    //Adjusts starting region back to 0
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

                    }
                    else
                    {
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
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////
                    //Merges Lists back into a single string array for printing
                    for (int i = 0; i < wavelengths.Count; i++)
                    {
                        string wave = Math.Round(wavelengths.ElementAt(i), 10).ToString();
                        string abs = Math.Round(absorbances.ElementAt(i), 10).ToString();

                        //makes data tab delimited
                        correctDataList.Add(wave + "\t" + abs);
                        //correctData[i] = wave + "," + abs;

                    }
                    //Write data to save file
                    correctData = correctDataList.ToArray();
                    File.WriteAllLines(sFile.FileName, correctData);
                    DialogResult result = MessageBox.Show("Data Has Been Corrected", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //reset all variables 
                    count = 0;
                    Array.Clear(correctData, 0, correctData.Length);
                    wavelengths.Clear();
                    absorbances.Clear();
                    correctDataList.Clear();
                    Array.Clear(correctData, 0, correctData.Length);
                    Array.Clear(incorrectData, 0, incorrectData.Length);
                    average = 0;

                }

            }
        }
    }
}