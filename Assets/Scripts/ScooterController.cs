using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class ScooterController : MonoBehaviour
{
    [Header("Kecepatan")]
    public float maxSpeed = 150f;
    public float maxReverseSpeed = 40f;
    public float acceleration = 70f;
    public float braking = 140f;
    public float friction = 120f;

    [Header("Belok")]
    public float turnSpeed = 90f;
    public float mouseSensitivity = 2f;
    public bool lockCursor = true;

    [Header("Lean / Kemiringan Body (opsional)")]
    public Transform leanTarget;
    public float maxLeanAngle = 18f;
    public float leanSmooth = 6f;

    [Header("Fisika")]
    public float gravity = 20f;

    [Header("UI Speedometer")]
    public TextMeshProUGUI textSpeed;

    // ── Ground detection manual (lebih reliable dari isGrounded bawaan CC) ──
    [Header("Ground Detection")]
    public LayerMask groundLayers = ~0;          // semua layer by default
    public float groundCheckDistance = 0.3f;     // raycast ke bawah sejauh ini
    public float groundSnapForce = 5f;           // gaya snap ke tanah agar tidak melayang

    private CharacterController cc;
    private float currentSpeed = 0f;
    private float yaw = 0f;
    private float verticalVelocity = 0f;
    private float currentLean = 0f;
    private Quaternion leanBaseRot;

    // ── State gravity ──
    private bool isGrounded = false;
    private bool wasGrounded = false;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        yaw = transform.eulerAngles.y;
        if (leanTarget != null) leanBaseRot = leanTarget.localRotation;
        if (lockCursor) Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // ── 0) Deteksi tanah manual via SphereCast ──────────────────────────
        // SphereCast lebih toleran dari Raycast untuk permukaan tidak rata
        Vector3 sphereOrigin = transform.position + Vector3.up * cc.radius;
        float castDist = cc.radius + groundCheckDistance;

        isGrounded = Physics.SphereCast(
            sphereOrigin,
            cc.radius * 0.9f,       // sedikit lebih kecil dari CC radius
            Vector3.down,
            out RaycastHit groundHit,
            castDist,
            groundLayers,
            QueryTriggerInteraction.Ignore
        );

        // ── 1) Throttle & Momentum ──────────────────────────────────────────
        float throttle    = Input.GetAxisRaw("Vertical");
        float steerInput  = Input.GetAxis("Horizontal");
        float mouseX      = Input.GetAxis("Mouse X");

        if (throttle > 0.1f)
            currentSpeed = Mathf.MoveTowards(currentSpeed, -maxSpeed,  acceleration * Time.deltaTime);
        else if (throttle < -0.1f)
            currentSpeed = Mathf.MoveTowards(currentSpeed,  maxReverseSpeed, braking * Time.deltaTime);
        else
            currentSpeed = Mathf.MoveTowards(currentSpeed,  0f, friction * Time.deltaTime);

        // ── 2) Steering ─────────────────────────────────────────────────────
        float speedFactor  = Mathf.Clamp01(Mathf.Abs(currentSpeed) / maxSpeed);
        float reverseSign  = currentSpeed < -0.1f ? -1f : 1f;
        float steer        = -steerInput + mouseX * mouseSensitivity;
        yaw               += steer * turnSpeed * speedFactor * reverseSign * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // ── 3) Gravity yang benar ───────────────────────────────────────────
        if (isGrounded)
        {
            // Reset vertical velocity saat landing dari udara
            if (!wasGrounded && verticalVelocity < 0f)
                verticalVelocity = 0f;

            // Paksa menempel ke tanah (snap) — eliminasi "floating gap"
            // Nilai negatif kecil agar CC tetap mendeteksi grounded di frame berikut
            verticalVelocity = -groundSnapForce;
        }
        else
        {
            // Jatuh bebas — gravity hanya aktif saat di udara
            verticalVelocity -= gravity * Time.deltaTime;

            // Clamp agar tidak jatuh tak terbatas (terminal velocity)
            verticalVelocity = Mathf.Max(verticalVelocity, -gravity * 3f);
        }

        wasGrounded = isGrounded;

        // ── 4) Move ─────────────────────────────────────────────────────────
        Vector3 move = transform.forward * currentSpeed;
        move.y = verticalVelocity;
        cc.Move(move * Time.deltaTime);

        // ── 5) Lean ─────────────────────────────────────────────────────────
        if (leanTarget != null)
        {
            float targetLean = -steer * maxLeanAngle * speedFactor;
            currentLean = Mathf.Lerp(currentLean, targetLean, leanSmooth * Time.deltaTime);
            leanTarget.localRotation = leanBaseRot * Quaternion.Euler(0f, 0f, currentLean);
        }

        // ── 6) Update Speedometer UI ─────────────────────────────────────────
        if (textSpeed != null)
        {
            // Ambil angka absolut agar kecepatan maju/mundur tidak minus
            float displaySpeed = Mathf.Abs(currentSpeed); 
            
            // Bulatkan angka dan gabungkan dengan teks " km/h"
            textSpeed.text = Mathf.RoundToInt(displaySpeed).ToString() + " km/h";
        }
    }

    // ── Debug: visualisasi SphereCast di Scene View ──────────────────────────
    void OnDrawGizmosSelected()
    {
        if (cc == null) cc = GetComponent<CharacterController>();
        if (cc == null) return;

        Vector3 origin = transform.position + Vector3.up * cc.radius;
        Gizmos.color  = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(origin + Vector3.down * (cc.radius + groundCheckDistance), cc.radius * 0.9f);
        Gizmos.DrawLine(origin, origin + Vector3.down * (cc.radius + groundCheckDistance));
    }
}