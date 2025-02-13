namespace fantasyrpg_learning_project.ItemCreator.Models
{
    public abstract class Item
    {
        private static int nextId = 1;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RarityEnum Rarity { get; set; }
        public int Value { get; set; }

        public ItemTypeEnum ItemType { get; set; }
        
        public Item(string name, string description, RarityEnum rarity) {
            Id = nextId++;
            Name = name;
            Description = description;
            Rarity = rarity;
        }

        public abstract void DisplayInfo();
    }

    public class Weapon : Item
    {
        public WeaponTypeEnum WeaponType { get; set; }

        public Weapon(string name, string description, RarityEnum rarity, WeaponTypeEnum type, int value) : base(name, description, rarity) { 
            WeaponType = type;
            Value = value;
            ItemType = ItemTypeEnum.Weapon;
        }
        public Weapon(int id, string name, string description, RarityEnum rarity, WeaponTypeEnum type, int value) : base(name, description, rarity) { 
            WeaponType = type;
            Value = value;
            ItemType = ItemTypeEnum.Weapon;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"{Name} is a weapon of {Rarity} rarity. Description: {Description}");
        }
    }

    public class UtilityItem : Item
    {
        public UtilityItem(string name, string description, RarityEnum rarity, int value) : base(name, description, rarity) 
        {
            Value = value;
            ItemType = ItemTypeEnum.Utility;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"{Name} is a UtilityItem of {Rarity} rarity. Description: {Description}");
        }
    }

    public class DefensiveItem : Item
    {
        public DefensiveItem(string name, string description, RarityEnum rarity, int value) : base(name, description, rarity) 
        {
            Value = value;
            ItemType = ItemTypeEnum.Defensive;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"{Name} is a DefensiveItem of {Rarity} rarity. Description: {Description}");
        }
    }

    public enum RarityEnum
    {
        Common,
        Magic,
        Rare,
        Legendary
    }

    public enum WeaponTypeEnum
    {
        Melee,
        Ranged
    }

    public enum ItemTypeEnum
    {
        Weapon,
        Defensive,
        Utility
    }
}