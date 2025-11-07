#!/usr/bin/env bash
set -euo pipefail

# ===== Settings =====
DB_CONT="${DB_CONT:-pj_db}"           # docker compose の MySQL コンテナ名
DB_NAME="${DB_NAME:-game}"            # DB名
IN_HOST="../pj-master/csv"            # ホスト側CSVディレクトリ
IN_CONT="/var/lib/mysql-files"        # MySQLのsecure_file_priv
# ====================

echo "==> Importing CSVs into MySQL (${DB_CONT}/${DB_NAME})"

# 入力CSVの存在チェック
for f in m_item_group.csv m_item.csv m_item_localize.csv; do
  if [ ! -f "${IN_HOST}/${f}" ]; then
    echo "ERROR: ${IN_HOST}/${f} が見つかりません"; exit 1
  fi
done

# コンテナへコピー
docker cp "${IN_HOST}/m_item_group.csv"    "${DB_CONT}:${IN_CONT}/m_item_group.csv"
docker cp "${IN_HOST}/m_item.csv"          "${DB_CONT}:${IN_CONT}/m_item.csv"
docker cp "${IN_HOST}/m_item_localize.csv" "${DB_CONT}:${IN_CONT}/m_item_localize.csv"

# 権限付与
docker exec -i "${DB_CONT}" sh -c "chown mysql:mysql ${IN_CONT}/m_item_group.csv ${IN_CONT}/m_item.csv ${IN_CONT}/m_item_localize.csv && chmod 644 ${IN_CONT}/m_item_group.csv ${IN_CONT}/m_item.csv ${IN_CONT}/m_item_localize.csv"


# ヘルパー：mysql を標準入力で実行
mysql_in() {
  docker exec -i "${DB_CONT}" sh -c 'mysql -uroot -p"$MYSQL_ROOT_PASSWORD" '"$DB_NAME"
}

# FK順序：Group -> Item -> Localize
# ステージングへ LOAD DATA → 本テーブルにマージ（INSERT ... ON DUPLICATE KEY UPDATE）

mysql_in <<'SQL'
-- 1) m_item_group
DROP TABLE IF EXISTS _stg_item_group;
CREATE TABLE _stg_item_group(
  item_group_id INT,
  item_group_name VARCHAR(255)
) ENGINE=InnoDB;

LOAD DATA INFILE '/var/lib/mysql-files/m_item_group.csv'
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
SQL

mysql_in <<'SQL'
-- 2) m_item
DROP TABLE IF EXISTS _stg_item;
CREATE TABLE _stg_item(
  item_id INT,
  item_group_id INT,
  rarity INT,
  max_possess_count INT
) ENGINE=InnoDB;

LOAD DATA INFILE '/var/lib/mysql-files/m_item.csv'
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
SQL

mysql_in <<'SQL'
-- 3) m_item_localize
DROP TABLE IF EXISTS _stg_item_localize;
CREATE TABLE _stg_item_localize(
  item_id INT,
  item_name VARCHAR(255)
) ENGINE=InnoDB;

LOAD DATA INFILE '/var/lib/mysql-files/m_item_localize.csv'
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
SQL

# 結果の件数を軽く表示
mysql_in <<'SQL'
SELECT 'm_item_group', COUNT(*) FROM m_item_group
UNION ALL
SELECT 'm_item', COUNT(*) FROM m_item
UNION ALL
SELECT 'm_item_localize', COUNT(*) FROM m_item_localize;
SQL

echo "==> Import finished successfully."
