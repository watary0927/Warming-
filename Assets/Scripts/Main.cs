using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using GoogleMobileAds.Api;

public class Main : MonoBehaviour
{
    public GameObject controller;
    public Button Change;
    public GameObject[] Starters;
    private ObscuredFloat CountdownTime;
    public ObscuredFloat Temperature;
    ObscuredBool[] CountAdmissions = new ObscuredBool[5];
    public GameObject Earth;
    public GameObject[] Alarms;
    public Text title, main;
    public GameObject PinsParent;
    public GameObject DecideButton;//ChnageModeからModeを請求するためだけの存在
    public GameObject Burner;
    public Material earthcolor;
    public Text Current;
    public AudioSource button,Countdown, last, start, Finish, hibana,bgm1,bgm2;
    public Image[] tapes;
    public Button configuration;
    public RectTransform Fukidashi;
    public Button Frag;
    private string[] difname = new string[4] { "Easy", "Normal", "Hard", "Noah" };

    public void SoundActivate(int i)
    {
        if (i == 0)
        {
            button.Play();
        }
        else if (i == 1)
        {
            Countdown.Play();
        }
        else if (i == 2)
        {
            last.Play();
        }
        else if (i == 3)
        {
            start.Play();
        }
        else if (i == 4)
        {
            Finish.Play();
        }
        else if (i == 5)
        {
            hibana.Play();
        }else if (i == 6)
        {
            bgm1.Play();
        }
        else if (i == 7)
        {
            bgm2.Play();
        }
        else return;
    }

    void Start()
    {
        //Debug.Log("Difficulty="+ObscuredPrefs.GetInt("Difficulty"));
        StartCoroutine("StartScene");
        AlarmErase();
        Change.interactable = false;
        Frag.interactable = false;
        /* var size=Fukidashi.sizeDelta;
         size.x = size.x * (ObscuredFloat)System.Math.Pow(2436f/1125f*Screen.width/Screen.height, 0.98);
         Fukidashi.sizeDelta = size;*/
        foreach (GameObject Starter in Starters)
        {
            Starter.SetActive(false);
        }
        for (int i = 0; i < 5; i++)
        {
            CountAdmissions[i] = true;
        }
        Burner.transform.position = new Vector3(0f,-10f,0f);
        configuration.interactable = false;
    }

    IEnumerator StartScene()
    {
        if (ObscuredPrefs.HasKey("HasSaveData") && ObscuredPrefs.GetInt("HasSaveData") == 1)
        {
            if (tapes[0].enabled == true)
            {
                tapes[0].enabled = false;
                tapes[1].enabled = false;
            }
        }
        controller.GetComponent<OnSceneChange>().Scenein();
        Earth.GetComponent<PutPins>().OnLeave();
        yield return new WaitForSeconds(0.5f);
        if (ObscuredPrefs.HasKey("HasSaveData") && ObscuredPrefs.GetInt("HasSaveData") == 1)
        {
            controller.GetComponent<AutoSaver>().Rewrite();
            CountAdmissions[0] = false;
            GameStart();
        }
        else
        {
            CountDown(4);
            CountdownTime = 0f;
        }
    }

    public void EndActivate()
    {
        StartCoroutine("EndScene");
    }

    IEnumerator EndScene()
    {
        SoundActivate(4);
        for (int i = 0; i < 2; i++) Alarms[i].SetActive(false);
        controller.GetComponent<AdShower>().Destroy();
        Earth.GetComponent<PutPins>().arriveallowed = false;
        Earth.GetComponent<PutPins>(). bararea.enabled = false;
        Earth.GetComponent<PutPins>().barcommand.enabled = false;
        Earth.GetComponent<PutPins>().graph.enabled = false;
        DecideButton.GetComponent<ChangeMode>().SS.SetActive(false);
        DecideButton.GetComponent<ChangeMode>().SE.SetActive(false);
        DecideButton.GetComponent<ChangeMode>().Panel.SetActive(false);
        controller.GetComponent<MissionManager>().MissionBlack.SetActive(false);
        tapes[0].enabled = true;
        tapes[1].enabled = true;
        tapes[1].transform.DOLocalMove(new Vector3(0f, 200f, 0f), 0.2f);
        tapes[0].transform.DOLocalMove(new Vector3(0f, 200f, 0f), 0.2f);
        Starters[0].SetActive(true);
        Starters[3].SetActive(false);
        Starters[5].SetActive(true);
        yield return new WaitForSeconds(1f);
        controller.GetComponent<OnSceneChange>().Sceneout();
        yield return new WaitForSeconds(0.25f);
        Starters[0].SetActive(false);
        Starters[5].SetActive(false);
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene("ResultScene");
    }


