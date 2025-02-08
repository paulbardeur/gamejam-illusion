
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RW.MonumentValley
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private float gizmoRadius = 0.1f;
        [SerializeField] private Color defaultGizmoColor = Color.black;
        [SerializeField] private Color selectedGizmoColor = Color.blue;
        [SerializeField] private Color inactiveGizmoColor = Color.gray;

        [SerializeField] private List<Edge> edges = new List<Edge>();

        [SerializeField] private List<Node> excludedNodes;

        private Graph graph;

        private Node previousNode;

        public UnityEvent gameEvent;

        public Node PreviousNode { get { return previousNode; } set { previousNode = value; } }
        public List<Edge> Edges => edges;

        public static Vector3[] neighborDirections =
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(-1f, 0f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 0f, -1f),
        };
         
        private void Start()
        {
            if (graph != null)
            {
                FindNeighbors();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = defaultGizmoColor;
            Gizmos.DrawSphere(transform.position, gizmoRadius);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = selectedGizmoColor;
            Gizmos.DrawSphere(transform.position, gizmoRadius);

            foreach (Edge e in edges)
            {
                if (e.neighbor != null)
                {
                    Gizmos.color = (e.isActive) ? selectedGizmoColor : inactiveGizmoColor;
                    Gizmos.DrawLine(transform.position, e.neighbor.transform.position);
                }
            }
        }

        public void FindNeighbors()
        {
            foreach (Vector3 direction in neighborDirections)
            {
                Node newNode = graph?.FindNodeAt(transform.position + direction);

                if (newNode != null && !HasNeighbor(newNode) && !excludedNodes.Contains(newNode))
                {
                    Edge newEdge = new Edge { neighbor = newNode, isActive = true };
                    edges.Add(newEdge);
                }
            }
        }

        private bool HasNeighbor(Node node)
        {
            foreach (Edge e in edges)
            {
                if (e.neighbor != null && e.neighbor.Equals(node))
                {
                    return true;
                }
            }
            return false;
        }

        public void EnableEdge(Node neighborNode, bool state)
        {
            foreach (Edge e in edges)
            {
                if (e.neighbor.Equals(neighborNode))
                {
                    e.isActive = state;
                }
            }
        }

        public void InitGraph(Graph graphToInit)
        {
            this.graph = graphToInit;
        }
    }
}