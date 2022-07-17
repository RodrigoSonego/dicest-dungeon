using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;

    public bool willFollow = true;
    private Vector3 currentVelocity;

    public static PlayerCamera instance;

    void OnEnable()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        if (willFollow == false) { return; }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 0.1f, smoothSpeed);

        transform.position = smoothedPosition;
    }
}
