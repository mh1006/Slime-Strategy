using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace SlimeStrategy.BattleMap.Grid.Pathfinding
{
    public class Dijkstra
    {
        private Graph Graph { get; }

        // Paths: (int[] dist, int[] prev)
        public Dictionary<GridSpace, (int[], int[])> Paths { get; }

        public Dijkstra(Graph graph)
        {
            Graph = graph;
            Paths = new Dictionary<GridSpace, (int[], int[])>();
        }

        public void Calculate(GridSpace from)
        {
            if (Paths.ContainsKey(from))
            {
                Debug.LogWarning($"[Dijkstra] <color=red>Pathfinding</color> from {from.Coordinate} when it already existed, skipping...");
                return;
            }
            Debug.Log($"[Dijkstra] <color=red>Pathfinding</color> from {from.Coordinate}");
            if (Graph.Nodes.Count < from.ID)
            {
                Debug.LogError("Tried to pathfind from a non existing node");
                return;
            }
            
            var fromNode = Graph.Nodes[from.ID];
            var visited = new HashSet<Node>();
            int graphSize = Graph.Nodes.Count;
            var dist = new int [graphSize + 1];
            Array.Fill(dist, int.MaxValue);
            var prev = new int [graphSize + 1];
            Array.Fill(prev, -1);
            var priorityQueue = new PriorityQueue<Node, int>();

            // Add initial node to the queue, with cost 0.
            priorityQueue.Enqueue(fromNode, 0);
            // Set its cost to 0
            dist[fromNode.ID] = 0;
            
            while (priorityQueue.Count != 0)
            {
                Node currentNode = priorityQueue.Dequeue();
                if (visited.Contains(currentNode))
                    continue;
                
                visited.Add(currentNode);
                var currentNodeDist = dist[currentNode.ID];
                var edges = currentNode.Edges;
                foreach (var edge in edges)
                {
                    var toNode = edge.Key;
                    if(visited.Contains(toNode)) continue;
                    if (currentNodeDist + edge.Value < dist[toNode.ID])
                    {
                        // Update distance
                        dist[toNode.ID] = currentNodeDist + edge.Value;
                        prev[toNode.ID] = currentNode.ID;
                        priorityQueue.Enqueue(toNode, dist[toNode.ID]);
                    } 
                }
            }
            Paths.Add(from, (dist, prev));
        }

        private void CalculateIfNotExists(GridSpace from)
        {
            if(!Paths.ContainsKey(from)) Calculate(from);
        }

        public int Distance(GridSpace from, GridSpace to)
        {
            CalculateIfNotExists(from);

            return Paths[from].Item1[to.ID];
        }

        public List<GridSpace> Path(GridSpace from, GridSpace to)
        {
            CalculateIfNotExists(from);

            if (!PathExists(from, to)) return null;
            
            var prev = Paths[from].Item2;
            var path = new List<GridSpace>();

            var spaceID = to.ID;
            path.Add(to);
            // While true is safe, Dijkstra cannot return a loop
            while (true)
            {
                spaceID = prev[spaceID];
                if (spaceID == from.ID) break;
                var space = Graph.Nodes[spaceID].GridSpace;
                path.Add(space);
            }

            path.Reverse();
            return path;
        }

        public bool PathExists(GridSpace from, GridSpace to)
        {
            CalculateIfNotExists(from);

            // Check if the prev array entry of the to space is not -1 (path not found)
            return Paths[from].Item2[to.ID] != -1;
        }

        public String DistancesPrettyPrint(GridSpace from)
        {
            return "";
        }

    }
}