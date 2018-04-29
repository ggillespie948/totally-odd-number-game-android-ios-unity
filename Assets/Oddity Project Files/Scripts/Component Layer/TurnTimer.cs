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
        pauseFlag = false;
        fillerImage.fillAmount = 1f;
    }

    void Update()
    {
        if(GameMaster.instance.TUTORIAL_MODE)
            return;

        fillerImage.fillAmount = 1f * timeLeft/startTime;
        if(timeLeft<10)
            timerText.text = ("00:0" + Mathf.Round(timeLeft -= Time.deltaTime)).ToString();
        else
            timerText.text = ("00:" + Mathf.Round(timeLeft -= Time.deltaTime)).ToString();
        
        if(timeLeft < 0.001f)
            BoardController.instance.CheckBoardValidity(true,false);

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
    }


    IEnumerator BeginTimer(float turnTime)
    {

        while (_currentSize > 0.05f)
        {
            if(pauseFlag)
                yield return null;


            //barOverlay.fillAmount -= 1.0f / turnTime * Time.deltaTime;
            int time = (int)((_currentSize - 1f / turnTime * Time.deltaTime) * 10 * 2);

            remainingtext.text = "Remaining time: " + time;
            remainingtext2.text = "Remaining time: " + time;
            _currentSize -= Time.deltaTime*0.045f;

            //Color lerpedColor = Color.Lerp(Color.red, Color.green, barOverlay.fillAmount);
            //this.GetComponent<Image>().color = lerpedColor;

            // Trail pos    //Trail.transform.position = new Vector3(barOverlay.fillAmount*10+barOverlay.rectTransform.position.x-5, barOverlay.transform.position.y, barOverlay.transform.position.z);
            yield return null;

        }
        GUI_Controller.instance.SpawnTextPopup("Times up!", Color.red, GUI_Controller.instance.gameObject.transform, 38);
        EndTurn();
        ResetTimerBars();
        
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
