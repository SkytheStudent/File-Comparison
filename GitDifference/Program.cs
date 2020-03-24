using System;

namespace GitDifference
{
    class Program
    {

        static string[] Getting_Files_Ask() // this method is called if the file names entered are not correct the first time the user is asked for them. - called from Getting_Files method
        {
            Console.WriteLine("What are the two files you wish to check?- seperated by a space! Please be specific and name only, please enter caps when needed eg. 'GitRepositories_1a GitRepositories_1b':");
            string user_input = Console.ReadLine();
            string[] filenames = user_input.Split(" ");
            return filenames; 
        }

        static Tuple<string, string, string[], string[]> Getting_Files(string[] filenames)
        {
            if(filenames.Length == 2) // checks to make sure that there are two filenames inputted, otherwise user asked again for filename
            {
                try // try used as we can catch the error/exception if the file names entered do not exist. 
                {
                    string[] file_1_raw_data;
                    string[] file_2_raw_data;
                    string file_name_1;
                    string file_name_2;
                    
                    file_1_raw_data = System.IO.File.ReadAllLines($"{filenames[0]}.txt"); // loads the contents of the files in
                    file_2_raw_data = System.IO.File.ReadAllLines($"{filenames[1]}.txt");
                    file_name_1 = filenames[0];
                    file_name_2 = filenames[1];

                    Tuple<string, string, string[], string[]> file_arrays = new Tuple<string, string, string[], string[]>(file_name_1, file_name_2, file_1_raw_data, file_2_raw_data); // easier to send filenames and file contents in one go as a tuple.
                    return file_arrays;
                }
                catch (Exception e) // if the file names entered are not correct, then Getting_Files_Ask is called to ask the user for another input. 
                {
                    Console.WriteLine(e.Message);
                    string[] filenames2 = new string[2];
                    filenames2 = Getting_Files_Ask(); 
                    return Getting_Files(filenames2);
                }
            }
            else
            {
                Console.WriteLine("Please enter two file names for the comparison!, try again!"); // if there is'nt two file names, then user asked again for file name input.
                string[] file_names = Getting_Files_Ask();
                return Getting_Files(file_names);
            }  
        }              
        static bool AreFilesEqual(string[] a_raw_data, string[] b_raw_data) 
        {
            int a_length = a_raw_data.Length; int b_length = b_raw_data.Length; //define variables
            Console.WriteLine(a_length);
            if (a_length == b_length) // first checks to see whether each file has the same number of lines before moving forward, if they dont, method returns a false.
            {
                int counter = 0;
                for (int i = 0; i <= a_length - 1; i++) // the next stage is to go through each line of one file and compare it to the line of the other. eg. is line 1 of file 1 equal to line 1 of file 2. 
                {
                    string line_a = a_raw_data[i];
                    string line_b = b_raw_data[i];

                    if (line_a == line_b) counter++;

                    else if (line_a != line_b) return false; // if any of the lines dont equal each other, then it just returns false. 
                }

                if (counter == a_length) // if the number of lines equal to its corresponding line in the opposite file is equal to the number of lines in one file then its returned true. 
                    return true;
                else
                    return false;
            }
            else
                return false;

        }
        static void OutputToConsole(string[] filenames, bool result) // this method is dedicated to printing the results to the console.
        {
            Console.WriteLine($"[Input] Diff {filenames[0]}  {filenames[1]}");
            if (result == true) // taking the boolean produced from AreFilesEqual, if the files are equal then this response is produced.
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[Output] {filenames[0]} and {filenames[1]} are the same");
                Console.ResetColor();
            }
            else // otherwise its this response. 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Output] {filenames[0]} and {filenames[1]} are not the same");
                Console.ResetColor();
            }
        }
        static void Main(string[] args)
        {
 
            Tuple<string,string,string[], string[]> file_arrays;


            string[] filenames = Getting_Files_Ask(); // this is where the user is first asked for the filenames which are to be compared. 

            file_arrays = Program.Getting_Files(filenames); // here the various methods are called in the needed order. Get files -> Check whether they're equal -> Output to Console screen.
            string[] file_names = { file_arrays.Item1, file_arrays.Item2 };
            bool result = AreFilesEqual(file_arrays.Item3, file_arrays.Item4);
            OutputToConsole(file_names, result);  
        }
    }
}
