using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;   // UI

using Keiwando.BigInteger; // BigInteger
using System.Globalization;

public class GameGUIManager : MonoBehaviour {

    [SerializeField] private Button button_clicker;
    [SerializeField] private Button button_deeper;

    [SerializeField] private Text text_money;
    [SerializeField] private Text text_prestige;

    // Use this for initialization
    void Start () {

        text_money = GameObject.Find("Text_money").GetComponent<Text>();
        text_prestige = GameObject.Find("Text_prestige").GetComponent<Text>();

        text_prestige.text = "Prestige : " + GameManager.instance.ToVerboseNumber(GameManager.instance.GameManagerPrestige);

        button_clicker = GameObject.Find("Button_clicker").GetComponent<Button>();
        button_deeper = GameObject.Find("Button_deeper").GetComponent<Button>();

        button_clicker.onClick.RemoveAllListeners();
        button_clicker.onClick.AddListener( () => OnClickOnButton_clicker() );

        button_deeper.onClick.RemoveAllListeners();
        button_deeper.onClick.AddListener(() => OnClickOnButton_reset());
    }
	
	// Update is called once per frame
	void Update () {

        button_clicker.GetComponentInChildren<Text>().text = "Click value : 1";
        button_deeper.GetComponentInChildren<Text>().text = "Go one layer deeper with : "+ GameManager.instance.ToVerboseNumber( GameManager.instance.GameManagerNextPrestige ) +" more prestiges.";
        text_money.text = GameManager.instance.GetPrimaryCurrencyAsVerboseString();
    }

    private void OnClickOnButton_clicker() {

        GameManager.instance.Earn( 1 );
    }

    private void OnClickOnButton_reset() {

        GameManager.instance.GoDeeper();

        text_prestige.text = "Prestige : " + GameManager.instance.ToVerboseNumber(GameManager.instance.GameManagerPrestige);
    }
}
