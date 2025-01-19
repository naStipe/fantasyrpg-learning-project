using fantasyrpg_learning_project.CharacterCreator.Models;

namespace fantasyrpg_learning_project.CommandHandler.Models
{
    public class AttackCommand : ICommand
    {
        private readonly Character _character;
        private readonly Character _target;

        public AttackCommand(Character character, Character target)
        {
            _character = character;
            _target = target;
        }

        public void Execute()
        {
            /*
            Console.WriteLine($"{_character.Name} attacks {_target.Name}!");
            _target.Health -= _character.Strength;
            */
            _character.SetAction(new AttackAction());
            _character.PerformAction();
        }
    }

    public class DefendCommand : ICommand
    {
        private readonly Character _character;

        public DefendCommand(Character character)
        {
            _character = character;
        }

        public void Execute()
        {
            Console.WriteLine($"{_character.Name} is defending!");
            _character.SetState(new DefendingState()); // Use the state pattern for defending state
        }
    }

    public class HealCommand : ICommand
    {
        private readonly Character _character;
        private readonly int _healingAmount;

        public HealCommand(Character character, int healingAmount)
        {
            _character = character;
            _healingAmount = healingAmount;
        }

        public void Execute()
        {
            Console.WriteLine($"{_character.Name} heals for {_healingAmount} points!");
            _character.Health += _healingAmount;
        }
    }

    public class MoveCommand : ICommand
    {
        private readonly Character _character;

        public MoveCommand(Character character)
        {
            _character = character;
        }

        public void Execute()
        {
            Console.WriteLine($"{_character.Name} is moving!");
        }
    }
    

    public class ChangeStateCommand : ICommand
    {
        private readonly Character _character;
        private ICharacterState _characterState;

        public ChangeStateCommand(Character character)
        {
            _character = character;
        }

        public void Execute()
        {
            Console.WriteLine($"Changing character {_character.Name} state");
            Console.WriteLine($"Press '1' for IdleState, '2' for ActionState, '3' for DefendingState. Press '0' to cancel");

            var stateChoice = Console.ReadKey();

            switch (stateChoice.KeyChar)
            {
                case '1':
                    _characterState = new IdleState();
                    break;
                case '2':
                    _characterState = new ActionState();
                    break;
                case '3':
                    _characterState = new DefendingState();
                    break;

                default: 
                    return;
            }

            _character.SetState( _characterState );

            Console.WriteLine($"{_character.Name} is changing its state to {_characterState}!");
        }
    }
}