using pj_master.Tools;
using pj_master.Tools.Test;

// default directory : pj-master/csv
var defaultMasterDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "csv"));
var defaultOutputBuildMasterDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "GenerateMessagePacks"));
const string defaultConnectionString = "Server=localhost;Database=game;User=app;Password=appsecret;TreatTinyAsBoolean=false;";

if (args.Length == 0)
{
    return 0;
}

var command = args[0];

try
{
    switch (command)
    {
        case "create_csv":
            CreateCsv(defaultMasterDir);
            break;
        case "import_db":
            ImportMaster(defaultMasterDir, defaultConnectionString);
            break;
        case "build_db":
            await MasterMemoryBuilder.Build(defaultMasterDir, defaultOutputBuildMasterDir);
            break;
        case "test":
            Test.LoadDatabase(defaultOutputBuildMasterDir);
            break;
        default:
            break;
    }
    
    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex);
    return 1;
}

static void CreateCsv(string masterDir)
{
    Console.WriteLine($"[Tool] MasterDir = {masterDir}");
    MasterCsvTool.CreateOrUpdateCsvAll(masterDir);
    Console.WriteLine("[Tool] done.");
}


static void ImportMaster(string masterDir, string connectionString)
{
    MasterImporterFromCsv.ImportAll(masterDir, connectionString);
}


