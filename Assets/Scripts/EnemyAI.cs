using System.Collections;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
public class EnemyAI : MonoBehaviour
{
    public GameObject controller;
    ObscuredFloat ToNextJudge;
    ObscuredFloat[][] baseTemperature = new ObscuredFloat[][] { new ObscuredFloat[] { 50, 55, 65, 80, 65 }, new ObscuredFloat[] { 50, 50, 45, 25, 25 }, new ObscuredFloat[] { 50, 30, 10, 0, 0 }, new ObscuredFloat[] { 50, 20, 0, -20, -30 } };
    ObscuredInt Rotation = 0;
    void Start()
    {
        ToNextJudge = 0;
        Rotation = 0;
    }

    public ObscuredBool AlStarted=false;
    public Slider[] sliders;

    // Update is called once per frame
    void Update()
    {
        ToNextJudge += Time.deltaTime;
        if ((ToNextJudge >= 1f|| (ToNextJudge >= 0.5f&&controller.GetComponent<ProsessCommand>().DoubleSpeed)) && controller.GetComponent<Timer>().IsWork()&& !controller.GetComponent<Timer>().IsStop())
        {
            ToNextJudge = 0;
            Judge(Rotation);
            Rotation = (Rotation + 1) % 21;
        }
        for(ObscuredInt j = 0; j < 5; j++)
        {
            ObscuredFloat d = 0;
            for (ObscuredInt i = 0; i < 21; i++)
            {
                if(j==0)d += Areas[i].Danger;
                if(j==1) d += Areas[i].Validity;
                if (j == 2) d += Areas[i].Frequency;
                if (j == 3) d += Areas[i].Company;
                if (j == 4) d += Areas[i].Collection;
            }
            if (j == 0) sliders[j].value = d / 21000f;//danger
            if (j == 1) sliders[j].value = d / 9450f;//validity
            if (j == 2) sliders[j].value = d / 21000f;//frequency
            if (j == 3) sliders[j].value = d / 9450f;//Company
            if (j == 4) sliders[j].value = d / 21f;//Collection
        }
    }

    void Judge(int num)
    {
        ObscuredFloat trial = UnityEngine.Random.Range(0.0f, 1.0f);
        //Debug.Log("Trial=" + trial + Areas[num].Jtitle+"のFrequency=" + Areas[num].Frequency);
        if (trial < Areas[num].Frequency / 2000f)
        {
           /* Debug.Log(trial + "+" + (Areas[num].Frequency + 5) / 100);
            Debug.Log("JudgeSucceeded");*/
            ObscuredString SelectCountry = Areas[num].Jcountries[UnityEngine.Random.Range(0, Areas[num].Jcountries.Length)];
            var policylines = controller.GetComponent<Policies>().policies;
            var policy = policylines[UnityEngine.Random.Range(0, policylines.Length)];
            /*ObscuredString title = policy.title;*/
            ObscuredString main = policy.main;
            ObscuredInt[] debuff= policy.debuff;
            for(ObscuredInt i=0;i<= ObscuredPrefs.GetInt("Difficulty"); i++)
            {
                ObscuredInt de = debuff[i];
                foreach(ObscuredInt afe in Areas[num].AffectedAreas)
                {
                    if (de < 13)
                    {
                        /*for(ObscuredInt j = 0; j < 21; j++)
                        {
                            Areas[j].DebuffedTime[de]++;
                        }*/
                        Areas[afe].DebuffedTime[de]++;
                    }
                    else
                    {
                        if (de == 13) //危機感
                        {
                            Areas[afe].Danger += 100f;
                            if (Areas[afe].Danger > 1000f) Areas[afe].Danger = 1000f;
                            if (Areas[afe].Danger < 0f) Areas[afe].Danger = 0f;
                        }
                        if (de == 14)//有効性
                        {
                            Areas[afe].Validity += 45f;
                            if (Areas[afe].Validity > 450f) Areas[afe].Validity = 450f;
                            if (Areas[afe].Validity < 0f) Areas[afe].Validity = 0f;
                        }
                        if (de == 15)//頻度
                        {
                            Areas[afe].Frequency += 100f;
                            if (Areas[afe].Frequency > 1000f) Areas[afe].Frequency = 1000f;
                            if (Areas[afe].Frequency < 0f) Areas[afe].Frequency = 0f;
                        }
                        if (de == 16)//企業
                        {
                            Areas[afe].Company += 45f;
                            if (Areas[afe].Company > 450f) Areas[afe].Company = 450f;
                            if (Areas[afe].Company < 0f) Areas[afe].Company = 0f;
                        }
                        if (de == 17)//回収量
                        {
                            Areas[afe].Collection += 0.05f;
                            if (Areas[afe].Collection > 1f) Areas[afe].Collection = 1f;
                        }
                    }
                }
               
            }
            Debug.Log(SelectCountry + "にて" + main);
            addNews(num, SelectCountry + "にて" + main, policy.title);
            addNews(21, SelectCountry + "にて" + main, policy.title);
            if (controller.GetComponent<PolicyFlow>().TextExist == false)
            {
                controller.GetComponent<PolicyFlow>().PolicyShow(SelectCountry + "にて" + main);
            }
            PoliticsDone += 5+ (ObscuredInt)(Areas[num].Validity/30f);
        }

    }

