using System;
using System.IO;
using System.Text;

namespace WriteToSpecifiedFile
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateFile();
            //console.writeline("press any key to run appendcharsafter5() !");
            //console.readkey();
            //appendcharsafter5();
            //console.writeline("add chars is completed, press any key to quit...");
            Console.ReadKey();
        }

        static void WriteToSpecifyPo

        static void GenerateFile()
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory + @"File\Demo.txt";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using (var fileStream = new FileStream(filePath, FileMode.CreateNew))
            {
                string conten = "1234567890";
                byte[] data = Encoding.ASCII.GetBytes(conten);
                fileStream.Write(data, 0, data.Length);
            }
        }

        static void AppendCharsAfter5()
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory + @"File\Demo.txt";
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                string conten = "ABC";
                byte[] data = Encoding.ASCII.GetBytes(conten);
                fileStream.Position = 5;
                fileStream.Write(data, 0, data.Length);
            }
        }

    }
}
