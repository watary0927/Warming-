using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;

public class EarthPosition : MonoBehaviour
{
    public GameObject Earth;//地球
    Vector3 CanvasPos;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        CanvasPos = new Vector3(Screen.width/2, Screen.height / 2, 0f);
        Vector3 localPos = new Vector3(0f, 400f* Screen.height/2688f, 0f);
        Debug.Log("localPos.y="+localPos.y);
        //ObscuredFloat Scale =(ObscuredFloat) 0.09 * 1920f* Screen.width / Screen.height / 1080f;
        //ObscuredFloat Scale =(ObscuredFloat) 0.09 * 1125f* Screen.height / Screen.width / 2436f;
        ObscuredFloat Scale = 0.1f;
        Debug.Log(localPos + CanvasPos);
        Vector3 offset = new Vector3(0f,40f, 0f);
        Earth.transform.position=cam.ScreenToWorldPoint(localPos+CanvasPos+offset);
        Earth.transform.position= new Vector3(Earth.transform.position.x, Earth.transform.position.y, 0f);
        Earth.transform.localScale = new Vector3(Scale, Scale, Scale);
    }
}
