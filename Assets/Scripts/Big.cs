using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Detectors;

public class Big : MonoBehaviour
{

    public GameObject[] Bubbles;
    public GameObject Earth;
    public GameObject[] Layers;

    public ObscuredInt Acceleration = 0;
    //0通常走行1加速2逆加速
    ObscuredFloat SpentTime = 0;
    ObscuredInt Num = 0;
    void Update()
    {
        SpentTime += Time.deltaTime;
        if (SpentTime >= 5)
        {
            StartCoroutine("Blink", Num * 2);
            Num = (Num + 1) % 4;
            SpentTime = 0;
        }
        if (Acceleration == 0)
        {
            Earth.transform.Rotate(new Vector3(0, 12 * Time.deltaTime, 0));
            Bubbles[0].transform.Translate(0.18f * Time.deltaTime, 0f, 0f);
            Bubbles[1].transform.Translate(0.18f * Time.deltaTime, 0f, 0f);
        }
        else if (Acceleration == 1)
        {
            Earth.transform.Rotate(new Vector3(0, 300 * Time.deltaTime, 0));
            Bubbles[0].transform.Translate(5f * Time.deltaTime, 0f, 0f);
            Bubbles[1].transform.Translate(5f * Time.deltaTime, 0f, 0f);
        }
        else if (Acceleration == 2)
        {
            Earth.transform.Rotate(new Vector3(0, -300 * Time.deltaTime, 0));
            Bubbles[0].transform.Translate(-5f * Time.deltaTime, 0f, 0f);
            Bubbles[1].transform.Translate(-5f * Time.deltaTime, 0f, 0f);
        }
            // Debug.Log("Moving");
            
        foreach(GameObject Bubble in Bubbles)
        {
            if (Bubble.transform.position.x >=4) Bubble.transform.position = new Vector3(Bubble.transform.position.x-8f, Bubble.transform.position.y, 0);
            if (Bubble.transform.position.x < -4) Bubble.transform.position = new Vector3(Bubble.transform.position.x + 8f, Bubble.transform.position.y, 0);
        }
    }

    IEnumerator Blink(int i)
    {
        Layers[i].SetActive(false);
        Layers[i+1].SetActive(false);
        yield return new WaitForSeconds(0.1f);
        Layers[i].SetActive(true);
        Layers[i + 1].SetActive(true);
    }
}
