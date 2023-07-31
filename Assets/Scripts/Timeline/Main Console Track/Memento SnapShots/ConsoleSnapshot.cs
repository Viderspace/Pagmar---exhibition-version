namespace Runtime.Timeline.Main_terminal_Track.Memento_SnapShots
{
    public class ConsoleSnapshot
    {
        private static int serialNo = 0;
        private int _id;
        private string _updatedText;
        private int _visibleCountOnEnter;

        public ConsoleSnapshot(string updatedText, int visibleCountOnEnter)
        {
            _id = serialNo++;
            _updatedText = updatedText;
            _visibleCountOnEnter = visibleCountOnEnter;
        }

        public string GetUpdatedText()
        {
            return _updatedText;
        }

        public int GetVisibleCountOnEnter()
        {
            return _visibleCountOnEnter;
        }

        public int GetId()
        {
            return _id;
        }
        
        public string Print()
        {
            return $"Snapshot {_id}:  Text: {_updatedText}, Visible count on enter: {_visibleCountOnEnter}";
        }
        
    }
}