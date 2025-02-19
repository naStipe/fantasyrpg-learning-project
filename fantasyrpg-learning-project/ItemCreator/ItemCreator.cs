using fantasyrpg_learning_project.ItemCreator.Models;

namespace fantasyrpg_learning_project.ItemCreator
{
    public abstract class ItemFacotry
    {
        public abstract Weapon CreateWeapon(string name, string description, WeaponTypeEnum weaponTypeEnum, int value);
        public abstract UtilityItem CreateUtilityItem(string name, string description, int value);
        public abstract DefensiveItem CreateDefensiveItem(string name, string description, int value);
        
    }

    public class CommonItemFactory : ItemFacotry
    {
        public override Weapon CreateWeapon(string name, string description, WeaponTypeEnum weaponTypeEnum, int value)
        {
            return new Weapon(name, description, RarityEnum.Common, weaponTypeEnum, value);
        }

        public override UtilityItem CreateUtilityItem(string name, string description, int value)
        {
            return new UtilityItem(name, description, RarityEnum.Common, value);
        }
        

        public override DefensiveItem CreateDefensiveItem(string name, string description, int value)
        {
            return new DefensiveItem(name, description, RarityEnum.Common, value);
        }
    }

    public class MagicItemFactory : ItemFacotry
    {
        public override Weapon CreateWeapon(string name, string description, WeaponTypeEnum weaponTypeEnum, int value)
        {
            return new Weapon(name, description, RarityEnum.Magic, weaponTypeEnum, value);
        }

        public override UtilityItem CreateUtilityItem(string name, string description, int value)
        {
            return new UtilityItem(name, description, RarityEnum.Magic, value);
        }
        

        public override DefensiveItem CreateDefensiveItem(string name, string description, int value)
        {
            return new DefensiveItem(name, description, RarityEnum.Magic, value);
        }
    }

    public class RareItemFactory : ItemFacotry
    {
        public override Weapon CreateWeapon(string name, string description, WeaponTypeEnum weaponTypeEnum, int value)
        {
            return new Weapon(name, description, RarityEnum.Rare, weaponTypeEnum, value);
        }

        public override UtilityItem CreateUtilityItem(string name, string description, int value)
        {
            return new UtilityItem(name, description, RarityEnum.Rare, value);
        }
        

        public override DefensiveItem CreateDefensiveItem(string name, string description, int value)
        {
            return new DefensiveItem(name, description, RarityEnum.Rare, value);
        }
    }

    public class LegendaryItemFactory : ItemFacotry
    {
        public override Weapon CreateWeapon(string name, string description, WeaponTypeEnum weaponTypeEnum, int value)
        {
            return new Weapon(name, description, RarityEnum.Legendary, weaponTypeEnum, value);
        }

        public override UtilityItem CreateUtilityItem(string name, string description, int value)
        {
            return new UtilityItem(name, description, RarityEnum.Legendary, value);
        }
        

        public override DefensiveItem CreateDefensiveItem(string name, string description, int value)
        {
            return new DefensiveItem(name, description, RarityEnum.Legendary, value);
        }
    }
}