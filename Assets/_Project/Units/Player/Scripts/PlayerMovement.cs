using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer sprite;

    private bool canMove = true;

    private Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput != 0 && canMove)
        {
            MoveHorizontally(horizontalInput);
            Player.instance.SetAnimatorWalk(true);
        }
        else
        {
            Player.instance.SetAnimatorWalk(false);
        }
    }

    void MoveHorizontally(float input)
    {
        float x = transform.position.x + (input * speed * Time.deltaTime);

        sprite.flipX = input < 0;

        transform.position = new Vector3(x, transform.position.y, 0);
    }

    public void DisableCollisionAndMovement()
    {
        rigidBody.simulated = false;
        canMove = false;
    }

    public void EnableCollisionAndMovement()
    {
        rigidBody.simulated = true;
        canMove = true;
    }
}
