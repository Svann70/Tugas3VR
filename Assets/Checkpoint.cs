using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static bool passed = false;

    [Header("UI Settings")]
    public GameObject checkpointUI; 
    
    [Tooltip("Berapa detik pop-up muncul sebelum hilang lagi")]
    public float durasiTeksMuncul = 3f; // Diatur 3 detik, bisa kamu ganti di Inspector

    private void Start()
    {
        passed = false;

        if (checkpointUI != null)
        {
            checkpointUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            passed = true;
            Debug.Log("Checkpoint dilewati!");

            if (checkpointUI != null)
            {
                // 1. MUNCULKAN POP-UP
                checkpointUI.SetActive(true);

                // 2. SEMBUNYIKAN OTOMATIS setelah beberapa detik
                // Fungsi ini akan memanggil fungsi "SembunyikanUI" sesuai waktu di variabel durasiTeksMuncul
                Invoke("SembunyikanUI", durasiTeksMuncul);
            }
        }
    }

    // Ini fungsi pembantu yang dipanggil oleh Invoke di atas
    void SembunyikanUI()
    {
        if (checkpointUI != null)
        {
            checkpointUI.SetActive(false);
        }
    }
}