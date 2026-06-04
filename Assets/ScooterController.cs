using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ScooterController : MonoBehaviour
{
    [Header("Kecepatan")]
    public float maxSpeed = 150f;        // kecepatan maksimum maju
    public float maxReverseSpeed = 40f;  // kecepatan maksimum mundur
    public float acceleration = 70f;     // seberapa cepat ngebut (gas)
    public float braking = 140f;         // seberapa cepat berhenti (rem)
    public float friction = 120f;         // melambat alami saat lepas gas

    [Header("Belok")]
    public float turnSpeed = 90f;        // derajat/detik saat belok penuh
    public float mouseSensitivity = 2f;
    public bool lockCursor = true;

    [Header("Lean / Kemiringan Body (opsional)")]
    public Transform leanTarget;         // drag objek visual motor ke sini (boleh kosong)
    public float maxLeanAngle = 18f;
    public float leanSmooth = 6f;

    [Header("Fisika")]
    public float gravity = 20f;

    private CharacterController cc;
    private float currentSpeed = 0f;     // kecepatan saat ini (ini yang bikin momentum)
    private float yaw = 0f;
    private float verticalVelocity = 0f;
    private float currentLean = 0f;
    private Quaternion leanBaseRot;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        yaw = transform.eulerAngles.y;
        if (leanTarget != null) leanBaseRot = leanTarget.localRotation;
        if (lockCursor) Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float throttle = Input.GetAxisRaw("Vertical");   // W / S
        float steerInput = Input.GetAxis("Horizontal");  // A / D
        float mouseX = Input.GetAxis("Mouse X");

        // 1) Momentum: kecepatan naik/turun bertahap, tidak instan
        if (throttle > 0.1f)
            currentSpeed = Mathf.MoveTowards(currentSpeed, -maxSpeed, acceleration * Time.deltaTime);
        else if (throttle < -0.1f)
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxReverseSpeed, braking * Time.deltaTime);
        else
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, friction * Time.deltaTime);

        // 2) Belok hanya terasa saat melaju (makin cepat makin responsif)
        float speedFactor = Mathf.Clamp01(Mathf.Abs(currentSpeed) / maxSpeed);
        float reverseSign = currentSpeed < -0.1f ? -1f : 1f; // mundur = setang terbalik
        float steer = -steerInput + mouseX * mouseSensitivity; // gabung A/D + mouse
        yaw += steer * turnSpeed * speedFactor * reverseSign * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // 3) Gerak maju + gravitasi
        if (cc.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
        verticalVelocity -= gravity * Time.deltaTime;
        Vector3 move = transform.forward * currentSpeed;
        move.y = verticalVelocity;
        cc.Move(move * Time.deltaTime);

        // 4) Lean body saat menikung (kalau leanTarget diisi)
        if (leanTarget != null)
        {
            float targetLean = -steer * maxLeanAngle * speedFactor;
            currentLean = Mathf.Lerp(currentLean, targetLean, leanSmooth * Time.deltaTime);
            leanTarget.localRotation = leanBaseRot * Quaternion.Euler(0f, 0f, currentLean);
        }
    }
}