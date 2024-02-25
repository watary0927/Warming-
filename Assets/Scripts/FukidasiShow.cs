using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;

public class FukidasiShow : MonoBehaviour
{
    public Text title;
    public Text main;
    public  Sprite[] Icons;
    public Image Mine;
    public Image mychild;
    public Image[] Yours;
    public GameObject BuffedTime;
    public Text debuff;
    public Text Year;
    public GameObject DecideButton;
    public GameObject[] Graphs;//2気温3人口5ニュース
    public GameObject Earth;
    public GameObject controller;

    private void Start()
    {
        BuffedTime.SetActive(false);
        foreach (Image Your in Yours)
        {
            Your.enabled = false;
        }
        Mine.enabled = false;
        DecideButton.SetActive(false);
    }

    public void ZeroStart()//モードがゼロになった時に請求されるやつ
    {
        Graphs[0].SetActive(false);
        Graphs[1].SetActive(false);
        Graphs[2].SetActive(true);
        Graphs[3].SetActive(true);
        Graphs[4].SetActive(false);
        Graphs[5].SetActive(true);
        Year.text = "";
    }

    IEnumerator Close()//ワールド、ローカルを移動した時に請求されるやつ、もしくは展開したやつを再度閉める時に請求されるやつ
    {
        for (ObscuredInt i = 0; i < 6; i++)
        {
            Graphs[i].transform.DOLocalMove(new Vector3(0f, 0f, 0f), 0.15f);
        }
        yield return new WaitForSeconds(0.07f);
        Graphs[0].SetActive(false);
        Graphs[1].SetActive(false);
        Graphs[2].SetActive(false);
        Graphs[3].SetActive(false);
        Graphs[4].SetActive(false);
        Graphs[5].SetActive(false);
        Year.text = "";
    }

    public void OnChangeMode()//モードが変わった時に請求されるやつ
    {
        Graphs[0].SetActive(false);
        Graphs[1].SetActive(false);
        Graphs[2].SetActive(false);
        Graphs[3].SetActive(false);
        Graphs[4].SetActive(false);
        Graphs[5].SetActive(false);
        Year.text = "";
    }

    public void Show(string Title, string Main,int MyColor, ObscuredInt[] YourColor,bool Admission,int year,int num)
    {
        title.text = Title;
        main.text = Main;
        for (ObscuredInt i = YourColor.Length; i < 3; i++)
        {
            Yours[i].enabled = false;
        }
        if (MyColor < 13&&MyColor>=0)
        {
            Mine.sprite = controller.GetComponent<ProsessCommand>().Icons[num];
            mychild.sprite = Icons[MyColor];
            Mine.enabled = true;
            BuffedTime.SetActive(true);
            ObscuredInt s = 0;
            ObscuredFloat k = 0;
            for(ObscuredInt i = 0; i < 21; i++)
            {
                s += controller.GetComponent<EnemyAI>().Areas[i].DebuffedTime[MyColor];
                k += controller.GetComponent<EnemyAI>().Areas[i].Validity;
            }
            ObscuredFloat t = s / 21f; ObscuredFloat m = k / 21f;
            ObscuredFloat r = (ObscuredFloat)System.Math.Pow(controller.GetComponent<ProsessCommand>().BuffPer[ObscuredPrefs.GetInt("Difficulty")], controller.GetComponent<ProsessCommand>().BuffedTime[MyColor]) * (ObscuredFloat)System.Math.Pow((95f - (m/30)) / 100f, t);
            Debug.Log("Buff="+controller.GetComponent<ProsessCommand>().BuffPer[ObscuredPrefs.GetInt("Difficulty")]+",rawper" + r+",s="+s+",k="+k);
            ObscuredInt ans = (ObscuredInt)(r*100);
            debuff.text = ans.ToString()+"%";
            Year.text =year.ToString()+"年";
        }
        else
        {
            Mine.enabled = false;
            Year.text = "";
            BuffedTime.SetActive(false);
        }

        for (ObscuredInt i = 0; i < YourColor.Length; i++) {
            Yours[i].sprite = Icons[YourColor[i]];
            Yours[i].enabled = true;
        }
       
        if (Admission)
        {
            DecideButton.SetActive(true);
        }else
        {
            DecideButton.SetActive(false);
        }
    }
}
