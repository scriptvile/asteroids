using UnityEngine;

public class BoosterEffectUI : MonoBehaviour
{
    // Privates
    Animator animator;


    #region Methods

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Show()
    {
        animator.Play("show", 0, 0);
        animator.SetBool("isHidden", false);
    }

    public void Hide()
    {
        animator.SetBool("isHidden", true);
    }
    #endregion
}