namespace BehaviorTree
{
    public class BehaviourTree : Node
    {
        public BehaviourTree(string name) : base(name)
        {

        }

        public override Status Process()
        {
            while (currentChildIndex < children.Count) 
            { 
                var status = children[currentChildIndex].Process();
                if (status != Status.Success)
                {
                    return status;
                }
                currentChildIndex++;
            }
            return Status.Success;
        }
    }
}
