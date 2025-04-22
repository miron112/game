using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusicController : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlayMenuMusic();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game"); // Ваше имя игровой сцены
    }
}