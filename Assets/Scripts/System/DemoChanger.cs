using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class DemoChanger : MonoBehaviour
{
    private float waitTime = 0f;
    private bool demoFlag = false;

    public GameObject demo;
    public CanvasGroup ui;

    private void Update()
    {
        ShowDemo();
        HideDemo();
        TimeReset();
    }

    private void ShowDemo()
    {
        waitTime += Time.deltaTime;
        if (waitTime > 5 && demoFlag == false)
        {
            demoFlag = true;
            demo.SetActive(true);
            ui.DOFade(0, 0.5f).SetLink(gameObject);
        }
    }

    private void HideDemo()
    {
        var pad = Gamepad.current;
        if (pad != null && pad.wasUpdatedThisFrame)
        {
            EndDemo();
            return;
        }
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.anyKey.wasPressedThisFrame)
        {
            EndDemo();
            return;
        }
    }

    private void EndDemo()
    {
        demoFlag = false;
        waitTime = 0f;
        demo.SetActive(false);
        ui.DOFade(1, 0.5f).SetLink(gameObject);
    }

    private void TimeReset()
    {
        if (demoFlag == true) return;

        var pad = Gamepad.current;
        if (pad != null && pad.wasUpdatedThisFrame)
        {
            waitTime = 0f;
        }
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.anyKey.wasPressedThisFrame)
        {
            waitTime = 0f;
        }
    }
}
