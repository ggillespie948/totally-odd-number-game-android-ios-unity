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

    public int placedBy;

    GameMaster GM;

    private Renderer renderer;
    public Material defaultSkin;
    public Material activeSkin;

    public Vector3 location;

    //public ParticleSystem ScoreEffect;

    public bool activated;

    public bool isFlashing = false;

    public List<Observer> observerList;

    public int x;
    public int y;

    public bool locked = false;

    void Awake()
    {
        observerList=new List<Observer>();
        this.renderer = this.GetComponent<Renderer>();
        pickedUp = false;
        activated=false;
        isFlashing = false;

        GM = GameMaster.instance;
        startPos = this.transform.position;
        this.renderer.material = defaultSkin;
    }

    public void ActiveTileSkin()
    {
        //StartCoroutine(GetComponent<GUI_Object>().RotateToPos(this.gameObject, new Vector3(0,0,0), .5f  ));
        this.renderer.material = activeSkin;
    }

    public void DeafultTileSkin()
    {
        this.renderer.material = defaultSkin;
        //change emission? temp
    }

    void OnMouseDrag()
    {
        if(!GameMaster.instance.humanTurn || locked) //temp for more players
            return;
        
        //Picking up a tile for the first time
        if(!pickedUp && !placed)
        {
            this.GetComponent<GUI_Object>().PickUpObject();
            pickedUp = true;
        }

        //Tile Movement         ADD RESTRICTION FOR TILES THIS ROUND
        GM.selectedTile = this;
        float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
        transform.rotation = Quaternion.Euler(0,0,GUI_Controller.instance.rotation);

        // Tutorial Code - temp - refactor this elsewhere
        if(GameMaster.instance.TUTORIAL_MODE && GameMaster.instance.TutorialController.clear4 && !GameMaster.instance.TutorialController.clear5)
        {
            if(observerList.Count==0)
                observerList.Add(GameMaster.instance.TutorialController);

            NotifyObservers(this, "Tutorial.5");
        }

        //Picking up a tile which has already been played that round
        if(placed)
        {
            BoardController.instance.gameGrid[x,y]=0;
            placed=false;
            activated=false;
            if(GameMaster.instance.objGameGrid[GameMaster.instance.selectedTile.x,GameMaster.instance.selectedTile.y].cellTile != null && GameMaster.instance.objGameGrid[GameMaster.instance.selectedTile.x,GameMaster.instance.selectedTile.y].cellTile==this)
                GameMaster.instance.objGameGrid[GameMaster.instance.selectedTile.x,GameMaster.instance.selectedTile.y].cellTile=null;
            GameMaster.instance.playedTiles.Remove(GameMaster.instance.objGameGrid[GameMaster.instance.selectedTile.x,GameMaster.instance.selectedTile.y]);
            DeafultTileSkin();
            BoardController.instance.CheckBoardValidity(false, false);
            //BoardController.instance.CheckMoveValidity(GameMaster.instance.activeCell);
            GameMaster.instance.currentHand.Add(this);
            GameMaster.instance.totalTiles--;
            if(GameMaster.instance.totalTiles<0)
                GameMaster.instance.totalTiles=0;
        }

        GameMaster.instance.DrawBoardDebug();
            

        // if(!placed)
        // {
        // } else if(activated==false && placed ==true)
        // {
        //     if(GameMaster.instance.playedTiles.Count > 0)
        //         GameMaster.instance.StateMachine.RevertToLastValidState(false);
        // }
    }

    void OnMouseUp()
    {
        if(GameMaster.instance.vsAi && !GameMaster.instance.humanTurn) // player cannot interact with tiles when not in turn
            return;

        if(this.GetComponent<GUI_Object>().GetState() == GUI_Object.GUIState.inAnimation)
        {
            Debug.Log("******** IN ANIMATION ********");
            return;
        }

        if(activated && (placedByAI || placedByAI) || locked)
            return;


        if(GameMaster.instance.selectedTile == null)
        {
            this.GetComponent<GUI_Object>().PutObjectDown();
            pickedUp = false;
            return;
        }

        //Releasing tile when outside of a board position
        if (GameMaster.instance.activeCell == null)
        {
            this.GetComponent<GUI_Object>().PutObjectDown();
            pickedUp = false;
            return;
        }

        // Tutorial Code temp
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
            //GameMaster.instance.selectedTile.transform.position = GameMaster.instance.selectedTile.GetComponent<GUI_Object>().startPos;
            GameMaster.instance.selectedTile.GetComponent<GUI_Object>().targetPos=GameMaster.instance.selectedTile.GetComponent<GUI_Object>().returnPos;
            StartCoroutine(GameMaster.instance.selectedTile.GetComponent<GUI_Object>().AnimateTo(GameMaster.instance.selectedTile.GetComponent<GUI_Object>().returnPos, .5f));
            
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
        GameMaster.instance.selectedTile.GetComponent<GUI_Object>().targetPos=GameMaster.instance.activeCell.transform.position + new Vector3(0, .05f, -1);
        x=GameMaster.instance.activeCell.x;
        y=GameMaster.instance.activeCell.y;

        StartCoroutine(GameMaster.instance.selectedTile.GetComponent<GUI_Object>().
            PreDockTile(GameMaster.instance.selectedTile.gameObject, GameMaster.instance.activeCell.gameObject, .15f));

        pickedUp = false;

        //Check if move is actually valid
        if (BoardController.instance.CheckMoveValidity(GameMaster.instance.activeCell))
        {
            //GUI_Controller.instance.ActivateTile(GameMaster.instance.activeCell.cellTile);
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
