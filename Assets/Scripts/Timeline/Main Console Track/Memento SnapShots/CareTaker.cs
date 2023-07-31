using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Timeline.Main_terminal_Track.Memento_SnapShots
{
    public class CareTaker
    {
        private List<ConsoleSnapshot> _mementos = new List<ConsoleSnapshot>();

        private ConsoleManager _originator = null;

        public CareTaker(ConsoleManager originator)
        {
            this._originator = originator;
        }

        public ConsoleSnapshot Backup(string textContent, int visibleCharsAtStart)
        {
            Debug.Log("\nCaretaker: Saving Originator's state...");
          
            var snapshot =  new ConsoleSnapshot(textContent,visibleCharsAtStart);
            this._mementos.Add(snapshot);
            ShowHistory();
            return snapshot;

        }

        public void Revert(int id)
        {
            if (this._mementos.Count == 0 || _mementos.Count < id)
            {
                Debug.Log("Caretaker: Can't revert to id: " + id + " because it doesn't exist");
                return;
            }

            var memento = _mementos.First(m => m.GetId() == id);

            Debug.Log("Caretaker: Restoring state to: " + memento.Print());
            
            this._originator.Restore(memento);
   
        }

        public void ShowHistory()
        {
            Debug.Log("Caretaker: Here's the list of mementos:");

            foreach (var memento in this._mementos)
            {
                Debug.Log(memento.Print() + "\n");
            }
        }
    }
}