using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayArea : MonoBehaviour
{
    // Inspector
    [SerializeField] AreaCollection edges;

    // Privates
    BoxCollider2D boxCollider;
    Bounds boundsX;
    Bounds boundsY;

    // Properties
    public Bounds BoundsX { get { return boundsX; } }
    public Bounds BoundsY { get { return boundsY; } }
    public AreaCollection Edges { get { return edges; } }


    #region Methods

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        UpdateBounds();
    }

    void UpdateBounds()
    {
        boundsX = new Bounds(boxCollider.bounds.min.x, boxCollider.bounds.max.x);
        boundsY = new Bounds(boxCollider.bounds.min.y, boxCollider.bounds.max.y);
    }
    #endregion
}