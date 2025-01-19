using fantasyrpg_learning_project.GameWorldCreator.Models;

namespace fantasyrpg_learning_project.GameWorldCreator
{
    public class GameWorldGenerator
    {
        private static Random random = new Random();

        // Generate new game world map
        public static WorldMap GenerateWorldMap(int width, int height, List<Biome> biomes)
        {
            WorldMap worldMap = new WorldMap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Generate elevation randomly
                    int elevation = random.Next(0, 200);

                    // Determine biome based on elevation
                    Biome selectedBiome = SelectBiome(biomes, elevation);

                    // Create the tile with selected biome and attributes
                    worldMap.Map[x, y] = new Tile(selectedBiome.Name, elevation);
                }
            }

            return worldMap;
        }

        // TODO: Could enhance Biome generation -> e.g. certain biomes can't be connected to each other
        private static Biome SelectBiome(List<Biome> biomes, int elevation)
        {
            foreach (var biome in biomes)
            {
                if (elevation >= biome.MinElevation && elevation <= biome.MaxElevation)
                {
                    return biome;
                }
            }

            // Default biome if no match (you could also throw an exception)
            return biomes[0];
        }

        // Terminal colors for biomes 
        // TODO: Could move to contructor inside Biome class instead
        private static void SetBiomeColor(string biome)
        {
            switch (biome)
            {
                case "Lake":
                    Console.ForegroundColor = ConsoleColor.Blue;  // Blue for Lake
                    break;
                case "Forest":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;  // DarkGreen for Forest
                    break;
                case "Desert":
                    Console.ForegroundColor = ConsoleColor.Yellow;  // Yellow for Desert
                    break;
                case "Mountain":
                    Console.ForegroundColor = ConsoleColor.Gray;  // Gray for Mountain
                    break;
                case "Plains":
                    Console.ForegroundColor = ConsoleColor.Green;  // Green for Plains
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;  // Default to white
                    break;
            }
        }

        // Reset console color to default
        private static void ResetColor()
        {
            Console.ResetColor();
        }

        // Print currently generated map
        public static void PrintMap(WorldMap worldMap)
        {
            // TODO: Could use more sophisticated approach
            for (int y = 0; y < worldMap.Height; y++)
            {
                for (int x = 0; x < worldMap.Width; x++)
                {
                    var tile = worldMap.Map[x, y];
                    SetBiomeColor(tile.Biome);
                    Console.Write(tile.Biome[0] + " ");  // Print the first letter of the biome
                    ResetColor();
                }
                Console.WriteLine();  // New line after each row
            }
        }
    }
}