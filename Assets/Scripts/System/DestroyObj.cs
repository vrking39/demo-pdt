using UnityEngine;

public class DestroyObj : MonoBehaviour
{
    public float destroyTime = 1f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
