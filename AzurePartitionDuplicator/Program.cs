using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the filename of the exported table entries:");
            string csvFilePath = Console.ReadLine();

            Console.WriteLine("Please enter the filename of the list of partitions to duplicate into:");
            string listFilePath = Console.ReadLine();

            string outputFilePath = "output.csv";

        try
        {
            var data2D = ReadDataToCopy(csvFilePath, out string[] headers);

            var partitionKeys = ReadPartition(listFilePath);
            
            BuildOutput(headers, data2D, partitionKeys, outputFilePath);

            Console.WriteLine("CSV file has been created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static string[][] ReadDataToCopy(string filePath, out string[] headers)
    {
        var lines = File.ReadAllLines(filePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        headers = lines[0].Split(',');
        return lines.Skip(1).Select(line => line.Split(',')).ToArray();
    }

    static string[] ReadPartition(string filePath)
    {
        var line = File.ReadAllText(filePath).Trim();
        return line.Split(',').Where(item => !string.IsNullOrWhiteSpace(item)).ToArray();
    }

    static void BuildOutput(string[] headers, string[][] data2D, string[] partitionKeys, string outputFilePath)
        {
            using (var writer = new StreamWriter(outputFilePath))
            {
                writer.WriteLine(string.Join(",", headers));

                foreach (var key in partitionKeys)
                {
                    foreach (var row in data2D)
                    {
                        var newRow = new List<string>(row);
                        newRow[0] = key;
                        writer.WriteLine(string.Join(",", newRow));
                    }
                }
            }
        }
    }

