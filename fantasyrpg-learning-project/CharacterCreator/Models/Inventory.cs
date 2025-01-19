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

        public void ListItems()
        {
            Console.WriteLine("Inventory items:");
            foreach (var item in _items)
            {
                Console.WriteLine($"- {item.Name} ({item.ItemType})");
            }
        }

        public bool Contains(Item item) => _items.Contains(item);
    }
}