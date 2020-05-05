using CommandLine;
using Lab1.CommandLineArguments;
using Lab1.Models;
using Lab1.Services;
//using System;
using System.Collections.Generic;

namespace Lab1
{
    class Program
    {
        const string jsonType = "JSON";
        const string excelType = "EXCEL";

        static void Main(string[] args)
        {
            List<Student> students = new List<Student>();
            string inputFile = string.Empty;
            string outputFile = string.Empty;
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(o =>
                   {
                       inputFile = o.InputFile;
                       outputFile = o.OutputFile;
                       if (!string.IsNullOrEmpty(inputFile))
                       {
                           CSVReader reader = new CSVReader();
                           students = reader.Read(inputFile);
                       }
                       if (o.FileType.ToUpper() == jsonType)
                       {
                           JsonWriter jsonWriter = new JsonWriter();
                           jsonWriter.Write(students, $"{outputFile}.json");
                       }
                       else if (o.FileType.ToUpper() == excelType)
                       {
                           ExcelWriter excelWriter = new ExcelWriter();
                           excelWriter.Write(students, $"{outputFile}.xls");
                       }
                   });
        }
    }
}
