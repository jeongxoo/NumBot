using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public GameObject Man;

    void Update()
    {
        GoUnit();
        DestroyUnitWhenTimeOver();
    }

    public void GoUnit() // 유닛 자동이동
    {
        Man.transform.Translate(4f * Time.deltaTime, 0, 0);
    }

    public void OnTriggerEnter2D(Collider2D other) // 적기지나 마법에부딪히면 사라지는 함수
    {
        switch (other.tag)
        {
            case "Enemy":
                Destroy(Man);
                //Debug.Log("Can`t Destory Shield..");
                break;

            case "Shield":
                Destroy(Man);
                break;
                

            case "Fire":
                Destroy(Man);
                break;

        }
    }

    public void DestroyUnitWhenTimeOver() // 게임버이후에 유닛이 들이박는걸 막기 위한 함수
    {
        if (GameManager.instance.gTime <= 0 || GameManager.instance.eHP <= 0)
        {
            Destroy(this);
        }
    }
}
