using fantasyrpg_learning_project.CharacterCreator.Models;

namespace fantasyrpg_learning_project.CharacterCreator
{
    public interface IEnemyFactory
    {
        Enemy CreateEnemy(int rank);
    }

    public class SlimeFactory : IEnemyFactory
    {
        public Enemy CreateEnemy(int rank)
        {
            // Define slime attributes, adjusting based on rank if necessary
            return new Enemy("Slime", health: 50 + rank * 10, mana: 0, strength: 10, agility: 5, rank: rank);
        }
    }

    public class GoblinFactory : IEnemyFactory
    {
        public Enemy CreateEnemy(int rank)
        {
            return new Enemy("Goblin", health: 80 + rank * 15, mana: 10, strength: 15, agility: 20, rank: rank);
        }
    }

    public class DragonFactory : IEnemyFactory
    {
        public Enemy CreateEnemy(int rank)
        {
            return new Enemy("Dragon", health: 500 + rank * 50, mana: 200, strength: 100, agility: 40, rank: rank);
        }
    }

}