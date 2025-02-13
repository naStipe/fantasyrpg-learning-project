using fantasyrpg_learning_project.ItemCreator.Models;

namespace fantasyrpg_learning_project;
using Npgsql;


public class DatabaseManager
{
    private const string ConnectionString = "Host=localhost;Port=5432;Database=fantasyrpg;Username=postgres;Password=admin";

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
                using (var cmd = new NpgsqlCommand("INSERT INTO items (name, description, rarity, value, item_type, weapon_type) VALUES (@name, @description, @rarity, @value, @item_type, @weapon_type)", conn))
                {
                    foreach (var item in items)
                    {
                        cmd.Parameters.Clear();
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

        public List<Item> LoadItems()
        {
            List<Item> items = new List<Item>();
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, name, description, rarity, value, item_type, weapon_type FROM items;", conn))
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string name = dr.GetString(1);
                        string description = dr.GetString(2);
                        RarityEnum rarity = Enum.Parse<RarityEnum>(dr.GetString(3));
                        int value = dr.GetInt32(4);
                        ItemTypeEnum itemType = Enum.Parse<ItemTypeEnum>(dr.GetString(5));
                        string weaponTypeStr = dr.IsDBNull(6) ? null : dr.GetString(6);

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

                        items.Add(item);
                    }
                }
                conn.Close();
            }
            return items;
        }

    public void ConnectToPostgres()
    {
        const string connectionString = "Host=localhost;Port=5432;Database=fantasyrpg;Username=postgres;Password=admin";

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            Console.WriteLine("Connection established");
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM weapons;", conn);
            NpgsqlDataReader dr = cmd.ExecuteReader();

            List<Weapon> weapons = LoadGameWorld();
            Console.WriteLine(weapons.Count);

            List<Weapon> newWeapons = new List<Weapon>();
            Weapon newWeapon = new Weapon("name", "description", RarityEnum.Common, WeaponTypeEnum.Melee, 150);
            newWeapons.Add(newWeapon);
            conn.Close();
        }
    }

    public List<Weapon> LoadGameWorld()
    {
        const string connectionString = "Host=localhost;Port=5432;Database=fantasyrpg;Username=postgres;Password=admin";

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            Console.WriteLine("Connection established");
            
            NpgsqlCommand cmdWeapons = new NpgsqlCommand("SELECT * FROM weapons;", conn);
            NpgsqlDataReader dr = cmdWeapons.ExecuteReader();

            List<Weapon> weapons = new List<Weapon>();

            if (dr.HasRows) // Move to the first row
            {
                while (dr.Read())
                {
                    int id = dr.GetInt32(0);
                    string name = dr.GetString(1);
                    string description = dr.GetString(2);
                    if (Enum.TryParse(dr.GetString(3), out RarityEnum rarity))
                    {
                        // Successfully parsed
                    }

                    if (Enum.TryParse(dr.GetString(4), out WeaponTypeEnum type))
                    {
                        // Successfully parsed
                    }

                    int damage = dr.GetInt32(5);

                    weapons.Add(new Weapon(name, description, rarity, type, damage));
                }


                return weapons;
            }
            else
            {
                Console.WriteLine("No rows returned.");
                return weapons;
            }
        }
    }

    public void SaveWeapons(NpgsqlConnection conn, List<Weapon> weapons)
    {
        foreach (var weapon in weapons)
        {
            
        }
        
    }

}