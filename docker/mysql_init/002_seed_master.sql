-- USE game;

-- INSERT INTO m_item_group(item_group_id, item_group_name, created_at)
-- VALUES
-- (1,'回復薬',NOW()),
-- (2,'武器',NOW())
-- ON DUPLICATE KEY UPDATE item_group_name=VALUES(item_group_name);

-- INSERT INTO m_item(item_id, item_group_id, rarity, max_possess_count, created_at)
-- VALUES
-- (1,1,1,99,NOW()),
-- (2,1,2,99,NOW()),
-- (3,2,5,99,NOW())
-- ON DUPLICATE KEY UPDATE item_group_id=VALUES(item_group_id), rarity=VALUES(rarity), max_possess_count=VALUES(max_possess_count);

-- INSERT INTO m_item_localize(item_id, item_name, created_at)
-- VALUES
-- (1,'きずぐすり',NOW()),
-- (2,'いいきずぐすり',NOW()),
-- (3,'かっこいい剣',NOW())
-- ON DUPLICATE KEY UPDATE item_name=VALUES(item_name);
