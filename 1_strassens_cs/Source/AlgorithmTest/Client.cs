using static System.Console;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.IO;
using System;

namespace StrassensOptimized
{
    class Program
    {
        static void Main(string[] args)
        {
            string computerName = null;
            string processor = null;
            uint numberOfCores = 0;
            int numberOfThreads = 0;
            string clockSpeed = null;
            string l2CacheSize = null;
            string l3CacheSize = null;
            foreach (ManagementObject mo in (new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor")).Get()) 
            {
                computerName = (string) mo["SystemName"];
                processor = (string) mo["Name"];
                numberOfCores = (uint) mo["NumberOfCores"];
                var clockSpeedInt = ((uint) mo["MaxClockSpeed"]);
                if (clockSpeedInt > 1024)
                {
                    clockSpeed = String.Format("{0:0.00}", clockSpeedInt / (double) 1024) + " GHz";
                }
                else
                {
                    clockSpeed = clockSpeedInt.ToString() + " Hz";
                }
                var l2CacheSizeInt = (uint) mo["L2CacheSize"];
                if (l2CacheSizeInt > 1024)
                {
                    l2CacheSize = String.Format("{0:0.00}", l2CacheSizeInt / (double) 1024) + " MB Per Core";
                }
                else
                {
                    l2CacheSize = l2CacheSizeInt.ToString() + " KB Per Core";
                }
                var l3CacheSizeInt = (uint) mo["L3CacheSize"];
                if (l3CacheSizeInt > 1024)
                {
                    l3CacheSize = String.Format("{0:0.00}", l3CacheSizeInt / (double) 1024) + " MB";
                }
                else
                {
                    l3CacheSize = l3CacheSizeInt.ToString() + " KB";
                }
                break;
            }
            numberOfThreads = Matrix.ThreadCount;
            WriteLine("Is hardware accelerated : {0}", Matrix.IsHardwareAccelerated);
            WriteLine("Computer Details =>\nName : {0}\nProcessor : {1}\nClock Speed : {2}\nCores and Threads {3}, {4}\nL2 Cache : {5}\nL3 Cache : {6}", computerName, processor, clockSpeed, numberOfCores, numberOfThreads, l2CacheSize, l3CacheSize);
            var dimension = 4096;
            var random = new System.Random();
            var stopwatch = new Stopwatch();
            var csv = new StreamWriter(computerName + ".csv");
            csv.WriteLine("Computer Details\nName," + computerName + "\nProcessor," + processor + "\nClock Speed," + clockSpeed + "\nCores and Threads," + numberOfCores + "," + numberOfThreads + "\nL2 Cache," + l2CacheSize + "\nL3 Cache," + l3CacheSize);
            csv.WriteLine("Dimension,Naive,Divide and Conquer,Strassens,Parallel Divide and Conquer,Parallel Strassens");
            WriteLine("Dimension : " + dimension);
            var mArray = new double[dimension][];
            var nArray = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                mArray[i] = new double[dimension];
                nArray[i] = new double[dimension];
                for (int j = 0; j < dimension; j++)
                {
                    mArray[i][j] = 100.0 * random.NextDouble();
                    nArray[i][j] = 100.0 * random.NextDouble();
                }
            }
            var m = new Matrix(mArray);
            var n = new Matrix(nArray);
            csv.Write(dimension);
            stopwatch.Start();
            var e = Matrix.StrassensP(m, n, numberOfThreads);
            stopwatch.Stop();
            csv.WriteLine(String.Format("{0:0.000000}", stopwatch.ElapsedMilliseconds));
            WriteLine("Time taken for parallel strassens : {0}", stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            csv.Close();
        }
    }
    public static class Extensions
    {
        public static void Print(this Matrix m)
        {
            WriteLine("Matrix =>");
            var length = m.matrix.Select((row) => row.Select((cell) => cell.ToString().Length).Max()).Max() + 4;
            for (var rowNumber = 0; rowNumber < m.rows; rowNumber++)
            {
                Write('|');
                for (var columnNumber = 0; columnNumber < m.columns; columnNumber++)
                {
                    var thisNumber = m.matrix[rowNumber][columnNumber].ToString();
                    var thisPadding = length - thisNumber.Length;
                    var leftPadding = (int)thisPadding / 2;
                    var rightPadding = thisPadding - leftPadding;
                    for (var paddingNumber = 0; paddingNumber < leftPadding; paddingNumber++)
                    {
                        Write(' ');
                    }
                    Write(thisNumber);
                    for (var paddingNumber = 0; paddingNumber < rightPadding; paddingNumber++)
                    {
                        Write(' ');
                    }
                }
                WriteLine('|');
            }
        }
    }
}