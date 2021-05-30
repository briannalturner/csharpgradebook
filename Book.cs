using System;
using System.Collections.Generic;
using System.IO;

namespace GradeBook
{

    public delegate void GradeAddedDelegate(object sender, EventArgs args, double grade);

    public class NamedObject
    {
        public NamedObject(string name, List<double> grades)
        {
            this.Name = name;
            this.Grades = grades;
        }

        public string Name
        {
            get;
            set;
        }

        public List<double> Grades
        {
            get;
            set;
        }
    }

    public interface IBook
    {
        void AddGrade(double grade);
        void UpdateName(string name);
        Statistics GetStatistics();
        string Name { get; }
        List<double> Grades { get; }
        event GradeAddedDelegate GradeAdded;
    }

    public abstract class Book : NamedObject, IBook
    {
        public Book(string name, List<double> grades) : base(name, grades)
        {
        }

        public abstract event GradeAddedDelegate GradeAdded;

        public abstract void AddGrade(double grade);

        public abstract Statistics GetStatistics();

        public abstract void UpdateName(string name);
    }

    public class DiskBook : Book
    {
        public DiskBook(string name, List<double> grades) : base(name, grades)
        {
        }

        public override event GradeAddedDelegate GradeAdded;

        public override void AddGrade(double grade)
        {
            // wrapping in using disposes of IDisposable object
            using (var writer = File.AppendText($"{Name}.txt"))
            {
                writer.WriteLine(grade);
                GradeAdded?.Invoke(this, new EventArgs(), grade);
            }
        }

        public override Statistics GetStatistics()
        {
            var result = new Statistics();
            using (var reader = File.OpenText($"{Name}.txt"))
            {
                var line = reader.ReadLine();
                while (line != null)
                {
                    var number = double.Parse(line);
                    result.Add(number);
                    line = reader.ReadLine();
                }
            }

            for (var i = 0; i < this.Grades.Count; i++)
            {
                result.Add(Grades[i]);
            }
            return result;
        }

        public override void UpdateName(string name)
        {
            if (name.Length > 0 && name.Length <= 50)
            {
                this.Name = name;
            }
            else
            {
                throw new ArgumentException("Name must be between 1 and 50 characters.");
            }
        }
    }

    public class InMemoryBook : Book
    {
        // readonly variables must be instantiated with a value and 
        //     can be changed in the constructor

        // readonly string category = "Science";

        public InMemoryBook(string name, List<double> grades) : base(name, grades)
        {
            // this.category = "English";
        }

        public override void UpdateName(string name)
        {
            if (name.Length > 0 && name.Length <= 50)
            {
                this.Name = name;
            }
            else
            {
                throw new ArgumentException("Name must be between 1 and 50 characters.");
            }
        }

        public override event GradeAddedDelegate GradeAdded;

        public override void AddGrade(double grade)
        {
            if (grade >= 0 && grade <= 100)
            {
                this.Grades.Add(grade);

                // checks if anyone is listening
                // if (GradeAdded != null)
                // {
                //     GradeAdded(this, new EventArgs());
                // }

                GradeAdded?.Invoke(this, new EventArgs(), grade);
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(grade)}.");
            }
        }

        public void AddLetterGrade(char letter)
        {
            switch (letter)
            {
                case 'A':
                    this.AddGrade(90);
                    break;
                case 'B':
                    this.AddGrade(80);
                    break;
                case 'C':
                    this.AddGrade(70);
                    break;
                case 'D':
                    this.AddGrade(60);
                    break;
                case 'F':
                    this.AddGrade(50);
                    break;
                default:
                    this.AddGrade(0);
                    break;
            }
        }

        public override Statistics GetStatistics()
        {
            var result = new Statistics();

            for (var i = 0; i < this.Grades.Count; i++)
            {
                result.Add(Grades[i]);
            }
            return result;
        }
    }
}