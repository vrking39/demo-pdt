using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public GameObject[] startButton;
    public GameObject[] ui;

    SE se;

    private void Start()
    {
        se = SE.Instance;
        Invoke(nameof(Selected), 0.5f);
    }

    private void Selected()
    {
        EventSystem.current.SetSelectedGameObject(startButton[0]);
    }

    public void OnStart()
    {
        if (ui[0].activeSelf == true) return;
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene("Stage");
        se.SEClick();
    }

    public void OnQuit()
    {
        if (ui[0].activeSelf == true) return;
        Application.Quit();
    }

}