    public struct area
    {
        public ObscuredString Etitle;//地域の名前(英語)
        public ObscuredString Jtitle;//地域の名前(英語)
        public ObscuredString[] Ecountries;//所属する国家の名前
        public ObscuredString[] Jcountries;//所属する国家の名前2019,2030,2050,2100年
        public ObscuredDouble[] populations;//地域の人口
        public ObscuredBool developed;//先進国か否か

        public ObscuredFloat Danger;//危機感　以下2つに対するバフ効果をもつ
        public ObscuredFloat Validity;//政治の有効性　0.8倍ベースのデバフを上昇させる
        public ObscuredFloat Frequency;//政策の頻度　デバフの頻度を下げる
        public ObscuredFloat Company;//瞬間的な次の行動に対するバフ
        public ObscuredFloat Collection;//CO2の回収量
        public ObscuredInt[] DebuffedTime;//地域ごとにデバフされた

        public ObscuredFloat[] DangerbyYear;//obsoleted

        public ObscuredInt[] AffectedAreas;//影響される地域

        public ObscuredInt pop(int Year)
        {
            ObscuredInt population=(ObscuredInt)(populations[0] * System.Math.Pow(Year-2020,3) + populations[1] * System.Math.Pow(Year-2020, 2) + populations[2] * (Year-2020)+ populations[3]);
//            Debug.Log(population);
            return population;
        }
    }

    public ObscuredFloat TotalCO2Amount;

    public ObscuredInt totalpop()
    {
        ObscuredInt popsum = 0;
        for(int i = 0; i < 21; i++)
        {
            popsum += Areas[i].pop(controller.GetComponent<GetDate>().CurrentYear);
        }
        return popsum;
    }

    public ObscuredInt totalpower()
    {
        ObscuredInt powersum = 0;
        for (int i = 0; i < 21; i++)
        {
            powersum += Areas[i].pop(controller.GetComponent<GetDate>().CurrentYear)* (controller.GetComponent<EnemyAI>().Areas[i].developed ? 10 : (3 * (2100 - controller.GetComponent<GetDate>().CurrentYear) + 10 * (controller.GetComponent<GetDate>().CurrentYear - 2020)) / 80);
        }
        return powersum;
    }

    public  ObscuredFloat AllCollection = 0;
    public  ObscuredInt PoliticsDone=0;

