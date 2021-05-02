using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Inspector
    [SerializeField] float rotationSpeed;


    #region Methods

    void OnEnable()
    {
        int r = Random.Range(0, 2);
        if (r == 1) rotationSpeed = rotationSpeed * -1;
    }

    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0,0, rotationSpeed));
    }
    #endregion
}