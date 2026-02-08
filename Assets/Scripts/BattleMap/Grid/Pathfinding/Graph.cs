using System.Collections.Generic;

namespace SlimeStrategy.BattleMap.Grid.Pathfinding
{
    public class Graph
    {
        private List<Node> _nodes;

        public List<Node> Nodes => _nodes;

        public Graph(int size)
        {
            _nodes = new List<Node>(size + 1);
            for (int i = 0; i < size; i++)
            {
                _nodes.Add(new Node(i));
            }
        }

        public void AssociateSpace(GridSpace space)
        {
            _nodes[space.ID].GridSpace = space;
        }

        public void MakeEdge(int from, int to)
        {
            // Debug.Log($"Edge {from}, {to}");
            // Debug.Log($"# of nodes = {_nodes.Count}");
            var fromNode = _nodes[from];
            var toNode = _nodes[to];
            
            fromNode.Edges.Add(toNode, 1);
        }
    }

    public class Node
    {
        public int ID { get; }
        public GridSpace GridSpace { get; set; }

        public Dictionary<Node, int> Edges { get; }

        public Node(int id)
        {
            this.ID = id;
            Edges = new Dictionary<Node, int>();
        }
    }
}