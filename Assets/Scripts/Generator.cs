using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Keiwando.BigInteger; // BigInteger
using System;

public class Generator : MonoBehaviour {

    [SerializeField] private string title;
    [SerializeField] private double costBase;
    [SerializeField] private double cost;
    [SerializeField] private float coefficient;
    [SerializeField] private int owned;
    [SerializeField] private float speedBase;
    [SerializeField] private float speed;
    [SerializeField] private float time;
    [SerializeField] private int multiplier;
    [SerializeField] private double productionBase;
    [SerializeField] private double production;
    [SerializeField] private bool autoBuy;
    [SerializeField] private bool canBuy;
    [SerializeField] private int buyMax;

    [SerializeField] private Text text_title;
    [SerializeField] private Text text_cost;
    [SerializeField] private Text text_owned;
    [SerializeField] private Text text_production;
    [SerializeField] private Text text_speed;
    [SerializeField] private Text text_multiplier;
    [SerializeField] private Text text_buyMax;
    [SerializeField] private Button button_buy;
    [SerializeField] private Button button_buyMax;
    [SerializeField] private Slider slider_time;

    void Init() {

        // donné changeantes
        costBase = 3.738f;
        coefficient = 1.07f;
        productionBase = 1.67f;
        speedBase = 0.6f;

        // innitialisations
        slider_time.value = 0;
        time = 0.0f;
        owned = 0;
        multiplier = 0;
        title = "???";

        // calculs
        CalculStats();
        UpdateBuyPossibilities();

        // mise à jour
        UpdateTexts();

        // texte qui ne change pas :
        text_title.text = title;

        this.gameObject.name = "Factory_" + title;
    }

    public void SetStats(string _title, double _costBase, float _coefficient, double _productionBase, float _speedBase) {

        // donné changeantes
        title = _title;
        costBase = _costBase;
        coefficient = _coefficient;
        productionBase = _productionBase;
        speedBase = _speedBase;

        // innitialisations
        slider_time.value = 0;
        time = 0.0f;
        owned = 0;
        multiplier = 0;

        // calculs
        CalculStats();
        UpdateBuyPossibilities();

        // mise à jour
        UpdateTexts();

        // texte qui ne change pas :
        text_title.text = title;

        this.gameObject.name = "Factory_" + title;
    }

    public void Reset() {

        slider_time.value = 0;
        time = 0.0f;
        owned = 0;
        multiplier = 0;

        // calculs
        CalculStats();
        UpdateBuyPossibilities();

        // mise à jour
        UpdateTexts();
    }

    void Update() {

        if (owned <= 0) return;

        slider_time.value = ( speed > 0.01f )? 1 / speed * time: 1f;

        time += Time.deltaTime;

        if ( time >= speed ) {

            GameManager.instance.Earn((ulong)Math.Round( production * ( time / speed ) ));

            time = time % speed;
        }
    }

    private void CalculStats() {

        cost = CalculCostNext();
        multiplier = CalculMultiplier();
        speed = CalculSpeed();

        production = CalculProductionTotal();
    }

    public void UpdateBuyPossibilities() {

        canBuy = calculCanBuy();
        buyMax = CalculMaxBuy();

        button_buy.interactable = canBuy;
        button_buyMax.interactable = buyMax > 0;

        text_buyMax.text = "Max (" + buyMax.ToString() + ")";

        //Debug.Log("Generator - UpdateBuyPossibilities() : buyMax = " + buyMax + " for the price of " + (ulong)Mathf.Round(CalculCost(buyMax)));
    }

    private double CalculCostNext() {

        return costBase * Math.Pow( coefficient, owned  );
    }

    private double CalculProductionTotal() {

        return ( ( productionBase * owned ) * multiplier ) * ( 1 + ( GameManager.instance.GameManagerPrestige / 100.0f ) );
    }

    private int CalculMultiplier() {

        return owned / 25 + 1;
    }

    private float CalculSpeed() {

        float result = speedBase * Mathf.Pow(0.5f, multiplier - 1);

        return result;
    }

    private int CalculMaxBuy() {

        int n = 0;
        double b = costBase;
        double r = coefficient;
        int k = owned;
        double c = GameManager.instance.GetPrimaryCurrency();

        double n1 = ( (c * (r - 1)) / ( b * (Math.Pow(r, k)) ) ) + 1  ;

        n = (int)Math.Floor( Math.Log(n1) / Math.Log(r) );

        return n;
    }

    private bool calculCanBuy() {

        return GameManager.instance.CanBuy( (ulong)Math.Round(cost) );
    }

    private double CalculCost(int n) {

        double b = costBase;
        double r = coefficient;
        int k = owned;

        double n1 = Math.Pow(r, k) * ( Math.Pow(r, n) - 1 );

        return b * ( n1 / ( r - 1 ) );
    }

    public void UpdateTexts() {

        text_owned.text = "Owned : " + owned.ToString();
        text_cost.text = "Cost : " + GameManager.instance.ToVerboseNumber( Math.Round( cost ) );
        text_production.text = "Prod : " + GameManager.instance.ToVerboseNumber( Math.Round( production / speed ) ) + "/s";
        text_multiplier.text = "Multiplier : " + multiplier.ToString();
        text_speed.text = "Speed : " + speed.ToString("0.0000");
    }

    public void OnClickOnButtonBuy() {

        double buyCost = Math.Round(cost);

        owned++;
        CalculStats();
        UpdateTexts();

        GameManager.instance.Buy(buyCost);
    }

    public void OnClickOnButtonBuyMax() {

        double buyMaxCost = Math.Round(CalculCost(buyMax));

        owned += buyMax;
        CalculStats();
        UpdateTexts();

        GameManager.instance.Buy(buyMaxCost);
    }
}
