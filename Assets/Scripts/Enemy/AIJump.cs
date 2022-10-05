using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AIJump : MonoBehaviour
{
    public List<Vector3> overrideTargetPosition;
    public List<Vector3> savedClosest = new List<Vector3>();
    public Transform targetTransform;

    public float moveSpeed = 2f;
    public float jumpHeight = 15f;

    public Transform groundCheckTranform;
    public LayerMask groundLayer;
    // public Transform groundCheck;


    public float stopDistance = 1f;                                     //Max jarak antara player dan musuh, bisa digunakan untuk minimal attack range
    public float overrideTargetPositionStopDistance = 1.3f;
    //
    public float shouldJumpVertical = 1f;                                          //minimal jarak Y dari enemy ke target untuk bisa melompat
    public float shouldGoingDownHorizontal = 2f;                                          //minimal jarak X dari enemy ke target untuk bisa melompat
    public float minDistanceToJump = 5;
    public bool useShouldJump;                                      //minimal jarak untuk melakukan lompatan
    public bool shouldJump;
    public bool useShouldGoingDown;
    public bool shouldGoingDown;
    [SerializeField] bool hasJump;
    [SerializeField] bool canJump;
    public bool useLowAngle;                                                     //jika true maka akan dilakukan kalkulasi lompatan yang lebih tinggi

    public bool jumpPressed;
    public float rbVelMagnitude;

    public float sideWallCheckRayLength = 5;
    public float groundCheckRayLength = 9;


    [Header("Distances")]
    [SerializeField] float distanceFromFootToGround;
    [SerializeField] float YDiferenceFromTarget;
    [SerializeField] float XDiferenceFromTarget;
    [SerializeField] float XDiferenceFromTargetOverrideTarget;
    [SerializeField] float distanceFromTarget;

    [Header("Debug variable"), Space(10)]
    bool flipDirection;
    bool hasSpawnPrefab;
    Rigidbody2D rb;

    public Transform debugColliderPrefab;

    Collider2D currentCollider;
    Transform mouseGO;

    //Ray for Enemy
    RaycastHit2D upperEnemyTransformRay;
    RaycastHit2D downEnemyTransformRay;
    //Raydown for target
    RaycastHit2D downTargetRay;

    //ray for Platform
    RaycastHit2D leftRay;
    RaycastHit2D RightRay;
    RaycastHit2D downRightRay;
    RaycastHit2D downLeftRay;
    [SerializeField] bool drawGizmos;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        overrideTargetPosition = new List<Vector3>();
        mouseGO = new GameObject("MouseTarget").transform;

    }

    private void LateUpdate()
    {
        rbVelMagnitude = rb.velocity.magnitude;
        FlipDirection();
        CalculateDistanceToTarget();
        CalculateYDifference();
        CalculateXDifference();
        IsGrounded();

        MoveToTarget();

        CheckIfCanJump();

        if (overrideTargetPosition.Count > 0)
            ResetOverridePositionWhenArrived();


        CheckIfTargetUnreachable();

        if (useShouldJump)
            if (CheckIfWeShouldJump() && IsGrounded() && !hasJump)
            {
                if (targetTransform)
                {
                    Collider2D previousCollider;
                    previousCollider = currentCollider;
                    var downHit = Physics2D.Raycast(targetTransform.position, Vector3.down, 2f, groundLayer);
                    //Checking side of the platform
                    if (downHit)
                    {
                        currentCollider = downHit.collider;
                    }

                    if (previousCollider != currentCollider)
                    {
                        hasJump = false;
                        CheckSideWallAndGroundToGoingUp(currentCollider);
                    }

                    float? angle = RotatePlayerAngle();
                    if (canJump)
                        Jump(angle);
                }

            }
        if (useShouldGoingDown)
            if (CheckIfWeSholdGoingdDown() && IsGrounded())
            {
                if (targetTransform)
                {
                    Collider2D previousCollider;
                    previousCollider = currentCollider;
                    downEnemyTransformRay = Physics2D.Raycast(groundCheckTranform.position, Vector2.down, .5f, groundLayer);
                    //Checking side of the platform
                    if (downEnemyTransformRay)
                    {
                        currentCollider = downEnemyTransformRay.collider;
                    }

                    if (previousCollider != currentCollider)
                    {
                        CheckGroundToGoingDown(currentCollider);
                    }
                }
            }

        CalculateFootDistanceToGround();

        // // CheckIfEnemyNeedToJump();

        // if (distanceFromTarget < maxDistanceFromTarget)
        // {
        //     ResetTargetRoutine();
        // }

        // CheckShouldJump(); Movement();

        // SetTargetWithMouse();
    }

    private void CheckIfTargetUnreachable()
    {
        //    if(targetTransform.)
    }

    private void CalculateDistanceToTarget()
    {
        if (overrideTargetPosition.Count > 0)
            distanceFromTarget = Vector2.Distance(transform.position, overrideTargetPosition[0]);

        else if (targetTransform)
            distanceFromTarget = Vector2.Distance(transform.position, targetTransform.position);

        else if (!targetTransform || overrideTargetPosition.Count > 0 && IsGrounded())
            rb.velocity = Vector3.zero;
    }

    private void CalculateFootDistanceToGround()
    {
        bool hasCalculate = false;
        if (IsGrounded())
        {
            if (hasCalculate == false)
            {
                var groundHit = Physics2D.Raycast(groundCheckTranform.position, Vector3.down, groundLayer);
                if (groundHit)
                {
                    distanceFromFootToGround = Mathf.Abs(groundCheckTranform.position.y - groundHit.point.y);
                }
                hasCalculate = true;
            }

        }
        else
            hasCalculate = false;
    }

    private void CalculateXDifference()
    {
        if (overrideTargetPosition.Count > 0)
            XDiferenceFromTargetOverrideTarget = Math.Abs(transform.position.x - overrideTargetPosition[0].x);
        if (targetTransform)
            XDiferenceFromTarget = Math.Abs(transform.position.x - targetTransform.position.x);

    }

    private void CalculateYDifference()
    {
        if (overrideTargetPosition.Count > 0)
            YDiferenceFromTarget = transform.position.y - overrideTargetPosition[0].y;
        else if (targetTransform)
            YDiferenceFromTarget = transform.position.y - targetTransform.position.y;

    }

    private bool CheckIfWeShouldJump()
    {
        return shouldJump = YDiferenceFromTarget <= -shouldJumpVertical;
    }

    private bool CheckIfWeSholdGoingdDown()
    {
        return shouldGoingDown = XDiferenceFromTarget <= shouldGoingDownHorizontal && YDiferenceFromTarget > shouldJumpVertical;
    }

    private bool CheckifShouldDeleteTargetOverrideValues()
    {
        return XDiferenceFromTarget <= shouldGoingDownHorizontal && YDiferenceFromTarget <= stopDistance;
    }

    private void ResetOverridePositionWhenArrived()
    {
        if (distanceFromTarget <= overrideTargetPositionStopDistance)
        {
            overrideTargetPosition.RemoveAt(0);
            // overrideTargetPositionIndex++;
        }
    }

    private void CheckIfCanJump()
    {
        upperEnemyTransformRay = Physics2D.Raycast(transform.position, Vector2.up, 4f, groundLayer);
        if (upperEnemyTransformRay)
        {
            canJump = false;
        }
        else canJump = true;
    }

    public void CheckIfEnemyNeedToJump()
    {
        if (targetTransform)
        {
            Collider2D previousCollider;
            previousCollider = currentCollider;
            var downHit = Physics2D.Raycast(targetTransform.position, Vector3.down, 2f, groundLayer);
            if (downHit)
            {
                currentCollider = downHit.collider;
            }

            if (previousCollider != currentCollider)
            {
                CheckSideWallAndGroundToGoingUp(currentCollider);
                hasSpawnPrefab = false;
                print("Not same col");
            }
        }
    }

    public void CheckSideWallAndGroundToGoingUp(Collider2D collider2D)
    {
        // Vector2? groundTargetPosition = null;
        var colTopRight = GetTopRightColliderPosition(collider2D);
        var colTopLeft = GetTopLeftColliderPosition(collider2D);
        RightRay = Physics2D.Raycast(GetTopRightColliderPosition(collider2D, -.2f, -.5f), Vector2.right, sideWallCheckRayLength, groundLayer);
        leftRay = Physics2D.Raycast(GetTopLeftColliderPosition(collider2D, -.2f, -.5f), Vector2.left, sideWallCheckRayLength, groundLayer);
        // if (!hasSpawnPrefab)
        // {
        if (!RightRay)
        {
            downRightRay = Physics2D.Raycast(GetTopRightColliderPosition(collider2D, -.2f - sideWallCheckRayLength, -.5f), Vector2.down, groundCheckRayLength, groundLayer);
            if (downRightRay)
            {
                overrideTargetPosition.Add(colTopRight);
                Instantiate(debugColliderPrefab, colTopRight, Quaternion.identity);
                print(downRightRay.collider.name);
                overrideTargetPosition.Insert(0, downRightRay.point);
                Instantiate(debugColliderPrefab, downRightRay.point, Quaternion.identity);
            }
        }

        if (!leftRay)
        {

            downLeftRay = Physics2D.Raycast(GetTopLeftColliderPosition(collider2D, -.2f - sideWallCheckRayLength, -.5f), Vector2.down, groundCheckRayLength, groundLayer);
            if (downLeftRay)
            {
                overrideTargetPosition.Add(colTopLeft);
                Instantiate(debugColliderPrefab, colTopLeft, Quaternion.identity);
                print(downLeftRay.collider.name);
                overrideTargetPosition.Insert(0, downLeftRay.point);
                Instantiate(debugColliderPrefab, downLeftRay.point, Quaternion.identity);
            }
        }

        hasSpawnPrefab = true;
        savedClosest = overrideTargetPosition.OrderBy(dist => Vector3.Distance(dist, transform.position)).ToList();
        overrideTargetPosition.Clear();
        overrideTargetPosition.Insert(0, savedClosest[1]);
        overrideTargetPosition.Insert(1,savedClosest[0]);




    }

    public void CheckGroundToGoingDown(Collider2D collider2D)
    {
        print("initiate going down");
        var colTopRight = GetTopRightColliderPosition(collider2D);
        var colTopLeft = GetTopLeftColliderPosition(collider2D);
        RightRay = Physics2D.Raycast(GetTopRightColliderPosition(collider2D, -.2f, -.5f), Vector2.right, sideWallCheckRayLength, groundLayer);
        leftRay = Physics2D.Raycast(GetTopLeftColliderPosition(collider2D, -.2f, -.5f), Vector2.left, sideWallCheckRayLength, groundLayer);
        // if (!hasSpawnPrefab)
        // {
        if (!RightRay)
        {
            downRightRay = Physics2D.Raycast(GetTopRightColliderPosition(collider2D, -.2f - sideWallCheckRayLength, -.5f), Vector2.down, groundCheckRayLength, groundLayer);
            if (downRightRay)
            {
                overrideTargetPosition.Add(downRightRay.point);
                Instantiate(debugColliderPrefab, downRightRay.point, Quaternion.identity);
            }
        }

        if (!leftRay)
        {

            downLeftRay = Physics2D.Raycast(GetTopLeftColliderPosition(collider2D, -.2f - sideWallCheckRayLength, -.5f), Vector2.down, groundCheckRayLength, groundLayer);
            if (downLeftRay)
            {
                overrideTargetPosition.Add(downLeftRay.point);
                Instantiate(debugColliderPrefab, downLeftRay.point, Quaternion.identity);
            }
        }

        savedClosest = overrideTargetPosition.OrderBy(dist => dist.y).ToList();
        overrideTargetPosition.Clear();
        for (int i = 0; i < 1; i++)
        {
            overrideTargetPosition.Add(savedClosest[i]);
        }


    }

    #region Platform Ray Collider Check
    public Vector2 GetTopRightColliderPosition(Collider2D collider2D, float xOffset = .5f, float yOffset = .1f)
    {
        Vector2 topRight = new Vector2(collider2D.bounds.center.x + collider2D.bounds.extents.x - xOffset, collider2D.bounds.center.y + collider2D.bounds.extents.y + yOffset);
        return topRight;
    }

    public Vector2 GetTopLeftColliderPosition(Collider2D collider2D, float xOffset = .5f, float yOffset = .1f)
    {
        Vector2 topLeft = new Vector2(collider2D.bounds.center.x + -collider2D.bounds.extents.x + xOffset, collider2D.bounds.center.y + collider2D.bounds.extents.y + yOffset);
        return topLeft;
    }

    public Vector2 GetDownRightColliderPosition(Collider2D collider2D)
    {
        Vector2 downRight = new Vector2(collider2D.bounds.center.x + collider2D.bounds.extents.x + 2, collider2D.bounds.center.y + -collider2D.bounds.extents.y);
        //TODO Optimize the Raycast
        var hit = Physics2D.Raycast(downRight, Vector2.down);

        return hit.point; ;
    }

    public Vector2 GetDownLeftColliderPosition(Collider2D collider2D)
    {
        Vector2 downLeft = new Vector2(collider2D.bounds.center.x + -collider2D.bounds.extents.x + -2, collider2D.bounds.center.y + -collider2D.bounds.extents.y);
        //TODO Optimize the Raycast
        var hit = Physics2D.Raycast(downLeft, Vector2.down);

        return hit.point;
    }
    #endregion

    void ResetTargetRoutine()
    {
        // yield return new WaitForSeconds(.2f);
        if (overrideTargetPosition.Count > 0)
        {
            overrideTargetPosition.RemoveAt(0);
            // overrideTargetPositionIndex++;
        }
        // else if (targetTransform)
        //     targetTransform = null;
    }

    private void FlipDirection()
    {
        if (overrideTargetPosition.Count > 0)
        {
            if (overrideTargetPosition[0].x > transform.position.x)
                flipDirection = false;
            else if (overrideTargetPosition[0].x < transform.position.x)
                flipDirection = true;
        }
        else if (targetTransform)
        {
            if (targetTransform.position.x > transform.position.x)
                flipDirection = false;
            else if (targetTransform.position.x < transform.position.x)
                flipDirection = true;
        }
    }

    public void CheckShouldJump()
    {
        if (overrideTargetPosition.Count > 0)
            YDiferenceFromTarget = (overrideTargetPosition[0].y - transform.position.y);
        else if (targetTransform)
            YDiferenceFromTarget = (targetTransform.position.y - transform.position.y);

        shouldJump = (YDiferenceFromTarget > shouldJumpVertical);

    }

    void Movement()
    {
        if (overrideTargetPosition.Count > 0 || targetTransform && IsGrounded())
        {
            if (distanceFromTarget > .2f)
            {
                MoveToTarget();

                float? angle = RotatePlayerAngle();
                if (canJump)
                    Jump(angle);

            }
            else
                rb.velocity = Vector3.zero;
        }

        // else
        //     rb.velocity = Vector3.zero;


    }

    private void Jump(float? angle)
    {
        if (distanceFromTarget < minDistanceToJump)
            if (angle != null)
            {
                rb.velocity = jumpHeight * (flipDirection ? -groundCheckTranform.right : groundCheckTranform.right);
                // rb.velocity. = Mathf.Clamp(rb.velocity.magnitude, 1, 2);
            }
    }

    void MoveToTarget()
    {
        float stopDist = overrideTargetPosition.Count > 0 ? overrideTargetPositionStopDistance : stopDistance;
        if (distanceFromTarget >= stopDist)
        {
            if (overrideTargetPosition.Count > 0)
            {
                if (overrideTargetPosition[0].x > transform.position.x)
                    Move(1);

                else if (overrideTargetPosition[0].x < transform.position.x)
                    Move(-1);

                // pos = overrideTargetPosition[overrideTargetPositionIndex];
                // goal = $"Chasing  {overrideTargetPosition[overrideTargetPositionIndex]} at {pos}";
            }
            else if (targetTransform)
            {
                if (targetTransform.position.x > transform.position.x)
                    Move(1);

                else if (targetTransform.position.x < transform.position.x)
                    Move(-1);

                // pos = targetTransform.position;
                // goal = $"Chasing {targetTransform.name} at {pos}";
            }
        }

        else rb.velocity = Vector3.zero;

        // }
    }

    #region Calculation
    float? CalculateAngle(bool low)
    {
        Vector3 targetDir = (overrideTargetPosition.Count > 0 ? overrideTargetPosition[0] : targetTransform.position) - transform.position;
        float y = targetDir.y;
        targetDir.y = 0;
        float x = targetDir.magnitude;
        float gravity = 9.8f;
        float sSqr = jumpHeight * jumpHeight;
        float underTheSqrRoot = (sSqr * sSqr) - gravity * (gravity * x * x + 2 * y * sSqr);

        if (underTheSqrRoot >= 0f)
        {
            float root = Mathf.Sqrt(underTheSqrRoot);
            float highAngle = sSqr + root;
            float lowAngle = sSqr - root;

            if (low)
                return (Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg);
            else
                return (Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg);
        }

        else
            return null;
    }

    float? RotatePlayerAngle()
    {
        float? angle = CalculateAngle(useLowAngle);
        if (angle != null)
        {
            if (flipDirection)
                groundCheckTranform.localEulerAngles = new Vector3(0f, 0f, 360f - (float)angle);
            else
                groundCheckTranform.localEulerAngles = new Vector3(0f, 0f, (float)angle);

        }

        return angle;
    }
    #endregion

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckTranform.position, 0.2f, groundLayer);
    }

    public void Move(int dir)
    {
        // float tempDir = IsGrounded() ? moveSpeed : moveSpeed * .1f;
        rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);

    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Debug.DrawRay(GetTopLeftColliderPosition(downTargetRay.collider, -.2f, -.5f), Vector3.left * sideWallCheckRayLength, Color.green);
            Debug.DrawRay(GetTopRightColliderPosition(downTargetRay.collider, -.2f - sideWallCheckRayLength, -.5f), Vector3.down * groundCheckRayLength, Color.green);

            Debug.DrawRay(GetTopLeftColliderPosition(downTargetRay.collider, -.2f - sideWallCheckRayLength, -.5f), Vector3.down * groundCheckRayLength, Color.green);
            Debug.DrawRay(GetTopRightColliderPosition(downTargetRay.collider, -.2f, -.5f), Vector3.right * sideWallCheckRayLength, Color.green);
        }
    }
}
