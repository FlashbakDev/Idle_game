using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Factory : MonoBehaviour {

    [SerializeField] private HighInteger baseCost;
    [SerializeField] private HighInteger cost;
    [SerializeField] private int factor;
    [SerializeField] private int count;
    [SerializeField] private float speed;
    [SerializeField] private float time;
    [SerializeField] private int level;
    [SerializeField] private HighInteger baseIncome;
    [SerializeField] private HighInteger income;

    [SerializeField] private Text text_cost;
    [SerializeField] private Text text_count;
    [SerializeField] private Text text_income;
    [SerializeField] private Button button_buy;
    [SerializeField] private Button button_buyMax;
    [SerializeField] private Slider slider_tick;

    void Start() {

        baseCost = new HighInteger(1);
        cost = new HighInteger(1);
        factor = 5;
        count = 0;
        speed = 0.5f;
        level = 1;
        baseIncome = new HighInteger(1000);
        income = new HighInteger(0);
        time = 0.0f;

        slider_tick.value = 0;

        UpdateTexts();
    }

    void Update() {

        button_buy.interactable = GameManager.instance.GameManagerMoney >= cost;
        button_buyMax.interactable = button_buy.interactable;

        if (count <= 0) return;

        slider_tick.value = 1 / speed * time;

        time += Time.deltaTime;

        if ( time >= speed) {

            time -= speed;

            GameManager.instance.GameManagerMoney += income;
        }
    }

    private void UpdateTexts() {

        text_count.text = "count : " + count.ToString();
        text_cost.text = "cost : " + cost.ToString();
        text_income.text = "income : " + income.ToString();
    }

    public void OnClickOnButtonBuy() {

        GameManager.instance.GameManagerMoney -= cost;

        count++;
        cost += baseCost * (factor * count);

        income = (count * level) * baseIncome;

        UpdateTexts();
    }

    public void OnClickOnButtonBuyMax() {

        var watch = System.Diagnostics.Stopwatch.StartNew();

        while (GameManager.instance.GameManagerMoney >= cost) {

            GameManager.instance.GameManagerMoney -= cost;

            count++;
            cost += baseCost * (factor * count);

            if ( watch.ElapsedMilliseconds > 1000) {

                Debug.Log("Factory - OnClickOnButtonBuyMax() : TimeOut");
                break;
            }
        }

        income = (count * level) * baseIncome;

        UpdateTexts();

        watch.Stop();

        Debug.Log("Factory - OnClickOnButtonBuyMax() : elapsed = " + watch.Elapsed.ToString());
    }
}
