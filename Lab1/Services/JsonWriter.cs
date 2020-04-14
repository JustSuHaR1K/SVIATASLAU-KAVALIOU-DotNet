using Lab1.Interfaces;
using Lab1.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab1.Services
{
    class JsonWriter : IWriter
    {
        public void Write(IEnumerable<Student> students, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                List<StudentToWrite> collectionOfStudents = new List<StudentToWrite>();
                foreach (Student student in students)
                {
                    var studentToWrite = new StudentToWrite
                    {
                        Name = student.Name,
                        Surname = student.Surname,
                        Patronymic = student.Patronymic,
                        AverageMark = student.GetAverageMark(),
                    };
                    collectionOfStudents.Add(studentToWrite);
                }
                writer.Write(JsonConvert.SerializeObject(collectionOfStudents));
            }
        }
    }
}
