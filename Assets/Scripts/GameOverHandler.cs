using UnityEngine;
using UnityEngine.SceneManagement; // Diperlukan untuk reload scene / pindah level

public class GameOverHandler : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject gameOverCanvas; // Masukkan Canvas GameOver di sini

    [Tooltip("Berapa detik jeda setelah mati sebelum game restart otomatis")]
    public float jedaRestart = 3f;

    private bool sudahMati = false;

    private void Start()
    {
        // Pastikan Canvas GameOver mati di awal permainan
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }

    // Menggunakan OnTriggerEnter jika objek rintangan diatur sebagai Is Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang menabrak adalah Player dan game belum dalam kondisi Game Over
        if (other.CompareTag("Player") && !sudahMati)
        {
            MulaiGameOver();
        }
    }

    // Sediakan juga OnCollisionEnter jika rintangan kamu bersifat padat (bukan Is Trigger)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !sudahMati)
        {
            MulaiGameOver();
        }
    }

    void MulaiGameOver()
    {
        sudahMati = true;
        Debug.Log("Player Menabrak Rintangan! Game Over.");

        // 1. Munculkan Canvas Game Over
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        // 2. Mengulang game dari awal setelah jeda beberapa detik
        Invoke("RestartLevel", jedaRestart);
    }

    void RestartLevel()
    {
        // Mengambil nama scene yang sedang aktif saat ini
        string namaSceneAktif = SceneManager.GetActiveScene().name;
        
        // Memuat ulang scene tersebut (mengulang dari awal)
        SceneManager.LoadScene(namaSceneAktif);
    }
}