using System.Collections.Generic;
using UnityEngine;

namespace RW.MonumentValley
{
    [RequireComponent(typeof(Graph))]
    public class Pathfinder : MonoBehaviour
    {

        [SerializeField] private Node startNode;

        [SerializeField] private Node destinationNode;
        [SerializeField] private bool searchOnStart;

        private List<Node> frontierNodes;

        private List<Node> exploredNodes;

        private List<Node> pathNodes;

        private bool isSearchComplete;

        private bool isPathComplete;

        private Graph graph;

        public Node StartNode { get { return startNode; } set { startNode = value; } }
        public Node DestinationNode { get { return destinationNode; } set { destinationNode = value; } }
        public List<Node> PathNodes => pathNodes;
        public bool IsPathComplete => isPathComplete;
        public bool SearchOnStart => searchOnStart;

        private void Awake()
        {
            graph = GetComponent<Graph>();
        }

        private void Start()
        {
            if (searchOnStart)
            {
                pathNodes = FindPath();
            }
        }

        private void InitGraph()
        {
            if (graph == null || startNode == null || destinationNode == null)
            {
                return;
            }

            frontierNodes = new List<Node>();
            exploredNodes = new List<Node>();
            pathNodes = new List<Node>();

            isSearchComplete = false;
            isPathComplete = false;

            graph.ResetNodes();

            frontierNodes.Add(startNode);
        }

        private void ExpandFrontier(Node node)
        {
            if (node == null)
            {
                return;
            }

            for (int i = 0; i < node.Edges.Count; i++)
            {
                if (node.Edges[i] == null ||
                    node.Edges.Count == 0 ||
                    exploredNodes.Contains(node.Edges[i].neighbor) ||
                    frontierNodes.Contains(node.Edges[i].neighbor))
                {
                    continue;
                }

                if (node.Edges[i].isActive && node.Edges[i].neighbor != null)
                {
                    node.Edges[i].neighbor.PreviousNode = node;

                    frontierNodes.Add(node.Edges[i].neighbor);
                }
            }
        }

        public List<Node> FindPath()
        {
            List<Node> newPath = new List<Node>();

            if (startNode == null || destinationNode == null || startNode == destinationNode)
            {
                return newPath;
            }

            const int maxIterations = 100;
            int iterations = 0;

            InitGraph();

            while (!isSearchComplete && frontierNodes != null && iterations < maxIterations)
            {
                iterations++;

                if (frontierNodes.Count > 0)
                {
                    Node currentNode = frontierNodes[0];
                    frontierNodes.RemoveAt(0);

                    if (!exploredNodes.Contains(currentNode))
                    {
                        exploredNodes.Add(currentNode);
                    }

                    ExpandFrontier(currentNode);

                    if (frontierNodes.Contains(destinationNode))
                    {
                        // generate the Path to the goal
                        newPath = GetPathNodes();
                        isSearchComplete = true;
                        isPathComplete = true;
                    }
                }
                else
                {
                    isSearchComplete = true;
                    isPathComplete = false;
                }
            }
            return newPath;
        }

        public List<Node> FindPath(Node start, Node destination)
        {
            this.destinationNode = destination;
            this.startNode = start;
            return FindPath();
        }

        public List<Node> FindBestPath(Node start, Node[] possibleDestinations)
        {
            List<Node> bestPath = new List<Node>();
            foreach (Node n in possibleDestinations)
            {
                List<Node> possiblePath = FindPath(start, n);

                if (!isPathComplete && isSearchComplete)
                {
                    continue;
                }

                if (bestPath.Count == 0 && possiblePath.Count > 0)
                {
                    bestPath = possiblePath;
                }

                if (bestPath.Count > 0 && possiblePath.Count < bestPath.Count)
                {
                    bestPath = possiblePath;
                }
            }

            if (bestPath.Count <= 1)
            {
                ClearPath();
                return new List<Node>();
            }

            destinationNode = bestPath[bestPath.Count - 1];
            pathNodes = bestPath;
            return bestPath;
        }

        public void ClearPath()
        {
            startNode = null;
            destinationNode = null;
            pathNodes = new List<Node>();
        }

        public List<Node> GetPathNodes()
        {
            List<Node> path = new List<Node>();

            if (destinationNode == null)
            {
                return path;
            }
            path.Add(destinationNode);

            Node currentNode = destinationNode.PreviousNode;

            while (currentNode != null)
            {
                path.Insert(0, currentNode);
                currentNode = currentNode.PreviousNode;
            }
            return path;
        }

        private void OnDrawGizmos()
        {
            if (isSearchComplete)
            {
                foreach (Node node in pathNodes)
                {

                    if (node == startNode)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(node.transform.position, new Vector3(0.25f, 0.25f, 0.25f));
                    }
                    else if (node == destinationNode)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(node.transform.position, new Vector3(0.25f, 0.25f, 0.25f));
                    }
                    else
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(node.transform.position, 0.15f);
                    }

                    Gizmos.color = Color.yellow;
                    if (node.PreviousNode != null)
                    {
                        Gizmos.DrawLine(node.transform.position, node.PreviousNode.transform.position);
                    }
                }
            }
        }

        public void SetStartNode(Vector3 position)
        {
            StartNode = graph.FindClosestNode(position);
        }
    }
}