    public void MonthlyAddition()
    {
        AllCollection = 0;
        for(ObscuredInt i = 0; i < 21; i++)
        {
            AllCollection += Areas[i].Collection;
        }
        ObscuredInt CurrentYear = controller.GetComponent<GetDate>().CurrentYear;
        ObscuredInt diff= ObscuredPrefs.GetInt("Difficulty");
        if (CurrentYear <= 2040)
        {
            TotalCO2Amount += (baseTemperature[diff][1] * (CurrentYear - 2020) + baseTemperature[diff][0] * (2040 - CurrentYear)) / 240;
        }
        else if (CurrentYear <= 2060)
        {
            TotalCO2Amount += (baseTemperature[diff][2] * (CurrentYear - 2040) + baseTemperature[diff][1] * (2060 - CurrentYear)) / 240;
        }
        else if (CurrentYear <= 2080)
        {
            TotalCO2Amount += (baseTemperature[diff][3] * (CurrentYear - 2060) + baseTemperature[diff][2] * (2080 - CurrentYear)) / 240;
        }
        else if (CurrentYear <= 2100)
        {
            TotalCO2Amount += (baseTemperature[diff][4] * (CurrentYear - 2080) + baseTemperature[diff][3] * (2100 - CurrentYear)) / 240;
        }
        for (ObscuredInt i = 0; i < 21; i++)
        {
            //Debug.Log(Areas[i].Jtitle + "+" + Areas[i].CO2Amount + "+" + Areas[i].Danger + "+" + Areas[i].Validity + "+" + Areas[i].Frequency + "+" + Areas[i].Company);
            Areas[i].Validity += Areas[i].Danger *0.0075f * (ObscuredFloat)System.Math.Pow(2, controller.GetComponent<CommandList>().Commands[41].DebNum);
            if (Areas[i].Validity > 450f) Areas[i].Validity = 450f;
            Areas[i].Frequency += Areas[i].Danger * 0.017f * (ObscuredFloat)System.Math.Pow(2, controller.GetComponent<CommandList>().Commands[40].DebNum);
            if (Areas[i].Frequency > 1000f) Areas[i].Frequency = 1000f;
            Areas[i].Company += Areas[i].Danger * 0.00375f;
            if (Areas[i].Company > 450f) Areas[i].Company = 450f;
            Areas[i].Collection += Areas[i].Company / 100000f;
            if (Areas[i].Collection > 1f) Areas[i].Collection = 1f;
            TotalCO2Amount -= Areas[i].Collection/12f;
        }
    }

    public void TempAddition(int i,float num,int kind)
    {
        if (kind == 0) {
            TotalCO2Amount += num;
        }
        else if (kind == 1)
        {
            if (num >= 0) Areas[i].Danger += num * (ObscuredFloat)System.Math.Pow(2, controller.GetComponent<CommandList>().Commands[40].DebNum);
            else Areas[i].Danger += num;

            if (Areas[i].Danger > 1000f) Areas[i].Danger = 1000f;
            if (Areas[i].Danger < 0f) Areas[i].Danger = 0f;
        }
        else if (kind == 2)
        {
            Areas[i].Validity += num;
            if (Areas[i].Validity > 450f) Areas[i].Validity = 450f;
            if (Areas[i].Validity < 0f) Areas[i].Validity = 0f;
        }
        else if (kind == 3)
        {
            Areas[i].Frequency += num;
            if (Areas[i].Frequency > 1000f) Areas[i].Frequency = 1000f;
            if (Areas[i].Frequency < 0f) Areas[i].Frequency = 0f;
        }
        else if (kind == 4)
        {
            Areas[i].Company += num;
            if (Areas[i].Company > 450f) Areas[i].Company = 450f;
            if (Areas[i].Company < 0f) Areas[i].Company = 0f;
        }

    }


