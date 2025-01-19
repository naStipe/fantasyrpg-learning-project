using fantasyrpg_learning_project.Quests.Models;

namespace fantasyrpg_learning_project.Quests
{
    public class QuestManager : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();
        private string _questStatus;

        public void RegisterObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers(string questStatus)
        {
            foreach (IObserver observer in _observers)
            {
                observer.Update(questStatus);
            }
        }

        public void UpdateQuestStatus(string status)
        {
            _questStatus = status;
            NotifyObservers(_questStatus);
        }
    }
}