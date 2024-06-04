using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Program
    {
        static void Main(string[] args)
        {
            // File paths
            string csvFilePath = @"C:\Users\nihol\Documents\export.csv";
            string listFilePath = @"C:\Users\nihol\Documents\partitions.csv";
            string outputFilePath = @"C:\Users\nihol\Documents\output.csv";

            // Read 2D CSV into a 2D array
            var data2D = Read2DArrayFromCsv(csvFilePath, out string[] headers);

            // Read list from CSV into a 1D array
            var partitionKeys = Read1DArrayFromCsv(listFilePath);

            // Create the output CSV
            CreateOutputCsv(headers, data2D, partitionKeys, outputFilePath);

            Console.WriteLine("CSV file has been created successfully.");
        }

        static string[][] Read2DArrayFromCsv(string filePath, out string[] headers)
        {
            var lines = File.ReadAllLines(filePath);
            headers = lines[0].Split(',');
            return lines.Skip(1).Select(line => line.Split(',')).ToArray();
        }

        static string[] Read1DArrayFromCsv(string filePath)
        {
            var line = File.ReadAllText(filePath);
            return line.Split(',');
        }

        static void CreateOutputCsv(string[] headers, string[][] data2D, string[] partitionKeys, string outputFilePath)
        {
            using (var writer = new StreamWriter(outputFilePath))
            {
                // Write the header row once
                writer.WriteLine(string.Join(",", headers));

                foreach (var key in partitionKeys)
                {
                    foreach (var row in data2D)
                    {
                        var newRow = new List<string>(row);
                        newRow[0] = key;  // Replace the partition key
                        writer.WriteLine(string.Join(",", newRow));
                    }
                }
            }
        }
    }

