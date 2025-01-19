using fantasyrpg_learning_project.ItemCreator.Models;

namespace fantasyrpg_learning_project.ItemCreator
{
    public abstract class ItemFacotry
    {
        public abstract Weapon CreateWeapon();
        public abstract UtilityItem CreateUtilityItem();
        public abstract DefensiveItem CreateDefensiveItem();
    }

    public class CommonItemFactory : ItemFacotry
    {
        public override Weapon CreateWeapon()
        {
            return new Weapon("Sword", "Basic sword", RarityEnum.Common, WeaponTypeEnum.Melee, 100);
        }

        public override UtilityItem CreateUtilityItem()
        {
            return new UtilityItem("Healing potion", "Basic healing potion", RarityEnum.Common, 20);
        }

        public override DefensiveItem CreateDefensiveItem()
        {
            return new DefensiveItem("Body armor", "Leather armor", RarityEnum.Common, 50);
        }
    }

    public class MagicItemFactory : ItemFacotry
    {
        public override Weapon CreateWeapon()
        {
            return new Weapon("Elfwood staff", "Magical staff", RarityEnum.Magic, WeaponTypeEnum.Ranged, 150);
        }

        public override UtilityItem CreateUtilityItem()
        {
            throw new NotImplementedException();
        }

        public override DefensiveItem CreateDefensiveItem()
        {
            throw new NotImplementedException();
        }
    }

    public class RareItemFactory : ItemFacotry
    {
        public override Weapon CreateWeapon()
        {
            return new Weapon("Golden bow", "Rare bow", RarityEnum.Rare, WeaponTypeEnum.Ranged, 200);
        }

        public override UtilityItem CreateUtilityItem()
        {
            throw new NotImplementedException();
        }

        public override DefensiveItem CreateDefensiveItem()
        {
            throw new NotImplementedException();
        }
    }

    public class LegendaryItemFactory : ItemFacotry
    {
        public override Weapon CreateWeapon()
        {
            return new Weapon("Punching gloves of the northern star", "Legendary gloves", RarityEnum.Legendary, WeaponTypeEnum.Melee, 9001);
        }

        public override UtilityItem CreateUtilityItem()
        {
            throw new NotImplementedException();
        }

        public override DefensiveItem CreateDefensiveItem()
        {
            throw new NotImplementedException();
        }
    }
}