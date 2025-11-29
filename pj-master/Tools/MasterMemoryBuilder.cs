using System.Collections;
using System.Reflection;
using MasterMemory;
using pj_master.Schemas;

namespace pj_master.Tools;

public static class MasterMemoryBuilder
{
    public static async Task Build(string csvDir, string outputPath)
    {
        var builder = new DatabaseBuilder();
        
        var asm = typeof(IMasterDefine).Assembly;

        var masterTypes = asm.GetTypes()
            .Where(t => typeof(IMasterDefine).IsAssignableFrom(t) && !t.IsAbstract && t.GetCustomAttribute<MemoryTableAttribute>() != null)
            .Select(t => new { Type = t, Attr = t.GetCustomAttribute<MemoryTableAttribute>()! })
            .ToArray();


        if (Directory.Exists(outputPath))
        {
            Directory.Delete(outputPath, true);
        }
        Directory.CreateDirectory(outputPath);

        foreach (var dic in masterTypes)
        {
            // csv to class object
            var csvPath = Path.Combine(csvDir, dic.Attr.TableName + ".csv");
            var list = LoadListFromCsv(dic.Type, csvPath);
            Append(builder, list, dic.Type);
        }
        
        var bin = builder.Build();
        var filePath = Path.Combine(outputPath, "master.bytes");
        
        await using var fs = File.Create(filePath);
        await fs.WriteAsync(bin);
    }
    
    private static IList LoadListFromCsv(Type type, string csvPath)
    {
        if (!File.Exists(csvPath)) throw new FileNotFoundException($"CSV not found: {csvPath}", csvPath);
        
        var lines = File.ReadAllLines(csvPath)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .ToArray();

        var listType = typeof(List<>).MakeGenericType(type);
        var list = (IList)Activator.CreateInstance(listType)!;
        
        if (lines.Length <= 1)
        {
            // 空の List<T> を返す
            return list;
        }

        // 1行目: ヘッダ
        var header = SplitCsvLine(lines[0])
            .Select(Unquote)
            .ToArray();
        
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite) // set; 可能なものだけ使う
            .ToArray();
        
        // ヘッダ列名 → index
        var colIndex = header
            .Select((name, idx) => new { name, idx })
            .ToDictionary(x => x.name, x => x.idx);
        
        foreach (var line in lines.Skip(1))
        {
            var cells = SplitCsvLine(line)
                .Select(Unquote)
                .ToArray();

            if (cells.All(string.IsNullOrWhiteSpace)) continue;
            
            var instance = Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Cannot create instance of {type.FullName}");

            foreach (var prop in props)
            {
                if (!colIndex.TryGetValue(prop.Name, out var idx) || idx >= cells.Length)
                {
                    // CSV に列が無い/不足 → スキップ or デフォルト
                    continue;
                }

                var raw = cells[idx];
                if (string.IsNullOrWhiteSpace(raw))
                {
                    // 空文字 → 型ごとのデフォルト (null or 0)
                    continue;
                }

                var value = ConvertString(raw, prop.PropertyType);
                prop.SetValue(instance, value);
            }

            list.Add(instance);
        }
        
        return list;
    }

    private static void Append(DatabaseBuilder builder, IList list, Type type)
    {
        var targetMethod = typeof(DatabaseBuilder)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .First(m => Match(m, type, "Append"));
        
        targetMethod.Invoke(builder, [list]);
    }

    private static bool Match(MethodInfo methodInfo, Type targetType, string methodName)
    {
        if(methodInfo.Name != methodName) return false;
        
        var parameters = methodInfo.GetParameters();

        foreach (var parameter in parameters)
        {
            var argumentType = parameter.ParameterType.GenericTypeArguments[0];
            if(argumentType == targetType) return true;
        }
        
        return false;
    }
    
    private static object? ConvertString(string raw, Type targetType)
    {
        if (targetType == typeof(string)) return raw;
        if (targetType == typeof(int) || targetType == typeof(int?))
            return int.Parse(raw);
        if (targetType == typeof(long) || targetType == typeof(long?))
            return long.Parse(raw);
        if (targetType == typeof(float) || targetType == typeof(float?))
            return float.Parse(raw);
        if (targetType == typeof(double) || targetType == typeof(double?))
            return double.Parse(raw);
        if (targetType == typeof(bool) || targetType == typeof(bool?))
            return raw == "1" || raw.Equals("true", StringComparison.OrdinalIgnoreCase);

        if (targetType.IsEnum)
            return Enum.Parse(targetType, raw, ignoreCase: true);

        // Nullable<T>
        if (Nullable.GetUnderlyingType(targetType) is Type inner)
            return ConvertString(raw, inner);

        // 最悪、ChangeType に投げる
        return Convert.ChangeType(raw, targetType);
    }
    
    private static string[] SplitCsvLine(string line) => line.Split(',');

    private static string Unquote(string s)
    {
        if (s.Length >= 2 && s[0] == '"' && s[^1] == '"')
            return s.Substring(1, s.Length - 2);
        return s;
    }
    
}
