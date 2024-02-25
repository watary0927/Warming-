using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Common;
using CodeStage.AntiCheat.Storage;
using CodeStage.AntiCheat.Detectors;

public class AutoSaver : MonoBehaviour
{

    public GameObject controller;

    public void AutoSave()//年ごとに色んなデータを保存する
    {
        Debug.Log("AutoSaving");

        ObscuredPrefs.SetInt("HasSaveData", 1);
        ObscuredPrefs.SetFloat("TotalCO2Amount", (ObscuredFloat)controller.GetComponent<EnemyAI>().TotalCO2Amount);

        ObscuredPrefs.SetInt("MissionGoing", controller.GetComponent<MissionManager>().MissionGoing==true?1:0);
        ObscuredPrefs.SetInt("MissionDone", controller.GetComponent<MissionManager>().MissionDone);
        ObscuredPrefs.SetString("titletext", controller.GetComponent<MissionManager>().titletext);
        ObscuredPrefs.SetString("maintext", controller.GetComponent<MissionManager>().maintext);
        ObscuredPrefs.SetInt("missionType", controller.GetComponent<MissionManager>().missionType);
        ObscuredPrefs.SetInt("missionDuration", controller.GetComponent<MissionManager>().missionDuration);
        ObscuredPrefs.SetInt("missionCommand", controller.GetComponent<MissionManager>().missionCommand);
        for(ObscuredInt i=0;i< ObscuredPrefs.GetInt("Year") / 20; i++)
        {
            ObscuredPrefs.SetInt("WhichMission"+i, controller.GetComponent<MissionManager>().WhichMission[i]);
        }

        ObscuredPrefs.SetInt("Halfed", controller.GetComponent<ProsessCommand>().Halfed==true?1:0);

        ObscuredPrefs.SetInt("Currentyear", controller.GetComponent<GetDate>().CurrentYear);
        for (int i = 0; i < 13; i++)
        {
            ObscuredPrefs.SetInt("BuffedTime"+i, controller.GetComponent<ProsessCommand>().BuffedTime[i]);
        }
        for (int i = 0; i < 3; i++)
        {
            ObscuredPrefs.SetInt("Command_Using"+i, controller.GetComponent<ProsessCommand>().Command_Using[i]);
            if (controller.GetComponent<ProsessCommand>().Command_Using[i]!=-1)ObscuredPrefs.SetFloat("Command_Per" + i, (controller.GetComponent<Timer>().GetTime() - controller.GetComponent<ProsessCommand>().Command_StartTime[i]) / (controller.GetComponent<CommandList>().Commands[controller.GetComponent<ProsessCommand>().Command_Using[i]].Year * 15f));
        }
        //割合を計測しなければならないんですが、色々と面倒なので後でどうにかする
            ObscuredPrefs.SetFloat("IncreasedTemp" + (controller.GetComponent<GetDate>().CurrentYear-2020), (ObscuredFloat)controller.GetComponent<GetDate>().IncreasedTemp[controller.GetComponent<GetDate>().CurrentYear-2020]);
        for(int i = 0; i < 21; i++)
        {
            ObscuredPrefs.SetFloat("Danger" + i, (ObscuredFloat)controller.GetComponent<EnemyAI>().Areas[i].Danger);
            ObscuredPrefs.SetFloat("Validity" + i, (ObscuredFloat)controller.GetComponent<EnemyAI>().Areas[i].Validity);
            ObscuredPrefs.SetFloat("Frequency" + i, (ObscuredFloat)controller.GetComponent<EnemyAI>().Areas[i].Frequency);
            ObscuredPrefs.SetFloat("Company" + i, (ObscuredFloat)controller.GetComponent<EnemyAI>().Areas[i].Company);
            ObscuredPrefs.SetFloat("Collection" + i, (ObscuredFloat)controller.GetComponent<EnemyAI>().Areas[i].Collection);
            for(ObscuredInt j = 0; j < 13; j++) {
                ObscuredPrefs.SetInt(i+"DebuffedTime" + j, controller.GetComponent<EnemyAI>().Areas[i].DebuffedTime[j]);
            }
        }
        for (int i = 0; i < 42; i++)
        {
            if(controller.GetComponent<CommandList>().Commands[i].SelfDebuff==true) ObscuredPrefs.SetInt("DebNum" + i, controller.GetComponent<CommandList>().Commands[i].DebNum);
            ObscuredPrefs.SetInt("NowUsed" + i, controller.GetComponent<CommandList>().Commands[i].NowUsed? 1 : 0);
            ObscuredPrefs.SetInt("AlreadyUsed" + i, controller.GetComponent<CommandList>().Commands[i].AlreadyUsed ? 1 : 0);
        }

        ObscuredPrefs.SetInt("ECommandNum", controller.GetComponent<ProsessCommand>().ECommandNum);
        ObscuredPrefs.SetInt("SCommandNum", controller.GetComponent<ProsessCommand>().SCommandNum);
        ObscuredPrefs.SetInt("PoliticsDone", controller.GetComponent<EnemyAI>().PoliticsDone);
        ObscuredPrefs.SetFloat("AllCollection", controller.GetComponent<EnemyAI>().AllCollection);
        ObscuredPrefs.SetInt("missionclear", controller.GetComponent<MissionManager>().missionclear);

        ObscuredPrefs.Save();
    }

