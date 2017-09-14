using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameGUIManager : MonoBehaviour {

    [SerializeField] private Button button_clicker;
    [SerializeField] private Button button_buy_10;
    [SerializeField] private Button button_buy_100;

    [SerializeField] private Text text_money;
    [SerializeField] private Text text_count_10;
    [SerializeField] private Text text_count_100;
    [SerializeField] private Text text_cost_10;
    [SerializeField] private Text text_cost_100;

    // Use this for initialization
    void Start () {

        text_money = GameObject.Find("Text_money").GetComponent<Text>();

        text_count_10 = GameObject.Find("Text_count_10").GetComponent<Text>();
        text_count_100 = GameObject.Find("Text_count_100").GetComponent<Text>();
        text_cost_10 = GameObject.Find("Text_cost_10").GetComponent<Text>();
        text_cost_100 = GameObject.Find("Text_cost_100").GetComponent<Text>();

        button_clicker = GameObject.Find("Button_clicker").GetComponent<Button>();
        button_buy_10 = GameObject.Find("Button_buy_10").GetComponent<Button>();
        button_buy_100 = GameObject.Find("Button_buy_100").GetComponent<Button>();

        button_clicker.onClick.RemoveAllListeners();
        button_clicker.onClick.AddListener( () => OnClickOnButton_clicker() );

        button_buy_10.onClick.RemoveAllListeners();
        button_buy_10.onClick.AddListener(() => OnClickOnButton_buy_10());

        button_buy_100.onClick.RemoveAllListeners();
        button_buy_100.onClick.AddListener(() => OnClickOnButton_buy_100());

        text_count_10.text = "count : 0";
        text_count_100.text = "count : 0";
        text_cost_10.text = "cost : 10";
        text_cost_100.text = "cost : 1000";
    }
	
	// Update is called once per frame
	void Update () {

        text_money.text = GameManager.instance.GameManagerMoney.HighIntegerFormatedNumber;
    }

    private void OnClickOnButton_clicker() {

        Debug.Log("GameGUIManager - OnClickOnButton_clicker()");

        GameManager.instance.GameManagerMoney += 100;
    }

    private void OnClickOnButton_buy_10() {

        Debug.Log("GameGUIManager - OnClickOnButton_buy_10()");

        GameManager.instance.GameManagerMoney += 1000;
    }

    private void OnClickOnButton_buy_100() {

        Debug.Log("GameGUIManager - OnClickOnButton_buy_100()");

        GameManager.instance.GameManagerMoney += 10000;
    }
}
