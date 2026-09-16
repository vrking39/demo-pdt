using UnityEngine;
using DG.Tweening;

public class Mana : MonoBehaviour
{
    private float waitTime = 0f;
    private bool onceFlag = false;

    Transform playerPos;
    SE se;

    private void Start()
    {
        se = SE.Instance;
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        float posX = Random.Range(-2f, 2f);
        float posY = Random.Range(-0.5f, 0.5f);
        transform.DOJump(new Vector3(posX, posY, 0), 1.5f, 1, 0.5f).SetRelative().SetLink(gameObject); // SetLink to remove warning

        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        CheckDis();
    }

    private void CheckDis()
    {
        if (playerPos == null) return;
        waitTime += Time.deltaTime;
        if (waitTime > 0.5) // waits for the jump animation to finish
        {
            float getDis = Vector3.Distance(playerPos.position, transform.position);
            if (getDis <= 0.5) // if the player is very close
            {
                // pick this up
                se.SEGet();
                playerPos.GetComponent<Player>().mana += 5;
                Destroy(gameObject);
            }
            if (getDis <= 2 && onceFlag == false)
            {
                onceFlag = true;
                transform.DOMove(playerPos.position, 0.1f).SetLink(gameObject); // absorb feature
            }
        }
    }
}
