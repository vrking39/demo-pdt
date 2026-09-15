using UnityEngine;

public class Crystal : MonoBehaviour
{
    public GameObject mana;
    public Collider2D myCol;
    public GameObject myTex;
    public GameObject eff;

    private int hp = 10;

    private void Update()
    {
        Dead();
        Recover();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            hp--;
            Instantiate(mana, transform.position, Quaternion.identity);
            Instantiate(eff, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }

    private void Dead()
    {
        if (hp <= 0)
        {
            hp = 10;
            myCol.enabled = false;
            myTex.SetActive(false);
        }
    }

    private float recoverTime = 0f;
    private void Recover()
    {
        if (myTex.activeSelf == false)
        {
            recoverTime += Time.deltaTime;
            if (recoverTime >= 25f)
            {
                recoverTime = 0f;
                myCol.enabled = true;
                myTex.SetActive(true);
            }
        }        
    }
}
