using System.Collections.Generic;
using UnityEngine;

public static class FindHelper<T>
{
    public static List<T> FindAllThatAreVisible(int layerMask = ~0)
    {
        Camera camera = Camera.main;

        Vector2 halfExtents = new Vector2()
        {
            x = camera.orthographicSize * camera.aspect,
            y = camera.orthographicSize
        };

        Vector2 topLeft = new Vector2()
        {
            x = camera.transform.position.x - halfExtents.x,
            y = camera.transform.position.y + halfExtents.y
        };

        Vector2 botRight = new Vector2()
        {
            x = camera.transform.position.x + halfExtents.x,
            y = camera.transform.position.y - halfExtents.y
        };

        Collider2D[] hits = Physics2D.OverlapAreaAll(topLeft, botRight, layerMask);

        List<T> results = new List<T>();

        foreach (var hit in hits) {
#pragma warning disable UNT0014 // Invalid type for call to GetComponent
            T[] children = hit.GetComponentsInChildren<T>();
#pragma warning restore UNT0014 // Invalid type for call to GetComponent
            results.AddRange(children);
        }

        return results;
    }
}
