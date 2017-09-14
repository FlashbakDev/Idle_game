using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject factoryPrefab;

    public static GameManager instance;
    private GameBoardManager boardScript;
    private GameGUIManager GUIScript;

    private HighInteger money;

    private float clock;

    void Awake() {

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<GameBoardManager>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    // Use this for initialization
    void Start () {

    }

    //Initializes the game for each level.
    void InitGame() {

        //Call the SetupScene function of the BoardManager script, pass it current level number.
        boardScript.SetupScene( 0 );

        clock = 0f;

        money = new HighInteger("100000000");
    }

    // Update is called once per frame
    void Update () {

        clock += Time.deltaTime;
    }

    private void OnTick() {

        money += 1;
    }

    // Accesseurs ===================================================================================================

    public GameBoardManager GameManagerBoardScript { get { return boardScript; } set { boardScript = value; } }

    public GameGUIManager GameManagerGUIScript { get { return GUIScript; } set { GUIScript = value; } }

    public HighInteger GameManagerMoney { get { return money; } set { money = value; } }
}
