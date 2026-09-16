using UnityEngine;
using DG.Tweening;

public class Bomb : MonoBehaviour
{
    GameManager gameManager;
    CameraManager cameraManager;
    SE se;

    private void Start()
    {
        se = SE.Instance;
        cameraManager = CameraManager.Instance;
        gameManager = GameManager.Instance;
        Invoke(nameof(SoundBomb), 0.05f);
        cameraManager.HitStop(1f);
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (gameManager.bombUpgradeLV == 0)
            {
                collision.GetComponent<Enemy>().TakeDMG(10);
            }
            else if (gameManager.bombUpgradeLV == 1)
            {
                collision.GetComponent<Enemy>().TakeDMG(20);
            }
            else if (gameManager.bombUpgradeLV == 2)
            {
                collision.GetComponent<Enemy>().TakeDMG(30);
            }

            if (collision.GetComponent<Enemy>().enemyType != Enemy.EnemyType.Base)
            {
                collision.transform.DOMoveX(2, 0.1f).SetRelative().SetLink(collision.gameObject);
            }
        }
    }

    private void SoundBomb()
    {
        se.SEBomb();
    }
}
