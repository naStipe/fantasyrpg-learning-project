using fantasyrpg_learning_project.CharacterCreator.Models;

namespace fantasyrpg_learning_project.CharacterCreator
{
    public abstract class CharacterFactory
    {
        public abstract Character CreateCharacter(string name, int health, int mana, int strength, int agility);
        public abstract Character CreateCharacter(string name);
    }

    public class WarriorFactory : CharacterFactory
    {
        private const int DefaultHealth = 150;
        private const int DefaultMana = 50;
        private const int DefaultStrength = 100;
        private const int DefaultAgility = 60;

        // ✅ Overloaded method for default values
        public override Character CreateCharacter(string name)
        {
            return CreateCharacter(name, DefaultHealth, DefaultMana, DefaultStrength, DefaultAgility);
        }

        // ✅ Override that allows full customization
        public override Character CreateCharacter(string name, int health, int mana, int strength, int agility)
        {
            return new Warrior(name, health, mana, strength, agility);
        }
    }

    public class MageFactory : CharacterFactory
    {
        private const int DefaultHealth = 80;
        private const int DefaultMana = 200;
        private const int DefaultStrength = 50;
        private const int DefaultAgility = 40;

        public override Character CreateCharacter(string name)
        {
            return CreateCharacter(name, DefaultHealth, DefaultMana, DefaultStrength, DefaultAgility);
        }

        public override Character CreateCharacter(string name, int health, int mana, int strength, int agility)
        {
            return new Mage(name, health, mana, strength, agility);
        }
    }

    public class ArcherFactory : CharacterFactory
    {
        private const int DefaultHealth = 100;
        private const int DefaultMana = 75;
        private const int DefaultStrength = 70;
        private const int DefaultAgility = 90;

        public override Character CreateCharacter(string name)
        {
            return CreateCharacter(name, DefaultHealth, DefaultMana, DefaultStrength, DefaultAgility);
        }

        public override Character CreateCharacter(string name, int health, int mana, int strength, int agility)
        {
            return new Archer(name, health, mana, strength, agility);
        }
    }

    public class NpcFactory : CharacterFactory
    {
        private const int DefaultHealth = 50;
        private const int DefaultMana = 20;
        private const int DefaultStrength = 20;
        private const int DefaultAgility = 20;
        public override Character CreateCharacter(string name, int health, int mana, int strength, int agility)
        {
            return new NPC(name, health, mana, strength, agility);
        }

        public override Character CreateCharacter(string name)
        {
            return new NPC(name, DefaultHealth, DefaultMana, DefaultStrength, DefaultAgility);
        }
    }
}