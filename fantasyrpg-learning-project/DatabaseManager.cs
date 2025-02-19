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
            using (var clearCmd = new NpgsqlCommand("TRUNCATE TABLE items RESTART IDENTITY", conn))
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
        }
    }
    
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
}