    public void Rewrite()//開始時にデータの読み込み、再設定を行う
    {
        Debug.Log("Rewriting");

        controller.GetComponent<Main>().SoundActivate(ObscuredPrefs.GetInt("BGM") + 6);

        controller.GetComponent<GetDate>().CurrentYear = ObscuredPrefs.GetInt("Currentyear");
        controller.GetComponent<GetDate>().startyear = ObscuredPrefs.GetInt("Currentyear");
        controller.GetComponent<EnemyAI>().TotalCO2Amount = ObscuredPrefs.GetFloat("TotalCO2Amount");
        controller.GetComponent<GetDate>().Date.text = $"{ObscuredPrefs.GetInt("Currentyear")}年01月";
        controller.GetComponent<MissionManager>().MissionGoing = ObscuredPrefs.GetInt("MissionGoing") == 1 ? true : false;
        controller.GetComponent<MissionManager>().MissionDone = ObscuredPrefs.GetInt("MissionDone");
        controller.GetComponent<MissionManager>().titletext = ObscuredPrefs.GetString("titletext");
        controller.GetComponent<MissionManager>().maintext = ObscuredPrefs.GetString("maintext");
        controller.GetComponent<MissionManager>().missionType = ObscuredPrefs.GetInt("missionType");
        controller.GetComponent<MissionManager>().missionDuration = ObscuredPrefs.GetInt("missionDuration");
        controller.GetComponent<MissionManager>().missionCommand = ObscuredPrefs.GetInt("missionCommand");
        if(ObscuredPrefs.GetInt("MissionGoing")==1|| ObscuredPrefs.GetInt("MissionDone") >= 1)
        {
            controller.GetComponent<MissionManager>().Frag.SetActive(true);
        }
        for (int i = 0; i < ObscuredPrefs.GetInt("Year") / 20; i++)
        {
            controller.GetComponent<MissionManager>().WhichMission[i] = ObscuredPrefs.GetInt("WhichMission" + i);
        }

        ObscuredPrefs.SetInt("Halfed", controller.GetComponent<ProsessCommand>().Halfed == true ? 1 : 0);

        controller.GetComponent<ProsessCommand>().Halfed = ObscuredPrefs.GetInt("Halfed") == 1 ? true : false;

        for (int i = 0; i < 13; i++)
        {
            controller.GetComponent<ProsessCommand>().BuffedTime[i] = ObscuredPrefs.GetInt("BuffedTime" + i);
        }
        for (int i = 0; i < 3; i++)
        {
            controller.GetComponent<ProsessCommand>().Command_Using[i] = ObscuredPrefs.GetInt("Command_Using" + i);
           // Debug.Log("Using=" + controller.GetComponent<ProsessCommand>().Command_Using[i]);
            if (controller.GetComponent<ProsessCommand>().Command_Using[i] != -1) {
                controller.GetComponent<ProsessCommand>().Command_StartTime[i] = controller.GetComponent<Timer>().GetTime() - (ObscuredPrefs.GetFloat("Command_Per"+i) * controller.GetComponent<CommandList>().Commands[controller.GetComponent<ProsessCommand>().Command_Using[i]].Year * 15f);
                Debug.Log("Starttime="+ ObscuredPrefs.GetFloat("Command_Per"));
            }
        }
        for (int i = 0; i <= ObscuredPrefs.GetInt("Currentyear")-2020; i++)
        {
            controller.GetComponent<GetDate>().IncreasedTemp[i] = ObscuredPrefs.GetFloat("IncreasedTemp" + i);
        }
        for (int i = 0; i < 21; i++)
        {
            controller.GetComponent<EnemyAI>().Areas[i].Danger = ObscuredPrefs.GetFloat("Danger" + i);
            controller.GetComponent<EnemyAI>().Areas[i].Validity = ObscuredPrefs.GetFloat("Validity" + i);
            controller.GetComponent<EnemyAI>().Areas[i].Frequency = ObscuredPrefs.GetFloat("Frequency" + i);
            controller.GetComponent<EnemyAI>().Areas[i].Company = ObscuredPrefs.GetFloat("Company" + i);
            controller.GetComponent<EnemyAI>().Areas[i].Collection = ObscuredPrefs.GetFloat("Collection" + i);
            for (int j = 0; j < 13; j++)
            {
                controller.GetComponent<EnemyAI>().Areas[i].DebuffedTime[j] = ObscuredPrefs.GetInt(i + "DebuffedTime" + j);
            }
        }
        for (int i = 0; i < 42; i++)
        {
            if (controller.GetComponent<CommandList>().Commands[i].SelfDebuff == true)controller.GetComponent<CommandList>().Commands[i].DebNum = ObscuredPrefs.GetInt("DebNum" + i);
            controller.GetComponent<CommandList>().Commands[i].NowUsed = (ObscuredPrefs.GetInt("NowUsed" + i)==1 ? true : false);
            controller.GetComponent<CommandList>().Commands[i].AlreadyUsed = ObscuredPrefs.GetInt("AlreadyUsed" + i)==1 ? true : false;
        }
        ObscuredPrefs.SetInt("ECommandNum", controller.GetComponent<ProsessCommand>().ECommandNum);
        ObscuredPrefs.SetInt("SCommandNum", controller.GetComponent<ProsessCommand>().SCommandNum);
        ObscuredPrefs.SetInt("PoliticsDone", controller.GetComponent<EnemyAI>().PoliticsDone);
        ObscuredPrefs.SetFloat("AllCollection", controller.GetComponent<EnemyAI>().AllCollection);
        ObscuredPrefs.SetInt("missionclear", controller.GetComponent<MissionManager>().missionclear);
        controller.GetComponent<ProsessCommand>().ECommandNum = ObscuredPrefs.GetInt("ECommandNum");
        controller.GetComponent<ProsessCommand>().SCommandNum = ObscuredPrefs.GetInt("SCommandNum");
        controller.GetComponent<EnemyAI>().PoliticsDone = ObscuredPrefs.GetInt("PoliticsDone");
        controller.GetComponent<EnemyAI>().AllCollection = ObscuredPrefs.GetFloat("AllCollection");
        controller.GetComponent<MissionManager>().missionclear = ObscuredPrefs.GetInt("missionclear");

    }

    public void BlockAccess()
    {
        ObscuredPrefs.SetInt("HasSaveData", 0);
        ObscuredPrefs.Save();
    }

}
