using System.Collections;
using System.Collections.Generic;
using Story.Terminal.ContentCreation.Terminal_Operations;

namespace Story.Terminal.System
{
    public class ProgramCommandQueue
    {
        private readonly LinkedList<IEnumerator> _commandList = new();

        public bool IsEmpty => _commandList.Count == 0;

        public void Reset()
        {
            _commandList.Clear();
        }

        public void PushLast(IEnumerator coroutine)
        {
            _commandList.AddLast(coroutine);
        }
        public void PushFirst(IEnumerator coroutine)
        {
            _commandList.AddFirst(coroutine);
        }
        

        public IEnumerator PopFirst()
        {
            var first = _commandList.First.Value;
            _commandList.RemoveFirst();
            return first;
        }
    }
}