using UnityEngine;
using DG.Tweening;
using System;

public class PlayerBullet : MonoBehaviour
{
    private float speed = 12f;
    [NonSerialized] public float upSpeed = 0f;

    public SpriteRenderer sprite;
    public GameObject eff;

    private void Start()
    {
        sprite.DOFade(1, 0.3f).SetLink(gameObject);
        Destroy(gameObject, 0.6f);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDMG(1);
            Instantiate(eff, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Unit"))
        {
            collision.transform.DOMoveX(0.2f, 0.1f).SetRelative().SetLink(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
