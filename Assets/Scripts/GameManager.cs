using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Keiwando.BigInteger; // BigInteger
using System.Globalization;

public class GameManager : MonoBehaviour {

    // for the verbose string conversion
    private string[] suffixes = { "", "",
        " million", " billion", " trillion", " quadrillion", " quintillion", " sextillion", " septillion", " octillion", " nonillion", " decillion",
        " undecillion", " duodecillion", " tredecillion", " quattuordecillion", " quindecillion", " sexdecillion", " septendecillion", " octodecillion", " novemdecillion", " vigintillion",
        " unvigintillion", " duovigintillion", " trevigintillion", " quattuorvigintillion", " quinvigintillion", " sexvigintillion", " septenvigintillion", " octovigintillion", " novemvigintillion", " trigintillion",
        " untrigintillion", " duotrigintillion", " tretrigintillion", " quattuortrigintillion", " quintrigintillion", " sextrigintillion", " septentrigintillion", " octotrigintillion", " novemtrigintillion", " quadragintillion",
        " unquadragintillion", " duoquadragintillion", " trequadragintillion", " quattuorquadragintillion", " quinquadragintillion", " sexquadragintillion", " septenquadragintillion", " octoquadragintillion", " novemquadragintillion", " quinquagintillion",
        " unquinquagintillion", " duoquinquagintillion", " trequinquagintillion", " quattuorquinquagintillion", " quinquinquagintillion", " sexquinquagintillion", " septenquinquagintillion", " octoquinquagintillion", " novemquinquagintillion", " sexagintillion",
        " unsexagintillion", " duosexagintillion", " tresexagintillion", " quattuorsexagintillion", " quinsexagintillion", " sexsexagintillion", " septensexagintillion", " octosexagintillion", " novemsexagintillion", " septuagintillion",
        " unseptuagintillion", " duoseptuagintillion", " treseptuagintillion", " quattuorseptuagintillion", " quinseptuagintillion", " sexseptuagintillion", " septenseptuagintillion", " octoseptuagintillion", " novemseptuagintillion", " octogintillion",
        " unoctogintillion", " duooctogintillion", " treoctogintillion", " quattuoroctogintillion", " quinoctogintillion", " sexoctogintillion", " septenoctogintillion", " octooctogintillion", " novemoctogintillion", " nonagintillion",
        " unnonagintillion", " duononagintillion", " trenonagintillion", " quattuornonagintillion", " quinnonagintillion", " sexnonagintillion", " septennonagintillion", " octononagintillion", " novemnonagintillion", " centillion",};

    private NumberFormatInfo noSeparator_noDecimal;
    private NumberFormatInfo coma_noDecimal;

    [SerializeField] private GameObject generatorPrefab;
    [SerializeField] private Transform generatorParent;

    public static GameManager instance;
    private GameBoardManager boardScript;
    private GameGUIManager GUIScript;

    [SerializeField] private double primaryCurrency;
    [SerializeField] private double lifeTimeCurrency;
    [SerializeField] private ulong prestige;
    [SerializeField] private ulong nextPrestige;
    [SerializeField] private int layer;
    [SerializeField] private List<Generator> generators;

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

        noSeparator_noDecimal = new CultureInfo("en-US", false).NumberFormat;
        noSeparator_noDecimal.NumberGroupSeparator = "";
        noSeparator_noDecimal.NumberDecimalDigits = 0;

        coma_noDecimal = new CultureInfo("en-US", false).NumberFormat;
        coma_noDecimal.NumberGroupSeparator = ",";
        coma_noDecimal.NumberDecimalDigits = 0;

        //Call the SetupScene function of the BoardManager script, pass it current level number.
        boardScript.SetupScene( 0 );

        clock = 0f;

        primaryCurrency = 0;
        lifeTimeCurrency = 0;
        prestige = 0;
        layer = 1;

        nextPrestige = calculNextPrestige();

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
        generatorTemp.SetStats(_title, _costBase, _coefficient, _productionBase, _speedBase);

        return generatorTemp;
    }

    public bool Buy( double price ) {

        primaryCurrency -= price;

        UpdateGeneratorsBuyPossibilities();

        return true;
    }

    public void Earn( double amount ) {

        primaryCurrency += amount;
        lifeTimeCurrency += amount;

        nextPrestige = calculNextPrestige();
        UpdateGeneratorsBuyPossibilities();
    }

    public void UpdateGeneratorsBuyPossibilities() {

        foreach (Generator g in generators) {

            g.UpdateBuyPossibilities();
        }
    }

    public void ResetGenerators() {

        foreach (Generator g in generators) {

            g.Reset();
        }
    }

    public bool CanBuy( double price ) {

        return primaryCurrency >= price;
    }

    public string GetPrimaryCurrencyAsVerboseString() {

        return ToVerboseNumber( primaryCurrency );
    }

    public double GetPrimaryCurrency() {

        return primaryCurrency;
    }

    public string ToVerboseNumber(double n) {

        string stringValue = n.ToString("N", noSeparator_noDecimal);
        int stringValueLength = stringValue.Length;

        string suffix = "";

        if (stringValueLength > 6) {

            suffix = ((stringValueLength - 1) / 3 < suffixes.Length) ? suffixes[(stringValueLength - 1) / 3] : " Infinity";
            if (suffix != suffixes[0] && (stringValueLength % 3 != 1 || stringValue[0] != '1')) suffix = suffix + "s";

            string nWithComa = n.ToString("N", CultureInfo.InvariantCulture);

            return nWithComa.Substring(0, nWithComa.IndexOf(',') + 3) + suffix;
        }

        return n.ToString("N", coma_noDecimal );
    }

    public ulong calculNextPrestige() {

        double cl = lifeTimeCurrency;

        return (ulong)( 150 * Math.Sqrt( ( cl ) / ( Math.Pow( 10, 15 ) ) ) );
    }

    // Update is called once per frame
    void Update () {

        clock += Time.deltaTime;
    }

    private void OnTick() {

        primaryCurrency += 1;
    }

    public void GoDeeper() {

        layer++;
        prestige += nextPrestige;

        primaryCurrency = 0;
        lifeTimeCurrency = 0;
        nextPrestige = calculNextPrestige();

        ResetGenerators();
    }

    // Accesseurs ===================================================================================================

    public GameBoardManager GameManagerBoardScript { get { return boardScript; } set { boardScript = value; } }

    public GameGUIManager GameManagerGUIScript { get { return GUIScript; } set { GUIScript = value; } }

    public ulong GameManagerPrestige { get { return prestige; } set { prestige = value; } }

    public ulong GameManagerNextPrestige { get { return nextPrestige; } set { nextPrestige = value; } }
}
