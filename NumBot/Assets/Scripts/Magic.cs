using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public int s_count = 0;
    public int s_Destroy = 5;

    public GameObject magicShield;

    public GameObject fireBall;

    public void StrengthOfShield()
    {
        if (s_count >= s_Destroy)
            Destroy(magicShield);
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
                    StrengthOfShield();
                    break;

                case "Man3":
                    s_count++;
                    StrengthOfShield();
                    break;

                case "Man4":
                    s_count++;
                    StrengthOfShield();
                    break;

                case "Man5":
                    s_count++;
                    StrengthOfShield();
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