    public area[] Areas = new area[]
    {
        new area{Etitle="Central Asia",Jtitle="中央アジア",Ecountries=new ObscuredString[]{"Kazakhstan","Kyrgyzstan","Tajikistan","Turkmenistan","Uzbekistan"},Jcountries=new ObscuredString[]{"カザフスタン","キルギス","タジキスタン","トルクメニスタン","ウズベキスタン" },populations=new ObscuredDouble[]{-0.035,-3.254,1006.45,73212},developed=false ,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 0, 1, 2, 3, 4, 18, 20 } },
        new area{Etitle="Eastern Asia",Jtitle="東アジア",Ecountries=new ObscuredString[]{"China","Taiwan", "People’s Republic of Korea","Mongolia","Republic of Korea"},Jcountries=new ObscuredString[]{ "中国","台湾","北朝鮮","モンゴル","韓国"},populations=new ObscuredDouble[]{2.317,-326.117,6297.63,1545724},developed=false,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 0, 1, 2, 3, 4, 18, 20 }  },
        new area{Etitle="Japan",Jtitle="日本",Ecountries=new ObscuredString[]{ "Japan"},Jcountries=new ObscuredString[]{"日本" },populations=new ObscuredDouble[]{0.1,-10.422,-452.18,126860},developed=true,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0 ,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 0, 1, 2, 3, 4, 18, 20 } },
        new area{Etitle="Australia and New Zealand",Jtitle="オーストラリア、ニュージーランド",Ecountries=new ObscuredString[]{"Australia","New Zealand" },Jcountries=new ObscuredString[]{ "オーストラリア","ニュージーランド"},populations=new ObscuredDouble[]{0.013,-2.229,328.772,29986},developed=true,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0 ,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 0, 1, 2, 3, 4, 18, 20 } },
        new area{Etitle="Oceania",Jtitle="オセアニア",Ecountries=new ObscuredString[]{ "Fiji","Papua New Guinea","Solomon Islands","Vanuatu","Guam","Marshall Islands","Micronesia (Fed. States of )","Nauru","Palau","Cook Islands","Niue","Samoa","Tonga","Tuvalu"},Jcountries=new ObscuredString[]{"フィジー","パプアニューギニア","ソロモン諸島","バヌアツ" ,"キリバス","マーシャル諸島","ミクロネシア連邦","ナウル","パラオ","クック諸島","ニウエ","サモア","トンガ","ツバル" },populations=new ObscuredDouble[]{-0.013,0.508,216.642,12142},developed=false,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 0, 1, 2, 3, 4, 18, 20 }  },
        new area{Etitle="Northern America",Jtitle="北アメリカ",Ecountries=new ObscuredString[]{"United States of America","Canada" },Jcountries=new ObscuredString[]{ "アメリカ","カナダ"},populations=new ObscuredDouble[]{0.098,-17.965,2368.75,363160},developed=true,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 5,6,7,8 }  },
        new area{Etitle="South America",Jtitle="南アメリカ",Ecountries=new ObscuredString[]{ "Argentina","Bolivia","Brazil ","Chile ","Colombia ","Ecuador ","Guyana ","Paraguay ","Peru ","Suriname ","Uruguay ","Venezuela "},Jcountries=new ObscuredString[]{ "アルゼンチン","ボリビア","ブラジル","チリ","コロンビア","エクアドル","ガイアナ","パラグアイ","ペルー","スリナム","ウルグアイ","ベネズエラ"},populations=new ObscuredDouble[]{0.185,-61.629,3798.11,426905},developed=false ,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 5,6,7,8 }  },
        new area{Etitle="Central America",Jtitle="中央アメリカ",Ecountries=new ObscuredString[]{ "Belize","Costa Rica","El Salvador","Guatemala","Honduras","Mexico","Nicaragua","Panama"},Jcountries=new ObscuredString[]{ "ベリーズ","コスタリカ","エルサルバドル","グアテマラ","ホンジュラス","メキシコ","ニカラグア","パナマ"},populations=new ObscuredDouble[]{6.484,-617.747,7918.27,177587},developed=false,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 5,6,7,8 }   },
        new area{Etitle="Caribbean",Jtitle="カリブ海地域",Ecountries=new ObscuredString[]{"Antigua and Barbuda","Bahamas","Barbados","Cuba","Dominica","Dominican Republic","Grenada","Haiti","Jamaica","SaObscuredInt Kitts and Nevis","SaObscuredInt Lucia","SaObscuredInt Vincent and the Grenadines","Trinidad and Tobago" },Jcountries=new ObscuredString[]{ "アンティグア・バーブーダ","バハマ","バルバドス","キューバ","ドミニカ国","ドミニカ共和国","グレナダ","ハイチ","ジャマイカ","セントクリストファー・ネイビス","セントルシア","セントビンセント・グレナディーン","トリニダード・トバゴ"},populations=new ObscuredDouble[]{0.006,-4.259,272.653,38983},developed= false,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0 ,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 5,6,7,8 } },
        new area{Etitle="Europe",Jtitle="ヨーロッパ",Ecountries=new ObscuredString[]{"Belarus","Bulgaria","Czechia","Hungary","Poland","Republic of Moldova","Romania ","Russian Federation","Slovakia","Ukraine" },Jcountries=new ObscuredString[]{"ベラルーシ","ブルガリア","チェコ","ハンガリー","ポーランド","モルドバ","ルーマニア","ロシア","スロバキア","ウクライナ" },populations=new ObscuredDouble[]{0.176,-17.628,-637.337,293445},developed=true,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 9,10,11,12 }   },
        new area{Etitle="Northern Europe",Jtitle="北ヨーロッパ",Ecountries=new ObscuredString[]{"Denmark","Estonia","Finland","Iceland","Ireland", "Latvia","Lithuania",  "Norway","Sweden","United Kingdom"},Jcountries=new ObscuredString[]{ "デンマーク","エストニア","フィンランド","アイスランド","アイルランド","ラトビア","リトアニア","ノルウェー","スウェーデン","イギリス"},populations=new ObscuredDouble[]{0.032,-5.728,444.349,106079},developed=true,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 9,10,11,12 }  },
        new area{Etitle="Southern Europe",Jtitle="南ヨーロッパ",Ecountries=new ObscuredString[]{ "Albania","Andorra","Bosnia and Herzegovina","Croatia" ,"Greece","Holy See","Italy","Malta","Montenegro","North Macedonia","Portugal","San Marino","Serbia","Slovenia","Spain"},Jcountries=new ObscuredString[]{"アルバニア","アンドラ","ボスニア・ヘルツェゴビナ","クロアチア","ギリシャ","バチカン","イタリア","マルタ","モンテネグロ","北マケドニア","ポルトガル","サンマリノ","セルビア","スロベニア","スペイン" },populations=new ObscuredDouble[]{0.065,-10.086,-259.361,152413},developed=true,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 9,10,11,12 }  },
        new area{Etitle="Western Europe",Jtitle="西ヨーロッパ",Ecountries=new ObscuredString[]{"Austria","Belgium","France","Germany","Liechtenstein","Luxembourg","Monaco","Netherlands","Switzerland" },Jcountries=new ObscuredString[]{"オーストリア","ベルギー","フランス","ドイツ","リヒテンシュタイン","ルクセンブルク","モナコ","オランダ","スイス" },populations=new ObscuredDouble[]{0.116,-15.564,437.936,198712},developed=true,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 9,10,11,12 }  },
        new area{Etitle="Western Africa",Jtitle="西アフリカ",Ecountries=new ObscuredString[]{ "Benin","Burkina Faso","Cabo Verde","Côte d’Ivoire","Gambia","Ghana","Guinea","Guinea-Bissau","Liberia","Mali","Mauritania","Niger","Nigeria","Senegal","Sierra Leone","Togo" },Jcountries=new ObscuredString[]{"ベナン","ブルキナファソ","カーボベルデ","コートジボワール","ガンビア","ガーナ","ギニア","ギニアビサウ","リベリア","マリ","モーリタニア","ニジェール","ナイジェリア","セネガル","シエラレオネ","トーゴ" },populations=new ObscuredDouble[]{-0.213,49.641,10860.9,391434},developed=false,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 13,14,15,16,17 }  },
        new area{Etitle="Middle Africa",Jtitle="中央アフリカ",Ecountries=new ObscuredString[]{"Angola","Cameroon","Central African Republic","Chad","Congo","Democratic Republic of the Congo","Equatorial Guinea","Gabon","Sao Tome and Principe"  },Jcountries=new ObscuredString[]{ "アンゴラ","カメルーン","中央アフリカ共和国","チャド","コンゴ共和国","コンゴ民主共和国","赤道ギニア","ガボン","サントメ・プリンシペ"},populations=new ObscuredDouble[]{-0.556,69.004,5115.28,174308},developed=false,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0 ,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 13,14,15,16,17 }  },
        new area{Etitle="Northern Africa",Jtitle="北アフリカ",Ecountries=new ObscuredString[]{"Algeria","Egypt","Libya","Morocco","Sudan","Tunisia","Western Sahara"  },Jcountries=new ObscuredString[]{"アルジェリア","エジプト","リビア","モロッコ","スーダン","チュニジア","西サハラ" },populations=new ObscuredDouble[]{-0.229,6.844,4193.47,241781},developed=false,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 13,14,15,16,17 }   },
        new area{Etitle="Southern Africa",Jtitle="南アフリカ",Ecountries=new ObscuredString[]{ "Botswana","Eswatini","Lesotho","Namibia","South Africa" },Jcountries=new ObscuredString[]{"ボツワナ","エスワティニ","レソト","ナミビア","南アフリカ共和国" },populations=new ObscuredDouble[]{-0.007,-5.934,860.135,66630},developed=false,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 13,14,15,16,17 }  },
        new area{Etitle="Eastern Africa",Jtitle="東アフリカ",Ecountries=new ObscuredString[]{"Burundi","Comoros","Djibouti","Eritrea","Ethiopia","Kenya","Madagascar","Malawi","Mauritius","Mayotte","Mozambique","Réunion","Rwanda","Seychelles","Somalia","South Sudan","Uganda","United Republic of Tanzania","Zambia","Zimbabwe" },Jcountries=new ObscuredString[]{"ブルンジ","コモロ","ジブチ","エリトリア","エチオピア","ケニア","マダガスカル","マラウイ","モーリシャス","モザンビーク","ルワンダ","セーシェル","ソマリア","南スーダン","ウガンダ","タンザニア","ザンビア","ジンバブエ" },populations=new ObscuredDouble[]{-1.053,100.11,11358.6,432450},developed=false,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 13,14,15,16,17 }   },
        new area{Etitle="SouthEastern Asia",Jtitle="東南アジア",Ecountries=new ObscuredString[]{"Brunei Darussalam","Cambodia","Indonesia","Lao People’s Democratic Republic"," Malaysia","Myanmar","Philippines","Singapore","Thailand","Timor-Leste","Viet Nam"  },Jcountries=new ObscuredString[]{"ブルネイ","カンボジア","インドネシア","ラオス","マレーシア","ミャンマー","フィリピン","シンガポール","タイ","東ティモール","ベトナム" },populations=new ObscuredDouble[]{0.271,-95.244,6949.59,662012},developed=false,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 0, 1, 2, 3, 4, 18, 20 }  },
        new area{Etitle="Southern Asia",Jtitle="南アジア",Ecountries=new ObscuredString[]{"Afghanistan","Bangladesh","Bhutan","India","Iran (Islamic Republic of)","Maldives","Nepal","Pakistan","Sri Lanka" },Jcountries=new ObscuredString[]{"アフガニスタン","バングラデシュ","ブータン","インド","イラン","モルディブ","ネパール","パキスタン","スリランカ" },populations=new ObscuredDouble[]{0.235,-261.302,23292.8,1918211},developed=false,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 19 }  },
        new area{Etitle="Western Asia",Jtitle="西アジア",Ecountries=new ObscuredString[]{ "Armenia","Azerbaijan","Bahrain","Cyprus","Georgia","Iraq","Israel","Jordan","Kuwait","Lebanon","Oman","Qatar","Saudi Arabia","State of Palestine","Syrian Arab Republic","Turkey","United Arab Emirates","Yemen"},Jcountries=new ObscuredString[]{"アルメニア","アゼルバイジャン","バーレーン","キプロス","イラク","ジョージア","イスラエル","ヨルダン","クウェート","レバノン","オマーン","カタール","サウジアラビア","パレスチナ","シリア","トルコ","アラブ首長国連邦","イエメン" },populations=new ObscuredDouble[]{-0.006,-31.967,4454.51,275325},developed=false,DebuffedTime=new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },Danger=0,Validity=0, Frequency=0,Company=0,Collection=0 ,DangerbyYear=new ObscuredFloat[81],AffectedAreas=new ObscuredInt[]{ 0, 1, 2, 3, 4, 18, 20 } }

    };

    public ObscuredString[][] news = new ObscuredString[22][] { new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { } };//地域21+全体1
    public ObscuredString[][] title = new ObscuredString[22][] { new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { }, new ObscuredString[] { } };//地域21+全体1

    public void addNews(int i, string s,string t) {
        Array.Resize(ref news[i], news[i].Length + 1);
        news[i][news[i].Length - 1] = s;
        Array.Resize(ref title[i], title[i].Length + 1);
        title[i][title[i].Length - 1] = t;
        if (10 <= i && i <= 12) {
            Array.Resize(ref news[9], news[9].Length + 1);
            news[9][news[9].Length - 1] = s;
            Array.Resize(ref title[9], title[9].Length + 1);
            title[9][title[9].Length - 1] = t;
        }
    }

    public GameObject Scrollview;
    public GameObject Content;

    public void NewsShow(int area)
    {
        if (news[area].Length == 0) return;
        Scrollview.SetActive(true);
        foreach (Transform c in Content.transform)
        {
            GameObject.Destroy(c.gameObject);
        }

        GameObject prefab = (GameObject)Resources.Load("News");
        for (ObscuredInt i=news[area].Length-1;i>=0; i--)
        {
            if (news[area].Length - 101 == i) break;
            var parent = Content.transform;
            GameObject NewsPanel=Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
            NewsPanel.transform.Find("Main").GetComponent<Text>().text = news[area][i];
            NewsPanel.transform.Find("Title").GetComponent<Text>().text = title[area][i];
        }
    }

}
