using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private bool downFlag = false;

    public int bombNum = 0;
    public GameObject[] unit;
    public GameObject[] spawnPoint;
    public Collider2D myCol;
    public GameObject[] hpIcon;
    public Animator animator;
    public Animator uiAnimator;
    public Text manaNum;
    public GameObject bullet;
    public GameObject bombUI;
    public Image bombGauge;
    public GameObject bomb;
    public GameObject myBody;
    public GameObject[] unitGaugeUI;
    public Image unitGauge;
    public GameObject bombCutUI;

    public float speed = 8f;
    public int hp = 3;
    public int mana = 0;
    public float recoverManaInterval = 1f;

    Vector3 move;
    GameManager gameManager;
    SE se;

    private void Start()
    {
        se = SE.Instance;
        gameManager = GameManager.Instance;
        ShowManaNum();
        ShowBombGauge();
    }

    private void Update()
    {
        UnitButtonPushing();
        Move();
        ReturnMyCol();
        Down();
        ReturnDown();
        RecoverMana();
        Shooting();
    }

    // Move
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    private bool idle = true;

    private void Move()
    {
        if (gameManager.noActionFlag == true) return;

        transform.Translate(move * speed * Time.deltaTime);

        Vector3 currentPos = transform.position;
        currentPos.x = Math.Clamp(currentPos.x, -13f, 13f);
        currentPos.y = Math.Clamp(currentPos.y, -2.4f, 5f);
        transform.position = currentPos;

        float x = move.x;
        if (x > 0.1f && idle == true)
        {
            idle = false;
            myBody.transform.DOLocalRotate(new Vector3(0, 0, -12), 0.1f).SetLink(gameObject);
        }
        else if (x < -0.1f && idle == true)
        {
            idle = false;
            myBody.transform.DOLocalRotate(new Vector3(0, 0, 8), 0.1f).SetLink(gameObject);
        }
        else if (Mathf.Abs(x) < 0.1f && idle == false)
        {
            idle = true;
            myBody.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f).SetLink(gameObject);
        }
    }

    // Fire bullet
    private bool shootingFlag = false;
    private float shotTime = 0.3f;
    public void OnShot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            shootingFlag = true;
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            shootingFlag = false;
            shotTime = 0.3f;
        }
    }

    private void Shooting()
    {
        if (gameManager.noActionFlag == true) return;
        if (downFlag == true) return;
        if (shootingFlag == true)
        {
            shotTime += Time.deltaTime;
            if (shotTime >= 0.3f)
            {
                shotTime = 0;
                se.SEShot();

                if (gameManager.shotUpgradeLV == 0)
                {
                    Instantiate(bullet, transform.position, Quaternion.identity);
                }
                else if (gameManager.shotUpgradeLV == 1)
                {
                    Vector3 pos = transform.position;
                    Instantiate(bullet, new Vector3(pos.x, pos.y + 0.2f, 0), Quaternion.identity);
                    Instantiate(bullet, new Vector3(pos.x, pos.y - 0.2f, 0), Quaternion.identity);
                }
                else if (gameManager.shotUpgradeLV == 2)
                {
                    GameObject bullet1 = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 4.76f));
                    bullet1.GetComponent<PlayerBullet>().upSpeed = 1;
                    Instantiate(bullet, transform.position, Quaternion.identity);
                    GameObject bullet2 = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, -4.76f));
                    bullet2.GetComponent<PlayerBullet>().upSpeed = -1;
                }
            }
        }
    }

    // Select unit
    public int unitSelectNum = 0;
    public void OnUnitSelectL(InputAction.CallbackContext context)
    {
        if (gameManager.noActionFlag == true) return;
        if (context.phase == InputActionPhase.Started)
        {
            unitSelectNum--;
            if (unitSelectNum < 0)
            {
                unitSelectNum = unit.Length - 1;
            }
            ShowUnitSelecting();
        }
    }

    public void OnUnitSelectR(InputAction.CallbackContext context)
    {
        if (gameManager.noActionFlag == true) return;
        if (context.phase == InputActionPhase.Started)
        {
            unitSelectNum++;
            if (unitSelectNum >= unit.Length)
            {
                unitSelectNum = 0;
            }
            ShowUnitSelecting();
        }
    }

    private void ShowUnitSelecting()
    {
        if (unitSelectNum == 0) uiAnimator.SetTrigger("unit00");
        if (unitSelectNum == 1) uiAnimator.SetTrigger("unit01");
        if (unitSelectNum == 2) uiAnimator.SetTrigger("unit02");
    }

    // spawn unit on button input
    [NonSerialized] public bool isUnitPushing = false;
    private float unitPushingTime = 0f;
    public void OnSpawn(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (gameManager.noActionFlag == true) return;
            if (downFlag) return;
            if (isAnySpawnPoint() == false) return;

            isUnitPushing = true;
            unitGaugeUI[0].SetActive(true);
            if (unitSelectNum == 0) unitGaugeUI[1].SetActive(true);
            if (unitSelectNum == 1) unitGaugeUI[2].SetActive(true);
            if (unitSelectNum == 2) unitGaugeUI[3].SetActive(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            isUnitPushing = false;
            unitGaugeUI[0].SetActive(false);
            unitGaugeUI[1].SetActive(false);
            unitGaugeUI[2].SetActive(false);
            unitGaugeUI[3].SetActive(false);
        }
    }

    private void UnitButtonPushing()
    {
        if (unitSelectNum == 0 && mana < 20) return;
        if (unitSelectNum == 1 && mana < 30) return;
        if (unitSelectNum == 2 && mana < 50) return;

        if (isUnitPushing)
        {
            unitPushingTime += Time.deltaTime;
            unitGauge.fillAmount = unitPushingTime;
            if (unitPushingTime > 1f)
            {
                unitPushingTime = 0;
                unitGauge.fillAmount = unitPushingTime;
                for (int i = 0; i < spawnPoint.Length; i++)
                {
                    float dy = Mathf.Abs(transform.position.y - spawnPoint[i].transform.position.y);
                    if (dy < 0.8f)
                    {
                        if (unitSelectNum == 0 && mana >= 20)
                        {
                            mana -= 20;
                            Instantiate(unit[0], spawnPoint[i].transform.position, Quaternion.identity);
                            animator.SetTrigger("spawn");
                        }
                        else if (unitSelectNum == 1 && mana >= 30)
                        {
                            mana -= 30;
                            Instantiate(unit[1], spawnPoint[i].transform.position, Quaternion.identity);
                            animator.SetTrigger("spawn");
                        }
                        else if (unitSelectNum == 2 && mana >= 50)
                        {
                            mana -= 50;
                            Instantiate(unit[2], spawnPoint[i].transform.position, Quaternion.identity);
                            animator.SetTrigger("spawn");
                        }
                    }
                }
            }
        }
        else
        {
            unitPushingTime = 0;
            unitGauge.fillAmount = unitPushingTime;
        }
    }

    // Fire bomb
    public void OnBomb(InputAction.CallbackContext context)
    {
        if (gameManager.noActionFlag == true) return;
        if (downFlag) return;

        if (context.phase == InputActionPhase.Started)
        {
            if (bombNum >= 1000)
            {
                bombNum = 0;
                Vector3 pos = transform.position;
                Instantiate(bomb, new Vector3(pos.x + 3, pos.y, 0), Quaternion.identity);
                Instantiate(bombCutUI, Vector3.zero, Quaternion.identity);
            }
        }
    }

    // Take damage
    public void TakeDMG()
    {
        if (gameManager.noActionFlag == true) return;
        hp--;
        myCol.enabled = false; // disable hitbox
        animator.SetBool("dmg", true);
        if (hp == 2) hpIcon[0].SetActive(false);
        else if (hp == 1) hpIcon[1].SetActive(false);
        else if (hp == 0) hpIcon[2].SetActive(false);
    }

    // Recover from damage
    private float returnMyColTime = 0f;
    private void ReturnMyCol()
    {
        if (downFlag == true) return;

        if (myCol.enabled == false)
        {
            returnMyColTime += Time.deltaTime;
            if (returnMyColTime > 2f) // invincibility window
            {
                returnMyColTime = 0f;
                myCol.enabled = true;
                animator.SetBool("dmg", false);
            }
        }
    }

    // Become downed
    private void Down()
    {
        if (gameManager.noActionFlag == true) return;
        if (hp <= 0 && downFlag == false)
        {
            downFlag = true;
            animator.SetBool("down", true);
            animator.SetBool("dmg", false);
        }

    }

    // Recover from being downed
    private float returnDownTime = 0f;
    private void ReturnDown()
    {
        if (downFlag)
        {
            returnDownTime += Time.deltaTime;
            if (returnDownTime > 5f)
            {
                returnDownTime = 0f;
                hp = 3;
                hpIcon[0].SetActive(true);
                hpIcon[1].SetActive(true);
                hpIcon[2].SetActive(true);
                downFlag = false;
                animator.SetBool("down", false);
            }
        }
    }

    // Mana regenerates automatically
    private int preMana = 0;
    private float recoverTime = 0f;
    private void RecoverMana()
    {
        if (gameManager.noActionFlag == true) return;
        recoverTime += Time.deltaTime;
        if (recoverTime >= recoverManaInterval)
        {
            recoverTime = 0f;
            mana++;
        }

        if (preMana != mana)
        {
            preMana = mana;
            ShowManaNum();
        }
    }

    // Display Mana Value
    private void ShowManaNum()
    {
        manaNum.text = mana.ToString();
    }

    // Convert bomb points
    public void AddBombNum(int num)
    {
        bombNum += num;
        if (bombNum >= 1000) bombNum = 1000;
        ShowBombGauge();
    }

    // Display bomb gauge
    private void ShowBombGauge()
    {
        bombGauge.fillAmount = (float)bombNum / 1000;
        if (bombNum >= 1000)
        {
            if (bombUI.activeSelf == false) animator.SetTrigger("bomb");
            bombUI.SetActive(true);
        }
        else
        {
            bombUI.SetActive(false);
        }
    }

    //Check whether a spawner exists in you own lane
    private bool isAnySpawnPoint()
    {
        float myY = transform.position.y;
        foreach(GameObject sp in spawnPoint)
        {
            if (sp == null) continue;
            float diffY = Mathf.Abs(sp.transform.position.y - myY);
            if (diffY <= 0.8f)
            {
                return true;
            }
        }
        return false;
    }
}
