using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Creature : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool alive = true;
    private Collider2D[] collidersTouching;
    private ContactFilter2D terrainFilter;
    private Color originalColor;

    [Header("Speed")]
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float maxJumpSeconds = 1.0f;
    [SerializeField] private float jumpForce = 200.0f;
    [SerializeField] private float maxJumpSpeed = 5.0f;

    [Header("Stats")]
    [SerializeField] private int hitPoints = 100;
    [SerializeField] private int maxHitPoints = 100;
    
    [Header("Touching Ground")]
    [SerializeField] private int maxCollidersTouching = 10;
    [SerializeField] private string terrainLayerName = "Terrain";
    
    [Header("Weapon")]
    [SerializeField] private Gun gun;
    [SerializeField] private Transform customPivot;
    [SerializeField] private float rotationSpeed = 1.0f;

    [Header("Hat")]
    [SerializeField] private GameObject hat;
    [SerializeField] private float hatYOffset = 1.0f;
    [SerializeField] private float hatFlyOffMultiplier = 2.0f;

    [Header("Death")]
    [SerializeField] private float deathForceX = 2.0f;
    [SerializeField] private float deathForceY = 2.0f;

    private SpriteRenderer sr;
    private bool isJumping = false;
    private bool isCrouching = false;
    private AnimationStateChanger myAnim;
    [SerializeField] private float hurtTimer = 1.0f;
    private bool invincible = false;
    [SerializeField] private bool grantInvincibility = false;
    
    public void Crouch()
    {
        if (!alive) {
            return;
        }

        if (isJumping) {
            return;
        }

        if (!TouchingGround()) {
            return;
        }
        
        isCrouching = true;

        if (myAnim != null) {
            myAnim.ChangeState("Crouching");
        }
    }

    public void ReleaseCrouch()
    {
        isCrouching = false;
    }
    
    public void MoveLeft()
    {
        if (!alive) {
            return;
        }

        // We are currently moving right but we want to change directions
        if (rb.velocity.x > 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Math.Abs(rb.velocity.x) >= maxSpeed)
        {
            return;
        }

        rb.AddForce(new Vector2(-1 * speed, 0), ForceMode2D.Impulse);
        sr.flipX = true;
    }

    public void MoveRight()
    {
        if (!alive) {
            return;
        }

        // We are currently moving left but we want to change directions
        if (rb.velocity.x < 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Math.Abs(rb.velocity.x) >= maxSpeed)
        {
            return;
        }

        rb.AddForce(new Vector2(speed, 0), ForceMode2D.Impulse);
        sr.flipX = false;
    }

    public void Stop()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private bool TouchingGround()
    {
        if (boxCollider == null)
        {
            return true;
        }

        return boxCollider.OverlapCollider(terrainFilter, collidersTouching) >= 1;
    }

    public void Jump()
    {
        if (!alive) {
            return;
        }

        if (isJumping) {
            return;
        }

        // Make sure we are touching the ground first
        if (!TouchingGround()) {
            return;
        }

        isJumping = true;
        StartCoroutine(JumpRoutine());
    }

    public void InstantJump(float force)
    {
        if (!alive) {
            return;
        }
        
        if (!TouchingGround())
        {
            return;
        }

        rb.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
    }

    public void ReleaseJump()
    {
        if (!isJumping) {
            return;
        }

        isJumping = false;
    }

    private IEnumerator JumpRoutine()
    {
        float timer = 0.0f;
        while(isJumping) {
            if (timer < maxJumpSeconds)
            {
                timer += Time.fixedDeltaTime;
            } else {
                ReleaseJump();
            }

            if (rb.velocity.y < maxJumpSpeed && isJumping) {
                rb.AddForce(new Vector2(0, Time.fixedDeltaTime * jumpForce), ForceMode2D.Impulse);
            }
            
            yield return new WaitForFixedUpdate();
        }
    }

    public void Kill()
    {
        if (!alive) {
            return;    
        }

        sr.color = originalColor;
        hitPoints = 0;

        // Fly backwards
        if (rb.velocity.x > 0) {
            rb.AddForce(new Vector2(deathForceX, deathForceY), ForceMode2D.Impulse);
        } else {
            rb.AddForce(new Vector2(deathForceX * -1, deathForceY), ForceMode2D.Impulse);
        }
        
        rb.MoveRotation(Quaternion.LookRotation(rb.velocity));

        alive = false;
        // Unlock Z rotation to allow for funny ragdolls
        rb.constraints &= ~RigidbodyConstraints2D.FreezeRotation;
        gun.gameObject.SetActive(false);

        // Make hat fly off
        if (hat != null)
        {
            if (myAnim != null) {
                myAnim.ChangeState("Dead");
            }
            
            GameObject newHat = Instantiate(hat, new Vector3(transform.position.x, transform.position.y + hatYOffset, transform.position.z), Quaternion.identity);
            newHat.GetComponent<Rigidbody2D>().AddForce(new Vector2(rb.velocity.x * hatFlyOffMultiplier, rb.velocity.y * hatFlyOffMultiplier));
        }
    }

    private IEnumerator HurtRoutine()
    {
        float timer = 0.0f;

        if (grantInvincibility)
        {
            invincible = true;
        }

        
        bool isClear = false;

        while (timer < hurtTimer)
        {
            // If you're not clear...
            if (!isClear)
            {
                // ..become clear
                isClear = true;
                sr.color = Color.clear;
            } else {
                // otherwise, you were clear so let's make you visible again.
                isClear = false;
                sr.color = originalColor;
            }
            timer += Time.deltaTime;
            
            yield return null;
        }

        sr.color = originalColor;
        invincible = false;
    }

    public void Hurt(int damage)
    {
        if (invincible)
        {
            return;
        }

        if (damage <= 0) {
            return;
        }

        hitPoints -= damage;
        StartCoroutine(HurtRoutine());

        if (hitPoints <= 0) {
            Kill();
        }
    }

    public int GetHitPoints()
    {
        return hitPoints;
    }

    public int GetMaxHitPoints()
    {
        return maxHitPoints;
    }

    public void FullyHeal()
    {
        hitPoints = maxHitPoints;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<AnimationStateChanger>();
        originalColor = sr.color;

        collidersTouching = new Collider2D[maxCollidersTouching];
        terrainFilter = new ContactFilter2D();
        terrainFilter.SetLayerMask(LayerMask.GetMask(terrainLayerName));

        hitPoints = maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (alive && myAnim != null)
        {
            if (isJumping || (Math.Abs(rb.velocity.y) > 0 && !isCrouching))
            {
                myAnim.ChangeState("Jumping");
            }
            else if (TouchingGround() && Math.Abs(rb.velocity.x) > 0 && !isCrouching)
            {
                myAnim.ChangeState("Walking");
            }
            else if (rb.velocity.x == 0 && rb.velocity.y == 0 && !isCrouching)
            {
                myAnim.ChangeState("Standing");
            }
        }
    }

    public void AimWeaponAtGradually(Vector3 target)
    {
        Vector3 pivotPoint = customPivot.position;
        Vector3 difference = target - pivotPoint;

        Transform muzzle = gun.GetMuzzle();
        float theta = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (Mathf.Abs(theta) > 90) {
            gun.GetComponent<SpriteRenderer>().flipY = true;
            //Debug.Log("Old pos " + muzzle.position);
            muzzle.localPosition = new Vector3(muzzle.localPosition.x, Mathf.Abs(muzzle.localPosition.y) * -1, muzzle.localPosition.z);
            //Debug.Log("New pos" + muzzle.position);
        } else {
            //Debug.Log("Old pos " + muzzle.position);
            gun.GetComponent<SpriteRenderer>().flipY = false;
            muzzle.localPosition = new Vector3(muzzle.localPosition.x, Mathf.Abs(muzzle.localPosition.y), muzzle.localPosition.z);
            //muzzle.position = new Vector3(muzzle.position.x, Mathf.Abs(muzzle.position.y), muzzle.position.z);
            //Debug.Log("New pos" + muzzle.position);
        }

        customPivot.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(customPivot.eulerAngles.z, theta, Time.deltaTime * rotationSpeed));
    }

    public void Attack()
    {
        if (!alive) {
            return;
        }
        
        if (gun != null) {
            gun.Shoot();
        }
    }

    public bool IsAlive()
    {
        return alive;
    }
}
