namespace fantasyrpg_learning_project.Quests.Models
{
    public interface IObserver
    {
        void Update(string questStatus);
    }

    public interface ISubject
    {
        void RegisterObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers(string questStatus);
    }
}