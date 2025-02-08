
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RW.MonumentValley
{
    [RequireComponent(typeof(PlayerAnimation))]
    public class PlayerController : MonoBehaviour
    {

        [Range(0.25f, 2f)]
        [SerializeField] private float moveTime = 0.5f;

        [SerializeField] Cursor cursor;

        private Animator cursorAnimController;

        private Clickable[] clickables;
        private Pathfinder pathfinder;
        private Graph graph;
        private Node currentNode;
        private Node nextNode;

        private bool isMoving;
        private bool isControlEnabled;
        private PlayerAnimation playerAnimation;

        private void Awake()
        {
            clickables = FindObjectsOfType<Clickable>();
            pathfinder = FindObjectOfType<Pathfinder>();
            playerAnimation = GetComponent<PlayerAnimation>();

            if (pathfinder != null)
            {
                graph = pathfinder.GetComponent<Graph>();
            }

            isMoving = false;
            isControlEnabled = true;
        }

        private void Start()
        {
            SnapToNearestNode();

            if (pathfinder != null && !pathfinder.SearchOnStart)
            {
                pathfinder.SetStartNode(transform.position);
            }

            foreach (Clickable c in clickables)
            {
                c.clickAction += OnClick;
            }
        }

        private void OnDisable()
        {
            foreach (Clickable c in clickables)
            {
                c.clickAction -= OnClick;
            }
        }

        private void OnClick(Clickable clickable, Vector3 position)
        {
            if (!isControlEnabled || clickable == null || pathfinder == null)
            {
                return;
            }

            List<Node> newPath = pathfinder.FindBestPath(currentNode, clickable.ChildNodes);

            if (isMoving)
            {
                StopAllCoroutines();
            }

            if (cursor != null)
            {
                cursor.ShowCursor(position);
            }

            if (newPath.Count > 1)
            {
                StartCoroutine(FollowPathRoutine(newPath));
            }
            else
            {
                isMoving = false;
                UpdateAnimation();
            }
        }



        private IEnumerator FollowPathRoutine(List<Node> path)
        {
            // start moving
            isMoving = true;

            if (path == null || path.Count <= 1)
            {
                Debug.Log("PLAYERCONTROLLER FollowPathRoutine: invalid path");
            }
            else
            {
                UpdateAnimation();

                // loop through all Nodes
                for (int i = 0; i < path.Count; i++)
                {
                    // use the current Node as the next waypoint
                    nextNode = path[i];

                    // aim at the Node after that to minimize flipping
                    int nextAimIndex = Mathf.Clamp(i + 1, 0, path.Count - 1);
                    Node aimNode = path[nextAimIndex];
                    FaceNextPosition(transform.position, aimNode.transform.position);

                    // move to the next Node
                    yield return StartCoroutine(MoveToNodeRoutine(transform.position, nextNode));
                }
            }

            isMoving = false;
            UpdateAnimation();

        }

        //  lerp to another Node from current position
        private IEnumerator MoveToNodeRoutine(Vector3 startPosition, Node targetNode)
        {

            float elapsedTime = 0;

            // validate move time
            moveTime = Mathf.Clamp(moveTime, 0.1f, 5f);

            while (elapsedTime < moveTime && targetNode != null && !HasReachedNode(targetNode))
            {

                elapsedTime += Time.deltaTime;
                float lerpValue = Mathf.Clamp(elapsedTime / moveTime, 0f, 1f);

                Vector3 targetPos = targetNode.transform.position;
                transform.position = Vector3.Lerp(startPosition, targetPos, lerpValue);

                // if over halfway, change parent to next node
                if (lerpValue > 0.51f)
                {
                    transform.parent = targetNode.transform;
                    currentNode = targetNode;

                    // invoke UnityEvent associated with next Node
                    targetNode.gameEvent.Invoke();
                    //Debug.Log("invoked GameEvent from targetNode: " + targetNode.name);
                }

                // wait one frame
                yield return null;
            }
        }

        // snap the Player to the nearest Node in Game view
        public void SnapToNearestNode()
        {
            Node nearestNode = graph?.FindClosestNode(transform.position);
            if (nearestNode != null)
            {
                currentNode = nearestNode;
                transform.position = nearestNode.transform.position;
            }
        }

        // turn face the next Node, always projected on a plane at the Player's feet
        public void FaceNextPosition(Vector3 startPosition, Vector3 nextPosition)
        {
            if (Camera.main == null)
            {
                return;
            }

            // convert next Node world space to screen space
            Vector3 nextPositionScreen = Camera.main.WorldToScreenPoint(nextPosition);

            // convert next Node screen point to Ray
            Ray rayToNextPosition = Camera.main.ScreenPointToRay(nextPositionScreen);

            // plane at player's feet
            Plane plane = new Plane(Vector3.up, startPosition);

            // distance from camera (used for projecting point onto plane)
            float cameraDistance = 0f;

            // project the nextNode onto the plane and face toward projected point
            if (plane.Raycast(rayToNextPosition, out cameraDistance))
            {
                Vector3 nextPositionOnPlane = rayToNextPosition.GetPoint(cameraDistance);
                Vector3 directionToNextNode = nextPositionOnPlane - startPosition;
                if (directionToNextNode != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(directionToNextNode);
                }
            }
        }

        // toggle between Idle and Walk animations
        private void UpdateAnimation()
        {
            if (playerAnimation != null)
            {
                playerAnimation.ToggleAnimation(isMoving);
            }
        }

        // have we reached a specific Node?
        public bool HasReachedNode(Node node)
        {
            if (pathfinder == null || graph == null || node == null)
            {
                return false;
            }

            float distanceSqr = (node.transform.position - transform.position).sqrMagnitude;

            return (distanceSqr < 0.01f);
        }

        // have we reached the end of the graph?
        public bool HasReachedGoal()
        {
            if (graph == null)
            {
                return false;
            }
            return HasReachedNode(graph.GoalNode);
        }

        //  enable/disable controls
        public void EnableControls(bool state)
        {
            isControlEnabled = state;
        }
    }
}