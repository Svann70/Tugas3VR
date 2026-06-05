using UnityEngine;
using UnityEngine.SceneManagement; // Wajib dimasukkan untuk mengatur scene

public class PlayScene : MonoBehaviour
{
    // Fungsi ini yang akan dipanggil saat tombol diklik
    public void KeSceneMulai()
    {
        // Ganti "NamaSceneMulaiKamu" dengan nama scene tujuanmu yang ada di Build Settings
        SceneManager.LoadScene("Main"); 
    }
}