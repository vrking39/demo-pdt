using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject line;

    Transform playerPos;
    Player player;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        playerPos = playerObj.GetComponent<Transform>();
        player = playerObj.GetComponent<Player>();
    }

    private void Update()
    {
        ShowLine();
    }

    private void ShowLine()
    {
        if (player.isUnitPushing == false)
        {
            if (line.activeSelf == true) line.SetActive(false);
            return;
        }

        float yDistance = Mathf.Abs(transform.position.y - playerPos.position.y);
        if (yDistance < 0.8f)
        {
            if (line.activeSelf == false) line.SetActive(true);
            Vector3 pos = line.transform.position;
            pos.x = playerPos.position.x;
            line.transform.position = pos;
        }
        else
        {
            if (line.activeSelf == true) line.SetActive(false);
        }
    }
}
