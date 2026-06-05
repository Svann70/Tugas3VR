using UnityEngine;

public class ScooterCamera : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target; // Drag objek skuter ke sini

    [Header("Offset Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 2.5f, -5f); // Jarak aman kamera dari skuter

    [Header("Movement Settings")]
    [SerializeField] private float smoothSpeed = 0.125f; // Semakin kecil, semakin halus/lambat gerakannya

    [Header("Rotation Settings")]
    [SerializeField] private bool followRotation = true; // Apakah kamera ikut berputar saat skuter belok?
    [SerializeField] private float rotationSmoothSpeed = 5f;

    private Vector3 currentVelocity;

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Menghitung posisi target yang diinginkan berdasarkan posisi dan rotasi skuter
        Vector3 desiredPosition;
        
        if (followRotation)
        {
            // Menghitung offset lokal berdasarkan arah hadap skuter
            desiredPosition = target.position + (target.rotation * offset);
        }
        else
        {
            // Kamera hanya mengikuti posisi, tidak terpengaruh rotasi skuter
            desiredPosition = target.position + offset;
        }

        // 2. Pergerakan halus (Smooth Damping) menuju posisi target
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);
        transform.position = smoothedPosition;

        // 3. Rotasi kamera
        if (followRotation)
        {
            // Kamera menghadap ke skuter secara halus mengikuti rotasinya
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);
        }
        else
        {
            // Kamera selalu menghadap ke arah skuter saja
            transform.LookAt(target);
        }
    }
}