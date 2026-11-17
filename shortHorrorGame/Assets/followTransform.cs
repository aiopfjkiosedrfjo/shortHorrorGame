using UnityEngine;

public class followTransform : MonoBehaviour
{
    [SerializeField] private Transform followTransformObject;
    void Update()
    {
        transform.position = followTransformObject.position;
    }
}
