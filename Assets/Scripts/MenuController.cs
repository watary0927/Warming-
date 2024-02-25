using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using GoogleMobileAds.Api;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;

public class MenuController : MonoBehaviour
{
    private ObscuredInt CurrentStage =0;//0は初期、1は難易度選択、2は時間選択、5で本ゲーム画面に移動、（初期),4で本ゲーム画面（引継ぎ）,3で確認

    public GameObject newgame;
    public GameObject restart;
    public GameObject configuration;
    public GameObject Return;
    public GameObject StartGame;
    public GameObject[] Years;
    public GameObject[] Difficulties;
    public GameObject[] Tapes;
    public GameObject controller;
    Vector3 canvas = new Vector3(Screen.width / 2, Screen.height / 2, 0);

    public ObscuredInt Year=20;//ゲーム内の年数
    public ObscuredInt Difficulty;//ゲーム難易度

    public GameObject[] Alarms;
    public Text title;

    public static ObscuredBool Sceneinallowed=false;

    public Material earthcolor;

    private BannerView bannerView;

    public Sprite[] ranks=new Sprite[5];
    public Image[] rankyear = new Image[4];
    public Image[] rankdif0 = new Image[4];
    public Image[] rankdif1 = new Image[4];
    public Image[] rankdif2 = new Image[4];
    public Image[] rankdif3 = new Image[4];


    [System.Obsolete]
    void Start()
    {
        rule.SetActive(false);
        Restore.SetActive(false);
#if UNITY_ANDROID && !UNITY_EDITOR
                    ObscuredString appId = "ca-app-pub-2102086917963437/5939115095";
#elif UNITY_IPHONE && !UNITY_EDITOR
                ObscuredString appId = "ca-app-pub-2102086917963437~2490789289";
#else
        ObscuredString appId = "unexpected_platform";
#endif
        if (appId != "unexpected_platform")
        {
            MobileAds.Initialize(appId);
            controller.GetComponent<AdShower>().RequestBanner();
        }

        Cancel();
        CurrentStage = 0;
        StageChange(CurrentStage);
        if(Sceneinallowed)StartCoroutine("StartScene");
        else
        {
            controller.GetComponent<OnSceneChange>().JustErase();
            Sceneinallowed = true;
        }
        foreach (GameObject Year in Years)
        {
            Year.SetActive(false);
            Year.transform.position += new Vector3(Screen.width,0,0);
        }
        foreach (GameObject Difficulty in Difficulties)
        {
            Difficulty.SetActive(false);
            Difficulty.transform.position += new Vector3(Screen.width, 0, 0);
        }
        if (!ObscuredPrefs.HasKey("BGM"))
        {
            ObscuredPrefs.SetInt("BGM", 0);
            ObscuredPrefs.Save();
        }
        Noah.interactable = AuNoah();
        Brand.interactable = AuMusic();
    }

    public AudioSource bgm;

