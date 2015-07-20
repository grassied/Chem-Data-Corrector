/********************************************
Gianpaolo Coletto
Data Correction


*********************************************/

#include <iostream>
#include <vector>
#include <istream> 
#include <iomanip>
#include <fstream>
#include <string>
using namespace std;




int main()
{
	string line;
	ifstream file ("Data.txt");
	ofstream output("Output.txt");
	vector <long double> wavelength;
	vector <long double> absorb;
	size_t size1 = 4;
	size_t size2 = 6;
	long double start = 0;
	long double end = 0;

	getline(file, line);			//gets first line (useless header)
	getline(file, line);			//gets second line (useless header)
	
	getline(file, line);			//saves 1st data line

	while (line.length() != 0)		//end of file check
	
	{
		

		string wave = line.substr(0, 5);
		string abs = line.substr(6, 13);

		wavelength.push_back(stod(wave));
		absorb.push_back(stod(abs));

		getline(file, line);			//saves next line

	}										// range of waves, average abs from range, sub avg from ALL abs
		cout << "Input range of wavelengths" << endl;
		cout << "Start: " <<endl;
		cin >> start;
		cout << "End: " << endl;
		cin >> end;
		
		long double number = (end - start) + 1;

		int startKey = 0;
		int endKey = 0;
		int key = 0;
		for ( unsigned int i = 0; i < wavelength.size(); i++)
		{
			if (wavelength.at(i) == start)
			{
				startKey = i;
			}

			else if (wavelength.at(i) == end)
			{
				endKey = i;
			}

			key++;
		}

		long double average = 0;

		for (int i = startKey; i <= endKey; i++)
		{
			average += absorb.at(i);

		}
		
		average = average / number;

		for (unsigned int i = 0; i < absorb.size(); i++)
		{
			absorb.at(i) = absorb.at(i) - average;
		}
//WRITE TO FILE
		output << "Corrected Data" << endl;
		output << setprecision(9) << "Average: " << average << endl;
		
		for (unsigned int i = 0; i < wavelength.size(); i++)
		{
			output << wavelength.at(i) << ", " << setprecision(7) << absorb.at(i) << endl;
		}
	
//WRITE TO SCREEN
		cout << "Corrected Data" << endl;
		cout << setprecision(9) << "Average: " << average << endl;

		for (unsigned int i = 0; i < wavelength.size(); i++)
		{
			cout << wavelength.at(i) << ", " << setprecision(7) << absorb.at(i) << endl;
		}


	system("pause");
	return 0;


}