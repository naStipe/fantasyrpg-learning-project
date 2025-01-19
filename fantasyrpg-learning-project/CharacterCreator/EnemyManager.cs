using fantasyrpg_learning_project.CharacterCreator.Models;

namespace fantasyrpg_learning_project.CharacterCreator
{
    public class EnemyManager
    {
        private Dictionary<string, IEnemyFactory> _factories = new Dictionary<string, IEnemyFactory>();

        public EnemyManager()
        {
            _factories["Slime"] = new SlimeFactory();
            _factories["Goblin"] = new GoblinFactory();
            _factories["Dragon"] = new DragonFactory();
        }

        public Enemy SpawnEnemy(string enemyType, int rank)
        {
            if (_factories.ContainsKey(enemyType))
            {
                return _factories[enemyType].CreateEnemy(rank);
            }
            throw new ArgumentException("Unknown enemy type.");
        }
    }


}