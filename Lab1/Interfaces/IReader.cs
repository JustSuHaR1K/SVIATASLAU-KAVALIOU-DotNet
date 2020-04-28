using Lab1.Models;
using System.Collections.Generic;

namespace Lab1.Interfaces
{
    interface IReader
    {
        public List<Student> Read(string path);
    }
}