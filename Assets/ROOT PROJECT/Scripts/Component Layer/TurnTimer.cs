using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnTimer : MonoBehaviour {

    public TextMeshProUGUI remainingtext;
    public TextMeshProUGUI remainingtext2;
    public bool pauseFlag {get; private set;}

    public float minWidth = 65f;
	public float maxWidth = 350f;

	public Image fillerImage;

    [SerializeField]
    private Color shadowRed;

    [SerializeField]
    private Color shadowGreen;

	public float _currentSize = 2f;
    public float timeLeft;

    public int startTime;

    public TextMeshProUGUI timerText;

    void Awake()
    {
        startTime = ApplicationModel.TURN_TIME;
        if(startTime ==0)
            startTime=30;

        timeLeft = startTime;
    }

    void Start()
    {
        pauseFlag = true;
        fillerImage.fillAmount = 1f;
    }

    public bool tutoralProgressFlag = false;

    void Update()
    {
        if((GameMaster.instance.TUTORIAL_MODE&& !tutoralProgressFlag) || pauseFlag)
            return;

        fillerImage.fillAmount = 1f * timeLeft/startTime;
        float totalTimeSec = Mathf.Floor(totalTimeSec=timeLeft-=Time.deltaTime);
        timerText.text=(totalTimeSec / 60 ).ToString("00")  + ":" + (totalTimeSec%60).ToString("00");

        if(timeLeft>7 && timeLeft<7.5f)
        {
            fillerImage.color=Color.red;
            fillerImage.GetComponent<Shadow>().effectColor=shadowRed;
        }
        
        if(timeLeft < 0.001f)
        {
            Debug.LogError("Time < 0000.0");
            GUI_Controller.instance.NotifyObservers(GUI_Controller.instance, "Event.TimesUp");
            fillerImage.color=Color.green;
            if(GameMaster.instance.selectedTile != null)
            {
                Debug.Log("Selected tile not null.. disbaling box collider 2d.");
                GameMaster.instance.humanTurn=false;
                GameMaster.instance.selectedTile.GetComponent<GUI_Object>().PutObjectDown();
                GUI_Controller.instance.AnimateTo(GameMaster.instance.selectedTile.GetComponent<GUI_Object>(), GameMaster.instance.selectedTile.startPos, 1.25f);
            }
            if(GameMaster.instance.soloPlay)
            {
                pauseFlag=true;
                timerText.text="00:00"; 
                GameMaster.instance.GameOver();

            } else{
                  
                pauseFlag=true;
                timerText.text="00:00";

                Debug.LogError("Time left < 0.1.. Check board validity?");
                Debug.LogError("turn over:: " + GameMaster.instance.turnOver);

                if(!GameMaster.instance.turnOver)
                    BoardController.instance.CheckBoardValidity(true,false);




            }
        }

        if(timeLeft < 5)
        {
            GetComponent<Animator>().enabled = true;
        }
        else
        {
            timerText.color = Color.white;
            GetComponent<Animator>().enabled = false;
        }
    }

    public void StartTurn()
    {
        timeLeft = ApplicationModel.TURN_TIME;
        pauseFlag=false;
        //fillerImage.color=Color.green;
        //fillerImage.GetComponent<Shadow>().effectColor=shadowGreen;
    }


    public IEnumerator refill()
    {
        fillerImage.color=Color.green;
        fillerImage.GetComponent<Shadow>().effectColor=shadowGreen;
        pauseFlag=true;
        timeLeft=startTime;
        float totalTimeSec = Mathf.Floor(totalTimeSec=timeLeft-=Time.deltaTime);
        timerText.text=(totalTimeSec / 60 ).ToString("00")  + ":" + (totalTimeSec%60).ToString("00");
        while(fillerImage.fillAmount<1f)
        {
            fillerImage.fillAmount+=0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        this.enabled=false;
        yield return null;

    }

    public void PauseTimer()
    {
        if(pauseFlag)        
            pauseFlag=false;
        else
            pauseFlag=true;

    }

    public void ResetTimerBars()
    {
        // _currentSize = 2f;
        // _fillerRect.sizeDelta = new Vector2(minWidth+(maxWidth - minWidth)*_currentSize, _fillerRect.sizeDelta.y);
        // _fillerRect2.sizeDelta = new Vector2(minWidth+(maxWidth - minWidth)*_currentSize, _fillerRect2.sizeDelta.y);
    }

    void EndTurn()
    {
        StopAllCoroutines();
        BoardController.instance.CheckBoardValidity(false, false);
        GameMaster.instance.EndTurn();
    }


}
