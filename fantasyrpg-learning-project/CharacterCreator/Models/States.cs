namespace fantasyrpg_learning_project.CharacterCreator.Models
{
    public interface ICharacterState
    {
        void HandleState(string characterName, IActionStrategy actionStrategy);

        bool CanUpdateAction(string characterName, IActionStrategy actionStrategy);
    }

    public class IdleState : ICharacterState
    {
        public void HandleState(string characterName, IActionStrategy actionStrategy)
        {
            Console.WriteLine($"{characterName} is in idle state.");
            actionStrategy.PerformAction(characterName);
        }

        public bool CanUpdateAction(string characterName, IActionStrategy actionStrategy)
        {
            return true;
        }
    }

    public class ActionState : ICharacterState
    {
        public void HandleState(string characterName, IActionStrategy actionStrategy)
        {
            Console.WriteLine($"{characterName} is in action mode.");
            actionStrategy.PerformAction(characterName);
        }

        public bool CanUpdateAction(string characterName, IActionStrategy actionStrategy)
        {
            return true;
        }
    }

    public class DefendingState : ICharacterState
    {
        public void HandleState(string characterName, IActionStrategy actionStrategy)
        {
            Console.WriteLine($"{characterName} is defending.");
        }

        public bool CanUpdateAction(string characterName, IActionStrategy actionStrategy)
        {
            Console.WriteLine($"{characterName} is currently in defeding state and cannot change it's action to {actionStrategy}.");
            return false;
        }
    }
}