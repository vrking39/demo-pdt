using UnityEngine;

public class SE : MonoBehaviour
{
    public static SE Instance;
    public AudioClip[] seSound;
    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // UI-related
    public void SEClick()
    {
        audioSource.PlayOneShot(seSound[1], 0.5f);
    }

    // Battle-related
    public void SEShot()
    {
        audioSource.PlayOneShot(seSound[4], 0.3f);
    }

    public void SEBomb()
    {
        audioSource.PlayOneShot(seSound[0], 1f);
    }

    public void SEDead()
    {
        audioSource.PlayOneShot(seSound[2], 0.75f);
    }

    public void SEGet()
    {
        audioSource.PlayOneShot(seSound[3], 0.5f);
    }
}
