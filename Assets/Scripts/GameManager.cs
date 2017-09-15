using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Keiwando.BigInteger; // BigInteger

public class GameManager : MonoBehaviour {

    // for the verbose string conversion
    private static string[] suffixes = { "", "", "million", "milliard", "billion", "billiard", "trillion", "trilliard", "quadrillion", "quadrilliard", "quintillion", "quintilliard", "sextillion", "sextilliard", "septillion", "septilliard", "octillion", "octilliard", "nonillion", "nonilliard", "decillion", "decilliard" };

    [SerializeField] private GameObject generatorPrefab;
    [SerializeField] private Transform generatorParent;

    public static GameManager instance;
    private GameBoardManager boardScript;
    private GameGUIManager GUIScript;

    private BigInteger primaryCurrency;
    private int prestige;

    private float clock;

    private List<Generator> generators;

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

        primaryCurrency = new BigInteger("0");
        prestige = 0;

        generators = new List<Generator>();

        generatorParent = GameObject.Find("Panel_generators").transform;

        generators.Add(CreateNewGenerator("Static Bot - sweet love",    3.738f,         1.07f, 1,               0.6f));
        generators.Add(CreateNewGenerator("Static Bot - sufod",         60,             1.15f, 60,              3));
        generators.Add(CreateNewGenerator("Static Bot - R7 - chair",    720,            1.14f, 540,             6));
        generators.Add(CreateNewGenerator("Static Bot - LeL",           8640,           1.13f, 4320,            12));
        generators.Add(CreateNewGenerator("Static Bot - Bitcoin",       103680,         1.12f, 51840,           24));
        generators.Add(CreateNewGenerator("Dynamic Bot - Scam",         1244160,        1.11f, 622080,          96));
        generators.Add(CreateNewGenerator("Dynamic Bot - Waw",          14929920,       1.10f, 7464960,         384));
        generators.Add(CreateNewGenerator("Dynamic Bot - Poker",        179159040,      1.09f, 89579520,        1536));
        generators.Add(CreateNewGenerator("Dynamic Bot - Chess",        2149908480,     1.08f, 1074954240,      6144));
        generators.Add(CreateNewGenerator("Dynamic Bot - Go Game",      25798901760,    1.07f, 29668737024,     36864));
    }

    private Generator CreateNewGenerator(string _title, double _costBase, float _coefficient, double _productionBase, float _speedBase) {

        Generator generatorTemp = Instantiate(generatorPrefab, generatorParent).GetComponent<Generator>();
        generatorTemp.Reset(_title, _costBase, _coefficient, _productionBase, _speedBase);

        return generatorTemp;
    }

    public bool Buy( ulong price ) {

        if (!CanBuy(price)) {

            Debug.Log("GameManager - Buy( ulong price = " + ToVerboseNumber(price.ToString()) + " ) : ERROR price > currency ! ( currency = " + GetCurrencyAsVerboseString() + " ) ");

            return false;
        }

        primaryCurrency -= price;

        UpdateGeneratorsBuyPossibilities();

        return true;
    }

    public void Earn(ulong amount) {

        primaryCurrency += amount;

        UpdateGeneratorsBuyPossibilities();
    }

    public void UpdateGeneratorsBuyPossibilities() {

        foreach (Generator g in generators) {

            g.UpdateBuyPossibilities();
        }
    }

    public bool CanBuy( ulong price) {

        return primaryCurrency >= price;
    }

    public string GetCurrencyAsVerboseString() {

        return ToVerboseNumber( primaryCurrency.ToString() );
    }

    public BigInteger GetPrimaryCurrency() {

        return primaryCurrency;
    }

    public string ToVerboseNumber(string n) {

        string stringValue = n.ToString();
        int stringValueLength = stringValue.Length;
        string result = "";

        string suffix = ((stringValueLength - 1) / 3 < suffixes.Length) ? suffixes[(stringValueLength - 1) / 3] : "???";
        if (suffix != suffixes[0] && (stringValueLength % 3 != 1 || stringValue[0] != '1')) suffix = suffix + "s";

        if (stringValueLength > 6)
            for (int i = 0; i < stringValueLength; i++) {

                if (i % 3 == 0 && i != 0) { result = "," + result; }

                result = stringValue[stringValueLength - i - 1] + result;
            }
        else
            for (int i = 0; i < stringValueLength; i++) {

                if (i % 3 == 0 && i != 0) { result = " " + result; }

                result = stringValue[stringValueLength - i - 1] + result;
            }

        if (result.IndexOf(',') != -1) {

            result = result.Substring(0, result.IndexOf(',') + 4);
        }

        return (suffix.Length > 0) ? result + " " + suffix : result ;
    }

    // Update is called once per frame
    void Update () {

        clock += Time.deltaTime;
    }

    private void OnTick() {

        primaryCurrency += 1;
    }

    // Accesseurs ===================================================================================================

    public GameBoardManager GameManagerBoardScript { get { return boardScript; } set { boardScript = value; } }

    public GameGUIManager GameManagerGUIScript { get { return GUIScript; } set { GUIScript = value; } }

    public int GameManagerPrestige { get { return prestige; } set { prestige = value; } }
}
