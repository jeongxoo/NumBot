using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameData gameData;

    #region 싱글톤
    static public GameManager instance; // 싱글톤 패턴을위한 클래스 자체 인스턴스화

    private void Awake()
    {
        if (instance == null) //인스턴스가 없다면
        {
            instance = this; //인스턴스에 게임매니저 자신을 초기화
            DontDestroyOnLoad(gameObject); //파방
        }
        else
        {
            Destroy(gameObject); //인스턴스가 이미 존재하기 때문에 새로운 인스턴스는파
        }
        
    }
    #endregion 싱글톤

    private void Start()
    {
        gameData.enemyHP = eHP;
        gameData.Time = gTime;
    }

    // JSON 데이터 save&load
    #region JSON

    // 데이터 저장 함수
    public void SaveGameDataToJson()
    {
        string jsonData = JsonUtility.ToJson(gameData, true);
        string path = Path.Combine(Application.persistentDataPath, "GameData.json");
        File.WriteAllText(path, jsonData);
    }

    // 데이터 로드 함수
    public void LoadGameDataFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "GameData.json");
        string jsonData = File.ReadAllText(path);
        gameData = JsonUtility.FromJson<GameData>(jsonData);
    }

    #endregion JSON

    public int eHP;
    public float gTime;
    public float maxTime;

    public void PlayTime(GameObject timeText, GameObject gamePlay, Canvas inGame, Canvas timeOver) // 게임 제한 시간을 측정하는 함수
    {
        if (gTime > 0) // 지 타임이 영보다 크다면(제한시간이 남아있다면)
        {
            gTime -= Time.deltaTime; //제한시간을 감소시키고
            gameData.Time = gTime; // 제한시간을 데이터에 넣고
            timeText.GetComponent<Text>().text = "PlayTime : " + (int)gTime; // 남은 시간을 출력해주고
            SaveGameDataToJson(); // 데이터를 저장한다.
        }
        else if (gTime <= 0) // 제한시간이 빵이하로되(
        {
            StopWhenTimeOver(gamePlay); //게임플레이를 파괴하고, 졌다는 문구 출력
            TimeOver(inGame, timeOver); // 메인으로 가는 버튼과 재시작 버튼 활성화
        }

    }

    public void StopWhenEnemyDie(GameObject gamePlay, GameObject win, GameObject playtime) // 적이 죽으면 게임 종료되는 함수
    {
        if (eHP <= 0) // 적의 체력이 0이하로 떨어지면
        {
            ShowGameInfo(win, playtime);
            Destroy(gamePlay); // 게임플레이 스크립트를 파괴
        }
        
    }

    public void StopWhenTimeOver(GameObject gamePlay) // 제한시간이 지나면
    {

        Destroy(gamePlay); // 게임플레이 스크립트를 파괴
    }

    public void TimeOver(Canvas inGame, Canvas timeOver) //제한시간이 지나면
    {
        inGame.gameObject.SetActive(false); // 인게임 캔버스는 비활
        timeOver.gameObject.SetActive(true); // 타임오버 캔버스는 활성화
    }

    public void ShowGameInfo(GameObject win, GameObject playtime) // 그판의 정보를출력
    {
        win.GetComponent<Text>().text = "WIN!!!!!"; // 이겼다는 문구 출력
        playtime.GetComponent<Text>().text = "play time : " + (maxTime - gTime); // 총 플레이 시간 알려줌
    }
}

[System.Serializable]
public class GameData
{
    public float Time;
    public int enemyHP;
}