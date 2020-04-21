using Lab1.Models;
using System.Collections.Generic;

namespace Lab1.Interfaces
{
    public interface IWriter
    {
        public void Write(IEnumerable<Student> students, string fileName);
    }
}
