using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandMade : MonoBehaviour
{
    public PlayerMovement playerTarget;
    public Transform footPos;

    private bool facingRight;
    private bool jumpReady = true;
    private bool moving;

    //Grounded stuff
    private bool grounded;
    public LayerMask whatIsGround;

    //variables
    public float moveSpeed = 2700f;
    public float jumpForce = 850f;

    private int currentJumps;
    public int maxJumps = 2;

    //limit
    public float maxMoveSpeed = 14;

    //Rb
    private Rigidbody2D rb;

    //Sprites and stuff
    private SpriteRenderer sprite;

    //standards
    private Vector2 standardScale;
    public float rayDistance;
    public bool blocked;
    private void Start()
    {
        currentJumps = maxJumps;
        facingRight = true;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = true;
        standardScale = transform.localScale;
    }

    private void Update()
    {
        // var raycastLeft = CreateRaycast(transform.position, Vector3.down )
        Ray rayRightDown = new Ray(transform.position + Vector3.right, -transform.up);
        Ray rayLeftDown = new Ray(transform.position + -Vector3.right, -transform.up);
        Ray rayUp = new Ray(transform.position + (-Vector3.right * .5f), transform.up);

        Debug.DrawRay(rayRightDown.origin, rayRightDown.direction * rayDistance, Color.yellow);
        Debug.DrawRay(rayLeftDown.origin, rayLeftDown.direction * rayDistance, Color.yellow);
        Debug.DrawRay(rayUp.origin, rayUp.direction * rayDistance, Color.yellow);

        var raycastRight = Physics2D.Raycast(rayRightDown.origin, rayRightDown.direction, rayDistance, whatIsGround);
        var raycastleft = Physics.Raycast(rayLeftDown.origin, rayLeftDown.direction, rayDistance, whatIsGround);
        var raycastUp = Physics2D.Raycast(rayLeftDown.origin, rayUp.direction, rayDistance, whatIsGround);


        Vector2 playerPos = playerTarget.transform.position;

        Vector2 lookPos = playerPos - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;

        Debug.DrawRay(rayUp.origin, rayUp.direction * rayDistance, Color.yellow);

        if (raycastUp)
        {
            blocked = true;

        }

        else
            blocked = false;


        if (blocked)
        {
            if (playerPos.x > transform.position.x)
                rb.AddForce(moveSpeed * Time.deltaTime * Vector2.right * 1);
            // else
            //     MoveHorizontal(-1);
        }
        else
            MoveNormal(playerPos);

        Debug.DrawRay(transform.position, lookPos, Color.yellow);

        // if (raycastRight)
        // {
        //     print(raycastRight.collider.name);
        // }




    }

    private void MoveHorizontal(int dir)
    {
        FlipActor(dir);

        moving = true;

        float xVel = rb.velocity.x;
        if (xVel < maxMoveSpeed && dir > 0)
            rb.AddForce(moveSpeed * Time.deltaTime * Vector2.right * dir);
        else if (xVel > -maxMoveSpeed && dir < 0)
            rb.AddForce(moveSpeed * Time.deltaTime * Vector2.right * dir);
    }

    private void MoveNormal(Vector3 playerPos)
    {
        if (playerPos.x > transform.position.x)
            Move(1);
        else Move(-1);

    }

    private void FixedUpdate()
    {
        if (moving) return;

        SlowDown();
    }

    public void Move(int dir)
    {
        FlipActor(dir);
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

    private void FlipActor(int dir)
    {
        if ((facingRight && dir < 0) || !facingRight && dir > 0)
        {
            facingRight = !facingRight;
            sprite.flipX = !sprite.flipX;
        }
    }

    public void CreateRaycast(Vector3 origin, Vector3 direction, float distance, LayerMask layer)
    {
        Physics.Raycast(origin, direction, distance, layer);
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
}
