using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceCounter_sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string canceltoken = "s";
            int separatorLength = 40;
            char separatorChar = '-';
            PerformanceCounter cpuCounter =
                new PerformanceCounter("Processor", "% Processor Time", "_Total");
            
            string processName = Process.GetCurrentProcess().ProcessName;
            PerformanceCounterCategory category = new PerformanceCounterCategory("Process");
            PerformanceCounter cpuHpiRunCounter = new PerformanceCounter("Process", "% Processor Time",
                processName,true);
            //string[] processes = category.GetInstanceNames();
            //Console.WriteLine(string.Join(",", processes));

            //Console.WriteLine(new string(separatorChar, separatorLength));

            //Console.WriteLine(processes.Any(p=>p==processName));
            Task.Run(() =>
            {
                while (true)
                {
                    // Perform some repetitive calculations to keep CPU busy
                    for (int i = 0; i < int.MaxValue; i++)
                    {
                        // Dummy calculation
                        Math.Sqrt(i);
                    }
                }
            });
            while (canceltoken != "e")
            {
                Console.WriteLine(new string(separatorChar, separatorLength));

                Console.WriteLine($"cpu usage:{cpuCounter.NextValue()}");


                Console.WriteLine($"cpu usage of process({processName}):{cpuHpiRunCounter.NextValue()}");

                Console.WriteLine(new string(separatorChar, separatorLength)); Console.WriteLine();



                Console.WriteLine("for end enter(e) and for continu press any key except(e)");
                canceltoken = Console.ReadLine();
            }

        }
    }
}
