Param(
    [string]$DbCont = "pj_db",
    [string]$DbName = "game",
    [string]$OutInCont = "/var/lib/mysql-files",
    [string]$OutHost = "..\pj-master\csv"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Write-Host "==> Exporting master CSVs from container $DbCont (db=$DbName)"

if (-not (Test-Path $OutHost)) {
    New-Item -ItemType Directory -Force -Path $OutHost | Out-Null
}

function ExecInDb([string]$Cmd) {
    docker exec -i $DbCont sh -c "$Cmd"
}

# 1) m_item_group
ExecInDb "rm -f $OutInCont/m_item_group.csv"
$SqlItemGroup = "SELECT 'item_group_id','item_group_name' " +
                "UNION ALL " +
                "SELECT CAST(ItemGroupId AS CHAR), ItemGroupName " +
                "FROM m_item_group " +
                "INTO OUTFILE '$OutInCont/m_item_group.csv' " +
                "FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' " +
                "LINES TERMINATED BY '\n';"
$CmdItemGroup = "mysql -uroot -p`"`$MYSQL_ROOT_PASSWORD`"` $DbName -e `"$SqlItemGroup`""
ExecInDb $CmdItemGroup

# 2) m_item
ExecInDb "rm -f $OutInCont/m_item.csv"
$SqlItem = "SELECT 'item_id','item_group_id','rarity','max_possess_count' " +
           "UNION ALL " +
           "SELECT CAST(ItemId AS CHAR), CAST(ItemGroupId AS CHAR), CAST(Rarity AS CHAR), CAST(MaxPossessCount AS CHAR) " +
           "FROM m_item " +
           "INTO OUTFILE '$OutInCont/m_item.csv' " +
           "FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' " +
           "LINES TERMINATED BY '\n';"
$CmdItem = "mysql -uroot -p`"`$MYSQL_ROOT_PASSWORD`"` $DbName -e `"$SqlItem`""
ExecInDb $CmdItem

# 3) m_item_localize
ExecInDb "rm -f $OutInCont/m_item_localize.csv"
$SqlLoc = "SELECT 'item_id','item_name' " +
          "UNION ALL " +
          "SELECT CAST(ItemId AS CHAR), ItemName " +
          "FROM m_item_localize " +
          "INTO OUTFILE '$OutInCont/m_item_localize.csv' " +
          "FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' " +
          "LINES TERMINATED BY '\n';"
$CmdLoc = "mysql -uroot -p`"`$MYSQL_ROOT_PASSWORD`"` $DbName -e `"$SqlLoc`""
ExecInDb $CmdLoc

# ホストへコピー
docker cp "$DbCont:$OutInCont/m_item_group.csv"    "$OutHost/m_item_group.csv"
docker cp "$DbCont:$OutInCont/m_item.csv"          "$OutHost/m_item.csv"
docker cp "$DbCont:$OutInCont/m_item_localize.csv" "$OutHost/m_item_localize.csv"

Write-Host "==> Done. CSVs written to $OutHost"
