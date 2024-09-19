using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 25f;  // Normal speed of the triangle
    public float speedBoost = 40f; // Speed when boosting
    public float acceleration = 10f;  // How fast the triangle accelerates towards the cursor
    public float rotationSpeed = 200f; // Speed at which the triangle rotates to face the cursor
    public float drag = 0.95f; // Friction-like effect to simulate slowing down over time (between 0 and 1)

    [Header("Charge Attack")]
    public float chargeSpeed = 100f; // Speed during the charge attack
    public float chargeDuration = 0.5f; // Duration of the charge attack
    public float chargeCooldown = 1f; // Cooldown time between charge attacks
    public float windupTime = 0.5f; // Time before the charge starts

    private Vector2 velocity = Vector2.zero;  // Current velocity of the triangle
    private TrailRenderer trail;
    private Rigidbody2D rb;

    public static bool isCharging = false;    // Static flag to track charging status
    private bool isWindup = false;      // Whether the player is in windup phase
    private float chargeTime = 0f;      // Timer to manage charge duration
    private float chargeCooldownTime = 0f; // Timer to manage charge cooldown
    private float windupTimer = 0f;     // Timer to manage windup phase
    private Vector2 chargeDirection;    // Direction of the charge
    private CameraShake cameraShake;


    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>(); // Ensure Rigidbody2D is tracked for physics
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    void Update()
    {
        if (isCharging)
        {
            HandleCharge();
        }
        else if (isWindup)
        {
            HandleWindup();
        }
        else
        {
            ApplyMovement();
            RotateTowardsCursor();
            SpeedBoost();

            if (Input.GetKeyDown(KeyCode.LeftShift) && chargeCooldownTime <= 0f)
            {
                StartWindup();
            }
        }

        // Update the charge cooldown timer
        if (chargeCooldownTime > 0f)
        {
            chargeCooldownTime -= Time.deltaTime;
        }
    }

    void SpeedBoost()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            moveSpeed = speedBoost;
            trail.time = 1.1f;
        }
        else
        {
            moveSpeed = 25;
            trail.time = 0.7f;
        }
    }

    void StartWindup()
{
    isWindup = true;
    windupTimer = windupTime;
    velocity = Vector2.zero; // Stop normal movement

    // Trigger camera shake during windup
    if (cameraShake != null)
    {
        cameraShake.Shake(0.2f, 0.3f); // Adjust duration and magnitude as needed
    }
}


    void HandleWindup()
    {
        if (windupTimer > 0f)
        {
            windupTimer -= Time.deltaTime;
            RotateTowardsCursor();
        }
        else
        {
            StartCharge();
        }
    }

    void StartCharge()
    {
        isWindup = false;
        isCharging = true; // Set the static flag to true
        chargeTime = chargeDuration;
        chargeCooldownTime = chargeCooldown;

        // Calculate charge direction based on current facing direction
        chargeDirection = (Vector2)transform.up; // Assuming the triangle’s forward direction is up
        velocity = Vector2.zero; // Stop normal movement
    }

    void HandleCharge()
{
    if (chargeTime > 0f)
    {
        chargeTime -= Time.deltaTime;

        // Trigger camera shake during charge
        if (cameraShake != null)
        {
            cameraShake.Shake(0.1f, 0.2f); // Adjust duration and magnitude as needed
        }

        // Move the player forward at the charge speed
        transform.position += (Vector3)chargeDirection * chargeSpeed * Time.deltaTime;
    }
    else
    {
        isCharging = false;
    }
}


    void ApplyMovement()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - transform.position).normalized;
        Vector2 targetVelocity = direction * moveSpeed;

        velocity = Vector2.Lerp(velocity, targetVelocity, acceleration * Time.deltaTime);
        velocity *= drag;

        rb.velocity = velocity; // Update Rigidbody2D's velocity

        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    void RotateTowardsCursor()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
