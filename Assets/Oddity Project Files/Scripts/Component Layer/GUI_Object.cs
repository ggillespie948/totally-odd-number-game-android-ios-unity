using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Object : MonoBehaviour {


        //MOVE THESE TO GUI CONTROLLER SURELY
    public AnimationCurve AnimateToLocationCurve;
    public AnimationCurve InvalidTileCurve;

    public AnimationCurve TileFlashCurve;

    public GameObject targetPosMarker;

    public Vector3 targetPos = new Vector3();
    public Vector3 startPos = new Vector3();
    public float Speed;
    public bool introAnim = true;

    BoxCollider2D collider;

    [SerializeField]
    private bool isMenu;

    public Vector3 startSize;


    //State
    public enum GUIState
    {
        idle,
        inAnimation
    }

    public static GUIState state = GUIState.idle;

    //STATE MODIFIERS
    public void SetState(GUIState _state)
    {
        if (state == _state)
            return;

        state = _state;
    }

    public GUIState GetState()
    {
        return state;
    }

    void Awake()
    {
        startPos = this.transform.position;
        startPos.z = 460;
        
    }


    // Use this for initialization
    void Start () {

        this.collider = GetComponent<BoxCollider2D>();

        if(introAnim == false)
        {
            return;
        }

        if (targetPosMarker != null)
        {
            targetPos = targetPosMarker.transform.position;
        }

        startSize = transform.localScale;

        
        

		
	}


    public void StartIntroAnim(float delay)    {StartCoroutine(IntroDelay(delay));}
    private IEnumerator IntroDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        IntroAnim();
    }

    private void IntroAnim()
    {
        AudioManager.instance.PlayAfterDelay(1f,"TileWoosh");
        startPos = this.transform.position;
        GUI_Controller.instance.animationCount++;
        StartCoroutine(AnimateIn(2f));
    }



    //This is a method used by grid tiles to slightly rotate a gameobject and 
    public void PickUpObject()
    {
        if(GetState() == GUIState.inAnimation)
            return;
        


        //this.gameObject.GetComponent<NoGravity>().enabled = false;
        Debug.Log("pick up.");
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        startSize = transform.localScale;
         
        transform.localScale = (new Vector3(startSize.x*1.2f,startSize.y*1.2f,startSize.z*1.2f));

        //  if(transform.localScale == new Vector3(0.57f, 0.57f ,0.57f))
        //      this.transform.localScale = new Vector3(.63f, .63f, .63f);

        AudioManager.instance.Play("DealTile");

    }

    public void PutObjectDown()
    {
        Debug.Log("Put down.");
        if(this.GetComponent<GridTile>().placed!=true)
        {
            this.gameObject.GetComponent<NoGravity>().enabled = true;
        }

        Debug.Log("StartSize: " + startSize);

        if(startSize.x == 0)
            startSize = new Vector3(GameMaster.TILE_SCALE,GameMaster.TILE_SCALE,GameMaster.TILE_SCALE);

        transform.localScale = startSize;

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, GUI_Controller.instance.rotation);


    }

    public void SetAnimationTarget(Vector3 t)
    {
        targetPos = t;
    }
	
	IEnumerator AnimateIn(float time)
    {
        if (collider != null)
            collider.enabled = false;
        
        float curveTime = 0.0f;
        //float curveAmount = AnimateToLocationCurve.Evaluate(curveTime);

        while(curveTime <= time)
        {
            //curveTime += Time.deltaTime * Speed;
            //curveAmount = AnimateToLocationCurve.Evaluate(curveTime);
            //Move to Animate pos

            transform.position = Vector3.Lerp(startPos, targetPos, AnimateToLocationCurve.Evaluate(curveTime/time));
            curveTime += Time.deltaTime;
            yield return null;
        }

        SetPos();

        if (collider != null)
            collider.enabled = true;        
    }

    public IEnumerator AnimateTo(Vector3 pos, float time)
    {
        //Change GUI State
        SetState(GUIState.inAnimation);

        //Disabled Object Collier While Animating
        if(collider != null)
            collider.enabled = false;

        float curveTime = 0.0f;
        Vector3 _startPos = transform.position;
               
        while (curveTime <= time)
        {
            if(transform != null)
                transform.position = Vector3.Lerp(_startPos, pos, 
                InvalidTileCurve.Evaluate(curveTime / time));
            curveTime += Time.deltaTime;
            yield return null;
        }

        if(!isMenu) //this is used as we need menus to retain their startPos unlike tiles and 
        {
                if(this.GetComponent<GridTile>().placedByAI == true)
                {
                    GUI_Controller.instance.ActivateCell(this.GetComponent<GridTile>());
                }
            SetPos();   //othe game-based GUI objects
        }

        if (collider != null)
            collider.enabled = true;

        

        //Set GUI State back to idle
        SetState(GUIState.idle);

    }

    

    public IEnumerator Shake(float time)
    {

        yield return null;
    }

    void SetPos()
    {
        transform.position = targetPos;
        GUI_Controller.instance.animationCount--;
    }


    public IEnumerator Flash(Color startCol, float time, bool enableEmission)
    {
        this.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");  

        Renderer rend = this.GetComponent<Renderer>();
        float curveTime = 0.0f;
        float floor = 0.3f;
        float ceiling = 1f;

               
        while (curveTime <= time)
        {
            if(this==null)
                yield return null; //if tile is destroyed while flashing (game over)

            curveTime += Time.deltaTime;

            Material mat = rend.material;
            
            float emission = floor + Mathf.PingPong (Time.time, ceiling - floor);
            Color finalColor = startCol * Mathf.LinearToGammaSpace (emission);
            mat.SetColor ("_EmissionColor", finalColor);
            yield return null;
        }

        rend.material.SetColor("_EmissionColor", startCol*0.6f);

        if(!enableEmission)
        {
            rend.material.DisableKeyword("_EMISSION");
        }


    }

    public IEnumerator GlowToEmission(Color startCol, float time)
    {
        this.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");  
        Renderer rend = this.GetComponent<Renderer>();
        float curveTime = 0.0f;
               
        while (curveTime <= time)
        {
            if(this==null)
                yield return null; //if tile is destroyed while flashing (game over)

            curveTime += Time.deltaTime;
            Material mat = rend.material;
            float emission = TileFlashCurve.Evaluate(curveTime/time);
            Color finalColor = startCol * Mathf.LinearToGammaSpace (emission);
            mat.SetColor ("_EmissionColor", finalColor);
            yield return null;
        }

        rend.material.SetColor("_EmissionColor", startCol*0.6f);

    }


    

    public IEnumerator ScaleUp()
	{
        float scaleDuration = .87f;                                //animation duration in seconds
        Vector3 actualScale = transform.localScale;             // scale of the object at the begining of the animation
        Vector3 targetScale = new Vector3(.015f, .015f, .015f);     // scale of the object at the end of the animation     // scale of the object at the end of the animation

        float curveTime = 0.0f;
        float curveAmount = InvalidTileCurve.Evaluate(curveTime);


        while (curveTime <= 1.0f)
        {
            curveTime += Time.deltaTime * scaleDuration;
            curveAmount = InvalidTileCurve.Evaluate(curveTime);
            transform.localScale = new Vector3(targetScale.x * curveAmount, targetScale.y * curveAmount, targetScale.z * curveAmount);
            //Move to Animate pos
            yield return null;
        }

	}

	public IEnumerator ScaleDown()
	{
        float scaleDuration = .87f;                                //animation duration in seconds
        Vector3 actualScale = transform.localScale;             // scale of the object at the begining of the animation
        Vector3 targetScale = new Vector3(0.006f, 0.006f, 0.006f);     // scale of the object at the end of the animation     // scale of the object at the end of the animation

        float curveTime = 0.0f;
        float curveAmount = InvalidTileCurve.Evaluate(curveTime);


        while (curveTime <= 1.0f)
        {
            curveTime += Time.deltaTime * scaleDuration;
            curveAmount = InvalidTileCurve.Evaluate(curveTime);
            transform.localScale = new Vector3(targetScale.x * curveAmount, targetScale.y * curveAmount, targetScale.z * curveAmount);
            //Move to Animate pos
            yield return null;
        }

	}

    public IEnumerator Rotate()
    {
        yield return null;
    }

    





    
}
