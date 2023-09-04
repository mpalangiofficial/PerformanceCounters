using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
            var process = Process.GetCurrentProcess();
            string processName = process.ProcessName;
            PerformanceCounterCategory category = new PerformanceCounterCategory("Process");
            PerformanceCounter cpuHpiRunCounter = new PerformanceCounter("Process", "% Processor Time",
                processName, true);

            PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
            string instance = performanceCounterCategory.GetInstanceNames()[0]; // 1st NIC !
            PerformanceCounter performanceCounterSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
            PerformanceCounter performanceCounterReceived = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
            PerformanceCounter performanceCounterSentProcess = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
            performanceCounterSentProcess.InstanceName = processName;
            PerformanceCounter performanceCounterReceivedProcess = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
            performanceCounterReceivedProcess.InstanceName = processName;

            var bytesSent = new PerformanceCounter("Process", "IO Data Bytes/sec", processName);
            var bytesReceived = new PerformanceCounter("Process", "IO Data Bytes/sec", processName);
            //for (int i = 0; i < 10; i++)
            //{
            //}
            //string[] processes = category.GetInstanceNames();
            //Console.WriteLine(string.Join(",", processes));

            //Console.WriteLine(new string(separatorChar, separatorLength));

            //Console.WriteLine(processes.Any(p=>p==processName));
            Task.Run(() =>
            {
                Thread.CurrentThread.Name = "Calc_Thread";
                var list = new List<double>();
                var synchronizedList = new SynchronizedCollection<double>(list);
                while (true)
                {
                    // Perform some repetitive calculations to keep CPU busy
                    for (int i = 0; i < int.MaxValue; i++)
                    {

                        // Dummy calculation
                        try
                        {
                            Math.Sqrt(i);
                            //synchronizedList.Add(Math.Sqrt(i));
                        }
                        catch (Exception ex)
                        {

                            // Console.WriteLine(ex.Message);
                        }
                    }
                }
            });
            while (canceltoken != "e")
            {
                Console.Clear();
                Console.WriteLine(new string(separatorChar, separatorLength));

                Console.WriteLine("bytes sent: {0}k\tbytes received: {1}k", performanceCounterSent.NextValue() / 1024, performanceCounterReceived.NextValue() / 1024);
                Console.WriteLine("process->bytes sent: {0}k\tbytes received: {1}k", bytesSent.NextValue() / 1024, bytesReceived.NextValue() / 1024);

                Console.WriteLine($"cpu usage:{cpuCounter.NextValue()}");


                Console.WriteLine($"cpu usage of process({processName}):{cpuHpiRunCounter.NextValue()}");

                Console.WriteLine("bytes sent: {0}k\tbytes received: {1}k", performanceCounterSent.NextValue() / 1024, performanceCounterReceived.NextValue() / 1024);

                Console.WriteLine(new string(separatorChar, separatorLength)); Console.WriteLine();

                //process

                Console.WriteLine("for end enter(e) and for continu press any key except(e)");
                canceltoken = Console.ReadLine();

            }

        }
    }
}
