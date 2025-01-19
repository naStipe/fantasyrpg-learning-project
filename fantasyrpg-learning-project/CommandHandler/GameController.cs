using fantasyrpg_learning_project.CharacterCreator.Models;
using fantasyrpg_learning_project.CommandHandler.Models;

namespace fantasyrpg_learning_project.CommandHandler
{
    public class GameController
    {
        private readonly Dictionary<ConsoleKey, ICommand> _keyMappings = new Dictionary<ConsoleKey, ICommand>();

        public GameController(Character character, Character target)
        {
            // Initialize key mappings for character commands
            _keyMappings[ConsoleKey.A] = new AttackCommand(character, target);
            _keyMappings[ConsoleKey.D] = new DefendCommand(character);
            _keyMappings[ConsoleKey.H] = new HealCommand(character, 20);
            _keyMappings[ConsoleKey.M] = new MoveCommand(character);
            _keyMappings[ConsoleKey.S] = new ChangeStateCommand(character); 
        }

        public void HandleInput(ConsoleKey key)
        {
            if (_keyMappings.ContainsKey(key))
            {
                _keyMappings[key].Execute();
            }
            else
            {
                Console.WriteLine("Invalid key pressed. No action mapped to this key.");
            }
        }

        // Method for custom key mappings
        public void SetKeyMapping(ConsoleKey key, ICommand command)
        {
            _keyMappings[key] = command;
        }
    }
}