    IEnumerator StartScene()
    {
        controller.GetComponent<OnSceneChange>().Scenein();
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator EndScene()
    {
        controller.GetComponent<AdShower>().Destroy();
        controller.GetComponent<OnSceneChange>().Sceneout();
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("GameScene");
    }

    IEnumerator ModeUp()
    {
        controller.GetComponent<Big>().Acceleration = 1;
        yield return new WaitForSeconds(0.15f);
        controller.GetComponent<Big>().Acceleration = 0;
    }

    IEnumerator ModeDown()
    {
        controller.GetComponent<Big>().Acceleration = 2;
        yield return new WaitForSeconds(0.15f);
        controller.GetComponent<Big>().Acceleration = 0;
    }

    public Button[] HardMode;

    public void NewGame() {
        StageChange(1);
        for(ObscuredInt i = 1; i < 5; i++) {
            if(!ObscuredPrefs.HasKey("Result" + 0 + i*20)|| ObscuredPrefs.GetInt("Result" + 0 + i*20)>3) {
                rankdif0[i - 1].sprite = ranks[4];
            }
            else
            {
                rankdif0[i - 1].sprite = ranks[ObscuredPrefs.GetInt("Result" + 0 + i * 20)];
            }
            rankdif0[i - 1].SetNativeSize();
        }
        for (int i = 1; i < 5; i++)
        {
            if (!ObscuredPrefs.HasKey("Result" + 1 + i * 20) || ObscuredPrefs.GetInt("Result" + 1 + i * 20) > 3)
            {
                rankdif1[i - 1].sprite = ranks[4];
            }
            else
            {
                rankdif1[i - 1].sprite = ranks[ObscuredPrefs.GetInt("Result" + 1 + i * 20)];
            }
            rankdif1[i - 1].SetNativeSize();
        }
        for (int i = 1; i < 5; i++)
        {
            if (!ObscuredPrefs.HasKey("Result" + 2 + i * 20) || ObscuredPrefs.GetInt("Result" + 2 + i * 20) > 3)
            {
                rankdif2[i - 1].sprite = ranks[4];
            }
            else
            {
                rankdif2[i - 1].sprite = ranks[ObscuredPrefs.GetInt("Result" + 2 + i * 20)];
            }
            rankdif2[i - 1].SetNativeSize();
        }
        for (int i = 1; i < 5; i++)
        {
            if (!ObscuredPrefs.HasKey("Result" + 3 + i * 20) || ObscuredPrefs.GetInt("Result" + 3 + i * 20) > 3)
            {
                rankdif3[i - 1].sprite = ranks[4];
            }
            else
            {
                rankdif3[i - 1].sprite = ranks[ObscuredPrefs.GetInt("Result" + 3 + i * 20)];
            }
            rankdif3[i - 1].SetNativeSize();
        }
    }
    public void Restart()
    {
        StageChange(4);
    }
    public void DecideDifficulty(int num)
    {
        Difficulty = num;
        ObscuredPrefs.SetInt("Difficulty",Difficulty);
        for (int i = 1; i < 5; i++)
        {
            if (!ObscuredPrefs.HasKey("Result" + ObscuredPrefs.GetInt("Difficulty") + i * 20) || ObscuredPrefs.GetInt("Result" + ObscuredPrefs.GetInt("Difficulty") + i * 20) > 3)
            {
                rankyear[i - 1].sprite = ranks[4];
            }
            else
            {
                rankyear[i - 1].sprite = ranks[ObscuredPrefs.GetInt("Result" + ObscuredPrefs.GetInt("Difficulty") + i * 20)];
            }
            rankyear[i - 1].SetNativeSize();
        }
        for (int i = 0; i < 4; i++)
        {
            HardMode[i].interactable = true;
        }
        if (Difficulty== 2) {
            int lim = -1;
            for(int i = 0; i < 4; i++) {
                HardMode[i].interactable = false;
            }
            for (int i = 4; i >=1; i--) {
                if (ObscuredPrefs.HasKey("Result" + 1 + i * 20) && ObscuredPrefs.GetInt("Result" + 1 + i * 20) > 1)//NormalでAランク以上
                {
                    lim = i;
                }
                if (lim != -1) break;
            }
            if (lim != -1) {
                for (int i = 0; i < lim; i++)
                {
                    HardMode[i].interactable = true;
                }
            }
        }

        StageChange(2);
    }
    public void DecideYear(int num)
    {
        Year = num * 20;
        ObscuredPrefs.SetInt("Year",Year);
        Debug.Log(ObscuredPrefs.GetInt("Year"));
        StageChange(3);
    }
    void AlarmShow(ObscuredString Title)//オプションとかを表示させる際のポップアップ
    {
        for (int i = 0; i < 2; i++) Alarms[i].SetActive(true);
        title.text = Title;
    }

    public  void AlarmErase()
    {
        for (int i = 0; i < 2; i++) Alarms[i].SetActive(false);
        if (sample.isPlaying)
        {
            sample.Stop();
        }
        if (!bgm.isPlaying)
        {
            bgm.Play();
        }
    }
    public void Cancel() {
        for (int i = 0; i < 2; i++) Alarms[i].SetActive(false);
    }


    public void Configuration()
    {//音量設定とか？が入る気がする
        Restore.SetActive(false);
        button.Play();
        for (int i = 0; i < 2; i++) Musics[i].SetActive(false);
        for (int i = 0; i < 2; i++) Buys[i].SetActive(false);
        conreturn.SetActive(false);
        title.color = new Color(0f, 0f, 0f, 1f);
        Music.transform.localPosition=new Vector2(0f,0f);
        Buy.transform.localPosition=new Vector2(0f,-200f);
        Music.SetActive(true);
        Buy.SetActive(true);
        Alarms[0].transform.localScale = new Vector2(0.5f, 0.5f);
        AlarmShow("設定");
    }
    public void Back()
    {
        StageChange(CurrentStage - 1);
    }
    public void StarttheGame()
    {
        StageChange(5);
    }

    public AudioSource button, tape;


    void StageChange(int Stage) {

        if (Stage == 0)//初期起動時
        {
            if (ObscuredPrefs.HasKey("HasSaveData") && ObscuredPrefs.GetInt("HasSaveData") ==1)
            {
                restart.SetActive(true);
            }else
            {
                restart.SetActive(false);
            }

            newgame.SetActive(true);
            Return.SetActive(false);
            foreach (GameObject Year in Years)
            {
                Year.SetActive(false);
            }
            if (Stage == CurrentStage + 1)
            {
                return;
            }
            else if (Stage == CurrentStage - 1)
            {
                button.Play();
                StartCoroutine("ModeDown");
                newgame.transform.DOMove(newgame.transform.position+new Vector3(Screen.width, 0, 0),0.2f);
                restart.transform.DOMove(restart.transform.position + new Vector3(Screen.width, 0, 0), 0.2f);
                foreach (GameObject Difficulty in Difficulties)
                {
                    Difficulty.transform.DOMove(Difficulty.transform.position + new Vector3(Screen.width, 0, 0), 0.2f);
                    DOVirtual.DelayedCall(0.2f, () => { Difficulty.SetActive(false); });
                }
                //
                Tapes[1].transform.DOLocalMove(new Vector3(-2000f, -650f, 0), 0.2f);
                DOVirtual.DelayedCall(0.2f, () => { Tapes[1].SetActive(false); });
                Tapes[2].transform.DOLocalMove(new Vector3(2800f, -400f, 0), 0.2f);
                DOVirtual.DelayedCall(0.2f, () => { Tapes[2].SetActive(false); });
            }
            StartGame.SetActive(false);
        }
        else if (Stage == 1)//難易度選択画面
        {
            Return.SetActive(true);
            foreach (GameObject Difficulty in Difficulties)
            {
                Difficulty.SetActive(true);
            }
            Noah.interactable = AuNoah();
            if (Stage == CurrentStage + 1)//順当に進んできたとき
            {
                tape.Play();
                newgame.transform.DOMove(newgame.transform.position + new Vector3(-Screen.width, 0, 0), 0.2f);
                DOVirtual.DelayedCall(0.2f, () => { newgame.SetActive(false); });
                restart.transform.DOMove(restart.transform.position + new Vector3(-Screen.width, 0, 0), 0.2f);
                DOVirtual.DelayedCall(0.2f, () => { restart.SetActive(false); });
                foreach (GameObject Difficulty in Difficulties)
                {
                    Difficulty.transform.DOMove(Difficulty.transform.position + new Vector3(-Screen.width, 0, 0), 0.2f);
                }
                StartCoroutine("ModeUp");
                Tapes[2].SetActive(true);
                Tapes[2].transform.DOLocalMove(new Vector3(0, 240f, 0), 0.2f);
                Tapes[1].SetActive(true);
                Tapes[1].transform.DOLocalMove(new Vector3(0, 210f, 0), 0.2f);

            }
            else if (Stage == CurrentStage - 1)//戻ってきたとき
            {
                button.Play();
                foreach (GameObject Difficulty in Difficulties)
                {
                    Difficulty.transform.DOMove(Difficulty.transform.position + new Vector3(Screen.width, 0, 0), 0.2f);
                }
                foreach (GameObject Year in Years)
                {
                    Year.transform.DOMove(Year.transform.position + new Vector3(Screen.width, 0, 0), 0.2f);
                    DOVirtual.DelayedCall(0.2f, () => { Year.SetActive(false); });
                }
                StartCoroutine("ModeDown");
                //
                Tapes[0].transform.DOLocalMove(new Vector3(-3000f, 950f, 0), 0.2f);
                Tapes[3].transform.DOLocalMove(new Vector3(2000f, 350f, 0), 0.2f);
                DOVirtual.DelayedCall(0.2f, () => { Tapes[0].SetActive(false); });
                DOVirtual.DelayedCall(0.2f, () => { Tapes[3].SetActive(false); });
            }
            StartGame.SetActive(false);
        }

        else if (Stage == 2)
        {
            Return.SetActive(true);
            foreach (GameObject Year in Years)
            {
                Year.SetActive(true);
            }
            if (Stage == CurrentStage + 1)//順当に進んできたとき
            {
                tape.Play();
                StartCoroutine("ModeUp");
                foreach (GameObject Difficulty in Difficulties)
                {
                    Difficulty.transform.DOMove(Difficulty.transform.position + new Vector3(-Screen.width, 0, 0), 0.2f);
                    DOVirtual.DelayedCall(0.2f, () => { Difficulty.SetActive(false); });
                }
                foreach (GameObject Year in Years)
                {
                    Year.transform.DOMove(Year.transform.position + new Vector3(-Screen.width, 0, 0), 0.2f);
                }
                //
                Tapes[0].SetActive(true);
                Tapes[3].SetActive(true);
                Tapes[0].transform.DOLocalMove(new Vector3(0, 500f, 0), 0.2f);
                Tapes[3].transform.DOLocalMove(new Vector3(0f, -100f, 0), 0.2f);
            }
            else if (Stage == CurrentStage - 1)//戻ってきたとき
            {
                button.Play();
                StartCoroutine("ModeDown");
                foreach (GameObject Year in Years)
                {
                    Year.transform.DOMove(Year.transform.position + new Vector3(Screen.width, 0, 0), 0.2f);
                }
                Tapes[0].SetActive(true);
                Tapes[0].transform.DOLocalMove(new Vector3(0, 500f, 0) , 0.2f);
                Tapes[1].SetActive(true);
                Tapes[1].transform.DOLocalMove(new Vector3(0, 210f, 0), 0.2f);
                Tapes[2].SetActive(true);
                Tapes[2].transform.DOLocalMove(new Vector3(0, 240f, 0), 0.2f);
                Tapes[3].SetActive(true);
                Tapes[3].transform.DOLocalMove(new Vector3(0f, -100f, 0), 0.2f);
            }
            StartGame.SetActive(false);
            StartGame.transform.localPosition = new Vector3(-2000f, -2000f, 0);
        }
        else if (Stage == 3)
        {
            newgame.SetActive(false);
            restart.SetActive(false);
            Return.SetActive(true);
            if (Stage == CurrentStage + 1)//順当に進んできたとき
            {
                tape.Play();
                StartCoroutine("ModeUp");
                foreach (GameObject Year in Years)
                {
                    Year.transform.DOMove(Year.transform.position + new Vector3(-Screen.width, 0, 0), 0.2f);
                    DOVirtual.DelayedCall(0.2f, () => { Year.SetActive(false); });
                }
                Tapes[0].transform.DOLocalMove(new Vector3(-3000f, 950f, 0), 0.2f);
                DOVirtual.DelayedCall(0.2f, () => { Tapes[0].SetActive(false); });
                Tapes[1].transform.DOLocalMove(new Vector3(-2000f, -650f, 0), 0.2f);
                DOVirtual.DelayedCall(0.2f, () => { Tapes[1].SetActive(false); });
                Tapes[2].transform.DOLocalMove(new Vector3(2800f, -400f, 0), 0.2f);
                DOVirtual.DelayedCall(0.2f, () => { Tapes[2].SetActive(false); });
                Tapes[3].transform.DOLocalMove(new Vector3(2000f, 350f, 0), 0.2f);
                DOVirtual.DelayedCall(0.2f, () => { Tapes[3].SetActive(false); });
            }
            else if (Stage == CurrentStage - 1)//戻ってきたとき
            {
                return;
            }
            StartGame.SetActive(true);
            StartGame.transform.DOLocalMove(new Vector3(0f, 0f,0f), 0.2f);
        }
        else if (Stage == 4)
        {
            bgm.DOFade(0f, 0.5f);
            //セーブデータが消去されますがよろしいですか？みたいなポップを出した方がいいかもしれない
            StartCoroutine("EndScene");
        }
        else if (Stage == 5) {
            //色んな数値の初期化設定
            bgm.DOFade(0f, 0.5f);
            ObscuredPrefs.Save();
            controller.GetComponent<AutoSaver>().BlockAccess();
            //セーブデータが消去されますがよろしいですか？みたいなポップを出した方がいいかもしれない
            StartCoroutine("EndScene");
        }
        CurrentStage = Stage;
        return;
    }

    public GameObject Music, Buy,conreturn;

    public GameObject[] Musics;
    public GameObject[] Buys;

    public Button Brand, Noah;

    public GameObject Restore;

    public void MusicMode()
    {
        button.Play();
        Alarms[0].transform.DOScale(new Vector2(0.75f, 0.75f), 0.25f);
        title.DOFade(0f, 0.1f);
        Buy.SetActive(false);
        Music.transform.DOLocalMoveY(210f, 0.25f);
        for (ObscuredInt i = 0; i < 2; i++) Musics[i].SetActive(true);
        Brand.interactable = AuMusic();
        conreturn.SetActive(true);
        ChangeColor();
        
    }
    public void BuyMode()
    {
        button.Play();
        Alarms[0].transform.DOScale(new Vector2(0.75f, 0.75f), 0.25f);
        title.DOFade(0f, 0.1f);
        Music.SetActive(false);
        Buy.transform.DOLocalMoveY(210f, 0.25f);
        Buys[0].SetActive(!AuNoah());
        Buys[1].SetActive(!AuMusic());
#if UNITY_IPHONE && !UNITY_EDITOR
Restore.SetActive(true);
#else
        Restore.SetActive(false);
#endif
        conreturn.SetActive(true);
        //既に買ってたらボタン効かないようにする
    }


    private bool AuMusic() {
        return ObscuredPrefs.HasKey("MusicBuy") && ObscuredPrefs.GetString("MusicBuy") == "74APNiSAZT73KUH3iCgy";
    }
    private bool AuNoah()
    {
        Debug.Log(ObscuredPrefs.HasKey("NoahBuy") && ObscuredPrefs.GetString("NoahBuy") == "MzWgSru2NVh7PmuZTueL");
        return ObscuredPrefs.HasKey("NoahBuy") && ObscuredPrefs.GetString("NoahBuy") == "MzWgSru2NVh7PmuZTueL";
    }


    public void ConReturn()
    {
        Restore.SetActive(false);
        for (int i = 0; i < 2; i++) Musics[i].SetActive(false);
        for (int i = 0; i < 2; i++) Buys[i].SetActive(false);
        Alarms[0].transform.DOScale(new Vector2(0.5f, 0.5f), 0.25f);
        Buy.SetActive(true);
        Music.SetActive(true);
        conreturn.SetActive(false);
        title.DOFade(1f, 0.2f);
        Music.transform.DOLocalMoveY(0f, 0.25f);
        Buy.transform.DOLocalMoveY(-200f, 0.25f);
        if (sample.isPlaying)
        {
            sample.Stop();
        }
        if (!bgm.isPlaying)
        {
            bgm.Play();
        }
    }
    public void ChangeMusic(int i)
    {
        ObscuredPrefs.SetInt("BGM", i);
        ObscuredPrefs.Save();
        ChangeColor();
    }

    public void BuyDif()
    {
        //後日実装
        ObscuredPrefs.SetString("NoahBuy", "MzWgSru2NVh7PmuZTueL");
        Noah.interactable = true;
        Buys[0].SetActive(false);
    }
    public void BuyMusic()
    {
        //後日実装
        Debug.Log("Buy2Pushed");
        ObscuredPrefs.SetString("MusicBuy", "74APNiSAZT73KUH3iCgy");
        Brand.interactable = true;
        Buys[1].SetActive(false);
    }

    void ChangeColor()
    {
        button.Play();
        ObscuredInt k = ObscuredPrefs.GetInt("BGM");
        Musics[k].transform.Find("Decide").GetComponent<Image>().color = Color.blue;
        Musics[k*-1+1].transform.Find("Decide").GetComponent<Image>().color = new Color(50f/255f, 50f / 255f, 50f / 255f,1f);
    }

    public AudioSource sample;

    public void TryHear()
    {
        if (bgm.isPlaying)
        {
            bgm.Stop();
        }
        sample.Play();
    }

    public GameObject rule;

    public void OpenRule() {
        rule.SetActive(true);
    }

    public void CloseRule()
    {
        rule.SetActive(false);
    }

}
