using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;

using DG.Tweening;

public class PolicyFlow : MonoBehaviour
{
    // テキストを流す
    public ObscuredBool TextExist;//テキストが存在しているかどうか
    public Text PolicyText;

    private void Start()
    {
        TextExist = false;
    }

    void Update()
    {
        if (TextExist)
        {
            /*Debug.Log("ProperlyMoving");*/
            PolicyText.transform.localPosition -= new Vector3(300f*Time.deltaTime, 0f, 0f);
            if(PolicyText.transform.localPosition.x<= -PolicyText.preferredWidth)
            {
                TextExist = false;
                PolicyText.text = "";
            }
        }
    }

    public void PolicyShow(string policy)
    {
        Debug.Log(policy);
        TextExist = true;
        PolicyText.text = policy;
        PolicyText.transform.localPosition = new Vector3(PolicyText.preferredWidth, 0f, 0f);
    }

}
