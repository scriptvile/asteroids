using System.Collections.Generic;
using UnityEngine;

public class AreaCollection : MonoBehaviour
{
    // Inspector
    [SerializeField] List<BoxCollider2D> boxColliders = new List<BoxCollider2D>();

    public Vector2 GenerateRandomPoint()
    {
        if (boxColliders.Count == 0)
        {
            Debug.LogError("There are no box colliders referenced.");
            return Vector2.zero;
        }

        int r = Random.Range(0, boxColliders.Count);
        Collider2D col = boxColliders[r];
        float rX = Random.Range(col.bounds.min.x, col.bounds.max.x);
        float rY = Random.Range(col.bounds.min.y, col.bounds.max.y);
        return new Vector2(rX, rY);
    }
}