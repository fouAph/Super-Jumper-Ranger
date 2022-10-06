using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    private CharacterController2D controller2D;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashingCooldownTime;
    private bool canDashing;

    Transform playerTransform;
    Rigidbody2D rb;
    private int direction;
    [SerializeField] private float targetDashPositionX;
    private float dashingTimer;
    [SerializeField] private bool isDashing;
    [SerializeField] private float offsetY = .3f;

    private void Awake()
    {
        controller2D = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();


        dashingTimer = dashingCooldownTime;

        if (transform.position.x >= playerTransform.position.x)
        {
            targetDashPositionX = transform.position.x;

        }
        else if (transform.position.x <= playerTransform.position.x)
        {
            targetDashPositionX = Mathf.Abs(transform.position.x);
        }
        isDashing = false;
    }

    private void Update()
    {
        if (dashingTimer >= 0 && !isDashing)
            dashingTimer -= Time.deltaTime;

        // canDashing = (controller2D.collisions.below && dashingTimer<=0);
        canDashing = (!isDashing && controller2D.collisions.below && dashingTimer <= 0);
        isDashing = (rb.velocity.magnitude > 0);

        if (canDashing)
        {
            DashAttack();


        }
        else if (playerTransform)
            FollowPlayerY();

        if (direction == -1 && transform.position.x <= targetDashPositionX && isDashing
                                                  || direction == 1 && transform.position.x >= targetDashPositionX && isDashing)
        {
            rb.velocity = Vector3.zero;
         StartCoroutine(SetFacingDirection());
        }
    }

    public void FollowPlayerY()
    {
        var tr = transform.position;
        // transform.position = new Vector3(transform.position.x,  Mathf.Lerp(playerTransform.position.y,playerTransform.position.y, moveSpeed), transform.position.z);

        if (playerTransform.position.y <= transform.position.y + offsetY)
            transform.Translate(0, -moveSpeed * Time.deltaTime, 0);
        else if (playerTransform.position.y >= transform.position.y + offsetY)
            transform.Translate(0, moveSpeed * Time.deltaTime, 0);
    }

    public void DashAttack()
    {

        isDashing = true;
        // dashPositionX = transform.position.x;
        SetDashDirection();
        // ((Vector2.right * direction) * dashSpeed)
        rb.velocity = new Vector3(dashSpeed * direction, 0, 0);


         dashingTimer = dashingCooldownTime;
    }

    private void SetDashDirection()
    {


        if (transform.position.x >= playerTransform.position.x)
        {
            direction = -1;
            targetDashPositionX *= -1;
        }
        else if (transform.position.x <= playerTransform.position.x)
        {
            direction = 1;
            targetDashPositionX = Mathf.Abs(targetDashPositionX);
        }
        // StartCoroutine(SetFacingDirection());
    }

    IEnumerator SetFacingDirection()
    {
        var tr = transform.localScale;
        yield return new WaitForSeconds(.1f);
        if (rb.velocity.x >= 0.01f)
            tr.x =  Mathf.Abs(tr.x);
    else tr.x = -tr.x;
        transform.localScale = tr;
    }


}
