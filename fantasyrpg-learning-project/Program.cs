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
            
            DatabaseManager databaseManager = new DatabaseManager();
            List<Item> items = databaseManager.LoadItems();
            // Initialize the game world instace
            var gameWorld = GameWorld.Instance;

            // Define biomes
            var biomes = new List<Biome>
            {
                new Biome("Lake", 0, 10),
                new Biome("Desert", 10, 25),
                new Biome("Forest", 25, 100),
                new Biome("Plains", 100, 150),
                new Biome("Mountain", 150, 200)
            };

            // Generate the world map
            gameWorld.InitializeWorld(10, 10, biomes);
            databaseManager.LoadCharacters();
            // if (gameWorld.WorldCharacters.Count == 0)
            // {
            //     // Create characters using the factory pattern
            //     CharacterFactory warriorFactory = new WarriorFactory();
            //     CharacterFactory mageFactory = new MageFactory();
            //     CharacterFactory archerFactory = new ArcherFactory();
            //
            //     // Display character information
            //     gameWorld.AddCharacter(warriorFactory.CreateCharacter("Conan"));
            //     gameWorld.AddCharacter(mageFactory.CreateCharacter("Merlin"));
            //     gameWorld.AddCharacter(archerFactory.CreateCharacter("Legolas"));
            // }
            
            // Add NPCs to the game world
            // TODO: Extend NPC class -> Jobs, Skills, Inventory, Roles
            // NpcFactory npcFactory = new NpcFactory();

            // gameWorld.AddCharacter(npcFactory.CreateCharacter("Villager"));
            // gameWorld.AddCharacter(npcFactory.CreateCharacter("Villager"));
            
            foreach (var character in gameWorld.WorldCharacters)
            {
                character.DisplayInfo();
            }

            // Item creation examples
            ItemFacotry commonItemFactory = new CommonItemFactory();
            
            // Weapon commonWeapon = commonItemFactory.CreateWeapon();
            // DefensiveItem commonArmor = commonItemFactory.CreateDefensiveItem();
            // UtilityItem commonPotion = commonItemFactory.CreateUtilityItem();


            // Get conan the warrior
            Character conan = gameWorld.WorldCharacters.First(character => character.Name == "Conan");

            // Add items to character's inventory
            // conan.Inventory.AddItem(commonWeapon);
            // conan.Inventory.AddItem(commonArmor);
            // conan.Inventory.AddItem(commonPotion);
            
            // conan.Inventory.AddItem(items[0]);
            // conan.Inventory.AddItem(items[1]);
            // conan.Inventory.AddItem(items[2]);

            // List inventory items
            conan.Inventory.ListItems();

            // Equip items
            // conan.EquipItem(commonWeapon);
            // conan.EquipItem(commonArmor);
            // conan.EquipItem(commonPotion);
            
            // conan.EquipItem(items[0]);
            // conan.EquipItem(items[1]);
            // conan.EquipItem(items[2]);

            // Show currently equipped items
            conan.ShowEquipment();
            List<Character> characters = gameWorld.GetCharacters();
            // Console.WriteLine("Character`s 1 items: {0}", characters[0].Inventory.ListItems().Count);
            // characters[0].ShowEquipment();
            
            // Enemy creation
            EnemyManager enemyManager = new EnemyManager();

            Enemy slime = enemyManager.SpawnEnemy("Slime", 1);
            slime.DisplayInfo();
            
            /*
            QuestManager questManager = new QuestManager();

            questManager.RegisterObserver(conan);

            // Quest started
            questManager.UpdateQuestStatus("Quest started: Retrieve the lost artifact");

            // Quest completed
            questManager.UpdateQuestStatus("Quest completed: Artifact retrieved!");
            */


            // Print the generated map 
            GameWorldGenerator.PrintMap(gameWorld.WorldMap);


            // Initialize gamecontroller
            GameController controller = new GameController(conan, slime);

            Console.WriteLine("Press 'A' to Attack, 'D' to Defend, 'H' to Heal, 'M' to Move, 'S' to change state. Press 'Q' to quit.");

            databaseManager.SaveItems(items);
            List<string> listOfClasses = new List<string>();
            foreach (var character in characters)
            {
                listOfClasses.Add(character.GetClassName());
            }
            databaseManager.SaveCharacters(characters, listOfClasses);
            
            while (true)
            {
                // Capture keyboard input
                ConsoleKey key = Console.ReadKey(intercept: true).Key;

                if (key == ConsoleKey.Q)
                {
                    Console.WriteLine("Exiting game...");
                    break;
                }

                // Handle the input using the controller
                controller.HandleInput(key);
            }

        }
    }
}