using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Object : MonoBehaviour {


        //MOVE THESE TO GUI CONTROLLER SURELY
    public AnimationCurve animateToLocationCurve;
    public AnimationCurve invalidTileCurve;

    public AnimationCurve dockCruve;

    public AnimationCurve tileFlashCurve;

    public GameObject targetPosMarker;

    public Vector3 targetPos = new Vector3();
    public Vector3 startPos = new Vector3();

    public Vector3 returnPos = new Vector3();
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
        
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        startSize = transform.localScale;
         
        transform.localScale = (new Vector3(startSize.x*1.2f,startSize.y*1.2f,startSize.z*1.2f));

        transform.position = transform.position - new Vector3(0,0,35);

        //  if(transform.localScale == new Vector3(0.57f, 0.57f ,0.57f))
        //      this.transform.localScale = new Vector3(.63f, .63f, .63f);

        AudioManager.instance.Play("DealTile");

    }

    /// <summary>
    /// Method specific to Oddity where the scale of the object is decreased to a hardcoded value and the attached NoGrav is enabled
    /// </summary>
    public void PutObjectDown()
    {
        if(this.GetComponent<GridTile>().placed!=true)
        {
            this.gameObject.GetComponent<NoGravity>().enabled = true;
        }


        if(startSize.x == 0)
            startSize = new Vector3(GameMaster.TILE_SCALE,GameMaster.TILE_SCALE,GameMaster.TILE_SCALE);

        transform.localScale = startSize;

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, GUI_Controller.instance.rotation);

        transform.position = transform.position + new Vector3(0,0,35);


    }

    /// <summary>
    /// Set the target position value of this GUI_Obejct toa  given Vector 3 position
    /// </summary>
    /// <param name="t"></param>
    public void SetAnimationTarget(Vector3 t)
    {
        targetPos = t;
        returnPos=t;
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

            transform.position = Vector3.Lerp(startPos, targetPos, animateToLocationCurve.Evaluate(curveTime/time));
            curveTime += Time.deltaTime;
            yield return null;
        }

        SetPos();

        startPos=transform.position;

        if (collider != null)
            collider.enabled = true;        
    }

    /// <summary>
    /// Method which interpolates from the current position of the attached gameObject to a given Vector 3 Position over a given time period
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator  AnimateTo(Vector3 pos, float time)
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
                invalidTileCurve.Evaluate(curveTime / time));
            curveTime += Time.deltaTime;
            yield return null;
        }

        if(this.GetComponent<GridTile>() != null)
        {
            if(this.GetComponent<GridTile>().placedByAI == true)
            {
                GUI_Controller.instance.ActivateTile(this.GetComponent<GridTile>());
            }
        } else 
        {
            //yield return null;
        }

        if(!isMenu) //this is used as we need menus to retain their startPos unlike tiles and 
        {
                
            SetPos();   //othe game-based GUI objects
        }

        if (collider != null)
            collider.enabled = true;

        

        //Set GUI State back to idle
        SetState(GUIState.idle);

    }

    /// <summary>
    /// Method 1 which produces the "Dock tiling effect" where a tile will smoothly move towards the selected cell 
    /// Method has been included in this class as it has the potential to be used in other generic scenarios
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="cell"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator  PreDockTile(GameObject tile, GameObject cell, float time)
    {
        //Disabled Object Collier While Animating
        if(collider != null)
            collider.enabled = false;

        float curveTime = 0.0f;
        Vector3 _startPos = transform.position;
               
        while (curveTime <= time)
        {
            if(transform != null)
                transform.position = Vector3.Lerp(_startPos, cell.transform.position - new Vector3(0f, -.15f, 17.5f), 
                dockCruve.Evaluate(curveTime / time));
            curveTime += Time.deltaTime;
            yield return null;
        }

        //Docking complete
        StartCoroutine(DockTile(tile, cell, .25f, .1f));
        //StartCoroutine(RotateToPos(tile, Quaternion.Euler(0,0,0), .55f));
        //GUI_Controller.instance.RotateObjectBackward(tile, .45f, -45);


    }

    /// <summary>
    /// Method 2 (*See PreDockTile() for further info ) which produces the "Dock tiling effect" where a tile will smoothly move towards the selected cell 
    /// Method has been included in this class as it has the potential to be used in other generic scenarios
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="cell"></param>
    /// <param name="time"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public IEnumerator  DockTile(GameObject tile, GameObject cell, float time, float delay)
    {
        
        float curveTime = 0.0f;
        Vector3 _startPos = transform.position;
               
        while (curveTime <= time)
        {
            if(transform != null)
                transform.position = Vector3.Lerp(_startPos, cell.transform.position + new Vector3(0f, 0f, 1.5f), 
                dockCruve.Evaluate(curveTime / time));
            curveTime += Time.deltaTime;
            yield return null;
        }

        //Disabled Object Collier While Animating
        if(collider != null)
            collider.enabled = true;

        SetPos();
        ResetRotation(tile);
        
    }

    public IEnumerator RotateToPos(GameObject obj, Quaternion targetRot, float time )
    {
        float curveTime = 0.0f;
        Quaternion _startRot = transform.rotation;
               
        while (curveTime <= time)
        {
            if(transform != null)
                transform.rotation = Quaternion.Lerp(_startRot, targetRot, 
                dockCruve.Evaluate(curveTime / time));
            curveTime += Time.deltaTime;
            yield return null;
        }

        //Disabled Object Collier While Animating
        if(collider != null)
            collider.enabled = true;

        ResetRotation(obj);


    }

    /// <summary>
    /// Resets the rotation of a given gameobject
    /// </summary>
    /// <param name="tile"></param>
    public void ResetRotation(GameObject tile)
    {
        tile.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
    }

    /// <summary>
    /// Sets the target position of the GUI Object to the transforms current position
    /// </summary>
    void SetPos()
    {
        transform.position = targetPos;
        GUI_Controller.instance.animationCount--;
    }


    /// <summary>
    /// Method which will Mathf.PingPong the emission value of the attached gameObjects current material b
    /// NOTE: The floor / ceiling of these values is currently hardcoded 
    /// </summary>
    /// <param name="startCol"></param>
    ///  original color of gameobjects material (used for default emission value)
    /// <param name="time"></param>
    /// the duration of the flashing
    /// <param name="enableEmission"></param>
    /// whether or not the emission value of the material should be enabled or disabled following the flashing
    /// <returns></returns>
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

    /// <summary>
    /// Method which increases the transform.Local scale of the attached gameObject from a set amount to a given amount 
    /// </summary>
    /// <param name="targetIncrease"></param>
    /// e.g. x1.39  increases the scale by 39%
    /// <param name="startScale"></param>
    /// e.g x1.00
    /// <returns></returns>
    public IEnumerator ScaleUp(float targetIncrease, float startScale)
	{
        float scaleDuration = 1.5f;                                //animation duration in seconds
        Vector3 actualScale = transform.localScale;             // scale of the object at the begining of the animation
        Vector3 targetScale = new Vector3(startScale*targetIncrease, startScale*targetIncrease, startScale*targetIncrease);     // scale of the object at the end of the animation     // scale of the object at the end of the animation

        float curveTime = .5f;
        float curveAmount = invalidTileCurve.Evaluate(curveTime);


        while (curveTime <= 1.0f)
        {
            curveTime += Time.deltaTime * scaleDuration;
            curveAmount = invalidTileCurve.Evaluate(curveTime);
            transform.localScale = new Vector3(targetScale.x * curveAmount, targetScale.y * curveAmount, targetScale.z * curveAmount);
            //Move to Animate pos
            yield return null;
        }
	}

    /// <summary>
    /// Method which interpolates between between two emission values at a given speed.
    /// The effect produced is a smooth transition between mateiral emission values (brightness)
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="speed"></param>
    /// <param name="reset"></param>
    /// <returns></returns>
    public IEnumerator GlowToEmission(float min, float max, float speed, bool reset)
    {
        Renderer rend = this.GetComponent<Renderer>();
        float t=0f;

        while (t< 1f)
        {
            // animate the position of the game object...
            rend.material.SetEmissionRate(Mathf.Lerp(min, max, t));

            // .. and increate the t interpolater
            t += speed * Time.deltaTime;
            yield return null;
        }

        rend.material.SetColor("_EmissionColor", GetComponent<Renderer>().material.color*0.6f);


        if(reset)
        {
            StartCoroutine(GlowToEmission(1f, .6f, 1f, false));
        }
    }

    public void ResetTransform()
    {
        this.gameObject.transform.rotation.Set(0f,0f,0f, 0f);
    }
    
}
