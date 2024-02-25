using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;

public class MissionManager : MonoBehaviour
{
    public GameObject controller;
    public GameObject MissionBlack;
    public GameObject MissionWhite;
    public GameObject Mission;
    public Text title;
    public Text main;
    public GameObject line;
    public GameObject Frag;
    public GameObject Earth;
    public Button DecideButton;
    public GameObject db;

    public ObscuredInt[] WhichMission = new ObscuredInt[4];


    public ObscuredString titletext;
    public ObscuredString maintext;

    public ObscuredInt MissionDone = 0;//ミッションが開催された回数
    public ObscuredBool MissionGoing = false;//ミッションが現在開催されているか

    ObscuredBool opened = false;

    public ObscuredInt missionclear=0;

    void Start()
    {
        maxmissions = ObscuredPrefs.GetInt("Year")/ 20;
       // Frag.SetActive(false);
        MissionBlack.transform.localScale = new Vector3(0f, 1f, 1f);
        MissionWhite.transform.localScale = new Vector3(1f, 0f, 1f);
        line.transform.localScale = new Vector3(0f, 1f, 1f);
        setText("Unupdated", "ミッションをお待ちください");
        for (ObscuredInt i = 0; i < ObscuredPrefs.GetInt("Year") / 20; i++)
        { ObscuredInt k = -1;
            while (k <= -1 || k >= 2)
            {
                k = UnityEngine.Random.Range(0, 2);
            }
            WhichMission[i] = k;
        }
    }

    public void setText(string tt, string mt)
    {
        titletext = tt;
        maintext = mt;
    }

    public void close()
    {
        if (opened == true)
        {
            opened = false;
            DecideButton.interactable = true;
            StartCoroutine("Close");
        }
    }

    IEnumerator Close()
    {
        title.text = "";
        main.text = "";
        line.transform.DOScale(new Vector3(0f, 1f, 1f), 0.15f);
        yield return new WaitForSeconds(0.15f);
        MissionWhite.transform.DOScale(new Vector3(1.2f, 0f, 1f), 0.15f);
        yield return new WaitForSeconds(0.15f);
        MissionBlack.transform.DOScale(new Vector3(0f, 1f, 1f), 0.15f);
        MissionWhite.transform.localScale = new Vector3(1f, 0f, 1f);
    }

    public void open()
    {
        Debug.Log(MissionGoing);
        controller.GetComponent<EnemyAI>().Scrollview.SetActive(false);
        Earth.GetComponent<PutPins>().chartPanel.SetActive(false);
        if (opened == false)
        {
            db.GetComponent<ChangeMode>().ShowMode(0);
            DecideButton.interactable = false;
            opened = true;
            StartCoroutine("Open");
        }
        else if (opened == true)
        {
            opened = false;
            DecideButton.interactable = true;
            StartCoroutine("Close");
        }
    }

    IEnumerator Open()
    {
        MissionBlack.transform.DOScale(new Vector3(1f, 1f, 1f), 0.15f);
        yield return new WaitForSeconds(0.15f);
        MissionWhite.transform.DOScale(new Vector3(1f, 1f, 1f), 0.15f);
        yield return new WaitForSeconds(0.15f);
        line.transform.DOScale(new Vector3(0.5f, 0.5f, 1f), 0.15f);
        yield return new WaitForSeconds(0.15f);
        title.text = titletext;
        main.text = maintext;
    }

    public ObscuredInt missionType = -1;
    public ObscuredInt missionDuration = 0;
    public ObscuredInt maxmissions;
    public ObscuredInt missionCommand = -1;//コマンドミッションの時の目標

    public Tuple<ObscuredInt, ObscuredInt>[][] StartTime = new Tuple<ObscuredInt, ObscuredInt>[][] { new Tuple<ObscuredInt, ObscuredInt>[] { new Tuple<ObscuredInt, ObscuredInt>(5, 10), new Tuple<ObscuredInt, ObscuredInt>(10, 5) }, new Tuple<ObscuredInt, ObscuredInt>[] { new Tuple<ObscuredInt, ObscuredInt>(20, 15), new Tuple<ObscuredInt, ObscuredInt>(25, 15) }, new Tuple<ObscuredInt, ObscuredInt>[] { new Tuple<ObscuredInt, ObscuredInt>(45, 10), new Tuple<ObscuredInt, ObscuredInt>(50, 5) }, new Tuple<ObscuredInt, ObscuredInt>[] { new Tuple<ObscuredInt, ObscuredInt>(65, 10), new Tuple<ObscuredInt, ObscuredInt>(60, 15) } };//順番、選択肢,<開始年、期間>



    public void MissionPresenter()
    {//ミッションが発動するときだけCallされる
        Frag.SetActive(true);
        ObscuredInt Selection = UnityEngine.Random.Range(0, 2);
        if (Selection == 0) {
            missionType = 0;
            CommandMission(StartTime[MissionDone][WhichMission[MissionDone]].Item2);
        }
        else if (Selection == 1)
        {
            missionType = 1;
            TempMission(StartTime[MissionDone][WhichMission[MissionDone]].Item2);
        }
       /* else if (Selection == 2)
        {
            missionType = 2;
            DangerMission(StartTime[MissionDone][WhichMission[MissionDone]].Item2);
        }*/
        else
        {
            MissionPresenter();//再抽選
            return;
        }
        missionDuration=StartTime[MissionDone][WhichMission[MissionDone]].Item2;
        MissionGoing = true;
    }

