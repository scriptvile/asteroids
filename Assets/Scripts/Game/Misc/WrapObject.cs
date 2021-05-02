using UnityEngine;

public class WrapObject : MonoBehaviour
{
    Bounds boundsX;
    Bounds boundsY;

    protected void InitBounds()
    {
        boundsX = Game.i.PlayArea.BoundsX;
        boundsY = Game.i.PlayArea.BoundsY;
    }

    protected void WrapCheck()
    {
        if (transform.position.x > boundsX.max) transform.position = new Vector2(boundsX.min, transform.position.y); 
        if (transform.position.x < boundsX.min) transform.position = new Vector2(boundsX.max, transform.position.y);
        if (transform.position.y > boundsY.max) transform.position = new Vector2(transform.position.x, boundsY.min);
        if (transform.position.y < boundsY.min) transform.position = new Vector2(transform.position.x, boundsY.max);   
    }
}