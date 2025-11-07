#!/usr/bin/env bash
set -euo pipefail

DB_CONT="${DB_CONT:-pj_db}"
DB_NAME="${DB_NAME:-game}"
OUT_INCONT="/var/lib/mysql-files"
OUT_HOST="../pj-master/csv"

echo "==> Exporting master CSVs from container ${DB_CONT} (db=${DB_NAME})"
mkdir -p "${OUT_HOST}"

# ヘルパー：コンテナ内で mysql を実行（SQL は標準入力で渡す）
mysql_in() {
  docker exec -i "${DB_CONT}" sh -c 'mysql -uroot -p"$MYSQL_ROOT_PASSWORD" '"$DB_NAME"
}

# 既存 CSV を掃除（存在しても失敗しない）
docker exec -i "${DB_CONT}" sh -c "rm -f ${OUT_INCONT}/m_item_group.csv ${OUT_INCONT}/m_item.csv ${OUT_INCONT}/m_item_localize.csv || true"

# 1) m_item_group
mysql_in <<'SQL'
SELECT 'item_group_id','item_group_name'
UNION ALL
SELECT CAST(ItemGroupId AS CHAR), ItemGroupName
FROM m_item_group
INTO OUTFILE '/var/lib/mysql-files/m_item_group.csv'
FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"'
LINES TERMINATED BY '\n';
SQL

# 2) m_item
mysql_in <<'SQL'
SELECT 'item_id','item_group_id','rarity','max_possess_count'
UNION ALL
SELECT CAST(ItemId AS CHAR), CAST(ItemGroupId AS CHAR), CAST(Rarity AS CHAR), CAST(MaxPossessCount AS CHAR)
FROM m_item
INTO OUTFILE '/var/lib/mysql-files/m_item.csv'
FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"'
LINES TERMINATED BY '\n';
SQL

# 3) m_item_localize
mysql_in <<'SQL'
SELECT 'item_id','item_name'
UNION ALL
SELECT CAST(ItemId AS CHAR), ItemName
FROM m_item_localize
INTO OUTFILE '/var/lib/mysql-files/m_item_localize.csv'
FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"'
LINES TERMINATED BY '\n';
SQL

# ホストへコピー
docker cp "${DB_CONT}:${OUT_INCONT}/m_item_group.csv"     "${OUT_HOST}/m_item_group.csv"
docker cp "${DB_CONT}:${OUT_INCONT}/m_item.csv"           "${OUT_HOST}/m_item.csv"
docker cp "${DB_CONT}:${OUT_INCONT}/m_item_localize.csv"  "${OUT_HOST}/m_item_localize.csv"

echo "==> Done. CSVs written to ${OUT_HOST}"
