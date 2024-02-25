using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;

public class ProsessCommand : MonoBehaviour
{
    public GameObject[] Buttons;
    public GameObject Controller;
    public GameObject MyIcon;

    public ObscuredInt[] BuffedTime = new ObscuredInt[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//そのジャンルがバフされた回数を表示
    public ObscuredInt[] Command_Using = { -1, -1, -1 };//どの番号のコマンドが使用しているか
    public ObscuredFloat[] Command_StartTime = { -1, -1, -1 };//コマンドが始まった時間

    public Image[] Afters;//クールタイムの円
    public Image[] Effects;//円のところに表示するアイコン
    public Sprite[] Icons;//アイコンのスプライト
    public GameObject DecideButton;//決定ボタン
    public GameObject Buff;

    public ObscuredInt ECommandNum=0;
    public ObscuredInt SCommandNum=0;

    public ObscuredBool DoubleSpeed = false;//倍速かどうか
    public Image speed;
    public Sprite[] sprites;

    public ObscuredFloat[] BuffPer = new ObscuredFloat[4] {1.2f,1.15f,1.1f,1.05f};

    public void Doubleon()
    {
        if (DoubleSpeed)
        {
            DoubleSpeed = false;
            speed.sprite = sprites[0];
        }
        else
        {
            DoubleSpeed = true;
            speed.sprite = sprites[1];
        }
    }

    private void Start()
    {
        Buff.SetActive(false);
        ObscuredInt Power=0;
        var Areas = Controller.GetComponent<EnemyAI>().Areas;
        for (ObscuredInt i = 0; i < 21; i++) {
            Power += Areas[i].pop(0) *( Areas[i].developed ? 10 : 3);
        }
        Debug.Log("Power=" + Power);
        ObscuredInt pop = 0;
        for (ObscuredInt i = 0; i < 21; i++)
        {
            pop += Areas[i].pop(0);
        }

        foreach (Image Effect in Effects)
        {
            Effect.enabled = false;//アイコンの一旦の消去
        }
        Check();
    }


    private void Update()//クールタイムの管理
    {
        var Commands = Controller.GetComponent<CommandList>().Commands;
        //ぐるっと回る部分の処理
        for (ObscuredInt i = 0; i < 3; i++)
        {
            if (Command_Using[i] != -1 && Controller.GetComponent<Timer>().GetTime() - Command_StartTime[i] >= Commands[Command_Using[i]].Year * 15f)
            {
                //終了時の処理
                Use(Command_Using[i], i);
                Command_Using[i] = -1;
                Command_StartTime[i] = -1;
                /*DOVirtual.DelayedCall(0.5f,() => Command_Using[i] = -1);
                DOVirtual.DelayedCall(0.5f, () => Command_StartTime[i] = -1);*/
            }
            else if (Command_Using[i] != -1)
            {//そうでない時
                if (Effects[i].enabled == false)
                {
                    Effects[i].enabled = true;
                    Effects[i].sprite= Icons[Command_Using[i]];
                }
                ObscuredFloat per = (Controller.GetComponent<Timer>().GetTime() - Command_StartTime[i]) / (Commands[Command_Using[i]].Year * 15f);
                /*Debug.Log(per);*/
                Afters[i].fillAmount = per;
            }
        }
    }
    public ObscuredInt CurrentCommand = -1;

    private void Check()
    {
        var Commands = Controller.GetComponent<CommandList>().Commands;
        for (ObscuredInt i = 0; i < 42; i++)
        {
            ObscuredBool Permission = true;
            foreach (ObscuredInt Root in Commands[i].Roots)
            {
                if (Commands[Root].AlreadyUsed == false)
                {
                    Permission = false;
                    break;
                }
            }
            if (Permission)
            {
                Buttons[i].GetComponent <Image>().color = new Color(255f / 255.0f, 255f / 255.0f, 255f / 255.0f, 255f / 255.0f);
            }
            else
            {
                Buttons[i].GetComponent<Image>().color = new Color(150f / 255.0f, 150f / 255.0f, 150f / 255.0f, 255f / 255.0f);
            }
        }
    }

    public void CommandShow(int Command_Num)//表示ボタンを押したときに吹き出しに情報を表示
    {
        MyIcon.SetActive(true);
        Buff.SetActive(true);
        var Commands = Controller.GetComponent<CommandList>().Commands;
        CurrentCommand = Command_Num;
        StartCoroutine("OnClickCommand");
        ObscuredBool Admission = true;
        foreach (ObscuredInt Root in Commands[Command_Num].Roots)
        {
            if (Commands[Root].AlreadyUsed == false)
            {
                Admission = false;
                break;
            }
        }//開放条件を満たしていなければ使用不可能
        if (Commands[Command_Num].NowUsed == true) Admission = false;//現在クールタイムに入っていれば使用不可能
        ObscuredBool CommandPermission = false;
        for (ObscuredInt i = 0; i < 3; i++)
        {
            if (Command_Using[i] == -1)
            {
                CommandPermission = true;
                break;
            }
        }
        if (!CommandPermission) Admission = false;//クールタイム中のコマンドが3つ埋まっている場合には使用不可能
        Controller.GetComponent<FukidasiShow>().Show(Commands[Command_Num].title, Commands[Command_Num].main, Commands[Command_Num].MyIcon, Commands[Command_Num].Buff, Admission, Commands[Command_Num].Year,Command_Num);
    }

    IEnumerator OnClickCommand()
    {
        if (CurrentCommand != -1)
        {
            if (CurrentCommand >= 0 && CurrentCommand <= 20) Buttons[CurrentCommand].transform.DOScale(new Vector2(1.2f, 1.2f), 0.1f).SetEase(Ease.OutCubic);
            else Buttons[CurrentCommand].transform.DOScale(new Vector2(1.62f, 1.62f), 0.1f).SetEase(Ease.OutCubic);
        }
        
        yield return new WaitForSeconds(0.1f);

        if (CurrentCommand != -1)
        {
            if (CurrentCommand >= 0 && CurrentCommand <= 20) Buttons[CurrentCommand].transform.DOScale(new Vector2(1f, 1f), 0.1f).SetEase(Ease.OutCubic);
            else Buttons[CurrentCommand].transform.DOScale(new Vector2(1.35f, 1.35f), 0.1f).SetEase(Ease.OutCubic);
        }

    }

    public void PreActivate()
    {
        DecideButton.SetActive(false);
        Activate(CurrentCommand);//決定ボタンが１つしかないゆえの関数
    }
    public void Activate(int Command_Num)//決定ボタンを押した時の関数
    {
        var Commands = Controller.GetComponent<CommandList>().Commands;
        ObscuredInt Command_Place = -1;
        for (ObscuredInt i = 0; i < 3; i++)
        {
            if (Command_Using[i] == -1)
            {
                Command_Place = i;
                Command_Using[i] = Command_Num;//使ってるのはこのナンバーのコマンドですよ
                break;
            }
        }
        if (Command_Place == -1) return;
        else
        {
            Effects[Command_Place].sprite = Icons[Command_Num];
           Effects[Command_Place].enabled = true;//表示、拡大の必要あり？
            Commands[Command_Num].NowUsed = true;
            Command_StartTime[Command_Place] = Controller.GetComponent<Timer>().GetTime();
            Controller.GetComponent<Main>().TutorialSection(3, Controller.GetComponent<CommandList>().Commands[Command_Num].title);
        }
    }




    public void Use(int Command_Num, int Command_Place)
    {
        var Commands = Controller.GetComponent<CommandList>().Commands;
        if (Command_Num < 0 || Command_Num > 41) return;
        else
        {
            if (Command_Num <= 20) { ECommandNum++; }
            else { SCommandNum++; }
            //環境関連の全部の処理をする
            Commands[Command_Num].NowUsed = false;
            Commands[Command_Num].AlreadyUsed = true;
            if (Command_Num == 30) {//マスメディア限定の特殊効果
                for(ObscuredInt i=0;i<21;i++)Controller.GetComponent<EnemyAI>().TempAddition(i,-10000f, 1);
            }
            if (Command_Num == 21)
            {//マスメディア限定の特殊効果
                Halfed = true;
            }
            if (Command_Num == 40)
            {//マスメディア限定の特殊効果
                for (ObscuredInt i = 0; i < 21; i++) Controller.GetComponent<EnemyAI>().TempAddition(i, -10000f, 3);
            }
            if (Command_Num == 41)
            {//マスメディア限定の特殊効果
                for (ObscuredInt i = 0; i < 21; i++) Controller.GetComponent<EnemyAI>().TempAddition(i, -10000f, 2);
            }
            EnvironmentPower(Command_Num);
            DangerPower(Command_Num);
            ValidityPower(Command_Num);
            FrequencyPower(Command_Num);
            CompanyPower(Command_Num);
            if (Commands[Command_Num].SelfDebuff) Commands[Command_Num].DebNum++;
            foreach (ObscuredInt buff in Commands[Command_Num].Buff)
            {
                BuffedTime[buff]++;
            }//バフ対象にバフ をかける
            Debug.Log("MissionGoing=" + Controller.GetComponent<MissionManager>().MissionGoing + "Command_Num"+ Command_Num+ "missionCommand="+ Controller.GetComponent<MissionManager>().missionCommand + "missionType"+ Controller.GetComponent<MissionManager>().missionType);
            if (Controller.GetComponent<MissionManager>().MissionGoing==true&&Command_Num ==Controller.GetComponent<MissionManager>().missionCommand&& Controller.GetComponent<MissionManager>().missionType == 0)
            {
                Debug.Log("Mission0Completed");
                Controller.GetComponent<Timer>().Stop();
                Controller.GetComponent<MissionManager>().OnClearMission();
            }
            StartCoroutine(EraseFill(Command_Place));
            //StartCoroutine(EraseNum(Command_Place));
            Check();
        }
    }
    /*IEnumerator EraseNum(ObscuredInt Command_Place)
    {
        yield return new WaitForSeconds(0.5f);
        Command_Using[Command_Place] = -1;
        Command_StartTime[Command_Place] = -1;
    }*/


    IEnumerator EraseFill(int Command_Place)
    {
        Effects[Command_Place].transform.DOScale(new Vector2(1.3f, 1.3f), 0.25f).SetEase(Ease.OutCubic);
        Controller.GetComponent<FlameController>().OnHibanapushed(Command_Place);
        yield return new WaitForSeconds(0.25f);
        Afters[Command_Place].fillAmount = 0;
        Effects[Command_Place].transform.DOScale(new Vector2(0f, 0f), 0.25f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(0.25f);
        Controller.GetComponent<Main>().TutorialSection(4, "");
        Effects[Command_Place].enabled = false;
        Effects[Command_Place].transform.localScale = new Vector2(1f, 1f);
    }


    public ObscuredBool Halfed = false;

    //環境のやつ
    private void EnvironmentPower(int Command_Num)
    {
        var Commands = Controller.GetComponent<CommandList>().Commands;
        if (Commands[Command_Num].PowerE == 0) return;
        Debug.Log("Buff=" + BuffPer[ObscuredPrefs.GetInt("Difficulty")] + " Num=" + ObscuredPrefs.GetInt("Difficulty"));
        double AddCO2 =(ObscuredFloat) Commands[Command_Num].PowerE * System.Math.Pow(BuffPer[ObscuredPrefs.GetInt("Difficulty")], BuffedTime[Commands[Command_Num].MyIcon]) * (Commands[Command_Num].SelfDebuff ? System.Math.Pow(0.9f, Commands[Command_Num].DebNum) : 1f)*(Halfed?0.5f:1f);
        ObscuredDouble l = 0;
        ObscuredInt power = Controller.GetComponent<EnemyAI>().totalpower();
        for (ObscuredInt i = 0; i < 21; i++) {
            var Area = Controller.GetComponent<EnemyAI>().Areas[i];
            float tempCO2 = (float)(AddCO2 / 32015013f/21f * (ObscuredFloat)power*(Exist(i,Commands[i].AdAreas)?1.2f:1f) * System.Math.Pow((95f - Area.Validity/30f) / 100f, Area.DebuffedTime[Commands[Command_Num].MyIcon])*(0.95f-Area.Company/2000f));
            l += tempCO2;
            Controller.GetComponent<EnemyAI>().TempAddition(i,tempCO2, 0);
            Controller.GetComponent<EnemyAI>().TempAddition(i, tempCO2*12.6f, 1);/*12.6f*/
        }
        Halfed = false;
        Debug.Log("SumCO2="+l);
    }
    private ObscuredBool Exist(int Area,ObscuredInt[] List)
    {
        ObscuredBool res = false;
        foreach(ObscuredInt comp in List)
        {
            if (Area == comp) res = true;
        }
        return res;
    }

    //危機感のやつ
    private void DangerPower(int Command_Num)
    {
        var Commands = Controller.GetComponent<CommandList>().Commands;
        if (Commands[Command_Num].PowerD == 0) return;
        ObscuredDouble l = 0;
        ObscuredDouble AddDanger = Commands[Command_Num].PowerD * System.Math.Pow(BuffPer[ObscuredPrefs.GetInt("Difficulty")], BuffedTime[Commands[Command_Num].MyIcon]) * (Commands[Command_Num].SelfDebuff ? System.Math.Pow(0.9, Commands[Command_Num].DebNum) : 1) ;
        ObscuredInt population = Controller.GetComponent<EnemyAI>().totalpop();
        for (ObscuredInt i = 0; i < 21; i++)
        {
            var Area = Controller.GetComponent<EnemyAI>().Areas[i];
            ObscuredDouble tempDanger = AddDanger / 7706807f /21f* (ObscuredFloat)population * (Exist(i, Commands[i].AdAreas) ? 1.2 : 1) * System.Math.Pow((95f - Area.Validity/30f) / 100f, Area.DebuffedTime[Commands[Command_Num].MyIcon]);
            l += tempDanger;
            Controller.GetComponent<EnemyAI>().TempAddition(i, (ObscuredFloat)(-tempDanger*1.5f), 1);
        }
        Debug.Log("SumDanger=" + l);
    }
    //政策の有効性のやつ
    private void ValidityPower(int Command_Num)
    {
        var Commands = Controller.GetComponent<CommandList>().Commands;
        if (Commands[Command_Num].PowerV == 0) return;
        ObscuredDouble l = 0;
        ObscuredDouble MinusValidity = Commands[Command_Num].PowerV * System.Math.Pow(BuffPer[ObscuredPrefs.GetInt("Difficulty")], BuffedTime[Commands[Command_Num].MyIcon]) * (Commands[Command_Num].SelfDebuff ? System.Math.Pow(0.9, Commands[Command_Num].DebNum) : 1);
        ObscuredInt population = Controller.GetComponent<EnemyAI>().totalpop();
        for (ObscuredInt i = 0; i < 21; i++)
        {
            var Area = Controller.GetComponent<EnemyAI>().Areas[i];
            Debug.Log(population);
            ObscuredDouble tempValidity = MinusValidity / 7706807f /21f* (ObscuredFloat)population * (Exist(i, Commands[i].AdAreas) ? 1.2f : 1f) * System.Math.Pow((95f - Area.Validity/ 30f) / 100f, Area.DebuffedTime[Commands[Command_Num].MyIcon]);
            l += tempValidity;
            Controller.GetComponent<EnemyAI>().TempAddition(i, (ObscuredFloat)(-tempValidity * 1.5f), 2);
        }
        Debug.Log("SumValidity=" + l);
    }
    //政策の頻度のやつ
    private void FrequencyPower(int Command_Num)
    {
        var Commands = Controller.GetComponent<CommandList>().Commands;
        if (Commands[Command_Num].PowerF == 0) return;
        ObscuredDouble l = 0;
        ObscuredDouble MinusFrequency = Commands[Command_Num].PowerF * System.Math.Pow(BuffPer[ObscuredPrefs.GetInt("Difficulty")], BuffedTime[Commands[Command_Num].MyIcon]) * (Commands[Command_Num].SelfDebuff ? System.Math.Pow(0.9, Commands[Command_Num].DebNum) : 1);
        ObscuredInt population = Controller.GetComponent<EnemyAI>().totalpop();
        for (ObscuredInt i = 0; i < 21; i++)
        {
            var Area = Controller.GetComponent<EnemyAI>().Areas[i];
            ObscuredDouble tempFrequency =  MinusFrequency / 7706807f / 21f * population * (Exist(i, Commands[i].AdAreas) ? 1.2 : 1) * System.Math.Pow((95f - Area.Validity/30f) / 100f, Area.DebuffedTime[Commands[Command_Num].MyIcon]);
            l += tempFrequency;
            Controller.GetComponent<EnemyAI>().TempAddition(i, (ObscuredFloat)(-tempFrequency * 1.5f), 3);
        }
        Debug.Log("SumFrequency=" + l);
    }
    //企業の姿勢のやつ
    private void CompanyPower(int Command_Num)
    {
        var Commands = Controller.GetComponent<CommandList>().Commands;
        if (Commands[Command_Num].PowerC == 0) return;
        ObscuredDouble l = 0;
        ObscuredDouble MinusCompany = Commands[Command_Num].PowerC * System.Math.Pow(BuffPer[ObscuredPrefs.GetInt("Difficulty")], BuffedTime[Commands[Command_Num].MyIcon]) * (Commands[Command_Num].SelfDebuff ? System.Math.Pow(0.9, Commands[Command_Num].DebNum) : 1);
        ObscuredInt population = Controller.GetComponent<EnemyAI>().totalpop();
        for (ObscuredInt i = 0; i < 21; i++)
        {
            var Area = Controller.GetComponent<EnemyAI>().Areas[i];
            ObscuredDouble tempCompany =  MinusCompany / 7706807f / 21f * population * (Exist(i, Commands[i].AdAreas) ? 1.2 : 1) * System.Math.Pow((95f - Area.Validity/30f) / 100f, Area.DebuffedTime[Commands[Command_Num].MyIcon]);
            l += tempCompany;
            Controller.GetComponent<EnemyAI>().TempAddition(i, (ObscuredFloat)(-tempCompany * 1.5f), 4);
        }
        Debug.Log("SumCompany=" + l);
    }
}
