using Lab1.Interfaces;
using Lab1.Models;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace Lab1.Services
{
    class ExcelWriter : IWriter
    {
        public void Write(IEnumerable<Student> students, string fileName)
        {
            const string worksheetName = "Sheet 1";
            const string averageGroupMark = "Average group mark:";
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //Create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(worksheetName);
                List<StudentToWrite> studentsToWrite = new List<StudentToWrite>();
                foreach (Student student in students)
                {
                    var studentToWrite = new StudentToWrite
                    {
                        Name = student.Name,
                        Surname = student.Surname,
                        Patronymic = student.Patronymic,
                        AverageMark = student.GetAverageMark(),
                    };
                    studentsToWrite.Add(studentToWrite);
                }
                //Add some text to cell A1
                worksheet.Cells["A1"].LoadFromCollection(studentsToWrite);
                worksheet.Cells["F1"].Value = averageGroupMark;
                worksheet.Cells["F2"].Value = Student.GetAverageGroupMark(students);

                //Save your file
                FileInfo fi = new FileInfo(fileName);
                excelPackage.SaveAs(fi);
            }
        }
    }
}