    public void OnClearMission()
    {//ミッションがクリアされた時に提示されるやつ
        //クリアイメージの提示
        //報酬の加算
        if (missionType == 0)
        {
            setText("ミッション成功", "クリア報酬がコマンドに適用されました");
            controller.GetComponent<ProsessCommand>().BuffedTime[controller.GetComponent<CommandList>().Commands[missionType].MyIcon] += missionDuration / 5;
        }else if (missionType == 1)
        {
            setText("ミッション成功", "クリア報酬が現在気温に適用されました");
            //達成目標の半数を加算
            controller.GetComponent<Main>().MissionalTemp += TList[(missionDuration / 5) - 1]/2;
        }
        else if (missionType == 2)
        {
            setText("ミッション成功", "クリア報酬が全世界の危機感に適用されました");
            //達成目標の半数を加算
            for (ObscuredInt i = 0; i < 21; i++)
            {
                controller.GetComponent<EnemyAI>().Areas[i].Danger -= DList[(missionDuration / 5) - 1] / 2;
            }
        }
        missionclear++;
        MissionGoing = false;
        MissionDone++;
        open();
    }

    public void OnFailMission()
    {//時間ぎれの時に表示されるやつ
     //失敗イメージの表示
     //ペナルティの加算
        if (missionType == 0)
        {
            setText("ミッション失敗", "失敗ペナルティがコマンドに適用されました");
            //達成目標だったコマンド分の減算を行う
            for (ObscuredInt i = 0; i < 21; i++)
            {
                controller.GetComponent<EnemyAI>().Areas[i].DebuffedTime[controller.GetComponent<CommandList>().Commands[missionType].MyIcon] += missionDuration / 5;
            }
        }
        else if (missionType == 1)
        {
            setText("ミッション失敗", "失敗ペナルティが現在気温に適用されました");
            //達成目標分気温を低下させる
            controller.GetComponent<Main>().MissionalTemp -= TList[(missionDuration / 5) - 1];
        }
        else if (missionType == 2)
        {
            //達成目標分世論を上昇させる
            for(ObscuredInt i = 0; i < 21; i++)
            {
                setText("ミッション失敗", "失敗ペナルティが全世界の危機感に適用されました");
                controller.GetComponent<EnemyAI>().Areas[i].Danger += DList[(missionDuration / 5) - 1];
            }
        }
        MissionGoing = false;
        MissionDone++;
        open();
    } 


    void CommandMission(int duration)
    {//コマンド系のミッション
        missionCommand = CList[(duration / 5) - 1][UnityEngine.Random.Range(0, CList[(duration / 5) - 1].Length)];
        var SelectedCommand = controller.GetComponent<CommandList>().Commands[missionCommand];
            
        setText("コマンドを実行せよ", $"「{SelectedCommand.title}」の影響力が低下している。このままでは「{SelectedCommand.title}」は衰退してしまうだろう。\n{duration}年以内に「{SelectedCommand.title}」を実行せよ。失敗すれば[ジャンル{SelectedCommand.MyIcon+1}]の効果が低下する。但し、成功すれば[ジャンル{SelectedCommand.MyIcon + 1}]の効果が上昇する。");
        
        open();
    }

    void TempMission(int duration)
    {//気温系のミッション

        setText("環境を破壊せよ", $"地球環境が改善の兆しを見せている。早いうちに芽は摘まねばならない。\n{duration}年以内に気温を「{TList[(duration / 5) - 1]}℃」上昇させろ。失敗すれば世界全体の気温が低下する。但し、成功すれば世界全体の気温が上昇する。");
        open();
    }

    void DangerMission(int duration)
    {//危機感系のミッション
        ObscuredInt SelectedArea = UnityEngine.Random.Range(0, 21);
        setText("危機感を鎮めよ", $"{controller.GetComponent<EnemyAI>().Areas[SelectedArea].Jtitle}で環境問題に対する意識が高まりつつある。市民感情は政策や企業戦略にも影響を与えうる。{duration}年以内に{controller.GetComponent<EnemyAI>().Areas[SelectedArea].Jtitle}の危機感を{DList[(duration / 5) - 1]}低下させろ。失敗すれば世界全体の危機感が増加する。但し、成功すれば世界全体の危機感が低下する。");
        open();
    }

    public ObscuredInt[][] CList = new ObscuredInt[][] { new ObscuredInt[] { 0, 2, 5, 8, 10, 13, 20, 22, 24, 30, 31, 34, 35, 37, }, new ObscuredInt[] { 1, 3, 4, 6, 9, 11, 14, 15, 23, 25, 32, 38, 40 }, new ObscuredInt[] { 7, 12, 16, 18, 26, 28, 33, 36, 39, 41 }, new ObscuredInt[] { 17, 19, 27, 29 } };
    public ObscuredFloat[] TList = new ObscuredFloat[] {0.25f,0.5f,0.75f,1.5f};
    public ObscuredFloat[] DList = new ObscuredFloat[] {3,6,9,15};

}