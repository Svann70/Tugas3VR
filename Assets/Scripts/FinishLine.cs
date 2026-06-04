using UnityEngine;
using UnityEngine.SceneManagement; // WAJIB untuk fitur pindah Scene

public class FinishLine : MonoBehaviour
{
    [Header("UI Settings")]
    [Tooltip("Tarik objek Canvas_Finish kamu ke sini lewat Inspector.")]
    public GameObject finishCanvas; 

    [Header("Scene Settings")]
    [Tooltip("Ketik NAMA SCENE MAIN MENU kamu di sini (harus persis hurufnya).")]
    public string namaSceneMainMenu = "UI Menu"; 

    [Tooltip("Berapa detik jeda sebelum game pindah ke Main Menu.")]
    public float jedaPindahScene = 3f;

    private bool hasFinished = false;

    private void Start()
    {
        // Matikan Canvas Finish di awal game supaya tidak langsung muncul
        if (finishCanvas != null)
        {
            finishCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Hanya hitung kalau: yang lewat adalah Player, checkpoint sudah dilewati,
        // dan belum pernah finish sebelumnya.
        if (other.CompareTag("Player") && Checkpoint.passed && !hasFinished)
        {
            hasFinished = true;
            Debug.Log("FINISH! Paket telah dikirimkan.");

            // 1. MUNCULKAN POP-UP CANVAS FINISH
            if (finishCanvas != null)
            {
                finishCanvas.SetActive(true);
            }

            // 2. PINDAH KE SCENE MAIN MENU DENGAN JEDA
            Invoke("PindahKeMainMenu", jedaPindahScene);
        }
    }

    void PindahKeMainMenu()
    {
        if (!string.IsNullOrEmpty(namaSceneMainMenu))
        {
            SceneManager.LoadScene(namaSceneMainMenu);
        }
        else
        {
            Debug.LogError("Nama Scene Main Menu belum diisi di Inspector!");
        }
    }
}