using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Enemy : MonoBehaviour
{
    public GameObject TextHP;
    public GameObject gamePlay;
    public GameObject winText;
    public GameObject playTime;
    public GameObject hpBar;

    public GameObject magicShield;
    public GameObject shieldPosition;
    public float coolTimeShield;
    public bool shieldOn;

    public GameObject fireBall;
    public GameObject[] fireBallPosition;
    public float coolTimeFireBall;
    public float coolTimeFireBallValue;
    public bool fireOn;

    public int dangerHP;
    public bool eDanger;

    public Canvas canvasInGame;
    public Canvas canvasEndGame;

    private void Awake()
    {

        GameManager.instance.SaveGameDataToJson();
        GameManager.instance.eHP = 20;

        //Debug.Log("myLight was not set in the inspector");

        shieldOn = true;
        fireOn = true;

        dangerHP = 5;

        coolTimeShield = 5f;

        coolTimeFireBallValue = 3f;
        coolTimeFireBall = coolTimeFireBallValue;

        hpBar.GetComponent<Slider>().maxValue = GameManager.instance.eHP;

        //Debug.Log(GameManager.instance.eHP);

        
    }

    private void Update()
    {
        TextHP.GetComponent<Text>().text = "HP : " + GameManager.instance.eHP;
        MagicShield();
        FireBall();

        hpBar.GetComponent<Slider>().value = GameManager.instance.eHP;
    }

    public void EnemyGameOver()
    {
        if (GameManager.instance.eHP <= 0)
        {
            canvasEndGame.gameObject.SetActive(true);
            canvasInGame.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        switch (other.tag)
        {
            case "Man1":
                GameManager.instance.eHP -= 1;
                EnemyGameOver();
                GameManager.instance.StopWhenEnemyDie(gamePlay, winText, playTime);
                break;

            case "Man2":
                GameManager.instance.eHP -= 2;
                EnemyGameOver();
                GameManager.instance.StopWhenEnemyDie(gamePlay, winText, playTime);
                break;

            case "Man3":
                GameManager.instance.eHP -= 3;
                EnemyGameOver();
                GameManager.instance.StopWhenEnemyDie(gamePlay, winText, playTime);
                break;

            case "Man4":
                GameManager.instance.eHP -= 4;
                EnemyGameOver();
                GameManager.instance.StopWhenEnemyDie(gamePlay, winText, playTime);
                break;

            case "Man5":
                GameManager.instance.eHP -= 5;
                EnemyGameOver();
                GameManager.instance.StopWhenEnemyDie(gamePlay, winText, playTime);
                break;
        }
       
    }

    public void MagicShield() // 마법 방패 실행
    {
        if (GameManager.instance.eHP <= 15) //체력이 15이하로 내려가면(20이 풀일떄 기준)
        {
            if(shieldOn) // 실드가 트루인지 확인
            {
                shieldOn = false; // 트루 면 펄스로 바꾸고 한번 실행
                Instantiate(magicShield, shieldPosition.transform.position, Quaternion.identity);
                Debug.Log("마법방패 시전!!");
            }
            else // 펄스면
            {
                coolTimeShield -= Time.deltaTime; // 쿨타임을 계속 감소시킴 (쿨타임은 5초)
                if (coolTimeShield <= 0) // 쿨타임이 0이되면
                {
                    if (GameObject.FindWithTag("Shield") == true)
                    {
                        //Debug.Log("방패 존재");
                    }
                    else
                    {
                        coolTimeShield = 5f; // 쿨타임을 다시 5로하고
                        shieldOn = true; // 실드를 다시 트루
                    }
                    
                }
            }
        }
        
    }

    public void FireBall()
    {
        if (GameManager.instance.eHP <= 10 )
        {
            if (fireOn)
            {
                coolTimeFireBall = coolTimeFireBallValue;
                fireOn = false;
                for (int i = 0; i < fireBallPosition.Length; i++)
                {   
                    Instantiate(fireBall, fireBallPosition[i].transform.position, Quaternion.identity);
                }
                Debug.Log("파이어볼 시전!!");
            }
            else
            {
                //Debug.Log("쿨감소 하고 있어?");
                coolTimeFireBall -= Time.deltaTime;
                if (coolTimeFireBall <= 0)
                {
                    fireOn = true;
                }
            }
        }
        else if (GameManager.instance.eHP <= dangerHP)
        {
            eDanger = true;
        }
    }


}
