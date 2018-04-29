using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridTile : MonoBehaviour {

    public int value;
    public Vector3 startPos;
    public bool overGrid = false;
    public bool placed = false;
    public bool pickedUp = true;

    public bool placedByAI = false;

    GameMaster GM;

    private Renderer renderer;
    public Material defaultSkin;
    public Material activeSkin;

    public Vector3 location;

    //public ParticleSystem ScoreEffect;

    public bool activated;

    public bool isFlashing = false;

    public List<Observer> observerList;


    void Awake()
    {
        observerList=new List<Observer>();
        this.renderer = this.GetComponent<Renderer>();
        pickedUp = false;
        activated=false;
        isFlashing = false;

        GM = GameMaster.instance;
        startPos = this.transform.position;
        defaultSkin = this.renderer.material;
    }

    public void ActiveTileSkin()
    {
        this.renderer.material = activeSkin;
    }

    public void DeafultTileSkin()
    {
        this.renderer.material = defaultSkin;
    }

    void OnMouseDrag()
    {
        if(!GameMaster.instance.humanTurn) //temp for more players
            return;
        
        if(!pickedUp && !placed)
        {
            Debug.Log("Pickup");
            this.GetComponent<GUI_Object>().PickUpObject();
            pickedUp = true;
        }


        if(!placed)
        {
            GM.selectedTile = this;
            float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
            transform.rotation = Quaternion.Euler(0,0,GUI_Controller.instance.rotation);
        } else if(activated==false && placed == true || placedByAI == true)
        {
            if(GameMaster.instance.playedTiles.Count > 0)
                GameMaster.instance.StateMachine.RevertToLastValidState(false);

            if(GameMaster.instance.TUTORIAL_MODE && GameMaster.instance.TutorialController.clear4 && !GameMaster.instance.TutorialController.clear5)
            {
                if(observerList.Count==0)
                    observerList.Add(GameMaster.instance.TutorialController);

                NotifyObservers(this, "Tutorial.5");
            }

        }
    }

    void OnMouseUp()
    {
        if(GameMaster.instance.vsAi && !GameMaster.instance.humanTurn) //temp - this will have to change when > 2 players implemented
            return;

        if(this.GetComponent<GUI_Object>().GetState() == GUI_Object.GUIState.inAnimation)
        {
            Debug.Log("******** IN ANIMATION ********");
            return;
        }

        if(activated && (placedByAI || placedByAI))
            return;


        
        // TEKMP - THIS CODE ALL here is so rough and need refactored asap
        if(GameMaster.instance.selectedTile == null)
        {
            this.GetComponent<GUI_Object>().PutObjectDown();
            pickedUp = false;
            return;
        }

        if (GameMaster.instance.activeCell == null)
        {
            this.GetComponent<GUI_Object>().PutObjectDown();
            pickedUp = false;
            return;
        }

        if(GameMaster.instance.TUTORIAL_MODE && GameMaster.instance.totalTiles == 1 && !(GameMaster.instance.activeCell.x == 2 && GameMaster.instance.activeCell.y == 3) && !GameMaster.instance.TutorialController.clear4)
        {
            StartCoroutine(this.GetComponent<GUI_Object>().AnimateTo(this.GetComponent<GUI_Object>().startPos, 2f));
            this.GetComponent<GUI_Object>().PutObjectDown();
            pickedUp = false;
            return;
        } else if (GameMaster.instance.TUTORIAL_MODE && GameMaster.instance.totalTiles == 1 && (GameMaster.instance.activeCell.x == 2 && GameMaster.instance.activeCell.y == 3))
        {
            if(observerList.Count==0)
                    observerList.Add(GameMaster.instance.TutorialController);

            NotifyObservers(this, "Tutorial.4");
        }

        if (BoardController.instance.gameGrid[GameMaster.instance.activeCell.x, GameMaster.instance.activeCell.y] != 0)
        {
            //GameMaster.instance.selectedTile.transform.position = GameMaster.instance.selectedTile.GetComponent<GUI_Object>().targetPos;
            StartCoroutine(GameMaster.instance.selectedTile.GetComponent<GUI_Object>().AnimateTo(GameMaster.instance.selectedTile.GetComponent<GUI_Object>().targetPos, .5f));
            GameMaster.instance.selectedTile = null;
            this.GetComponent<GUI_Object>().PutObjectDown();
            pickedUp = false;
            return;
        } 

        //Temporarily place the tile
        GUI_Controller.instance.LastPlacedTile = this.gameObject;
        BoardController.instance.gameGrid[GameMaster.instance.activeCell.x, GameMaster.instance.activeCell.y] = this.value;
        GameMaster.instance.playedTiles.Add(GameMaster.instance.activeCell);
        GameMaster.instance.activeCell.cellTile = GameMaster.instance.selectedTile;
        GameMaster.instance.selectedTile.placed = true;
        GameMaster.instance.selectedTile.transform.position = GameMaster.instance.activeCell.transform.position + new Vector3(0, 0, -1);
        GameMaster.instance.selectedTile.location = GameMaster.instance.selectedTile.transform.position;
        
        pickedUp = false;


        //Check if move is actually valid
        if (BoardController.instance.CheckMoveValidity(GameMaster.instance.activeCell))
        {
            Debug.Log("Activating Player Move");
            GUI_Controller.instance.ActivateCell(GameMaster.instance.activeCell.cellTile);
            
        }

        if(GameMaster.instance.TUTORIAL_MODE && GameMaster.instance.TutorialController.clear5 && GameMaster.instance.currentHand.Count >= 1 && GameMaster.instance.currentHand.Count <3) 
        {
            if(observerList.Count==0)
                    observerList.Add(GameMaster.instance.TutorialController);

            NotifyObservers(this, "Tutorial.6");
        }

        GameMaster.instance.selectedTile = null;
        this.GetComponent<GUI_Object>().PutObjectDown();
    }

    public virtual void AddObserver(Observer ob)
    {
        observerList.Add(ob);
    }

    public virtual void DeleteObserver(Observer ob)
    {
        observerList.Remove(ob);

    }

    public virtual void NotifyObservers(MonoBehaviour _class, string _event)
    {
        foreach (var Observer in observerList)
        {
            Observer.OnNotification(_class, _event);
        }

    }

    
}
