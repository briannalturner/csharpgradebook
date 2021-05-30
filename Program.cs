using System;
using System.Collections.Generic;

namespace GradeBook
{
    class Program
    {
        static void Main(string[] args)
        {

            // creating new gradebook
            IBook book = new DiskBook(name: "", grades: new List<double>() { });
            book.GradeAdded += OnGradeAdded;

            // entering name and grades
            bool skipCreation = false;
            skipCreation = EnterName(book, skipCreation);
            EnterGrade(book, skipCreation);

            // printing out statistics
            var stats = book.GetStatistics();

            if (stats.Count <= 0)
            {
                Console.WriteLine("Gradebook creation exited.");
            }
            else
            {
                Console.WriteLine("\n ----------------------------------- ");
                Console.WriteLine($"           {book.Name}");
                Console.WriteLine(" ----------------------------------- \n");
                Console.WriteLine($"The highest grade is {stats.High:N2}.");
                Console.WriteLine($"The lowest grade is {stats.Low:N2}.");
                Console.WriteLine($"The average grade is {stats.Average:N2}.");
                Console.WriteLine($"The average letter grade is {stats.Letter}.");
            }

            // event handler for grade added
            void OnGradeAdded(object sender, EventArgs e, double grade)
            {
                Console.WriteLine($"Successfully added {grade:N2} to {book.Name}. \n");
            }
        }

        private static void EnterGrade(IBook book, bool skipCreation)
        {
            while (!skipCreation)
            {
                Console.WriteLine("Enter a grade, or 'q' to quit");
                var input = Console.ReadLine();

                if (input == "q")
                {
                    break;
                }

                try
                {
                    var grade = double.Parse(input);
                    book.AddGrade(grade);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"{ex.Message} \n");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"{ex.Message} \n");
                }
                finally
                {
                    // executes after catches and try
                }
            }
        }

        private static bool EnterName(IBook book, bool skipCreation)
        {
            while (book.Name == "")
            {
                Console.WriteLine("Please enter a name for your gradebook, or 'q' to quit");
                var input = Console.ReadLine();

                if (input == "q")
                {
                    skipCreation = true;
                    break;
                }

                try
                {
                    book.UpdateName(input);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message} \n");
                }
            }
            return skipCreation;
        }
    }
}
