using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] string _levelSceneToLoad = "Level0";

    private void Start()
    {
        Cursor.visible = true;
    }
    public void StartGame()
    {
        SceneManager.LoadScene(_levelSceneToLoad);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
