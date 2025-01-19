namespace fantasyrpg_learning_project.CharacterCreator.Models
{
    public class Enemy : Character
    {
        public int Rank { get; private set; }

        public Enemy(string name, int health, int mana, int strength, int agility, int rank)
            : base(name, health, mana, strength, agility)
        {
            Rank = rank;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Enemy: {Name}, Health: {Health}, Agility: {Agility}, Strength: {Strength}, Rank: {Rank}");
        }
    }


}