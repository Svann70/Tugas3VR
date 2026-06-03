using UnityEngine;

// ScooterController
// Kontrol skuter pakai keyboard (WASD) + rotasi mouse.
// Memakai Character Controller (sudah menempel di objek SCOOTER) supaya tidak
// menembus jalan/tembok, dan punya gravity supaya menempel ke tanah.
//
// PENTING: script ini pakai Input.GetAxis (input cara lama), jadi project HARUS
// di-set: Edit > Project Settings > Player > Active Input Handling = "Both".
// Kalau belum, akan muncul error "InvalidOperationException ... Input System".

[RequireComponent(typeof(CharacterController))]
public class ScooterController : MonoBehaviour
{
    [Header("Kecepatan")]
    [Tooltip("Kecepatan maju/mundur. Naikkan kalau terasa lambat (skala objek besar).")]
    public float moveSpeed = 190f;

    [Tooltip("Kecepatan belok kiri/kanan (derajat per detik).")]
    public float turnSpeed = 90f;

    [Header("Mouse")]
    [Tooltip("Aktifkan rotasi pakai mouse (kriteria dosen: rotasi mouse).")]
    public bool useMouseRotation = true;

    [Tooltip("Sensitivitas mouse untuk memutar arah skuter.")]
    public float mouseSensitivity = 2f;

    [Tooltip("Kunci kursor di tengah layar saat main (tekan Esc untuk lepas).")]
    public bool lockCursor = true;

    [Header("Fisika")]
    [Tooltip("Kekuatan gravitasi supaya skuter menempel ke tanah.")]
    public float gravity = 20f;

    private CharacterController controller;
    private float yaw;                // arah hadap skuter (rotasi sumbu Y)
    private float verticalVelocity;   // kecepatan jatuh (gravity)

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Mulai dari arah hadap skuter sekarang, supaya tidak "loncat" arah saat mulai main.
        yaw = transform.eulerAngles.y;

        if (lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // --- 1. ROTASI (belok) ---
        // Belok pakai A/D (keyboard)
        float turnInput = Input.GetAxis("Horizontal"); // A = -1, D = +1
        yaw += turnInput * turnSpeed * Time.deltaTime;

        // Belok pakai mouse (memenuhi kriteria "rotasi mouse")
        if (useMouseRotation)
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;

        // Terapkan rotasi ke skuter. Karena Main Camera adalah anak SCOOTER,
        // kamera otomatis ikut berputar.
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // --- 2. GERAK MAJU/MUNDUR ---
        float moveInput = Input.GetAxis("Vertical"); // W = +1, S = -1
        // Gerak relatif arah hadap skuter (natural untuk kendaraan).
        Vector3 horizontalMove = -transform.forward * moveInput * moveSpeed;

        // --- 3. GRAVITY ---
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f; // dorong sedikit ke bawah biar tetap "grounded"
        else
            verticalVelocity -= gravity * Time.deltaTime;

        // --- 4. JALANKAN GERAKAN ---
        Vector3 velocity = horizontalMove + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }
}
