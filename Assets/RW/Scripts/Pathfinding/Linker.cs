using UnityEngine;

namespace RW.MonumentValley
{
    [System.Serializable]
    public class RotationLink
    {
        public Transform linkedTransform;

        public Vector3 activeEulerAngle;
        [Header("Nodes to activate")]
        public Node nodeA;
        public Node nodeB;
    }

    public class Linker : MonoBehaviour
    {
        [SerializeField] public RotationLink[] rotationLinks;

        public void EnableLink(Node nodeA, Node nodeB, bool state)
        {
            if (nodeA == null || nodeB == null)
                return;

            nodeA.EnableEdge(nodeB, state);
            nodeB.EnableEdge(nodeA, state);
        }

        public void UpdateRotationLinks()
        {
            foreach (RotationLink l in rotationLinks)
            {
                if (l.linkedTransform == null || l.nodeA == null || l.nodeB == null)
                    continue;

                Quaternion targetAngle = Quaternion.Euler(l.activeEulerAngle);
                float angleDiff = Quaternion.Angle(l.linkedTransform.rotation, targetAngle);

                if (Mathf.Abs(angleDiff) < 0.01f)
                {
                    EnableLink(l.nodeA, l.nodeB, true);
                }
                else
                {
                    EnableLink(l.nodeA, l.nodeB, false);
                }
            }
        }

        private void Start()
        {
            UpdateRotationLinks();
        }
    }
}