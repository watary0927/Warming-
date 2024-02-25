using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Detectors;
using CodeStage.AntiCheat.Storage;

public class ChangeMode : MonoBehaviour
{
    public ObscuredInt Mode;
    //0は地球儀、1はEnvironment、2はSociety
    public GameObject SE;
    public GameObject SS;
    public GameObject Panel;
    public GameObject controller;
    public GameObject Earth;
    public GameObject Buff;
    public GameObject MyIcon;
    public Button DecideButton;
    public GameObject db;
    public GameObject TitleText;
    void Start()
    {
        Panel.SetActive(false);
        SS.SetActive(false);
        SE.SetActive(false);
        Mode = 0;
        ShowMode(Mode);
        controller.GetComponent<EnemyAI>().Scrollview.SetActive(false);

    }
    public void OnClickChange()
    {
        DecideButton.interactable = false;
        if(Mode==0)db.transform.DORotate(new Vector3(0f, 0f,120f), 0.15f).SetEase(Ease.InOutCubic);
        else if(Mode==1) db.transform.DORotate(new Vector3(0f, 0f, 240f), 0.15f).SetEase(Ease.InOutCubic);
        else db.transform.DORotate(new Vector3(0f, 0f, 0f), 0.15f).SetEase(Ease.InOutCubic);
        Earth.GetComponent<PutPins>().initialize(); 
        Mode = (Mode + 1) % 3;
        ShowMode(Mode);
        Invoke("ReActivate", 0.15f);
    }
    public void ShowMode(int mode)
    {
        Mode = mode;//ミッションの時の変更に対応
            Earth.GetComponent<PutPins>().ALLBleach();
        if (mode == 0)
        {
            controller.GetComponent<FukidasiShow>().Show("Home", "", 13, new ObscuredInt[] { }, false, 0, 0);
            TitleText.transform.localPosition = new Vector3(0f, 238.64f, 0);
            Buff.SetActive(false);
            MyIcon.SetActive(false);
            controller.GetComponent<FukidasiShow>().ZeroStart();
            Earth.GetComponent<PutPins>().OnArrive();
            Panel.transform.DOScale(new Vector2(1f,1f), 0.15f).SetEase(Ease.InCubic);
            SS.transform.DOScale(new Vector2(0.2f,0.2f), 0.15f).SetEase(Ease.InCubic);
            Panel.GetComponent<Image>().DOFade(0f,0.15f).SetEase(Ease.InCubic);
            SS.GetComponent<Image>().DOFade(0f, 0.15f).SetEase(Ease.InCubic);
            SE.SetActive(false);
            /*if (controller.GetComponent<ProsessCommand>().CurrentCommand>=21&& controller.GetComponent<ProsessCommand>().CurrentCommand <=41)
            {
                controller.GetComponent<ProsessCommand>().Buttons[controller.GetComponent<ProsessCommand>().CurrentCommand].transform.DOScale(new Vector2(1.35f, 1.35f), 0.1f);
            }*/
            Invoke("Zero", 0.15f);
        }
        else if (mode == 1)
        {
            controller.GetComponent<FukidasiShow>().Show("", "", 13, new ObscuredInt[] { }, false, 0, 0);
            TitleText.transform.localPosition = new Vector3(-160f, 238.64f, 0);
            Buff.SetActive(true);
            MyIcon.SetActive(false);
            controller.GetComponent<FukidasiShow>().OnChangeMode();
            Panel.SetActive(true);
            SE.SetActive(true);
            SS.SetActive(false);
            Panel.transform.DOScale(new Vector2(2.5f, 2.5f), 0.15f).SetEase(Ease.OutCubic);
            SE.transform.DOScale(new Vector2(0.5f, 0.5f), 0.15f).SetEase(Ease.OutCubic);
            Panel.GetComponent<Image>().DOFade(30f/225f, 0.15f).SetEase(Ease.OutCubic);
            SE.GetComponent<Image>().DOFade(1f, 0.15f).SetEase(Ease.OutCubic);
            Earth.GetComponent<PutPins>().OnLeave();
            controller.GetComponent<EnemyAI>().Scrollview.SetActive(false);
            Earth.GetComponent<PutPins>().bararea.enabled = false;
            Earth.GetComponent<PutPins>().barcommand.enabled = false;
            Earth.GetComponent<PutPins>().graph.enabled = false;
            controller.GetComponent<Main>().TutorialSection(1, "");
        }
        else
        {
            controller.GetComponent<FukidasiShow>().Show("", "", 13, new ObscuredInt[] { }, false, 0, 0);
            MyIcon.SetActive(false);
            Panel.SetActive(true);
            SE.SetActive(false);
            SS.SetActive(true);
            SE.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            SS.GetComponent<Image>().DOFade(1f, 0.15f).SetEase(Ease.OutCubic);
            SS.transform.DOScale(new Vector2(0.5f, 0.5f), 0.15f).SetEase(Ease.OutCubic);
            if (controller.GetComponent<ProsessCommand>().CurrentCommand>=0&& controller.GetComponent<ProsessCommand>().CurrentCommand <=20)
            {
                controller.GetComponent<ProsessCommand>().Buttons[controller.GetComponent<ProsessCommand>().CurrentCommand].transform.DOScale(new Vector2(1f, 1f), 0.05f);
            }
            controller.GetComponent<EnemyAI>().Scrollview.SetActive(false);
            controller.GetComponent<Main>().TutorialSection(2, "");
        }
    }

    void Zero()
    {
        Panel.SetActive(false);
        SE.SetActive(false);
        SS.SetActive(false);
        SE.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        SS.transform.localScale = new Vector3(0.3f,0.3f, 1f);
        Panel.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        Panel.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        SE.GetComponent<Image>().color=new Color(1f,1f,1f,0f);
        SS.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        db.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }
    void ReActivate()
    {
        DecideButton.interactable = true;
    }
}
