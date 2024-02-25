using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OnSceneChange : MonoBehaviour
{
    public GameObject Parent;//親オブジェクト 
    public GameObject Pin;//我らがアイコンピン
    public Image Blackground;//黒背景　透過する
    //遷移時直後・直前にはblackが完全に黒、


    void Start()
    {
        /*Parent.SetActive(false);//何かの間違い対策一回*/
    }

    public void Sceneout()
    {
        Debug.Log("SceneoutStart");
        Blackground.color =new Color(0f, 0f, 0f, 0f);
        Pin.transform.localScale= new Vector3(1f, 1f, 1f);
        Parent.SetActive(true);
        Blackground.DOFade(1f, 0.5f);
        Pin.transform.DORotate(new Vector3(0f, 0f, 360f),0.5f, RotateMode.FastBeyond360);
        Pin.transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f);
        //だんだん背景が黒に
        //ピンが一回転しながら縮小
        //0.5秒後に完全に暗転、シーン移動
    }
    public void Scenein()
    {
         Debug.Log("SceneinStart"); 
        Blackground.color = new Color(0f, 0f, 0f, 1f);
        Pin.transform.localScale = new Vector3(0f, 0f, 0f);
        Parent.SetActive(true);
        Blackground.DOFade(0f, 0.5f);
        Pin.transform.DORotate(new Vector3(0f,0f, 360f), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic);
        Pin.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        StartCoroutine("setfalse");
    }

    public void JustErase()
    {
        Parent.SetActive(false);
    }

     IEnumerator setfalse()
    {
        yield return new WaitForSeconds(0.5f); 
        Parent.SetActive(false);
    }
}

