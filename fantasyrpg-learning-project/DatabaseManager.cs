using fantasyrpg_learning_project.CharacterCreator;
using fantasyrpg_learning_project.CharacterCreator.Models;
using fantasyrpg_learning_project.GameWorldCreator.Models;
using fantasyrpg_learning_project.ItemCreator;
using fantasyrpg_learning_project.ItemCreator.Models;
using Newtonsoft.Json;

namespace fantasyrpg_learning_project;
using Npgsql;


public class DatabaseManager
{
    private const string ConnectionString = "Host=localhost;Port=5432;Database=fantasyrpg;Username=postgres;Password=admin";
    
    // Function to save items to the database
    public void SaveItems(List<Item> items)
    {
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();
            
            // Clearing the table before saving any information from the current session
            using (var clearCmd = new NpgsqlCommand("TRUNCATE TABLE items RESTART IDENTITY CASCADE", conn))
            {
                clearCmd.ExecuteNonQuery();
            }
            
            // Saving all items (weapons, armor, and utilities) to the database
            using (var cmd = new NpgsqlCommand("INSERT INTO items (id, name, description, rarity, value, item_type, weapon_type) VALUES (@id, @name, @description, @rarity, @value, @item_type, @weapon_type)", conn))
            {
                foreach (var item in items)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("id", item.Id);
                    cmd.Parameters.AddWithValue("name", item.Name);
                    cmd.Parameters.AddWithValue("description", item.Description);
                    cmd.Parameters.AddWithValue("rarity", item.Rarity.ToString());
                    cmd.Parameters.AddWithValue("value", item.Value);
                    cmd.Parameters.AddWithValue("item_type", item.ItemType.ToString());
                    
                    if (item is Weapon weapon)
                    {
                        cmd.Parameters.AddWithValue("weapon_type", weapon.WeaponType.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("weapon_type", DBNull.Value);
                    }
                    
                    cmd.ExecuteNonQuery();
                }
            }
            conn.Close();
        }
    }

    // Function to load items from the database
    public List<Item> LoadItems()
    {
        List<Item> items = new List<Item>();
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();

            // Retrieving items from the database
            using (var cmd = new NpgsqlCommand(
                       "SELECT id, name, description, rarity, value, item_type, weapon_type FROM items;", conn))
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    // Reading data from the items table in database and saving them in temporary variables
                    int id = dr.GetInt32(0);
                    string name = dr.GetString(1);
                    string description = dr.GetString(2);
                    RarityEnum rarity = Enum.Parse<RarityEnum>(dr.GetString(3));
                    int value = dr.GetInt32(4);
                    ItemTypeEnum itemType = Enum.Parse<ItemTypeEnum>(dr.GetString(5));
                    string weaponTypeStr = dr.IsDBNull(6) ? null : dr.GetString(6);
                    
                    // Saving data to a new Item
                    Item item;
                    if (itemType == ItemTypeEnum.Weapon && weaponTypeStr != null)
                    {
                        WeaponTypeEnum weaponType = Enum.Parse<WeaponTypeEnum>(weaponTypeStr);
                        item = new Weapon(id, name, description, rarity, weaponType, value);
                    }
                    else if (itemType == ItemTypeEnum.Defensive)
                    {
                        item = new DefensiveItem(name, description, rarity, value);
                    }
                    else
                    {
                        item = new UtilityItem(name, description, rarity, value);
                    }

                    // Adding an item to the list
                    items.Add(item);
                }
            }
            conn.Close();
        }
        // Returning a list of items for the database
        return items;
    }

    // Function to save characters to the database
    public void SaveCharacters(List<Character> characters, List<string> listOfClasses)
    {
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();
                
            // Clearing the table before saving new characters
            using (var clearCmd = new NpgsqlCommand("TRUNCATE TABLE characters RESTART IDENTITY CASCADE", conn))
            {
                clearCmd.ExecuteNonQuery();
            }
            
            // Saving characters
            using (var cmd = new NpgsqlCommand("INSERT INTO characters (id, name, health, mana, strength, agility, class) VALUES (@id, @name, @health, @mana, @strength, @agility, @class)", conn))
            {   
                Console.WriteLine($"Characters: , {characters.Count}, Classes: , {listOfClasses.Count}");
                for (int i = 0; i < characters.Count; i++)
                {
                    // TODO Add the table to the database
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("id", characters[i].Id);
                    cmd.Parameters.AddWithValue("name", characters[i].Name);
                    cmd.Parameters.AddWithValue("health", characters[i].Health);
                    cmd.Parameters.AddWithValue("mana", characters[i].Mana);
                    cmd.Parameters.AddWithValue("strength", characters[i].Strength);
                    cmd.Parameters.AddWithValue("agility", characters[i].Agility);
                    cmd.Parameters.AddWithValue("class", listOfClasses[i]);
                    cmd.ExecuteNonQuery();
                }
            }
            conn.Close();
            
            SaveCharacterInventories(characters);
            SaveCharacterEquipment(characters);
        }
    }
    
    // Function to load characters from the database
    public void LoadCharacters()
    {
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT id, name, health, mana, strength, agility, class FROM characters;", conn))
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    int id = dr.GetInt32(0);
                    string name = dr.GetString(1);
                    int health = dr.GetInt32(2);
                    int mana = dr.GetInt32(3);
                    int strength = dr.GetInt32(4);
                    int agility = dr.GetInt32(5);
                    string classOfCharacter = dr.GetString(6);

                    switch (classOfCharacter)
                    {
                        case "Warrior":
                            WarriorFactory warriorFactory = new WarriorFactory();
                            GameWorld.Instance.AddCharacter(warriorFactory.CreateCharacter(name, health, mana, strength, agility));
                            break;
                        case "Mage":
                            MageFactory mageFactory = new MageFactory();
                            GameWorld.Instance.AddCharacter(mageFactory.CreateCharacter(name, health, mana, strength, agility));
                            break;
                        case "Archer":
                            ArcherFactory archerFactory = new ArcherFactory();
                            GameWorld.Instance.AddCharacter(archerFactory.CreateCharacter(name, health, mana, strength, agility));
                            break;
                        case "NPC":
                            NpcFactory npcFactory = new NpcFactory();
                            GameWorld.Instance.AddCharacter(npcFactory.CreateCharacter(name, health, mana, strength, agility));
                            break;
                        default:
                            throw new Exception("Invalid character type");
                    }
                }
            }
            conn.Close();
        }
    }
    
    // Function to save character inventory to the database
    public void SaveCharacterInventories(List<Character> characters)
    {
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();

            // Clearing the entire inventory table before saving new data
            using (var clearCmd = new NpgsqlCommand("TRUNCATE TABLE character_inventory RESTART IDENTITY CASCADE", conn))
            {
                clearCmd.ExecuteNonQuery();
            }

            // Preparing the insert command
            using (var cmd = new NpgsqlCommand("INSERT INTO character_inventory (character_id, item_id) VALUES (@character_id, @item_id)", conn))
            {
                foreach (var character in characters)
                {
                    List<Item> inventory = character.Inventory.GetItems();
                
                    foreach (var item in inventory)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("character_id", character.Id);
                        cmd.Parameters.AddWithValue("item_id", item.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            conn.Close();
        }
    }
    
    // Function to save character equipment to the database
    public void SaveCharacterEquipment(List<Character> characters)
    {
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();

            // Clearing the entire inventory table before saving new data
            using (var clearCmd = new NpgsqlCommand("TRUNCATE TABLE character_equipment RESTART IDENTITY CASCADE", conn))
            {
                clearCmd.ExecuteNonQuery();
            }

            // Preparing the insert command
            using (var cmd = new NpgsqlCommand("INSERT INTO character_equipment (character_id, item_id, item_slot) VALUES (@character_id, @item_id, @item_slot)", conn))
            {
                foreach (var character in characters)
                {
                    List<Item> equipment = character.GetEquipemt();
                    string[] slots = {"weapon_slot", "defensive_slot", "utility_slot"};
                    for (int i = 0; i < 3; i++)
                    {
                        if (i < equipment.Count && equipment[i] != null) // Ensure equipment[i] is not null
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("character_id", character.Id);
                            cmd.Parameters.AddWithValue("item_id", equipment[i].Id);
                            cmd.Parameters.AddWithValue("item_slot", slots[i]); // Ensure valid slot name
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            conn.Close();
        }
    }
    
    // Function to character inventories biomes from the database
    public void LoadCharacterInventories(List<Character> characters, List<Item> items)
    {
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();
    
            // Retrieve all character-item relations
            using (var cmd = new NpgsqlCommand("SELECT character_id, item_id FROM character_inventory;", conn))
            using (var dr = cmd.ExecuteReader())
            {
                List<(int characterId, int itemId)> inventoryRelations = new List<(int, int)>();
    
                while (dr.Read())
                {
                    int characterId = dr.GetInt32(0);
                    int itemId = dr.GetInt32(1);
                    inventoryRelations.Add((characterId, itemId));
                }
    
                // Now fetch items and add them to characters
                foreach (var relation in inventoryRelations)
                {
                    int characterId = relation.characterId;
                    int itemId = relation.itemId;
    
                    // Find the character object
                    Character character = characters.FirstOrDefault(c => c.Id == characterId);
                    if (character != null)
                    {
                        // Retrieve the item from the passed-in list (instead of querying the database)
                        Item item = items.FirstOrDefault(i => i.Id == itemId);
                        if (item != null)
                        {
                            GameWorld.Instance.WorldCharacters.First(c => c.Id == characterId).Inventory.AddItem(item);
                        }
                    }
                }
            }
            conn.Close();
        }
    }
    
    // Function to load character equipment from the database
    public void LoadCharacterEquipment(List<Character> characters, List<Item> items)
    {
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();

            // Retrieve all character-equipment relations
            using (var cmd = new NpgsqlCommand("SELECT character_id, item_id FROM character_equipment;", conn))
            using (var dr = cmd.ExecuteReader())
            {
                List<(int characterId, int itemId)> equipmentRelations = new List<(int, int)>();

                while (dr.Read())
                {
                    int characterId = dr.GetInt32(0);
                    int itemId = dr.GetInt32(1);
                    equipmentRelations.Add((characterId, itemId));
                }

                // Now fetch items and equip them to characters
                foreach (var relation in equipmentRelations)
                {
                    int characterId = relation.characterId;
                    int itemId = relation.itemId;

                    // Find the character object
                    Character character = characters.FirstOrDefault(c => c.Id == characterId);
                    if (character != null)
                    {
                        // Retrieve the item from the passed-in list
                        Item item = items.FirstOrDefault(i => i.Id == itemId);
                        if (item != null)
                        {
                            GameWorld.Instance.WorldCharacters.First(c => c.Id == characterId).Inventory.AddItem(item);
                            GameWorld.Instance.WorldCharacters.First(c => c.Id == characterId).EquipItem(item);
                        }
                    }
                }
            }
            conn.Close();
        }
    }
    
    // Function to save enemies to the database
    public void SaveEnemies(List<Enemy> enemies)
    {
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();

            // Clearing the entire enemies table before saving new data
            using (var clearCmd = new NpgsqlCommand("TRUNCATE TABLE enemies RESTART IDENTITY CASCADE", conn))
            {
                clearCmd.ExecuteNonQuery();
            }

            // Preparing the insert command
            using (var cmd = new NpgsqlCommand("INSERT INTO enemies (id, name, rank) VALUES (@id, @name, @rank)", conn))
            {
                foreach (var enemy in enemies)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("id", enemy.Id);
                    cmd.Parameters.AddWithValue("name", enemy.Name);
                    cmd.Parameters.AddWithValue("rank", enemy.Rank);
                    cmd.ExecuteNonQuery();
                }
            }
            conn.Close();
        }
    }
    
    // Function to load enemies from the database
    public List<Enemy> LoadEnemies()
    {
        List<Enemy> enemies = new List<Enemy>();
        EnemyManager enemyManager = new EnemyManager();
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();
            // Retrieving enemies from the database
            using (var cmd = new NpgsqlCommand(
                       "SELECT id, name, rank FROM enemies;", conn))
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    // Reading data from the enemies table in database and saving them in temporary variables
                    int id = dr.GetInt32(0);
                    string name = dr.GetString(1);
                    int rank = dr.GetInt32(2);
                    
                    // Adding an enemy to the list
                    enemies.Add(enemyManager.SpawnEnemy(name, rank));
                }
            }
            conn.Close();
        }
        // Returning a list of enemies for the database
        return enemies;
    }
    
    // Function to save biomes to the database
    public void SaveBiomes(List<Biome> biomes)
    {
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();

            // Clearing the entire biomes table before saving new data
            using (var clearCmd = new NpgsqlCommand("TRUNCATE TABLE biomes RESTART IDENTITY CASCADE", conn))
            {
                clearCmd.ExecuteNonQuery();
            }

            // Preparing the insert command
            using (var cmd = new NpgsqlCommand("INSERT INTO biomes (name, min_elevation, max_elevation) VALUES (@name, @min_elevation, @max_elevation)", conn))
            {
                foreach (var biome in biomes)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("name", biome.Name);
                    cmd.Parameters.AddWithValue("min_elevation", biome.MinElevation);
                    cmd.Parameters.AddWithValue("max_elevation", biome.MinElevation);

                    cmd.ExecuteNonQuery();
                }
            }
            conn.Close();
        }
    }
    
    // Function to load biomes from the database
    public List<Biome> LoadBiomes()
    {
        List<Biome> biomes = new List<Biome>();
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();
            // Retrieving biomes from the database
            using (var cmd = new NpgsqlCommand(
                       "SELECT name, min_elevation, max_elevation FROM biomes;", conn))
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    // Reading data from the biomes table in database and saving them in temporary variables
                    string name = dr.GetString(0);
                    int minElevation = dr.GetInt32(1);
                    int maxElevation = dr.GetInt32(2);
                    
                    biomes.Add(new Biome(name, minElevation, maxElevation));
                }
            }
            conn.Close();
        }
        // Returning a list of biomes for the database
        return biomes;
    }
    
    // Fucntion to save the world map to the database
    public void SaveWorldMap()
    {
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();

            // Convert worldMap to a string array using the existing function
            string[] mapData = GameWorld.Instance.SaveMapToStringArray();

            // Extract width and height from the first element ("Width:Height")
            string[] dimensions = mapData[0].Split(':');
            int width = int.Parse(dimensions[0]);
            int height = int.Parse(dimensions[1]);

            // Truncate existing world map data
            using (var clearCmd = new NpgsqlCommand("TRUNCATE TABLE world_map RESTART IDENTITY CASCADE", conn))
            {
                clearCmd.ExecuteNonQuery();
            }

            // Insert new world map data
            using (var cmd = new NpgsqlCommand("INSERT INTO world_map (width, height, map_data) VALUES (@width, @height, @map_data)", conn))
            {
                cmd.Parameters.AddWithValue("width", width);
                cmd.Parameters.AddWithValue("height", height);
                cmd.Parameters.AddWithValue("map_data", mapData); // PostgreSQL array format

                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }
    }
    
    // Function to retrieve the world map from the database
    public (int width, int height, string[] mapData) LoadWorldMap()
    {
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT width, height, map_data FROM world_map LIMIT 1;", conn))
            using (var dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    int width = dr.GetInt32(0);
                    int height = dr.GetInt32(1);
                    string[] mapData = (string[])dr["map_data"]; // Retrieve map_data as a string array

                    conn.Close();
                    return (width, height, mapData);
                }
            }

            conn.Close();
        }

        return (0, 0, new string[0]); // Return default values if no map data is found
    }
    
    // Function to ensure that the tables exist. If they don`t exist, they will be created
    public void EnsureTablesExist()
    {
        using (var conn = new NpgsqlConnection(ConnectionString))
        {
            conn.Open();
            
            // List of tables to check and create if missing
            var tables = new Dictionary<string, string>
            {
                { "characters", @"
                    CREATE TABLE characters (
                        id SERIAL PRIMARY KEY,
                        name TEXT NOT NULL,
                        health INT NOT NULL,
                        mana INT NOT NULL,
                        strength INT NOT NULL,
                        agility INT NOT NULL,
                        class TEXT NOT NULL
                    );" 
                },

                { "items", @"
                    CREATE TABLE items (
                        id SERIAL PRIMARY KEY,
                        name TEXT NOT NULL,
                        description TEXT,
                        rarity TEXT NOT NULL,
                        value INT NOT NULL,
                        item_type TEXT NOT NULL,
                        weapon_type TEXT
                    );"
                },

                { "character_inventory", @"
                    CREATE TABLE character_inventory (
                        character_id INT NOT NULL,
                        item_id INT NOT NULL,
                        PRIMARY KEY (character_id, item_id),
                        FOREIGN KEY (character_id) REFERENCES characters(id) ON DELETE CASCADE,
                        FOREIGN KEY (item_id) REFERENCES items(id) ON DELETE CASCADE
                    );"
                },

                { "character_equipment", @"
                    CREATE TABLE character_equipment (
                        character_id INT NOT NULL,
                        item_id INT NOT NULL,
                        item_slot TEXT NOT NULL,
                        PRIMARY KEY (character_id, item_slot),
                        FOREIGN KEY (character_id) REFERENCES characters(id) ON DELETE CASCADE,
                        FOREIGN KEY (item_id) REFERENCES items(id) ON DELETE CASCADE
                    );"
                },

                { "enemies", @"
                    CREATE TABLE enemies (
                        id SERIAL PRIMARY KEY,
                        name TEXT NOT NULL,
                        rank INT NOT NULL
                    );"
                },

                { "biomes", @"
                    CREATE TABLE biomes (
                        id SERIAL PRIMARY KEY,
                        name TEXT NOT NULL,
                        min_elevation INT NOT NULL,
                        max_elevation INT NOT NULL
                    );"
                },

                { "world_map", @"
                    CREATE TABLE world_map (
                        id SERIAL PRIMARY KEY,
                        width INT NOT NULL,
                        height INT NOT NULL,
                        map_data TEXT[] NOT NULL
                    );"
                }
            };

            foreach (var table in tables)
            {
                // Check if table exists
                using (var checkCmd = new NpgsqlCommand($"SELECT EXISTS (SELECT FROM pg_tables WHERE schemaname = 'public' AND tablename = '{table.Key}');", conn))
                {
                    bool exists = (bool)checkCmd.ExecuteScalar();
                    if (!exists)
                    {
                        // Create table if it does not exist
                        using (var createCmd = new NpgsqlCommand(table.Value, conn))
                        {
                            createCmd.ExecuteNonQuery();
                            Console.WriteLine($"Created missing table: {table.Key}");
                        }
                    }
                }
            }
            
            conn.Close();
        }
    }
}