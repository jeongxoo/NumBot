using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    // 유닛 소환을 위해 유닛 프리펩을 받아오기
    public GameObject Man1;
    public GameObject Man2;
    public GameObject Man3;
    public GameObject Man4;
    public GameObject Man5;

    // 소환 위치를 위한 위치 
    public GameObject SpawnPoint;

    // 게임 플레이시간과 현재 에너지 출력을 위한 애들 
    public GameObject textEnergy;
    public GameObject textTime;
    public GameObject textEnergyGain;
    public GameObject textUpgradeCost;
    public GameObject gamePlay;    

    // 캔버스 끄고 키기 위한거
    public Canvas inGame;
    public Canvas timeOver;
   
    // 버튼을 에너지에 따라 키고꺼주기 위해 받아오는 버튼들
    public Button man1Button;
    public Button man2Button;
    public Button man3Button;
    public Button man4Button;
    public Button man5Button;
    public Button upgradeButton;

    // 에너지 충전을 위한 시간과 에너지 자체
    public float energyTime;
    public float changedEnergyTime;
    public int energy;
    public int changedEnergy;
    public int needEnergy; // 업그레이드를 하기위해 필요한 에너지양을 저장하는 변수

    private void Awake()
    {
        changedEnergyTime = 1f;
        energyTime = changedEnergyTime;
        changedEnergy = 1;
        needEnergy = 5;

        GameManager.instance.gTime = 60f;
        GameManager.instance.maxTime = GameManager.instance.gTime;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.instance.PlayTime(textTime, gamePlay, inGame, timeOver);
        GetEnergy(textEnergy);
        ShowEnergyGain(textEnergyGain, textUpgradeCost);
        ChangeAmountOfEnergyWhichUseToUpgradeProduction();

        Button1(energy);
        Button2(energy);
        Button3(energy);
        Button4(energy);
        Button5(energy);
        UpgradeButton(energy);
    }

    #region Energy
    public void GetEnergy(GameObject textEnergy) // 초당 생성되는 에너지를 1로 설#
    {
        energyTime -= Time.deltaTime;

        if (energyTime <= 0f) // 1초마다 에너지 증가 델타타임은 실수기때문에 ==로 하면안됨
        {
            energyTime = changedEnergyTime;
            energy += changedEnergy;
        }

        textEnergy.GetComponent<Text>().text = "Energy : " + energy;
    }

    public void UpgradeEnergy()
    {
        energy -= needEnergy;
        Debug.Log(changedEnergy + " 의 생산량을 가질 때 업그레이드 시 소모되는 비용" + needEnergy);
        changedEnergy += 1;         
    }

    public void ChangeAmountOfEnergyWhichUseToUpgradeProduction()
    {
        if (changedEnergy < 2)
        {
            return;
        }
        else
        {
            needEnergy = Mathf.CeilToInt(Mathf.Pow(1.8f, changedEnergy + 2));
        }        
    }

    public void ShowEnergyGain(GameObject energyGain, GameObject upgradeCost)
    {
        energyGain.GetComponent<Text>().text = "에너지 생산량 : " + changedEnergy; // 남은 시간을 출력해주고
        upgradeCost.GetComponent<Text>().text = "업그레이드 비용 : " + needEnergy;
    }
    #endregion Energy


    #region Spawn
    public void spawnMan1()
    {
        Instantiate(Man1, SpawnPoint.transform.position, Quaternion.identity);
        energy -= 1;
    }

    public void spawnMan2()
    {
        Instantiate(Man2, SpawnPoint.transform.position, Quaternion.identity);
        energy -= 2;
    }

    public void spawnMan3()
    {
        Instantiate(Man3, SpawnPoint.transform.position, Quaternion.identity);
        energy -= 3;
    }

    public void spawnMan4()
    {
        Instantiate(Man4, SpawnPoint.transform.position, Quaternion.identity);
        energy -= 4;
    }

    public void spawnMan5()
    {
        Instantiate(Man5, SpawnPoint.transform.position, Quaternion.identity);
        energy -= 5;
    }
    #endregion Spawn

    #region ButtonFunction
    public void Button1(int energy)
    {
        if (energy >= 1)
        {
            man1Button.interactable = true;
        }
        else
        {
            man1Button.interactable = false;
        }
    }

    public void Button2(int energy)
    {
        if (energy >= 2)
        {
            man2Button.interactable = true;
        }
        else
        {
            man2Button.interactable = false;
        }
    }

    public void Button3(int energy)
    {
        if (energy >= 3)
        {
            man3Button.interactable = true;
        }
        else
        {
            man3Button.interactable = false;
        }
    }

    public void Button4(int energy)
    {
        if (energy >= 4)
        {
            man4Button.interactable = true;
        }
        else
        {
            man4Button.interactable = false;
        }
    }

    public void Button5(int energy)
    {
        if (energy >= 5)
        {
            man5Button.interactable = true;
        }
        else
        {
            man5Button.interactable = false;
        }
    }

    public void UpgradeButton(int energy)
    {
        if (energy >= needEnergy)
        {
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeButton.interactable = false;
        }
    }
    #endregion ButtonFunction
}
