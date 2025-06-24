using UnityEngine;
using System.Collections.Generic;
using BehaviorTree.Interfaces;
namespace BehaviorTree
{
    public class Node
    {
        
        public enum Status { Running, Success, Failure }
      
        public readonly string name;
        public readonly List<Node> children = new List<Node>();
        protected int currentChildIndex;

        public Node(string name = "Node")
        {
            this.name = name;
        }
        public void AddChild(Node child)
        {
            children.Add(child);
        }
        public virtual Status Process() => children[currentChildIndex].Process();

        public virtual void Reset()
        {
            currentChildIndex = 0;
            foreach (var child in children)
            {
                child.Reset();
            }
        }
    }
}
