namespace fantasyrpg_learning_project.CharacterCreator.Models
{
    public interface IActionStrategy
    {
        void PerformAction(string characterName);
    }

    public class DefaultAction : IActionStrategy
    {
        public void PerformAction(string characterName)
        {
            Console.WriteLine($"{characterName} is looking around curiously");
        }
    }

    public class MovementAction : IActionStrategy
    {
        public void PerformAction(string characterName)
        {
            Console.WriteLine($"{characterName} is moving around");
        }
    }

    public class AttackAction : IActionStrategy
    {
        public void PerformAction(string characterName)
        {
            Console.WriteLine($"{characterName} is perfroming an attack!");
        }
    }

    public class SpellAction : IActionStrategy
    {
        public void PerformAction(string characterName)
        {
            Console.WriteLine($"{characterName} is casting a magic spell!");
        }
    }

    public class ItemAction : IActionStrategy
    {
        public void PerformAction(string characterName)
        {
            Console.WriteLine($"{characterName} is performing melee attack!");
        }
    }
}