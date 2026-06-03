using UnityEngine;

// Checkpoint
// Pasang di objek trigger di sisi TERJAUH dari Start/Finish.
// Gunanya: supaya pemain tidak bisa curang langsung menyentuh Finish.
// Finish baru sah kalau Checkpoint ini sudah dilewati dulu.
//
// Cara pakai:
// 1. Objek ini WAJIB punya Collider dengan "Is Trigger" dicentang.
// 2. Pemain (SCOOTER) harus ber-Tag "Player".

public class Checkpoint : MonoBehaviour
{
    // "static" artinya nilai ini bisa dibaca dari script lain (FinishLine).
    public static bool passed = false;

    private void Start()
    {
        // Reset setiap kali scene dimulai, supaya tidak terbawa dari main sebelumnya.
        passed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            passed = true;
            Debug.Log("Checkpoint dilewati! Sekarang menuju Finish.");
        }
    }
}
