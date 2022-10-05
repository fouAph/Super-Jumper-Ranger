using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform footPos;
    public Transform currentPlayer;
    public LayerMask whatIsRaycastable;

    private bool facingRight;
    private bool jumpReady = true;
    private bool moving;
    protected bool winding;

    //Grounded stuff
    private bool grounded;
    public LayerMask whatIsGround;

    //variables
    public float moveSpeed = 2700f;
    public float jumpForce = 850f;
    private float windupSpeed = 3f;

    private int currentJumps;
    public int maxJumps = 2;

    private Rigidbody2D rb;
    public float maxMoveSpeed = 14;

    private Vector2 standardScale;
    //Sprites and stuff
    private SpriteRenderer sprite;
    protected Color actorColor;

    protected void Start()
    {
        currentJumps = maxJumps;
        sprite = GetComponent<SpriteRenderer>();
        facingRight = true;
        rb = GetComponent<Rigidbody2D>();
        standardScale = transform.localScale;
    }

    private void Update()
    {
        if (currentPlayer == null) return;

        Vector2 playerPos = currentPlayer.transform.position;
        Vector2 lookPos = playerPos - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, lookPos, 100, whatIsRaycastable);

        float playerX = playerPos.x;
        Behaviour(playerX, hit);
    }

    private void FixedUpdate()
    {
        SlowDown();
    }

    private void LateUpdate()
    {
        grounded = Physics2D.OverlapCircle(footPos.position, 0.7f, whatIsGround);
        if (currentJumps < maxJumps && grounded)
            currentJumps = maxJumps;

        if (transform.localScale.y < standardScale.y)
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y + 0.03f);
        if (transform.localScale.x < standardScale.x)
            transform.localScale = new Vector2(transform.localScale.x + 0.03f, transform.localScale.y);
    }

    protected void Move(int dir)
    {
        //flip player
        // FlipActor(dir);

        //Actually move player
        moving = true;

        float xVel = rb.velocity.x;

        if (xVel < maxMoveSpeed && dir > 0)
            rb.AddForce(moveSpeed * Time.deltaTime * Vector2.right * dir);
        else if (xVel > -maxMoveSpeed && dir < 0)
            rb.AddForce(moveSpeed * Time.deltaTime * Vector2.right * dir);

        if (dir == 0)
        {

        }

        //If player is turning around, help turn faster
        if (xVel > 0.2f && dir < 0)
            rb.AddForce(moveSpeed * 3.2f * Time.deltaTime * -Vector2.right);
        if (xVel < 0.2f && dir > 0)
        {
            rb.AddForce(moveSpeed * 3.2f * Time.deltaTime * Vector2.right);
        }
    }

    protected virtual void Behaviour(float playerX, RaycastHit2D hit2D)
    {
        if (playerX > transform.position.x)
            Move(1);
        else Move(-1);

        if (Random.Range(0, 1f) < 0.0025)
            Jump();

    }

    private void SlowDown()
    {
        if (moving) return;

        //If no key pressed but still moving, slow down player
        if (rb.velocity.x > 0.2f)
            rb.AddForce(moveSpeed * Time.deltaTime * -Vector2.right);
        else if (rb.velocity.x < -0.2f)
            rb.AddForce(moveSpeed * Time.deltaTime * Vector2.right);
    }

    protected void StopMoving()
    {
        moving = false;
    }

    private void FlipActor(int dir)
    {
        if ((facingRight && dir < 0) || !facingRight && dir > 0)
        {
            facingRight = !facingRight;
            sprite.flipX = !sprite.flipX;
        }
    }

    protected void Jump()
    {
        if (currentJumps <= 1) return;

        float angleMod = transform.rotation.eulerAngles.z % 180;

        if (angleMod < 45 || angleMod > 135)
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y / 4);
        else
            transform.localScale = new Vector2(transform.localScale.x / 4, transform.localScale.y);

        //Spawn jump fx
        // Instantiate(jumpFx, transform.position, transform.rotation);

        //Play jump sound
        // AudioManager.Play("Jump");

        rb.velocity = new Vector2(rb.velocity.x, 0);

        rb.AddForce(Vector2.up * jumpForce);
        currentJumps--;
        winding = false;
    }

    private void JumpReset()
    {
        currentJumps = maxJumps;
    }
}
