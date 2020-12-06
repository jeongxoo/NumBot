using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ML : MonoBehaviour
{
    public DataSet UserData; // 유저의 플레이 데이터를 모음
    public List<float> distance; // 계산된 거리 데이터를 모으기 위한 리스트
    public List<DataSet> TrainData; // 학습데이터를 가져오기 위한 리스트

    public int kNumber; // 사용자가 설정하는 K개의 데이터를 뽑을때 쓰는 변수

    public float bestLabel; // 거리 계산 후 최적의 레이블을 담기 위함
    public float bestDistance; // 가장 가까운 거리를 갱신하기 위해 필/

    public float[] labelSet; // k개의 레이블들을 담기 위해
    public int countNumber; // 몇번째 거리, 몇번째 트레인데이터 등을 확인하기 위해

    public List<float> saveTime; //데이터를 죽인다음 다시 살리기 위해
    public List<int> saveHP; //데이터를 죽인다음 다시 살리기 위해
    public List<int> saveEnergyProduction; //데이터를 죽인다음 다시 살리기 위해
    public List<int> saveEnergy; //데이터를 죽인다음 다시 살리기 위해
    public List<int> saveShield;
    public List<float> saveResult;

    public float addResult; // 예측값의 평균을 계산하기위해 예측값을 다 담아주는 변수

    public GamePlay gamePlay;
    public Enemy enemy;

    public float mlTime;

    public Image recommendBox;
    public Sprite[] recommendResult;

    private int recommendtime;

    public bool switch_dataset;
    public int switch_dataset_index;
    private StringReader stringReader_set;

    void Start()
    {
        TrainData = new List<DataSet>();
        distance = new List<float>();

        kNumber = 10; // 사용자 지정 k

        labelSet = new float[kNumber];

        saveTime = new List<float>();
        saveHP = new List<int>();        
        saveEnergyProduction = new List<int>();
        saveEnergy = new List<int>();
        saveShield = new List<int>();
        saveResult = new List<float>();

        recommendtime = 1;
        mlTime = 0f;

        switch(switch_dataset_index)
        {
            case 1:
                ReadData("Train");
                break;
            case 2:
                ReadData("Train1");
                break;
            case 3:
                ReadData("Train2");
                break;
        }
    }

    private void Update()
    {
        if (mlTime > 0)
        {
            mlTime -= Time.deltaTime;
        }
        else
        {
            recommendtime++;
            CalculateDistance();
            Debug.Log(recommendtime + "번 째 추천");
            PrintKNN();
            mlTime = 2f;
        }
    }

    public void ReadData(string dataname)
    {
        TrainData.Clear();
        /*
        #region UseIF
        if (switch_dataset == true)
        {
            TextAsset textFile = Resources.Load(dataname) as TextAsset;
            StringReader stringReader_set = new StringReader(textFile.text);
        }
        else
        {
            TextAsset textFile = Resources.Load("Train") as TextAsset;
            StringReader stringReader_set = new StringReader(textFile.text);
        }
        StringReader stringReader = stringReader_set;
        //TextAsset textFile = Resources.Load("Train2") as TextAsset;
        //TextAsset textFile = Resources.Load("Train1") as TextAsset;
        //TextAsset textFile = Resources.Load("Train") as TextAsset;

        #endregion UseIF
        */
        Debug.Log(dataname);
        TextAsset textFile = Resources.Load(dataname) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null) //데이터가 존재한다면 계속 읽어오기   
        {
            string line = stringReader.ReadLine(); // 계속 읽기

            if (line == null)
            {
                break;
            }

            //데이터 생성
            DataSet dataSet = new DataSet();
            dataSet.time = float.Parse(line.Split(',')[0]);
            dataSet.enemyHp = int.Parse(line.Split(',')[1]);
            dataSet.energyProduction = int.Parse(line.Split(',')[2]);
            dataSet.energyAmount = int.Parse(line.Split(',')[3]);
            dataSet.shieldOn = int.Parse(line.Split(',')[4]);
            dataSet.result = int.Parse(line.Split(',')[5]);

            TrainData.Add(dataSet); // TrainData에 나눈 항목들하나씩저장해주기

        }

        stringReader.Close(); //파일 닫기
    }

    public void CalculateDistance()
    {
        UserData.time = GameManager.instance.gTime;
        UserData.enemyHp = GameManager.instance.eHP;
        UserData.energyProduction = gamePlay.changedEnergy;
        UserData.energyAmount = gamePlay.energy;
        FindShield(enemy.shieldOn);

        //Debug.Log("time : " + UserData.time);
        //Debug.Log("eHP : " + UserData.enemyHp);
        //Debug.Log("energyProduction : " + UserData.energyProduction);
        //Debug.Log("energyAmount : " + UserData.energyAmount);
       // Debug.Log("shieldOn : " + UserData.shieldOn);

        distance.Clear();

        for (int i = 0; i < TrainData.Count; i++)
        {
            distance.Add(Mathf.Pow(Twice(TrainData[i].time- UserData.time)
                + Twice(TrainData[i].enemyHp - UserData.enemyHp)
                + Twice(TrainData[i].energyProduction - UserData.energyProduction)
                + Twice(TrainData[i].energyAmount - UserData.energyAmount)
                + Twice((TrainData[i].shieldOn - UserData.shieldOn))
                , 0.5f)); // 거리계산, 재시작횟수 가중치 높히기 위해 *10
        }

        ChooseData();
    }

    public void ChooseData() // 입력 데이터와 거리가 가까운 K개의 데이터의 레이블을 고르는 함수
    {

        addResult = 0;

        for (int j = 0; j < kNumber; j++) // k만큼 반복문 실행
        {
            for (int i = 0; i < distance.Count - 1; i++) // 주어진 데이터들을 다 비교하기 위한 반복문 
            {
                if (distance[i] > distance[i + 1]) // 현재 저장된 가장 가까운 거리가 새로운 거리보다 크다면 갱신
                {
                    bestDistance = distance[i + 1];
                    bestLabel = TrainData[i + 1].result; // 거리가 가장 가까운 놈의 레이블을 저장
                    countNumber = i + 1; // 몇번쩨 데이터를 저장했는지 알기 위해서 > k만큼 반복을 돌릴때 이전에 결과값은 제외하기 위해                    
                }
                else if (distance[i] == distance[i + 1]) // 같아도 갱신
                {
                    bestDistance = distance[i + 1];
                    bestLabel = TrainData[i + 1].result;
                    countNumber = i + 1;
                }
                else
                {
                    bestDistance = distance[i];
                    bestLabel = TrainData[i].result;
                    countNumber = i;
                }
            }

            saveTime.Add(TrainData[countNumber].time); //데이터를 데이터 집합에서 삭제하기전 다시 살리기 위해 백업
            saveHP.Add(TrainData[countNumber].enemyHp);
            saveEnergyProduction.Add(TrainData[countNumber].energyProduction);
            saveEnergy.Add(TrainData[countNumber].energyAmount);
            saveShield.Add(TrainData[countNumber].shieldOn);
            saveResult.Add(TrainData[countNumber].result);

            distance.Remove(distance[countNumber]); // 가장 가까운 거리데이터를 제거
            TrainData.Remove(TrainData[countNumber]); // 가장 가까운 거리데이터의 결과를 가져온 트레인데이터를 제거

            //Debug.Log("111"+TrainData.Count);

            labelSet[j] = bestLabel; // 찾아낸 최근접 데이터의 레이블을 수집
            addResult += labelSet[j]; // 레이블들의 평균을 계산하기 위해 한곳에 계속 더해둠                                
        }

        bestLabel = Mathf.Round(addResult / kNumber); // 최종적으로 입력 데이터의 레이블을 구함
        //Debug.Log("반올림 직전의 값은? : " + addResult / kNumber);
        Debug.Log("선택 결과는?? : " + bestLabel);

        for (int i = 0; i < kNumber; i++) // k개의 데이터, 레이블을 고르는 과정에서 삭제한 트레인 데이터를 복구시켜줌
        {
            DataSet saveData = new DataSet();
            saveData.time = saveTime[i];
            saveData.enemyHp = saveHP[i];
            saveData.energyProduction = saveEnergyProduction[i];
            saveData.energyAmount = saveEnergy[i];
            saveData.shieldOn = saveShield[i];
            saveData.result = saveResult[i];
            TrainData.Add(saveData);
        }

        saveTime.Clear();
        saveHP.Clear();
        saveEnergyProduction.Clear();
        saveEnergy.Clear();
        saveShield.Clear();
        //Debug.Log("222"+TrainData.Count);

    }

    public void PrintKNN()
    {
        switch (bestLabel)
        {
            case 1:
                recommendBox.sprite = recommendResult[0];
                break;
            case 2:
                recommendBox.sprite = recommendResult[1];
                break;
            case 3:
                recommendBox.sprite = recommendResult[2];
                break;
            case 4:
                recommendBox.sprite = recommendResult[3];
                break;
            case 5:
                recommendBox.sprite = recommendResult[4];
                break;
            case 6:
                recommendBox.sprite = recommendResult[5];
                break;
        }
    }

    public void FindShield(bool on)
    {
        if (on)
        {
            UserData.shieldOn = 0;
        }
        else
        {
            UserData.shieldOn = 1;
        }
    }

    public float Twice(float a)
    {
        return a * a;
    }
}

[System.Serializable]
public class DataSet // 데이터를 위한 클래스
{
    public float time = 0;
    public int enemyHp = 0;
    public int energyProduction = 0;
    public int energyAmount = 0;
    public int shieldOn;
    public float result = 0;
}
