using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Please enter the filename of the 2D CSV file:");
        string csvFilePath = Console.ReadLine();

        Console.WriteLine("Please enter the filename of the list CSV file:");
        string listFilePath = Console.ReadLine();

        string outputFilePath = "C:\\Users\\NicholasMacdonald\\OneDrive - nurtur.group Ltd\\Apps\\AzurePartitionDuplicator\\output.csv";

        try
        {
            // Read 2D CSV into a list of string arrays
            var data2D = Read2DArrayFromCsv(csvFilePath, out string[] headers);

            // Read list from CSV into a list of KeyRow objects
            var keyRows = ReadKeyRowsFromCsv(listFilePath);

            // Create the output CSV
            CreateOutputCsv(headers, data2D, keyRows, outputFilePath);

            Console.WriteLine("CSV file has been created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static List<string[]> Read2DArrayFromCsv(string filePath, out string[] headers)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
            MissingFieldFound = null, // Ignore missing fields
            BadDataFound = context => { } // Ignore bad data
        }))
        {
            csv.Read();
            csv.ReadHeader();
            headers = csv.HeaderRecord;

            var records = new List<string[]>();
            while (csv.Read())
            {
                var record = new string[headers.Length];
                for (var i = 0; i < headers.Length; i++)
                {
                    record[i] = csv.GetField(i);
                }
                records.Add(record);
            }
            return records;
        }
    }

    static List<KeyRow> ReadKeyRowsFromCsv(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
            MissingFieldFound = null, // Ignore missing fields
            BadDataFound = context => { } // Ignore bad data
        }))
        {
            return csv.GetRecords<KeyRow>().ToList();
        }
    }

    static void CreateOutputCsv(string[] headers, List<string[]> data2D, List<KeyRow> keyRows, string outputFilePath)
    {
        using (var writer = new StreamWriter(outputFilePath))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            TrimOptions = TrimOptions.Trim,
            NewLine = Environment.NewLine,
            ShouldQuote = (args) => true // Quote all fields
        }))
        {
            // Write the header row once
            foreach (var header in headers)
            {
                csv.WriteField(header);
            }
            csv.NextRecord();

            foreach (var keyRow in keyRows)
            {
                foreach (var row in data2D)
                {
                    csv.WriteField(keyRow.PartitionKey); // Column 1: Partition Key
                    csv.WriteField(row[1] + keyRow.RowKey); // Column 2: Original Column 2 with Row Key appended
                    csv.WriteField(keyRow.RowKey); // Column 3: Row Key

                    // Add the remaining columns from the original row
                    for (int i = 3; i < row.Length; i++)
                    {
                        csv.WriteField(row[i]);
                    }
                    csv.NextRecord();
                }
            }
        }
    }
}

public class KeyRow
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
}
