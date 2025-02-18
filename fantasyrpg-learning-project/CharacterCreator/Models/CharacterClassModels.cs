namespace fantasyrpg_learning_project.CharacterCreator.Models
{
    // Character model for Warrior class
    public class Warrior : Character
    {
        public Warrior(string name, int health = 150, int mana = 50, int strength = 100, int agility = 60) : base(name, health, mana, strength, agility) { }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Warrior: {Name}, Health: {Health}, Strength: {Strength}, Agility: {Agility}");
        }

        public override string GetClassName()
        {
            return "Warrior";
        }
    }

    // Character model for Mage class
    public class Mage : Character
    {
        public Mage(string name, int health = 80, int mana = 200, int strength = 50, int agility = 40) : base(name, health, mana, strength, agility) { }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Mage: {Name}, Health: {Health}, Mana: {Mana}, Agility: {Agility}");
        }
        public override string GetClassName()
        {
            return "Mage";
        }
    }

    // Character model for Archer class
    public class Archer : Character
    {
        public Archer(string name, int health = 100, int mana = 75, int strength = 70, int agility = 90) : base(name, health, mana, strength, agility) { }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Archer: {Name}, Health: {Health}, Agility: {Agility}, Strength: {Strength}");
        }
        public override string GetClassName()
        {
            return "Archer";
        }
    }

    // Character model for NPC
    public class NPC : Character
    {
        public NPC(string name, int health = 50, int mana = 20, int strength = 20, int agility = 20) : base(name, health, mana, strength, agility) { }

        public override void DisplayInfo()
        {
            Console.WriteLine($"NPC: {Name}, Health: {Health}, Agility: {Agility}, Strength: {Strength}");
        }
        public override string GetClassName()
        {
            return "NPC";
        }
    }
}