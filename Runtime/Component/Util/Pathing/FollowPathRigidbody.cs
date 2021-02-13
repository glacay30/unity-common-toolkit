using System.Collections.Generic;
using System.Net;
using UnityEngine;

[ExecuteAlways]
public class FollowPathRigidbody : MonoBehaviour, IResettable
{
    private enum MovementType
    {
        ConstantSpeed
    }

    [SerializeField] private MovementPath path = null;
    [SerializeField] private MovementType movementType = MovementType.ConstantSpeed;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private bool stopAtEnd = false;

    private Rigidbody2D rb = null;
    private readonly float EpsilonToTouchNode = 0.1f;
    private IEnumerator<Transform> nodesEnumerator = null;

    public void ResetToInitial()
    {
        ResetState();
    }

    public Vector3 GetNextNode()
    {
        if (nodesEnumerator != null && nodesEnumerator.Current != null) {
            return nodesEnumerator.Current.position;
        }
        return new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    }

    public void ResetState()
    {
        path.Restart();
        nodesEnumerator = path.GetNextPathPoint();
        nodesEnumerator.MoveNext();
        transform.position = nodesEnumerator.Current.position;
    }

    private void Start()
    {
        if (Application.IsPlaying(gameObject)) {
            if (path == null) {
                Debug.LogError("No path to follow.");
                return;
            }

            rb = GetComponent<Rigidbody2D>();

            nodesEnumerator = path.GetNextPathPoint();
            nodesEnumerator.MoveNext();

            transform.position = nodesEnumerator.Current.position;
        }
    }

    private void Update()
    {
        if (!Application.IsPlaying(gameObject)) {
            StickToRoot();
        }
    }

    private void FixedUpdate()
    {
        if (Application.IsPlaying(gameObject)) {
            if (!HasValidPath()) {
                return;
            }

            if (movementType == MovementType.ConstantSpeed) {
                //rb.velocity = Vector2.zero;
                var to = nodesEnumerator.Current.position;
                var pos = transform.position;
                var move = (to - pos).normalized;
                rb.MovePosition(transform.position + move * Time.fixedDeltaTime * speed);
            }

            float sqrDistance = (transform.position - nodesEnumerator.Current.position).sqrMagnitude;
            bool reachedNode = sqrDistance < EpsilonToTouchNode * EpsilonToTouchNode;

            if (reachedNode && stopAtEnd && path.ReachedEnd()) {
                return;
            }
            else if (reachedNode) {
                nodesEnumerator.MoveNext();
            }
        }
    }

    private void StickToRoot()
    {
        if (path != null) {
            Transform root = path.GetRootNode();
            if (root != null) {
                transform.position = root.position;
            }
        }
    }

    private bool HasValidPath()
    {
        bool validPath = nodesEnumerator != null && nodesEnumerator.Current != null;

        if (validPath) {
            return true;
        }

        // try getting the enumator again in case it was lost (e.g. during a hot reload)

        if (path) {
            nodesEnumerator = path.GetNextPathPoint();

            if (nodesEnumerator != null) {
                nodesEnumerator.MoveNext();
            }
        }

        // try checking again
        validPath = nodesEnumerator != null && nodesEnumerator.Current != null;
        return validPath;
    }
}
