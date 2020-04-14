using System;
using System.Collections.Generic;
using System.Text;
using Lab1.Models;
using NLog;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.TypeConversion;

namespace Lab1.Services
{
    public class CSVReader : Interfaces.IReader
    {
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();
        const int indexOfSubjects = 3;
        public List<Student> Read(string path)
        {
            using (var reader = new StreamReader(path))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = new List<Student>();
                    List<Subject> listOfSubjects;
                    int index;
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        if (csv.Context.Record.Length != csv.Context.HeaderRecord.Length)
                        {
                            logger.Error($"Student's {csv.Context.HeaderRecord[0]} count of subjects didn't match the count of marks");
                            throw new InvalidDataException("Wrong number of parametres");
                        }
                        index = indexOfSubjects;
                        listOfSubjects = new List<Subject>();
                        while (index < csv.Context.Record.Length)
                        {
                            try
                            {
                                var subject = new Subject
                                {
                                    Name = csv.Context.HeaderRecord[index],
                                    Mark = csv.GetField<int>(index),
                                };
                                index++;
                                listOfSubjects.Add(subject);
                            }
                            catch (ArgumentException)
                            {
                                logger.Error($"Student's {csv.Context.HeaderRecord[0]} mark of {csv.Context.HeaderRecord[index]} isn't integer");
                                throw;
                            }
                            catch (IndexOutOfRangeException)
                            {
                                logger.Error($"Student's {csv.Context.HeaderRecord[0]} count of subjects didn't match the count of marks");
                                throw;
                            }
                            catch (TypeConverterException)
                            {
                                logger.Error($"Student's {csv.Context.HeaderRecord[0]} mark of {csv.Context.HeaderRecord[index]} can't be converted in integer");
                                throw;
                            }
                            catch (Exception)
                            {
                                logger.Error("Unexpected error");
                                throw;
                            }
                        }
                        var record = new Student
                        {
                            Surname = csv.Context.Record[0],
                            Name = csv.Context.Record[1],
                            Patronymic = csv.Context.Record[2],
                            Subjects = listOfSubjects,
                        };
                        records.Add(record);
                    }
                    return records;
                }
            }
        }
    }
}
