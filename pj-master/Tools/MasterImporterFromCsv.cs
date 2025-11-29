
using System.Reflection;
using MasterMemory;
using MySqlConnector;
using pj_master.Schemas;

namespace pj_master.Tools;

public static class MasterImporterFromCsv
{
    public static void ImportAll(string csvDir, string connectionString)
    {
        Console.WriteLine($"[MasterImporter] csv = {csvDir}");
        Console.WriteLine($"[MasterImporter] conn = {connectionString}");
       
        if (!Directory.Exists(csvDir))
        {
            throw new FileNotFoundException($"{csvDir} が見つかりません");
        }
        
        var assembly = typeof(IMasterDefine).Assembly;
        var masterTypes = assembly.GetTypes()
            .Where(t => typeof(IMasterDefine).IsAssignableFrom(t) && !t.IsAbstract && t.GetCustomAttribute<MemoryTableAttribute>() != null)
            .ToArray();
        
        using var conn = new MySqlConnection(connectionString);
        conn.Open();
        
        using var tx = conn.BeginTransaction();

        foreach (var type in masterTypes)
        {
            var attr = type.GetCustomAttribute<MemoryTableAttribute>();;
            var tableName = attr?.TableName ?? type.Name;

            var csvPath = Path.Combine(csvDir, tableName + ".csv");

            Console.WriteLine($"[MasterImporter] import {type.Name} -> table '{tableName}', csv '{csvPath}'");

            ImportOneTable(conn, tx, tableName, csvPath);
        }

        tx.Commit();

        Console.WriteLine("[MasterImporter] all masters imported.");
    }
    
    private static void ImportOneTable(MySqlConnection conn, MySqlTransaction tx, string tableName, string csvPath)
    {
        if (!File.Exists(csvPath))
        {
            throw new FileNotFoundException($"CSV not found: {csvPath}", csvPath);
        }

        var lines = File.ReadAllLines(csvPath)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .ToArray();

        if (lines.Length <= 1)
        {
            Console.WriteLine($"[MasterImporter] skip {tableName} (no data)");
            return;
        }

        // 1行目: ヘッダ（"id","item_group_id",...）
        var header = SplitCsvLine(lines[0])
            .Select(Unquote)
            .ToArray();

        Console.WriteLine($"[MasterImporter] table {tableName}, columns = {string.Join(",", header)}");
        
        // ---- INSERT 準備 ----
        var colList = string.Join(",", header.Select(c => $"`{c}`"));
        var paramList = string.Join(",", header.Select((c, i) => $"@p{i}"));
        var updateList = string.Join(",", header.Select(c => $"`{c}` = VALUES(`{c}`)"));
        
        // PK 衝突時は UPDATE に切り替わる
        var sql = $"INSERT INTO `{tableName}` ({colList}) VALUES ({paramList}) " +
                  $"ON DUPLICATE KEY UPDATE {updateList};";

        foreach (var line in lines.Skip(1))
        {
            var cells = SplitCsvLine(line)
                .Select(Unquote)
                .ToArray();

            if (cells.All(string.IsNullOrWhiteSpace))
                continue;

            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = sql;

            for (int i = 0; i < header.Length; i++)
            {
                cmd.Parameters.AddWithValue($"@p{i}", cells.Length > i ? cells[i] : null);
            }

            cmd.ExecuteNonQuery();
        }
    }

    private static string[] SplitCsvLine(string line) => line.Split(','); 

    private static string Unquote(string s)
    {
        if (s.Length >= 2 && s[0] == '"' && s[^1] == '"')
            return s.Substring(1, s.Length - 2);
        return s;
    }
    
}