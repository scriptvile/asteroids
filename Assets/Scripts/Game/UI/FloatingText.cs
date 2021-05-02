using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour, IPoolable
{
    // Enums
    public enum Size { Small, Normal }

    // Inspector
    [SerializeField] TMP_Text t_text;


    #region Methods

    void Start()
    {
        transform.localScale = Vector3.one;
    }

    public void Spawn()
    {
        transform.position = Camera.main.ScreenToWorldPoint(transform.position);
    }

    public void Despawn()
    {
        Pool<FloatingText>.i.ReturnToPool(this);
    }

    public void SetText(string text, Color color, Size size)
    {
        t_text.text = text;
        t_text.color = color;

        switch (size)
        {
            case Size.Small: t_text.fontSize = 20;
                break;
            case Size.Normal: t_text.fontSize = 30;
                break;
            default: Debug.LogError("Unexpected Size");
                break;
        }
    }
    #endregion
}