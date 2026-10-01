using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [NonSerialized] public bool noActionFlag = false;

    [NonSerialized] public int shotUpgradeLV = 0;
    [NonSerialized] public int unitUpgradeLV = 0;
    [NonSerialized] public int bombUpgradeLV = 0;
    [NonSerialized] public bool bossSpawnFlag = false;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void GameOver()
    {
        noActionFlag = true;
        // Scene Transistion
        SceneManager.LoadScene("Title");
    }

    public void GameClear()
    {
        noActionFlag = true;
        // Scene Transistion
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
