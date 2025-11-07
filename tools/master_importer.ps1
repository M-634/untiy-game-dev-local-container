Param(
    [string]$DbCont = "pj_db",
    [string]$DbName = "game",
    [string]$InHost = "..\pj-master\csv",
    [string]$InCont = "/var/lib/mysql-files"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Write-Host "==> Importing CSVs into MySQL ($DbCont/$DbName)"

$files = @("m_item_group.csv", "m_item.csv", "m_item_localize.csv")
foreach ($f in $files) {
    $path = Join-Path $InHost $f
    if (-not (Test-Path $path)) {
        throw "CSV not found: $path"
    }
}

# コンテナへコピー
docker cp (Join-Path $InHost "m_item_group.csv")    "$DbCont:$InCont/m_item_group.csv"
docker cp (Join-Path $InHost "m_item.csv")          "$DbCont:$InCont/m_item.csv"
docker cp (Join-Path $InHost "m_item_localize.csv") "$DbCont:$InCont/m_item_localize.csv"

# パーミッション調整（mysqlユーザで読めるように）
docker exec -i $DbCont sh -c "chown mysql:mysql $InCont/m_item_group.csv $InCont/m_item.csv $InCont/m_item_localize.csv && chmod 644 $InCont/*.csv"

function MySqlIn([string]$Sql) {
    $cmd = "mysql -uroot -p`"`$MYSQL_ROOT_PASSWORD`"` $DbName -e `"$Sql`""
    docker exec -i $DbCont sh -c "$cmd"
}

# 1) m_item_group
$Sql1 = @"
DROP TABLE IF EXISTS _stg_item_group;
CREATE TABLE _stg_item_group(
  item_group_id INT,
  item_group_name VARCHAR(255)
) ENGINE=InnoDB;

LOAD DATA INFILE '$InCont/m_item_group.csv'
INTO TABLE _stg_item_group
FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"'
LINES TERMINATED BY '\n'
IGNORE 1 LINES
(item_group_id, item_group_name);

INSERT INTO m_item_group (ItemGroupId, ItemGroupName)
SELECT item_group_id, item_group_name FROM _stg_item_group
ON DUPLICATE KEY UPDATE
  ItemGroupName = VALUES(ItemGroupName);

DROP TABLE _stg_item_group;
"@
MySqlIn $Sql1

# 2) m_item
$Sql2 = @"
DROP TABLE IF EXISTS _stg_item;
CREATE TABLE _stg_item(
  item_id INT,
  item_group_id INT,
  rarity INT,
  max_possess_count INT
) ENGINE=InnoDB;

LOAD DATA INFILE '$InCont/m_item.csv'
INTO TABLE _stg_item
FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"'
LINES TERMINATED BY '\n'
IGNORE 1 LINES
(item_id, item_group_id, rarity, max_possess_count);

INSERT INTO m_item (ItemId, ItemGroupId, Rarity, MaxPossessCount)
SELECT item_id, item_group_id, rarity, max_possess_count FROM _stg_item
ON DUPLICATE KEY UPDATE
  ItemGroupId = VALUES(ItemGroupId),
  Rarity = VALUES(Rarity),
  MaxPossessCount = VALUES(MaxPossessCount);

DROP TABLE _stg_item;
"@
MySqlIn $Sql2

# 3) m_item_localize
$Sql3 = @"
DROP TABLE IF EXISTS _stg_item_localize;
CREATE TABLE _stg_item_localize(
  item_id INT,
  item_name VARCHAR(255)
) ENGINE=InnoDB;

LOAD DATA INFILE '$InCont/m_item_localize.csv'
INTO TABLE _stg_item_localize
FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"'
LINES TERMINATED BY '\n'
IGNORE 1 LINES
(item_id, item_name);

INSERT INTO m_item_localize (ItemId, ItemName)
SELECT item_id, item_name FROM _stg_item_localize
ON DUPLICATE KEY UPDATE
  ItemName = VALUES(ItemName);

DROP TABLE _stg_item_localize;
"@
MySqlIn $Sql3

# 件数確認
$CheckSql = @"
SELECT 'm_item_group', COUNT(*) FROM m_item_group
UNION ALL
SELECT 'm_item', COUNT(*) FROM m_item
UNION ALL
SELECT 'm_item_localize', COUNT(*) FROM m_item_localize;
"@
MySqlIn $CheckSql

Write-Host "==> Import finished successfully."