    public ObscuredFloat MissionalTemp = 0;
    public GameObject[] starboxes;

    private void Update()
    {
        ObscuredFloat red = (ObscuredFloat)(Temperature / 10f);
        if (red > 1) red = 1;
        if (red < 0) red = 0;
        //earthcolor.SetColor("_EmissionColor", new Color(red,0,0,0));
        Burner.transform.position = new Vector3(0f,-10f+red*3,0f);
        ObscuredInt k = UnityEngine.Random.Range(45, 136);
        Temperature = controller.GetComponent<EnemyAI>().TotalCO2Amount * 0.0006f+ MissionalTemp;
        if (CountAdmissions[0] == true)
        {
            CountdownTime += Time.deltaTime;
            if (CountdownTime >= 1f && CountAdmissions[3] == true)
            {
                CountDown(3);
            }
            if (CountdownTime >= 2f && CountAdmissions[2] == true)
            {
                CountDown(2);
            }
            if (CountdownTime >= 3f && CountAdmissions[1] == true)
            {
                CountDown(1);
            }
            if (CountdownTime >= 4f && CountAdmissions[0] == true)
            {
                CountDown(0);
            }
        }
        ObscuredDouble temp = System.Math.Round((ObscuredFloat)(Temperature * 10f)) / 10;
        Current.text = $"+{temp}°C";
    }

    void CountDown(int num)
    {
        CountAdmissions[num] = false;
        if (num == 4) {
            Starters[0].SetActive(true);
            Starters[1].SetActive(true);
            Starters[2].SetActive(false);
            Starters[3].SetActive(false);
            Starters[4].SetActive(false);
            SoundActivate(1);
        }
        else if (num == 3)
        {
            Starters[0].SetActive(true);
            Starters[1].SetActive(false);
            Starters[2].SetActive(true);
            Starters[3].SetActive(false);
            Starters[4].SetActive(false);
            SoundActivate(1);
        }
        else if (num == 2)
        {
            Starters[0].SetActive(true);
            Starters[1].SetActive(false);
            Starters[2].SetActive(false);
            Starters[3].SetActive(true);
            Starters[4].SetActive(false);
            SoundActivate(1);
        }
        else if (num == 1)
        {
            Starters[0].SetActive(true);
            Starters[1].SetActive(false);
            Starters[2].SetActive(false);
            Starters[3].SetActive(false);
            Starters[4].SetActive(true);
            SoundActivate(2);
            tapes[0].transform.DOLocalMove(new Vector3(-1500f, -700f, 0f), 0.2f);
            tapes[1].transform.DOLocalMove(new Vector3(1500f, -700f, 0f), 0.2f);
            DOVirtual.DelayedCall(0.2f, () => { tapes[0].enabled=false; });
            DOVirtual.DelayedCall(0.2f, () => { tapes[1].enabled = false; });
        }
        else if (num == 0)
        {
            SoundActivate(ObscuredPrefs.GetInt("BGM")+6);
            Starters[0].SetActive(false);
            Starters[1].SetActive(false);
            Starters[2].SetActive(false);
            Starters[3].SetActive(false);
            Starters[4].SetActive(false);
            GameStart();
        }
    }

    public void GameStart()
    {
        configuration.interactable = true;
        Frag.interactable = true;
        Earth.GetComponent<PutPins>().arriveallowed = true;
        Earth.GetComponent<PutPins>().OnArrive();
        controller.GetComponent<EnemyAI>().AlStarted = true;
        Debug.Log("GameStart");
        Change.interactable = true;
        PinsParent.SetActive(true);
        controller.GetComponent<Timer>().StartTimer(ObscuredPrefs.GetInt("Year") * 100f+10f, false, 0);
        TutorialSection(0,"");
    }

