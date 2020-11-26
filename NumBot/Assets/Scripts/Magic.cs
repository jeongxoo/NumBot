using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public GameObject shield;
    public int s_count = 0;
    public int s_Destroy = 5;
    public GameObject fireBall;

    private void Update()
    {
        DestroyUnitWhenTimeOver();
        if (s_count >= s_Destroy)
            Destroy(shield);
    }

    public void DestroyUnitWhenTimeOver() // 게임버이후에 마법이 들이박는걸 막기 위한 함수
    {
        if (GameManager.instance.gTime <= 0 || GameManager.instance.eHP <= 0)
        {
            Destroy(shield);
            Destroy(fireBall);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareTag("Shield"))
        {
            //Debug.Log("cursh on shield");
            switch (other.tag)
            {
                case "Man1":
                    break;

                case "Man2":
                    s_count++;
                    break;

                case "Man3":
                    s_count++;
                    break;

                case "Man4":
                    s_count++;
                    break;

                case "Man5":
                    s_count++;
                    break;
            }
        }
        else if (CompareTag("Fire"))
        {
            //Debug.Log("cursh on fireball");
            switch (other.tag)
            {
                case "Man1":
                    Destroy(fireBall);
                    break;

                case "Man2":
                    Destroy(fireBall);
                    break;

                case "Man3":
                    Destroy(fireBall);
                    break;

                case "Man4":
                    Destroy(fireBall);
                    break;

                case "Man5":
                    Destroy(fireBall);
                    break;

                case "Ground":
                    Destroy(fireBall);
                    break;
            }
        }
        

    }

}
