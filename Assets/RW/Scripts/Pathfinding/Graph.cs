using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RW.MonumentValley
{
    public class Graph : MonoBehaviour
    {
        private List<Node> allNodes = new List<Node>();

        [SerializeField] private Node goalNode;
        public Node GoalNode => goalNode;

        private void Awake()
        {
            allNodes = FindObjectsOfType<Node>().ToList();
            InitNodes();
        }

        private void Start()
        {
            InitNeighbors();
        }

        public Node FindNodeAt(Vector3 pos)
        {
            foreach (Node n in allNodes)
            {
                Vector3 diff = n.transform.position - pos;

                if (diff.sqrMagnitude < 0.01f)
                {
                    return n;
                }
            }
            return null;
        }

        public Node FindClosestNode(Node[] nodes, Vector3 pos)
        {
            Node closestNode = null;
            float closestDistanceSqr = Mathf.Infinity;

            foreach (Node n in nodes)
            {
                Vector3 diff = n.transform.position - pos;

                Vector3 nodeScreenPosition = Camera.main.WorldToScreenPoint(n.transform.position);
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(pos);
                diff = nodeScreenPosition - screenPosition;

                if (diff.sqrMagnitude < closestDistanceSqr)
                {
                    closestNode = n;
                    closestDistanceSqr = diff.sqrMagnitude;
                }
            }
            return closestNode;
        }

        public Node FindClosestNode(Vector3 pos)
        {
            return FindClosestNode(allNodes.ToArray(), pos);
        }

        public void ResetNodes()
        {
            foreach (Node node in allNodes)
            {
                node.PreviousNode = null;
            }
        }

        private void InitNodes()
        {
            foreach (Node n in allNodes)
            {
                if (n != null)
                {
                    n.InitGraph(this);
                }
            }
        }

        private void InitNeighbors()
        {
            foreach (Node n in allNodes)
            {
                if (n != null)
                {
                    n.FindNeighbors();
                }
            }
        }
    }

}