using fantasyrpg_learning_project.CharacterCreator.Models;
using fantasyrpg_learning_project.CharacterCreator;
using fantasyrpg_learning_project.GameWorldCreator.Models;
using fantasyrpg_learning_project.GameWorldCreator;
using fantasyrpg_learning_project.CommandHandler;
using fantasyrpg_learning_project.ItemCreator;
using fantasyrpg_learning_project.ItemCreator.Models;

namespace fantasyrpg_learning_project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize the game world instace
            var gameWorld = GameWorld.Instance;
            
            // Create character factories
            CharacterFactory warriorFactory = new WarriorFactory();
            CharacterFactory mageFactory = new MageFactory();
            CharacterFactory archerFactory = new ArcherFactory();
            NpcFactory npcFactory = new NpcFactory();
            
            // Create item factories
            ItemFacotry commonItemFactory = new CommonItemFactory();
            ItemFacotry rareItemFactory = new RareItemFactory();
            ItemFacotry magicItemFactory = new MagicItemFactory();
            ItemFacotry legendaryItemFactory = new LegendaryItemFactory();
            
            // Create an enemy manager
            EnemyManager enemyManager = new EnemyManager();
            
            // Create a database manager
            DatabaseManager databaseManager = new DatabaseManager();

            // Check if tables exist and create those that are missing
            databaseManager.EnsureTablesExist();
            
            // Load all the data from the database
            List<Item> items = databaseManager.LoadItems();
            var biomes = new List<Biome>(databaseManager.LoadBiomes());
            (int width, int height, string[] mapData) = databaseManager.LoadWorldMap();
            gameWorld.InitializeWorldFromDatabase(width, height, mapData);
            databaseManager.LoadCharacters();
            List<Character> characters = gameWorld.GetCharacters();
            databaseManager.LoadCharacterInventories(characters, items);
            databaseManager.LoadCharacterEquipment(characters, items);
            List<Enemy> enemies = databaseManager.LoadEnemies();
            
            // Define biomes, if the database is empty
            if (biomes.Count == 0)
            {
                biomes = new List<Biome>
                {
                    new Biome("Lake", 0, 10),
                    new Biome("Desert", 10, 25),
                    new Biome("Forest", 25, 100),
                    new Biome("Plains", 100, 150),
                    new Biome("Mountain", 150, 200)
                };
            }
            
            // Generate the world map, if it doesn`t exist in the database
            if (mapData == null || mapData.Length == 0)
            {
                gameWorld.InitializeWorld(10, 10, biomes);
            }
            
            // Create characters, if they do not exist in the database
            if (gameWorld.WorldCharacters.Count == 0)
            {
                gameWorld.AddCharacter(warriorFactory.CreateCharacter("Conan"));
                gameWorld.AddCharacter(mageFactory.CreateCharacter("Merlin"));
                gameWorld.AddCharacter(archerFactory.CreateCharacter("Legolas"));
            }
            
            foreach (var character in gameWorld.WorldCharacters)
            {
                character.DisplayInfo();
            }
            
            // Get conan the warrior
            Character conan = gameWorld.WorldCharacters.First(character => character.Name == "Conan");
            
            // Enemy creation
            if (enemies.Count == 0)
            {
                enemies.Add(enemyManager.SpawnEnemy("Slime", 1));
            }
            enemies[0].DisplayInfo();

            // Print the generated map 
            GameWorldGenerator.PrintMap(gameWorld.WorldMap);


            // Initialize gamecontroller
            GameController controller = new GameController(conan, enemies[0]);

            Console.WriteLine("Press 'A' to Attack, 'D' to Defend, 'H' to Heal, 'M' to Move, 'S' to change state. Press 'Q' to quit.");

            
            
            while (true)
            {
                // Capture keyboard input
                ConsoleKey key = Console.ReadKey(intercept: true).Key;

                if (key == ConsoleKey.Q)
                {
                    Console.WriteLine("Saving game data...");
                    
                    // Save all the data to the database
                    databaseManager.SaveItems(items);
                    List<string> listOfClasses = new List<string>();
                    foreach (var character in characters)
                    {
                        listOfClasses.Add(character.GetClassName());
                    }
                    databaseManager.SaveCharacters(characters, listOfClasses);
                    databaseManager.SaveCharacterInventories(characters);
                    databaseManager.SaveCharacterEquipment(characters);
                    databaseManager.SaveEnemies(enemies);
                    databaseManager.SaveBiomes(biomes);
                    databaseManager.SaveWorldMap();
                    
                    Console.WriteLine("Exiting game...");
                    break;
                }

                // Handle the input using the controller
                controller.HandleInput(key);
            }
        }
    }
}