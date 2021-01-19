using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    Animator animator;
    Vector2 velocity = Vector2.zero;
    Vector2 boxColSize;
    Vector2 boxColOffset;

    public CapsuleCollider2D playerCollider;

    public static PlayerMovement instance;
    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerMouvement dans la scéne");
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxColSize = GetComponent<BoxCollider2D>().size;
        boxColOffset = GetComponent<BoxCollider2D>().offset;
    }

    public void Crouch()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        animator.SetBool("crouching", true);
        GetComponent<BoxCollider2D>().offset = new Vector2(GetComponent<BoxCollider2D>().offset.x, -0.7f);
        GetComponent<BoxCollider2D>().size = new Vector2(boxColSize.x, boxColSize.y / 2);
    }

    public void MoveRight(float movementSpeed)
    {
        Standard();

        Vector3 localScale = Vector3.one;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, new Vector2(movementSpeed, rb.velocity.y), ref velocity, 0.05f);
        //transform.rotation = Quaternion.Euler(0, 0, 0);
        localScale.x = 1f;
        transform.localScale = localScale;

        if (!animator.GetBool("isJumping"))
            animator.SetFloat("direction", Mathf.Abs(rb.velocity.x));
    }

    public void MoveLeft(float movementSpeed)
    {
        Standard();

        Vector3 localScale = Vector3.one;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, new Vector2(-movementSpeed, rb.velocity.y), ref velocity, 0.05f);
        //transform.rotation = Quaternion.Euler(0, 180, 0);
        localScale.x = -1f;
        transform.localScale = localScale;

        if(!animator.GetBool("isJumping"))
            animator.SetFloat("direction", Mathf.Abs(rb.velocity.x));
    }

    public void Jump(float jumpVelocity)
    {
        rb.velocity += Vector2.up * jumpVelocity;
        animator.SetBool("isJumping", true);
        animator.SetFloat("direction", 0);
    }

    public void Stay()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
    
    public void Standard()
    {
        animator.SetBool("crouching", false);
        GetComponent<BoxCollider2D>().offset = new Vector2(boxColOffset.x, boxColOffset.y);
        GetComponent<BoxCollider2D>().size = new Vector2(boxColSize.x, boxColSize.y);
    }
}
