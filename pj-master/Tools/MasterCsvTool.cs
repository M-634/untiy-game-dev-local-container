using System.Reflection;
using MasterMemory;
using pj_master.Schemas;

namespace pj_master.Tools;

public static class MasterCsvTool
{
    internal static void CreateOrUpdateCsvAll(string masterDir)
    {
        var assembly  = typeof(IMasterDefine).Assembly;
        
        var masterTypes = assembly.GetTypes()
            .Where(t => typeof(IMasterDefine).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false });

        foreach (var type in masterTypes)
        {
            var attribute = type.GetCustomAttribute<MemoryTableAttribute>();
            if (attribute == null)
            {
                Console.WriteLine($"[MasterCsvTool] skip {type.Name}. MasterAttribute not found.");
                continue;
            }
            
            var csvPath = Path.Combine(masterDir, $"{attribute.TableName}.csv");
            CreateOrUpdateCsv(type, csvPath);
        }
    }

    private static void CreateOrUpdateCsv(Type targetType, string outputCsvPath)
    {
        Console.WriteLine($"[MasterCsvTool] ensure csv for {targetType.Name} -> {outputCsvPath}");
        
        var props = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var newColumns =  props.Select(p => ConvertHeaderToSnakeCase(p.Name)).ToArray();

        foreach (var test in newColumns)
        {
            Console.WriteLine("column : " + test);
        }
        
        
        // create .csv file
        if (!File.Exists(outputCsvPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outputCsvPath)!);
            File.WriteAllText(outputCsvPath, string.Join(",", newColumns) + Environment.NewLine);
            return;
        }
        
        var lines = File.ReadAllLines(outputCsvPath).ToList();
        if (lines.Count == 0)
        {
            lines.Add(string.Join(",", newColumns));
            return;
        }
        
        
        // update .csv file
        var oldHeader = SplitCsvLine(lines[0]);
        var oldColumnToIndex = oldHeader
            .Select((name, index) => new { name, index })
            .ToDictionary(x => x.name, x => x.index);
        
        var newHeader = newColumns;
        var newLines = new List<string> { string.Join(",", newHeader) };
        
        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var cells = SplitCsvLine(line);
            var newCells = new string[newHeader.Length];

            for (int i = 0; i < newHeader.Length; i++)
            {
                var colName = newHeader[i];
                if (oldColumnToIndex.TryGetValue(colName, out int oldIdx) && oldIdx < cells.Length)
                {
                    newCells[i] = ConvertCsvValue(cells[oldIdx]);
                }
                else
                {
                    newCells[i] = "";
                }
            }

            if (newCells.All(c => !string.IsNullOrWhiteSpace(c)))
            {
                newLines.Add(string.Join(",", newCells));
            }
        }

        File.WriteAllLines(outputCsvPath, newLines);
    }
    
    private static string[] SplitCsvLine(string line) => line.Split(',');
    private static string ConvertCsvValue(string value) => "\"" +  value + "\"";
    private static string ConvertHeaderToSnakeCase(string header)
    {
        return ConvertCsvValue(header);
        // var text = new StringBuilder();
        // foreach (var c in header)
        // {
        //     if (char.IsUpper(c))
        //     {
        //         if (text.Length > 0)
        //         {
        //             text.Append('_');
        //         }
        //         text.Append(char.ToLowerInvariant(c));
        //     }
        //     else
        //     {
        //         text.Append(c);
        //     }
        // }
        //
        // return ConvertCsvValue(text.ToString());
    }
}