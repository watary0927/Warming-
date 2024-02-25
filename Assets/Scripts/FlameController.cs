using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;

public class FlameController : MonoBehaviour
{
    public GameObject[] Hibanas;
    // Start is called before the first frame update
    public GameObject Burner;
    public GameObject controller;
    void Start()
    {
        foreach(GameObject Hibana in Hibanas)
        {
            Hibana.SetActive(false);
        }
    }

    public void OnHibanapushed(int i)
    {
        Change(i);
        controller.GetComponent<Main>().SoundActivate(5);
        DOVirtual.DelayedCall(0.05f, () => Change(i));
        DOVirtual.DelayedCall(0.1f, () => Change(i));
        DOVirtual.DelayedCall(0.15f, () => Hibanas[i].SetActive(false));
    }

    void Change(int i)
    {
        ObscuredInt rotation = UnityEngine.Random.Range(0, 360);
        ObscuredFloat size= UnityEngine.Random.Range(1f, 1.5f);
        Hibanas[i].transform.Rotate(new Vector3(0, 0, rotation));
        Hibanas[i].transform.localScale=new Vector3(size,size,1);
        Hibanas[i].SetActive(true);
    }
}
