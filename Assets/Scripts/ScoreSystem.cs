using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier;
    [SerializeField] private TMP_Text moneyText;

    public const string HighScoreKey = "HighScore";

    private float score;

    public const string MoneyKey = "Money";

    private float money = 0;
    public bool hitMoney = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime * scoreMultiplier;

        scoreText.text = Mathf.FloorToInt(score).ToString();//Round down the value
        moneyText.text = Mathf.FloorToInt(money).ToString();

        if(hitMoney) 
        {
            money += (score/3);
            hitMoney = false;
        }
    }

    private void OnDestroy()
    {
        //Save player's highscore and money
        float currentMoney = PlayerPrefs.GetInt(MoneyKey, 0);
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        float addedMoney = currentMoney + money;
        PlayerPrefs.SetInt(MoneyKey, Mathf.FloorToInt(addedMoney));
        if(score > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, Mathf.FloorToInt(score));
        }
    }
}
