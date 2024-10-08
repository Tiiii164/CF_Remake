using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    public GameObject target;
    void LateUpdate()
    {
        transform.position = target.transform.position + offset;
    }
}