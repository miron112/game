using UnityEngine;

public class GameMusicController : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlayRandomGameMusic();
    }
}