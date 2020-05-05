using Lab1.Models;
using System.Collections.Generic;

namespace Lab1.Interfaces
{
    interface IWriter
    {
        void Write(IEnumerable<Student> students, string fileName);
    }
}
