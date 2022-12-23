using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    CanvasManager canvas;

    [Header("Text")]
    public TMP_Text coinsText;
    public TMP_Text healthCountText;
    public TMP_Text slowCountText;
    public TMP_Text healthCostText;
    public TMP_Text slowCostText;

    [Header("Buttons")]
    public Button healthItemButton;
    public Button slowItemButton;
    public Button exitShopButton;

    [Header("Item Costs")]
    public int healthItemCost = 150;
    public int slowItemCost = 300;

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.parent.GetComponent<CanvasManager>();

        if (coinsText)
            coinsText.text = GameManager.Instance.coins.ToString();
        if (healthCountText)
            healthCountText.text = GameManager.Instance.luckyClover.ToString();
        if (slowCountText)
            slowCountText.text = GameManager.Instance.slowFeather.ToString();
        if (healthCostText)
            healthCostText.text = healthItemCost.ToString();
        if (slowCostText)
            slowCostText.text = slowItemCost.ToString();

        if (healthItemButton)
            healthItemButton.onClick.AddListener(AddHealthItem);
        if (slowItemButton)
            slowItemButton.onClick.AddListener(AddSlowItem);
        if (exitShopButton)
            exitShopButton.onClick.AddListener(CloseShop);
    }

    void AddHealthItem()
    {
        if (GameManager.Instance.coins >= healthItemCost)
        {
            GameManager.Instance.coins -= healthItemCost;
            GameManager.Instance.luckyClover++;

            if (coinsText)
                coinsText.text = GameManager.Instance.coins.ToString();
            if (canvas.coinsText)
                canvas.coinsText.text = GameManager.Instance.coins.ToString();
            if (healthCountText)
                healthCountText.text = GameManager.Instance.luckyClover.ToString();

            GameManager.Instance.SaveGame();
        }
    }

    void AddSlowItem()
    {
        if (GameManager.Instance.coins >= slowItemCost)
        {
            GameManager.Instance.coins -= slowItemCost;
            GameManager.Instance.slowFeather++;

            if (coinsText)
                coinsText.text = GameManager.Instance.coins.ToString();
            if (canvas.coinsText)
                canvas.coinsText.text = GameManager.Instance.coins.ToString();
            if (slowCountText)
                slowCountText.text = GameManager.Instance.slowFeather.ToString();

            GameManager.Instance.SaveGame();
        }
    }

    void CloseShop()
    {
        gameObject.SetActive(false);
    }
}
