using fantasyrpg_learning_project.ItemCreator.Models;

namespace fantasyrpg_learning_project.CharacterCreator.Models
{
    public class Inventory
    {
        private List<Item> _items = new List<Item>();

        public void AddItem(Item item)
        {
            _items.Add(item);
            Console.WriteLine($"{item.Name} added to inventory.");
        }

        public void RemoveItem(Item item)
        {
            if (_items.Contains(item))
            {
                _items.Remove(item);
                Console.WriteLine($"{item.Name} removed from inventory.");
            }
            else
            {
                Console.WriteLine($"{item.Name} not found in inventory.");
            }
        }

        public List<Item> GetItems()
        {
            return _items;
        }

        public bool Contains(Item item) => _items.Contains(item);
    }
}