    public GameObject QuitGame;

    void AlarmShow(string Title, string Main,bool quit)
    {
        Debug.Log("AlarmShowCalled");
        title.text = Title; main.text = Main;
        for (ObscuredInt i = 0; i < 2; i++) Alarms[i].SetActive(true);
        //タイマー止める
        controller.GetComponent<Timer>().Stop();
        if (quit) QuitGame.SetActive(true);
        else QuitGame.SetActive(false);
    }
    public void AlarmErase()
    {
        //タイマー再起動
        for (ObscuredInt i = 0; i < 2; i++) Alarms[i].SetActive(false);
        if(controller.GetComponent<Timer>().IsStop())controller.GetComponent<Timer>().Resume();
    }

    public void TutorialSection(int i,string s)
    {
        if (ObscuredPrefs.HasKey("Tutorial" + i)) return;
        ObscuredPrefs.SetInt("Tutorial" + i, 1);
        if (i == 0) AlarmShow("チュートリアル1", "右下の青いボタンを押して、温暖化のためのコマンドを確認しましょう", false);
        else if (i == 1) AlarmShow("チュートリアル2", "環境コマンドです。使用すると大気中の温室効果ガスの量が増加しますが、市民の危機感も増加します。", false);
        else if (i == 2) AlarmShow("チュートリアル3", "社会コマンドです。環境の悪化による対策の激化を抑制しましょう。", false);
        else if (i == 3) AlarmShow("チュートリアル4", $"あなたは{s}に着手しました。未だコマンドの効果は反映されていないことに注意してください。コマンドが効果を発揮するのは、ワームタイムの経過後です。", false);
        else if (i == 4) AlarmShow("チュートリアル5", "ワームタイムが経過したのでコマンドが実行されました。", false);
        else if (i == 5) AlarmShow("チュートリアル6", "ピンをタップすると、各地域の詳細情報が閲覧できます。的確な戦略の参考にしましょう。", false);
        else if (i == 6) AlarmShow("チュートリアル7", "赤いピンは、タップすると着手中のコマンドのワームタイムを1年間短縮します。一定時間で消えてしまうので、定期的にチェックしましょう。", false);
        else return;
    }

    public void Configuration()
    {//音量設定とか？が入る気がする
        Earth.GetComponent<PutPins>().barcommand.enabled = false;
        Earth.GetComponent<PutPins>().bararea.enabled = false;
        Earth.GetComponent<PutPins>().graph.enabled = false;
        Earth.GetComponent<PutPins>().chartPanel.SetActive(false);
        controller.GetComponent<EnemyAI>().Scrollview.SetActive(false);
        AlarmShow($"設定(難易度:{difname[ObscuredPrefs.GetInt("Difficulty")]},期間:{ObscuredPrefs.GetInt("Year")}年)", "", true);
    }

    /*public void back(){//これセーブするか決めないけないやつじゃん
        Debug.Log("BackCalled");
        AlarmShow("メニューに戻る","※データはオートセーブにより保存されます",true);
    }*/

    public void SaveGame()//ゲームをセーブする
    {
        Debug.Log("GameSaved");
        controller.GetComponent<OnSceneChange>().Sceneout();
        DOVirtual.DelayedCall(1f, ()=> SceneManager.LoadScene("MenuScene"));
    }
    public void CurrentTemp()
    {
        AlarmErase();
        if (DecideButton.GetComponent<ChangeMode>().Mode == 0)
        {
            Earth.GetComponent<PutPins>().initialize();
            Earth.GetComponent<PutPins>().CurrentArea = 21;
            Earth.GetComponent<PutPins>().ALLBleach();
            controller.GetComponent<FukidasiShow>().Show("Home", "", 13, new ObscuredInt[] { }, false,0,0);
            if (controller.GetComponent<Timer>().IsStop()) controller.GetComponent<Timer>().Resume();
        }
    }
}
