using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;

public class PhenoManager : MonoBehaviour
{

    //温度上昇に比例した確率で赤にする
    //30秒たった段階で消去する
    //そのボタンがタップされた時にも色を消去+コマンドの待機時間を1年進める
    public GameObject controller;
    public GameObject Earth;
    ObscuredFloat PassedTime = 0;

    // Update is called once per frame
    ObscuredInt num = 0;
    void Update()
    {
        PassedTime += Time.deltaTime;
        if ((PassedTime >= 1||(PassedTime >= 0.5f && controller.GetComponent<ProsessCommand>().DoubleSpeed))&& controller.GetComponent<Timer>().IsWork() && !controller.GetComponent<Timer>().IsStop())
        {
            PassedTime = 0;
            RedTrial(num);
            num = (num + 1) % 21;
        }
        for (int i=0;i<rtime.Count;i++)
        {
            if(rtime[i]+30f>= controller.GetComponent<Timer>().GetTime())
            {
                Earth.GetComponent<PutPins>().ChangeColor(num, 0);//白色に変色
                Erase(i);
            }
        }
    }

    public List<ObscuredInt> rnum= new List<ObscuredInt>();
    public List<ObscuredFloat> rtime = new List<ObscuredFloat>();
    void RedTrial(int num)//numは地域の番号
    {
        if (rnum.Contains(num)) return;
        ObscuredFloat a = (ObscuredFloat)controller.GetComponent<Main>().Temperature / 100;
        ObscuredFloat trial = UnityEngine.Random.Range(0.0f, 1.0f);
        if (a > 1) a = 1;if (a < 0) a = 0;
        if (trial < a)
        {
            ObscuredString SelectCountry = controller.GetComponent<EnemyAI>().Areas[num].Jcountries[UnityEngine.Random.Range(0, controller.GetComponent<EnemyAI>().Areas[num].Jcountries.Length)];
            ObscuredString phe = Phenomenons[UnityEngine.Random.Range(0, Phenomenons.Length)];
            controller.GetComponent<EnemyAI>().addNews(num, SelectCountry + "にて" + phe,"影響");
            controller.GetComponent<EnemyAI>().addNews(21, SelectCountry + "にて" + phe,"影響");
            rnum.Add(num);
            rtime.Add(controller.GetComponent<Timer>().GetTime());
            Earth.GetComponent<PutPins>().ChangeColor(num, 1);//赤色に変色
        }
    }

    public void Erase(int num)//タップ、もしくは時間切れによってcallされる関数,numは地域の番号
    {
        ObscuredInt n = rnum.IndexOf(num);
        if (n == -1) return;
        rnum.RemoveAt(n);
        rtime.RemoveAt(n);
    }


    ObscuredString[] Phenomenons = new ObscuredString[]
    {
        "降水量の地域差が激しくなる。",
"海面上昇を示すデータが科学者によって発見される。",
"海の酸性化によって近海から珊瑚が減少している。",
        "大雨の頻度が増加している。",
"大雨による被害が拡大しつつある。",
"年間を通した冬日の減少が確認される。",
"年間を通した夏日の増加が確認される。",
"固有の植物の絶滅が認定され、レッドリストに追加される。",
"生息地が減少し、生態系の多様性の低下が懸念される。",
"気温上昇によって作物が生育適温で育てられなくなり、生産高が減少する。",
"温暖化による乾燥地域が増加する。",
"冷房機器の増加が暖房機器の低下を上回り、経済に影響が出る。"
    };
}
