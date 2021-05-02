using UnityEngine;

public class LifeUI : MonoBehaviour, IPoolable
{
    // Privates
    RectTransform rt;


    #region Methods

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    void Start()
    {
        FixRendering();
    }

    public void Spawn() { }

    public void Despawn()
    {
        Pool_LifeUI.i.ReturnToPool(this);
    }

    void FixRendering()
    {
        transform.localScale = Vector2.one;
        rt.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }
    #endregion
}