using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameScene : MonoBehaviour
{


    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(LoadGame);
        }
        else
        {
            Debug.LogError("Скрипт LoadGameScene должен быть attached к объекту с компонентом Button!");
        }
    }

    void LoadGame()
{

    SceneManager.LoadScene("Game");
}

}
