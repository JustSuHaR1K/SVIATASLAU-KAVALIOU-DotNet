using Lab1.Models;
using System.Collections.Generic;

namespace Lab1.Interfaces
{
    interface IReader
    {
        List<Student> Read(string path);
    }
}