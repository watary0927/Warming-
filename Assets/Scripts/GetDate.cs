using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;

public class GetDate : MonoBehaviour
{
    public GameObject controller;
    public Text Date;
    public ObscuredFloat[] IncreasedTemp;
    public ObscuredInt startyear=2020;//開始年
    public ObscuredInt CurrentYear=2020;
    private void Start()
    {
       IncreasedTemp = new ObscuredFloat[ObscuredPrefs.GetInt("Year") + 1];
       IncreasedTemp[0] = 0;
        ObscuredPrefs.SetFloat("IncreasedTemp" + 0, IncreasedTemp[0]);
        for (ObscuredInt i = 0; i < 21; i++)
        {
            controller.GetComponent<EnemyAI>().Areas[i].DangerbyYear[0] = controller.GetComponent<EnemyAI>().Areas[i].Danger;
        }
        Debug.Log("CurrentYear="+ CurrentYear);
    }
    // Update is called once per frame
    
    ObscuredInt CurrentMonth = 0;
    void Update()
    {
        //期間確認フェイズ  
        ObscuredInt PassedMonths =(ObscuredInt) (controller.GetComponent<Timer>().GetTime() / 1.25f);
        //Debug.Log(controller.GetComponent<Timer>().GetTime());
        ObscuredInt Year = (PassedMonths / 12)+startyear;
        ObscuredInt Month =(PassedMonths % 12)+1;
        //Debug.Log(PassedMonths);
        if (CurrentYear != Year)
        {
            CurrentYear = Year;
            /*現在気温を取得*/
            IncreasedTemp[Year - 2020] = controller.GetComponent<Main>().Temperature;
            DOVirtual.DelayedCall(0.1f,()=>controller.GetComponent<AutoSaver>().AutoSave());
            /*Debug.Log(CurrentYear);
            Debug.Log(MenuController.Year + 2020);*/
            if (CurrentYear == ObscuredPrefs.GetInt("Year")+ 2020)
            {
                controller.GetComponent<Timer>().Stop();
                /*終了処理*/
                /*なんかFinish!とか表示したい*/
                Debug.Log("finish");
                controller.GetComponent<Main>().EndActivate();
            }
            for (ObscuredInt i = 0; i < 21; i++)
            {
                controller.GetComponent<EnemyAI>().Areas[i].DangerbyYear[CurrentYear - 2020] = controller.GetComponent<EnemyAI>().Areas[i].Danger;
            }
            if (controller.GetComponent<MissionManager>().MissionDone < 4 && controller.GetComponent<MissionManager>().WhichMission[controller.GetComponent<MissionManager>().MissionDone] <= 1) {
                Tuple<ObscuredInt, ObscuredInt> t = controller.GetComponent<MissionManager>().StartTime[controller.GetComponent<MissionManager>().MissionDone][controller.GetComponent<MissionManager>().WhichMission[controller.GetComponent<MissionManager>().MissionDone]];
                //ミッション　気温上昇確認フェイズ
                if (controller.GetComponent<MissionManager>().MissionGoing && controller.GetComponent<MissionManager>().missionType == 1 && controller.GetComponent<Main>().Temperature >= IncreasedTemp[t.Item1] + controller.GetComponent<MissionManager>().TList[(controller.GetComponent<MissionManager>().missionDuration / 5) - 1])
                {
                    Debug.Log("Mission1Completed");
                    controller.GetComponent<Timer>().Stop();
                    controller.GetComponent<MissionManager>().OnClearMission();
                }
                if (CurrentYear - 2020 == t.Item1 && controller.GetComponent<MissionManager>().MissionDone != controller.GetComponent<MissionManager>().maxmissions)
                {//ミッション発動処理
                    controller.GetComponent<Timer>().Stop();
                    controller.GetComponent<MissionManager>().MissionPresenter();
                }
                if (CurrentYear - 2020 == t.Item1 + t.Item2 && controller.GetComponent<MissionManager>().MissionGoing == true && controller.GetComponent<MissionManager>().MissionDone != controller.GetComponent<MissionManager>().maxmissions)
                {//時間切れ失敗処理
                    controller.GetComponent<Timer>().Stop();
                    controller.GetComponent<MissionManager>().OnFailMission();
                }
            }
            
        }
        if (CurrentMonth != Month)
        {
            CurrentMonth = Month;
            if(Month<10)Date.text = $"{Year}年0{Month}月";
            else Date.text = $"{Year}年{Month}月";
            if (CurrentYear == ObscuredPrefs.GetInt("Year") + 2019&&CurrentMonth==9)
            {
                controller.GetComponent<Main>().Starters[1].SetActive(true);
                controller.GetComponent<Main>().Starters[1].GetComponent<Image>().DOFade(0f, 0.5f);
            }
            if (CurrentYear == ObscuredPrefs.GetInt("Year") + 2019 && CurrentMonth == 10)
            {
                controller.GetComponent<Main>().Starters[2].SetActive(true);
                controller.GetComponent<Main>().Starters[2].GetComponent<Image>().DOFade(0f, 0.5f);
            }
            if (CurrentYear == ObscuredPrefs.GetInt("Year") + 2019 && CurrentMonth == 11)
            {
                controller.GetComponent<Main>().Starters[3].SetActive(true);
                controller.GetComponent<Main>().Starters[3].GetComponent<Image>().DOFade(0f, 0.5f);
            }
            //危機感の値を各自政策系統に追加
            //政策の有効性、経済を回収技術に追加
            controller.GetComponent<EnemyAI>().MonthlyAddition();
        }
    }
}
