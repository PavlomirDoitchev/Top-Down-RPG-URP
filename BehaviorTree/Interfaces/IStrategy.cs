using BehaviorTree;

namespace BehaviorTree.Interfaces
{
    public interface IStrategy
    {
        Node.Status Process();
        void Reset();
    }
}
