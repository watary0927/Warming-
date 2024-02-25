using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using ChartAndGraph;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Detectors;
using CodeStage.AntiCheat.Storage;


public class ResultController : MonoBehaviour
{
    // Start is called before the first frame update
    public Image[] Ranks;
    public GameObject[] Parents;
    public Sprite[] RankSprites;
    public GameObject GoToMenu;
    public Image Result;
    public GameObject controller;

    public Text[] texts;

    ObscuredInt sum = 10;


    private string[] difname = new string[4] { "Easy", "Normal", "Hard", "Noah" };

    void Start()
    {
        sum = 10;
        controller.GetComponent<AutoSaver>().BlockAccess();
        for (ObscuredInt i = 0; i < 5; i++)
        {
            Ranks[i].enabled = false;
            Parents[i].SetActive(false);
        }
        texts[0].text = "期間:" + ObscuredPrefs.GetInt("Year") + "年";
        texts[1].text = "難易度:" + difname[ObscuredPrefs.GetInt("Difficulty")];
        Result.enabled = false;
        GoToMenu.SetActive(false);
        StartCoroutine("StartScene");
    }
    public GraphChart graph;

    public AudioSource rank, uchiwake;

    IEnumerator StartScene()
    {
        ObscuredInt[] results = new ObscuredInt[6];
        for(ObscuredInt i = 0; i < 6; i++)
        {
            results[i] = ResultDecide(i);
            sum -= results[i];
        }
        
        controller.GetComponent<OnSceneChange>().Scenein();
        yield return new WaitForSeconds(0.5f);
        
        //ここからグラフ処理です
        graph.transform.DOScale(new Vector3(1.44f, 1.44f, 1.44f), 0.15f);
        //ここから成績処理です
        yield return new WaitForSeconds(0.15f);
        for (ObscuredInt i = 0; i < 5; i++)
        {
            uchiwake.Play();
            Ranks[i].sprite = RankSprites[results[i]];//暫定処
            Ranks[i].SetNativeSize();
            Ranks[i].enabled = true;
            Parents[i].SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }

        rank.Play();
        Result.sprite = RankSprites[results[5]];//暫定処置
        Result.SetNativeSize();
        Result.enabled = true;
        Result.transform.DORotate(new Vector3(0f, 0f, 360f), 0.5f, RotateMode.FastBeyond360);
        Result.transform.DOScale(new Vector3(3f, 3f, 3f), 0.5f);

        if(ObscuredPrefs.HasKey("Result" + ObscuredPrefs.GetInt("Difficulty") + ObscuredPrefs.GetInt("Year"))) {
            ObscuredInt a = ObscuredPrefs.GetInt("Result" + ObscuredPrefs.GetInt("Difficulty") + ObscuredPrefs.GetInt("Year"));
            int res = a < results[5] ? a : results[5];
            ObscuredPrefs.SetInt("Result" + ObscuredPrefs.GetInt("Difficulty") + ObscuredPrefs.GetInt("Year"), res);
        }
        else ObscuredPrefs.SetInt("Result" + ObscuredPrefs.GetInt("Difficulty") + ObscuredPrefs.GetInt("Year"), results[5]);

        yield return new WaitForSeconds(0.5f);
        GoToMenu.SetActive(true);
    }

    IEnumerator EndScene()
    {
        controller.GetComponent<AdShower>().Destroy();
        controller.GetComponent<OnSceneChange>().Sceneout();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MenuScene");
    }
    ObscuredInt ResultDecide(ObscuredInt i)
    {
        ObscuredInt Year = ObscuredPrefs.GetInt("Year")/ 20;

        //環境 使用したコマンドの回数
        if (i == 0)
        {
            ObscuredInt r= ObscuredPrefs.GetInt("ECommandNum") / Year;
            if (r >= 15) return 0;
            else if (r >= 10) return 1;
            else if (r >= 5) return 2;
            else return 3;
        }
        //政治　上に同じ
        if (i == 1)
        {
            ObscuredInt r = ObscuredPrefs.GetInt("SCommandNum") / Year;
            if (r >= 15) return 0;
            else if (r >= 10) return 1;
            else if (r >= 5) return 2;
            else return 3;
        }
        //対策　使用された対策コマンドの回数
        if (i == 2)
        {
            ObscuredInt r = ObscuredPrefs.GetInt("PoliticsDone") / Year;
            if (r >= 500)
            {
                return 3;
            }
            else if (r >= 300)
            {
                return 2;
            }
            else if (r >= 100)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        //二酸化炭素回収　回収総量
        if (i == 3)
        {
            ObscuredInt r =(ObscuredInt)ObscuredPrefs.GetFloat("AllCollection") / Year;
            if (r >= 150)
            {
                return 3;
            }else if (r >= 100)
            {
                return 2;
            }else if (r >= 50)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        //ミッション成功 ミッション解決回数
        if (i == 4)
        {
            ObscuredInt res = Year - ObscuredPrefs.GetInt("missionclear");
            if (res >= 0 && res <= 3) return res;
            else return 3;
        }
        if (i == 5)
        { ObscuredFloat resulttemp = ObscuredPrefs.GetFloat("IncreasedTemp" + ObscuredPrefs.GetInt("Year")) + (sum * 0.1f);
            //Debug.Log("resulttemp=" + resulttemp + "IncreasedTemp" + ObscuredPrefs.GetFloat("IncreasedTemp" + ObscuredPrefs.GetInt("Year")) + "Year" + Year + "sum" + sum);
            if (resulttemp >= 1f+ Year)
            {
                return 0;
            }
            else if (resulttemp >= 0.5f+ Year)
            {
                return 1;
            }
            else if (resulttemp >= Year)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
        return 3;
    }
    


    public void Go()
    {
        StartCoroutine("EndScene");
    }

}
