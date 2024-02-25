using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ChartAndGraph;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;

public class PutPins : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Earth;
    ObscuredFloat radius;
    public GameObject[] Pins;
    public GameObject ParentPin;
    ObscuredFloat rotate=0;
    ObscuredFloat[] initialLongitudes=new ObscuredFloat[] {246.6f,210.0f,175.2f,153.0f,156.47f,56.8f,19.1f,43.1f,21.9f,290.5f,303.8f,302.1f,307.0f,323.0f,302.4f,305.0f,291.0f,276.6f,207.4f,238.4f,271.6f };//初期の西経45度からみた角度
    ObscuredFloat[] latitudes=new ObscuredFloat[] { 42.4f, 36.7f, 35.6f, -38.3f,-6.0f,48.9f,-12.1f,23.8f,16.6f,49.2f,57.5f,42.0f,48.9f,20.6f,10.8f,40.4f,-17.4f,4.5f,8.5f,24.7f,31.3f };//緯度
    public Camera cam;
    Vector3 offset = new Vector3(0f, 0f, 10f);
    public GameObject controller;

    public BarChart barcommand;
    public BarChart bararea;
    public GraphChart graph;
    public GameObject chartPanel;
    public GameObject scrollView;

    public ObscuredInt CurrentArea=21;//0-20が各エリア、21が全体
    void Start()
    {
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 500, sequencesCapacity: 200);
        rotate = 0;
        Pins[10].SetActive(false);
        Pins[11].SetActive(false);
        Pins[12].SetActive(false);
        initialize();
    }

    public void OnPushedNews()
    {
        barcommand.enabled = false;
        bararea.enabled = false;
        graph.enabled = false;
        chartPanel.SetActive(false);
        controller.GetComponent<EnemyAI>().NewsShow(CurrentArea);
    }

    public void initialize()
    {
       bararea.enabled = false;
        barcommand.enabled = false;
        graph.enabled = false;
        chartPanel.SetActive(false);
        scrollView.SetActive(false);
        bararea.DataSource.AutomaticMaxValue = true;
       // graph.DataSource.
    }

    // Update is called once per frame
    void Update()
    {
        rotate += 12f * Time.deltaTime;
        if (rotate >= 360f) rotate -= 360;
        Put(false);
    }

    void Put(bool ini)
    {
        for(ObscuredInt i = 0; i < 21; i++)
        {
            if (10 <= i && i <= 12) continue;
            ObscuredFloat longitude=initialLongitudes[i] + rotate;
            ObscuredFloat latitude = latitudes[i];
            //if ((i >= 6 &&i<=8)||(i<=10&& i <= 17)) latitude += 10;
            while (longitude > 360f) longitude -= 360f;
            if (longitude >= 60 && longitude <= 300)
            {
                if (longitude - 12*Time.deltaTime <= 60)
                {
                    Pins[i].transform.DOScale(new Vector2(0f, 0f), 0.1f);
                }
                else if(longitude>=65)
                {
                    Pins[i].SetActive(false);
                }
            }
            else
            {
                Pins[i].transform.position = RectTransformUtility.WorldToScreenPoint(cam, Earth.transform.position + Rotation(longitude, latitude) + offset);
                if (longitude - 12 * Time.deltaTime < 300 || ini == true)
                {
                    Pins[i].SetActive(true);
                    Pins[i].transform.DOScale(new Vector2(1f, 1f), 0.1f);
                }
            }
        }
    }
    
    Vector3 Rotation(float longitude,float latitude)
    {
        //ObscuredFloat x = -2 * Mathf.Cos(latitude * Mathf.Deg2Rad) * Mathf.Sin(longitude * Mathf.Deg2Rad) *Screen.width / Screen.height / 1080 * 1920;
        //ObscuredFloat y = 2 * Mathf.Sin(latitude * Mathf.Deg2Rad) * Screen.width / Screen.height / 1080 * 1920;
        ObscuredFloat x = -2.1f * Mathf.Cos(latitude * Mathf.Deg2Rad) * Mathf.Sin(longitude * Mathf.Deg2Rad);
        ObscuredFloat y = 2.1f * Mathf.Sin(latitude * Mathf.Deg2Rad);
        return new Vector3(x, y, 0);
    }

    public void Disappear(int i)
    {
        Pins[i].SetActive(false);
    }
    public ObscuredBool arriveallowed=false;
    public void OnLeave()
    {
        ParentPin.SetActive(false);
    }
    public void OnArrive()
    {
        if (arriveallowed)
        {
            ParentPin.SetActive(true);
            Put(true);
        }
    }

    public void ALLBleach()
    {
        for (int i = 0; i < 21; i++)
        {
            if (controller.GetComponent<PhenoManager>().rnum.Contains(i)) Pins[i].GetComponent<Image>().color = Color.red;
            else Pins[i].GetComponent<Image>().color = Color.white;
        }
    }

    public void ChangeColor(int num,int i)
    {
        if (i == 0)
        {
            Pins[num].GetComponent<Image>().color = Color.white;
        }else if (i == 1)
        {
            Pins[num].GetComponent<Image>().color = Color.red;
        }
    }

    public void TouchPin(int Pin)
    {

        controller.GetComponent<Main>().TutorialSection(5, "");
        CurrentArea = Pin;
        if (controller.GetComponent<PhenoManager>().rnum.Contains(CurrentArea))
        {
            controller.GetComponent<Main>().TutorialSection(6, "");
            controller.GetComponent<PhenoManager>().Erase(CurrentArea);
            for(int i = 0; i < 3; i++)
            {
                if (controller.GetComponent<ProsessCommand>().Command_Using[i] != -1)
                {
                    controller.GetComponent<ProsessCommand>().Command_StartTime[i] -= 15f;
                }
            }
            //ここに追加処理
        }
        for (int i = 0; i < 21; i++)
        {
            if (i != Pin)
            {
                if(!controller.GetComponent<PhenoManager>().rnum.Contains(i))Pins[i].GetComponent<Image>().color = Color.white;
                else Pins[i].GetComponent<Image>().color = Color.red;
            }
        }
        Pins[CurrentArea].GetComponent<Image>().color = Color.yellow;
        //吹き出しに諸データ(危機感、政策の有効性、頻度、企業意識、回収、人口)
        var Area = controller.GetComponent<EnemyAI>().Areas[Pin];
        //地球儀のところに地域のニュース、及びバフされた回数、デバフされた回数をそれぞれ表示
        Debug.Log(Pin+"+"+Area.Jtitle);
        ObscuredInt Year = controller.GetComponent<GetDate>().CurrentYear;
        controller.GetComponent<FukidasiShow>().Show(Area.Jtitle,Explanations[Pin], 13, new ObscuredInt[] { }, false,0,0);
    }

    ObscuredString[] Explanations = new ObscuredString[21]
    {
        "乾燥気候で日較差が大きく、灌漑による綿花栽培や放牧が行われている。降水や氷河・積雪の減少で水不足が懸念される。権威体制など独特な政治体制をとる経済が発展途上の地域。",
        "比較的温暖で、北部では畑作、南部では稲作が盛んである。季節風の影響を大きく受け、温暖化による災害の増加に見舞われている。人口が非常に多く、経済発展も著しい。",
        "ユーラシア大陸の東に位置する温暖な島国。人口が多い、民主主義の経済先進国である。地球温暖化により、水資源や森林、農林水産業への深刻な影響を受ける。",
        "南太平洋に位置し、温暖で経済の発展している民主主義の2か国。水資源が不足しているほか、サンゴ礁などの観光資源への損害や、森林火災の激化も懸念される。",
        "太平洋の真ん中に位置する、火山島が多い温暖な地域。水没や塩害の被害に遭う国や、温暖化対策を行うことが難しい国が存在し、経済も発展途上である。",
        "広大な平野が広がり、やや寒冷で農工業が盛んな地域。人口が多く、経済は発展しているが、気温や海水温の上昇により、水不足やハリケーンの被害拡大が起こっている。",
        "熱帯雨林を擁し、二酸化炭素の吸収量が多い温暖な地域。民主主義をとる国が多いが不安定になることもあり、経済格差が激しい。森林破壊やパタゴニアの氷河融解が問題となっている。",
        "主に山岳地形や島嶼部からなる、雨量の多い温暖な地域。温暖化により、海岸浸食やハリケーンなどの影響が出ている。民主主義の定着や経済発展は途上にある。",
        "島弧を形成する島や岩礁が多く存在する温暖な地域。温暖化により、ハリケーンやサンゴ礁の死滅などの影響が出ている。経済は発展途上にある。",
        "緯度の割には温暖な場所が多く、農業地帯や工業地帯が広がる。熱波や埋蔵資源など、地球温暖化に関わる事象が多い。人口が多く、共和制をとる国の多い、経済先進地域。",
        "緯度の割には温暖な場所が多く、農業地帯や工業地帯が広がる。熱波や埋蔵資源など、地球温暖化に関わる事象が多い。人口が多く、共和制をとる国の多い、経済先進地域。",
        "緯度の割には温暖な場所が多く、農業地帯や工業地帯が広がる。熱波や埋蔵資源など、地球温暖化に関わる事象が多い。人口が多く、共和制をとる国の多い、経済先進地域。",
        "緯度の割には温暖な場所が多く、農業地帯や工業地帯が広がる。熱波や埋蔵資源など、地球温暖化に関わる事象が多い。人口が多く、共和制をとる国の多い、経済先進地域。",
        "ギニア湾岸ではプランテーションが盛んで、各国に輸出されている。しかし温暖で経済が発展途上なため、乾燥した地域では蝗害や旱魃が発生する。政情が安定しない地域がある。",
        "ニジェール川と大地溝帯に挟まれた地域で、大河川が多く存在する。温暖な気候により存在する熱帯雨林は、温暖化と密接に関わっている。経済は発展途上である。",
        "サハラ砂漠より北に位置し、アラブ系のイスラム教徒が多く居住する温暖で比較的乾燥した地域。洪水が頻発し、食料供給や生活に影響が出ている。経済格差も大きい。",
        "穏やかな丘陵地帯で、大部分がステップ気候の温暖な地域。経済は発展途上であり、多くの雇用と食料を供給する農業や漁業への被害が危惧されている。政情が安定しない地域がある。",
        "インド洋に面し、かつてはイスラム商人の交易で栄えていた。コーヒーや茶の栽培が盛んだが、温暖かつ経済が発展途上であり、蝗害や旱魃が深刻化している。",
        "ほとんど熱帯に属し、雨季にスコールがよく発生する湿潤な地域。経済成長が著しいが、熱帯林の減少や、従来の農業の継続が困難になる問題が発生している。",
        "モンスーンによる雨季と乾季があり、人口が多く、経済が成長している温暖な地域。低地での海岸浸食や氾濫、またサイクロンの被害拡大という脅威に晒されている。",
        "ほとんどが乾燥気候にあり、外来河川が流れる温暖な地域。経済が成長しており、王政を残す国もある。熱波や水資源の不足、そして短時間の豪雨による洪水被害が発生している。"
    };




    void barClearCategories() {
        barcommand.DataSource.ClearValues();
        bararea.DataSource.ClearValues();
        bararea.DataSource.ClearGroups();
    }
    void graphClearCategories()
    {
        if (graph.DataSource.HasCategory("Temperature"))
        {
            graph.DataSource.ClearCategory("Temperature");
        }
        if (graph.DataSource.HasCategory("Population"))
        {
            graph.DataSource.ClearCategory("Population");
        }
        if (graph.DataSource.HasCategory("Danger"))
        {
            graph.DataSource.ClearCategory("Danger");
        }
    }

    ObscuredString[] Nums = new ObscuredString[] {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" ,"13"};
    public Material categoryMaterial;
    Color hoverColor = Color.white, selectedColor = Color.yellow;

    public Text GraphEx;

   /* public void BuffGraphShow()//棒グラフ
    {
        controller.GetComponent<EnemyAI>().Scrollview.SetActive(false);
        chartPanel.SetActive(true);
        bararea.enabled = false;
        barcommand.enabled = true;
        graph.enabled = false;
        barClearCategories();
        barcommand.DataSource.AddCategory("バフ",new ChartDynamicMaterial(categoryMaterial, hoverColor, selectedColor));
        //挿入するのは全体のバフ
        Debug.Log("BuffGraph");
        for (ObscuredInt i = 0; i < 13; i++)
        {
                Debug.Log(Nums[i]+"+"+ controller.GetComponent<ProsessCommand>().BuffedTime[i]);
                barcommand.DataSource.SetValue("バフ", Nums[i], controller.GetComponent<ProsessCommand>().BuffedTime[i]);
        }
    }*/
    public void DebuffGraphShow()//棒グラフ
    {
        GraphEx.text = "施策回数";
        controller.GetComponent<EnemyAI>().Scrollview.SetActive(false);
        chartPanel.SetActive(true);
        bararea.enabled = false;
        barcommand.enabled = true;
        graph.enabled = false;
        controller.GetComponent<MissionManager>().close();
        barClearCategories();
        // 挿入するのは該当地域のデバフ
        for (ObscuredInt i = 0; i < 13; i++)
        {
            if (!barcommand.DataSource.HasCategory(Nums[i])) barcommand.DataSource.AddCategory(Nums[i], new ChartDynamicMaterial(categoryMaterial, hoverColor, selectedColor));
            barcommand.DataSource.SetValue( Nums[i],"デバフ", controller.GetComponent<EnemyAI>().Areas[CurrentArea].DebuffedTime[i]);
        }
    }

    public void TemperatureGraphShow()//折れ線グラフ
    {
        for (ObscuredInt i = 0; i < 2; i++) controller.GetComponent<Main>().Alarms[i].SetActive(false);
        if (controller.GetComponent<GetDate>().CurrentYear == 2020) { return; }
        GraphEx.text = "気温上昇";
        controller.GetComponent<EnemyAI>().Scrollview.SetActive(false);
        chartPanel.SetActive(true);
        bararea.enabled = false;
        barcommand.enabled = false;
        graph.enabled = true;
        controller.GetComponent<MissionManager>().close();
        controller.GetComponent<EnemyAI>().Scrollview.SetActive(false);
        graphClearCategories();
        //挿入するのは全体の気温
        graph.DataSource.StartBatch(); 
        for (ObscuredInt i = 0; i <= controller.GetComponent<GetDate>().CurrentYear-2020; i++)
        {
            if (graph.DataSource.HasCategory("Temperature"))
            {
                graph.DataSource.AddPointToCategory("Temperature", i+2020, ObscuredPrefs.GetFloat("IncreasedTemp" + i));
            }
        }
        graph.DataSource.EndBatch();
    }
    public void PopulationGraphShow()//全体時は棒グラフ（国別）、地域時は折れ線グラフ（時間軸）
    {
        if (controller.GetComponent<GetDate>().CurrentYear == 2020) { return; }
        GraphEx.text = "人口(1000人)";
        controller.GetComponent<EnemyAI>().Scrollview.SetActive(false);
        controller.GetComponent<MissionManager>().close();
        chartPanel.SetActive(true);
        if (CurrentArea == 21)//全体棒グラフ
        {
            bararea.enabled = true;
            barcommand.enabled = false;
            graph.enabled = false;
            controller.GetComponent<EnemyAI>().Scrollview.SetActive(false);
            barClearCategories();
            //挿入するのは人口（国別）
            Debug.Log("PopulationbyCountryStarted");
            bararea.DataSource.AddGroup("人口");
            for (ObscuredInt i = 0; i < 21; i++)
            {
                if (i != 9)
                {
                    if (!bararea.DataSource.HasCategory(controller.GetComponent<EnemyAI>().Areas[i].Jtitle)) bararea.DataSource.AddCategory(controller.GetComponent<EnemyAI>().Areas[i].Jtitle, new ChartDynamicMaterial(categoryMaterial, hoverColor, selectedColor));
                    bararea.DataSource.SetValue(controller.GetComponent<EnemyAI>().Areas[i].Jtitle, "人口", controller.GetComponent<EnemyAI>().Areas[i].pop(controller.GetComponent<GetDate>().CurrentYear));
                }
                else
                {
                    if (!bararea.DataSource.HasCategory("東"+controller.GetComponent<EnemyAI>().Areas[i].Jtitle)) bararea.DataSource.AddCategory("東"+controller.GetComponent<EnemyAI>().Areas[i].Jtitle, new ChartDynamicMaterial(categoryMaterial, hoverColor, selectedColor));
                    bararea.DataSource.SetValue("東" + controller.GetComponent<EnemyAI>().Areas[i].Jtitle, "人口", controller.GetComponent<EnemyAI>().Areas[i].pop(controller.GetComponent<GetDate>().CurrentYear));
                }
            }
        }
        else//地域、折れ線グラフ
        {
            if (controller.GetComponent<GetDate>().CurrentYear == 2020) { return; }
            GraphEx.text = "人口推移";
            bararea.enabled = false;
            barcommand.enabled =false;
            graph.enabled = true;
            controller.GetComponent<MissionManager>().close();
            controller.GetComponent<EnemyAI>().Scrollview.SetActive(false);
            graphClearCategories();
            graph.DataSource.StartBatch();
            for (ObscuredInt i = 0; i <= controller.GetComponent<GetDate>().CurrentYear - 2020; i++)
            {
                if (graph.DataSource.HasCategory("Population"))
                {
                    if (9 <= CurrentArea && CurrentArea <= 12)
                    {
                        ObscuredInt europop = controller.GetComponent<EnemyAI>().Areas[9].pop(i + 2020) + controller.GetComponent<EnemyAI>().Areas[10].pop(i + 2020) + controller.GetComponent<EnemyAI>().Areas[11].pop(i + 2020) + controller.GetComponent<EnemyAI>().Areas[12].pop(i + 2020);
                        graph.DataSource.AddPointToCategory("Population", i + 2020,europop);
                    }
                    else
                    {
                        graph.DataSource.AddPointToCategory("Population", i + 2020, controller.GetComponent<EnemyAI>().Areas[CurrentArea].pop(i + 2020));
                    }
                }
                else
                {
                    Debug.Log("Something Occured");
                }
            }
            graph.DataSource.EndBatch();
        }

    }
    public void DangerGraphShow()//折れ線グラフ
    {
        if (controller.GetComponent<GetDate>().CurrentYear == 2020) { return; }
        GraphEx.text = "危機感推移";
        chartPanel.SetActive(true);
        controller.GetComponent<EnemyAI>().Scrollview.SetActive(false);
        bararea.enabled = false;
        barcommand.enabled = false;
        graph.enabled = true;
        controller.GetComponent<EnemyAI>().Scrollview.SetActive(false);
        Debug.Log("graph.enabled="+graph.enabled);
        graphClearCategories();
        graph.DataSource.StartBatch();
        for (ObscuredInt i = 0; i <= controller.GetComponent<GetDate>().CurrentYear - 2020; i++)
        {
            Debug.Log(i+"+"+ controller.GetComponent<EnemyAI>().Areas[CurrentArea].DangerbyYear[i]);
            if (graph.DataSource.HasCategory("Danger"))
            {
                graph.DataSource.AddPointToCategory("Danger", i+2020,controller.GetComponent<EnemyAI>().Areas[CurrentArea].DangerbyYear[i]);
            }
        }
        graph.DataSource.EndBatch();
    }

}
