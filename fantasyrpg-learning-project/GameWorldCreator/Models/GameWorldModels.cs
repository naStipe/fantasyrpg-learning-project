using fantasyrpg_learning_project.CharacterCreator.Models;

namespace fantasyrpg_learning_project.GameWorldCreator.Models
{
    public class GameWorld
    {
        // Static variable that holds the single instance
        private static GameWorld _instance;

        // Object used for locking to avoid threading issues
        private static readonly object lockObj = new object();

        public WorldMap WorldMap { get; private set; }
        public List<Character> WorldCharacters { get; private set; }
        public WorldTimeEnum TimeOfDay { get; set; }

        // Private constructor to prevent external instantiation
        private GameWorld()
        {
            WorldCharacters = new List<Character>();
            TimeOfDay = WorldTimeEnum.Morning; // Default value
        }

        public List<Character> GetCharacters()
        {
            return WorldCharacters;
        }

        // Public static method to access the single instance
        public static GameWorld Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new GameWorld();
                    }
                    return _instance;
                }
            }
        }

        // Initialize the world
        public void InitializeWorld(int width, int height, List<Biome> biomes)
        {
            WorldMap = GameWorldGenerator.GenerateWorldMap(width, height, biomes);
        }

        public void InitializeWorldFromDatabase(int width, int height, string[] mapData)
        {
            WorldMap = GameWorldGenerator.GenerateWorldMapFromDatabase(width, height, mapData);
        }

        // Add a character to the world
        public void AddCharacter(Character character)
        {
            WorldCharacters.Add(character);
        }
        
        public string[] SaveMapToStringArray()
        {
            if (WorldMap == null)
                return new string[0]; // Return an empty array if the map is not initialized

            string[] mapData = new string[WorldMap.Height + 1]; // +1 for storing width & height

            // First element stores the dimensions: "Width:Height"
            mapData[0] = $"{WorldMap.Width}:{WorldMap.Height}";

            for (int y = 0; y < WorldMap.Height; y++)
            {
                List<string> rowTiles = new List<string>();

                for (int x = 0; x < WorldMap.Width; x++)
                {
                    Tile tile = WorldMap.Map[x, y];
                    rowTiles.Add($"{tile.Biome}:{tile.Elevation}"); // Example: "Forest:5"
                }

                mapData[y + 1] = string.Join(",", rowTiles); // Join row elements into a single string
            }
            return mapData;
        }
    }

    // Represents a single tile on the map, which will have properties like biome, elevation, etc.
    public class Tile
    {
        public string Biome { get; set; }
        public int Elevation { get; set; }
        
        public Tile(string biome, int elevation)
        {
            Biome = biome;
            Elevation = elevation;
        }
    }

    // Represents different biomes with their own characteristics.
    public class Biome
    {
        public string Name { get; set; }
        public int MinElevation { get; set; }
        public int MaxElevation { get; set; }

        public Biome(string name, int minElevation, int maxElevation)
        {
            Name = name;
            MinElevation = minElevation;
            MaxElevation = maxElevation;
        }
    }

    // Represents the entire game world, which is a grid of tiles.
    public class WorldMap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Tile[,] Map { get; private set; }

        public WorldMap(int width, int height)
        {
            Width = width;
            Height = height;
            Map = new Tile[width, height];
        }
    }

    // Enumerable for world time
    public enum WorldTimeEnum
    {
        Morning,
        Day,
        Afternoon,
        Night
    }
}