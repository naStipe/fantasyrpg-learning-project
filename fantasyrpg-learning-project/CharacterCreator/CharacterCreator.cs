using fantasyrpg_learning_project.CharacterCreator.Models;

namespace fantasyrpg_learning_project.CharacterCreator
{
    public abstract class CharacterFactory
    {
        public abstract Character CreateCharacter(string name);
    }

    public class WarriorFactory : CharacterFactory
    {
        public override Character CreateCharacter(string name)
        {
            return new Warrior(name);
        }
    }

    public class MageFactory : CharacterFactory
    {
        public override Character CreateCharacter(string name)
        {
            return new Mage(name);
        }
    }

    public class ArcherFactory : CharacterFactory
    {
        public override Character CreateCharacter(string name)
        {
            return new Archer(name);
        }
    }

    public class NpcFactory : CharacterFactory
    {
        public override Character CreateCharacter(string name)
        {
            return new NPC(name);
        }
    }
}