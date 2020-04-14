using System.Collections.Generic;
using System.Linq;

namespace Lab1.Models
{
    public class Student
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public IEnumerable<Subject> Subjects { get; set; }

        public double GetAverageMark()
        {
            double averageMark = Subjects.Sum(x => x.Mark) / Subjects.Count();
            return averageMark;
        }
        public static double GetAverageGroupMark(IEnumerable<Student> students)
        {
            double averageGroupMark = students.Sum(x => x.GetAverageMark()) / students.Count();
            return averageGroupMark;
        }
    }
}
