using UnityEngine;

public class PlayerMovement : MonoBehaviour
{ 
    public float movementSpeed = 4.5f;
    
    private Rigidbody2D playerRigidbody;
    private Vector2 movementDirection;
    private bool playerFacingRight = true;
    Animator animator;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (PlayerPrefs.GetInt("maxSpeed") != 0)
        {
            movementSpeed = PlayerPrefs.GetFloat("maxSpeed");
        }
    }

    void Update()
    {
        movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        HandleFlip(Input.GetAxisRaw("Horizontal"));
        PlayerPrefs.SetFloat("maxSpeed", movementSpeed);
    }

    void FixedUpdate()
    {
        playerRigidbody.velocity = movementDirection * movementSpeed;
        Animation(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void Animation(float h, float v)
    {
        bool walking = h!=0f || v!=0f;
        animator.SetBool("IsWalking", walking);
    }

    void HandleFlip(float horizontal)
    {
        if (horizontal > 0 && !playerFacingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && playerFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        playerFacingRight = !playerFacingRight;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    public void IncreaseSpeed(float amount)
    {
        movementSpeed += amount;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("maxSpeed");
    }
}
