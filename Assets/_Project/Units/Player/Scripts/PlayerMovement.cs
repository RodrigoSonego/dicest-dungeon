using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool canMove = true;

    private Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0 && canMove)
        {
            MoveHorizontally(horizontalInput);
        }        
    }

    void MoveHorizontally(float input)
    {
        float x = transform.position.x + (input * speed * Time.deltaTime);

        transform.position = new Vector3(x, transform.position.y, 0);
    }

    public void DisableCollisionAndMovement()
    {
        rigidBody.simulated = false;
        canMove = false;
    }
}
