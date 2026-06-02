using UnityEngine;
using UnityEngine.InputSystem; // WAJIB: Menggunakan namespace Input System Baru

[RequireComponent(typeof(CharacterController))]
public class ScooterController : MonoBehaviour
{
    [Header("Spesifikasi Skuter")]
    [SerializeField] private float moveSpeed = 16f;       
    [SerializeField] private float turnSpeed = 120f;      
    [SerializeField] private float gravity = 15f;         

    private CharacterController controller;
    private Vector3 currentMoveDirection = Vector3.zero;
    
    // Variabel untuk menyimpan nilai input
    private Vector2 inputVector = Vector2.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Fungsi ini otomatis dipanggil oleh komponen Player Input
    public void OnMove(InputValue value)
    {
        // Mengambil input Vector2 (X = Horizontal/Belok, Y = Vertical/Maju)
        inputVector = value.Get<Vector2>();
    }

    void Update()
    {
        // 1. Ambil nilai input dari Vector2
        float moveInput = inputVector.y; // Maju/Mundur (W/S atau Panah Atas/Bawah)
        float turnInput = inputVector.x; // Belok Kiri/Kanan (A/D atau Panah Kiri/Kanan)

        // 2. Menghitung Rotasi/Belok Skuter
        float rotation = turnInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);

        // 3. Menghitung Arah Maju/Mundur
        Vector3 forwardMovement = transform.forward * moveInput * moveSpeed;

        // 4. Menangani Gravitasi
        if (!controller.isGrounded)
        {
            currentMoveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            currentMoveDirection.y = -1f; 
        }

        currentMoveDirection.x = forwardMovement.x;
        currentMoveDirection.z = forwardMovement.z;

        // 5. Jalankan pergerakan skuter
        controller.Move(currentMoveDirection * Time.deltaTime);
    }
}