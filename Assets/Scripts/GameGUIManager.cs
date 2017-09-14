using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameGUIManager : MonoBehaviour {

    [SerializeField] private Button button_clicker;

    [SerializeField] private Text text_money;

    // Use this for initialization
    void Start () {

        text_money = GameObject.Find("Text_money").GetComponent<Text>();

        button_clicker = GameObject.Find("Button_clicker").GetComponent<Button>();

        button_clicker.onClick.RemoveAllListeners();
        button_clicker.onClick.AddListener( () => OnClickOnButton_clicker() );
    }
	
	// Update is called once per frame
	void Update () {

        text_money.text = GameManager.instance.GameManagerMoney.ToString();
    }

    private void OnClickOnButton_clicker() {

        Debug.Log("GameGUIManager - OnClickOnButton_clicker()");

        GameManager.instance.GameManagerMoney += 1;
    }
}
