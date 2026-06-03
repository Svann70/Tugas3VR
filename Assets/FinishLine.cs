using UnityEngine;

// FinishLine
// Pasang di objek garis Finish (dekat Start).
// Finish hanya sah kalau Checkpoint sudah dilewati = pemain benar-benar 1 putaran.
//
// Cara pakai:
// 1. Objek ini WAJIB punya Collider dengan "Is Trigger" dicentang.
// 2. Pemain (SCOOTER) harus ber-Tag "Player".
// 3. (Opsional) Drag sebuah objek Teks UI ke slot "Win Text" di Inspector
//    supaya muncul tulisan "FINISH!" saat menang.

using UnityEngine.UI; // untuk Text UI (opsional)

public class FinishLine : MonoBehaviour
{
    [Header("UI Opsional")]
    [Tooltip("Drag objek Text UI ke sini untuk menampilkan pesan menang. Boleh dikosongkan.")]
    public Text winText;

    [Tooltip("Pesan yang muncul saat menang.")]
    public string winMessage = "FINISH! Satu putaran selesai!";

    private bool hasFinished = false;

    private void Start()
    {
        // Sembunyikan teks menang di awal (kalau ada).
        if (winText != null)
            winText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Hanya hitung kalau: yang lewat adalah Player, checkpoint sudah dilewati,
        // dan belum pernah finish sebelumnya.
        if (other.CompareTag("Player") && Checkpoint.passed && !hasFinished)
        {
            hasFinished = true;
            Debug.Log("FINISH! Satu putaran selesai.");

            if (winText != null)
            {
                winText.gameObject.SetActive(true);
                winText.text = winMessage;
            }
        }
    }
}
