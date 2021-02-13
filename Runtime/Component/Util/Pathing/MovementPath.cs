using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteAlways]
public class MovementPath : MonoBehaviour
{
    private enum PathType
    {
        Linear,
        Loop
    }

    [SerializeField] private PathType pathType = PathType.Linear;
    [SerializeField] [Range(-1, 1)] private int direction = 1;
    private List<Transform> pathSequence = new List<Transform>();

    private bool hasMoved = false;
    private int moveToNodeIndex = 0;

    public void Restart()
    {
        moveToNodeIndex = 0;
        direction = 1;
    }

    public IEnumerator<Transform> GetNextPathPoint()
    {
        bool validPath = pathSequence != null && pathSequence.Count > 0;
        if (!validPath) {
            yield break;
        }

        while (true) {
            yield return pathSequence[moveToNodeIndex];

            if (pathSequence.Count == 1) {
                continue;
            }

            hasMoved = true;

            moveToNodeIndex += direction;

            if (pathType == PathType.Linear) {
                bool atBoundsOfPath =
                    moveToNodeIndex < 0 ||
                    moveToNodeIndex > pathSequence.Count - 1;

                if (atBoundsOfPath) {
                    direction *= -1;
                }

                // fix overflow/underflow by wrapping inward
                if (moveToNodeIndex < 0) {
                    moveToNodeIndex = 0;
                }
                else if (moveToNodeIndex > pathSequence.Count - 1) {
                    moveToNodeIndex = pathSequence.Count - 1;
                }
            }
            else if (pathType == PathType.Loop) {
                // fix overflow/underflow by wrapping outword
                moveToNodeIndex %= pathSequence.Count;
                if (moveToNodeIndex < 0) {
                    moveToNodeIndex += pathSequence.Count;
                }
            }
        }
    }

    public Transform GetRootNode()
    {
        bool safe =
            pathSequence != null &&
            pathSequence.Count > 0;

        if (safe) {
            return pathSequence[0];
        }
        else {
            return null;
        }
    }

    public bool ReachedEnd()
    {
        if (pathType == PathType.Linear) {
            return hasMoved && moveToNodeIndex == pathSequence.Count - 1;
        }
        else if (pathType == PathType.Loop) {
            return hasMoved && moveToNodeIndex == 0;
        }

        return false;
    }

    public int GetPathSize()
    {
        if (pathSequence != null) {
            return pathSequence.Count;
        }
        return 0;
    }

    private void Start()
    {
        PopulatePathFromChildren();
    }

    private void OnTransformChildrenChanged()
    {
        if (Application.isEditor) {
            PopulatePathFromChildren();
        }
    }

    private void OnDrawGizmos()
    {
        bool validPath = pathSequence != null && pathSequence.Count >= 2;

        if (!validPath) {
            return;
        }

        int pathSize = pathSequence.Count;

        for (int i = 0; i + 1 < pathSize; i++) {
            Gizmos.DrawLine(pathSequence[i].position, pathSequence[i + 1].position);
            Gizmos.DrawSphere(pathSequence[i].position, 0.1f);
            Gizmos.DrawSphere(pathSequence[i + 1].position, 0.1f);
        }

        if (pathType == PathType.Loop) {
            Gizmos.DrawLine(pathSequence[0].position, pathSequence[pathSize - 1].position);
        }
    }

    private void PopulatePathFromChildren()
    {
        pathSequence.Clear();

        foreach(Transform child in gameObject.transform) {
            pathSequence.Add(child);
        }
    }
}
