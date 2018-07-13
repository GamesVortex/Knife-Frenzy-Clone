using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreControllerScript : MonoBehaviour {

	public static ScoreControllerScript instance;

	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI scoreTextFront;
	public TextMeshProUGUI coinText;
	public TextMeshProUGUI coinTextFront;

	public TextMeshProUGUI gameOverScoreText;
	public TextMeshProUGUI gameOverScoreTextFront;
	public TextMeshProUGUI gameOverCoinText;
	public TextMeshProUGUI gameOverCoinTextFront;
	public TextMeshProUGUI gameOverStageText;
	public TextMeshProUGUI gameOverStageTextFront;

	public TextMeshProUGUI challengePanelCoinText;
	public TextMeshProUGUI challengePanelCoinTextFront;

	public TextMeshProUGUI knifePanelCoinText;
	public TextMeshProUGUI knifePanelCoinTextFront;

	public TextMeshProUGUI startCoinText;
	public TextMeshProUGUI startCoinTextFront;

	private int score;
	private int coinAmount;

	public int CoinAmount { get { return coinAmount; } }
	public int Score { get { return score; } }

	private bool increaseCoinCount;
    private int increaseCoinValueTo;
    private int previousCoinValue;
    private float coinValueTime;
    public float coinValueIncrementBy;

    public int IncreaseCoinValueTo
    {
        set { increaseCoinValueTo = value; }
    }

    public bool IncreaseCoinCount {
        set {
            increaseCoinCount = value;
            coinValueTime = 0;
            previousCoinValue = coinAmount;
        }
    }

	void Awake(){
		instance = this;
		ResetScore ();
		ResetCoins ();
	}

	void Update() {
        if (increaseCoinCount)
        {
            coinAmount = (int)Mathf.Ceil(Mathf.Lerp(previousCoinValue, increaseCoinValueTo, coinValueTime));
            coinValueTime += Time.unscaledDeltaTime * coinValueIncrementBy;
            UpdateCoinText();
			UpdateOtherGUICoinText();

            if (coinValueTime >= 1)
            {
                increaseCoinCount = false;
            }
        }
    }

	public void AddScore(int value){
		score += value;
		UpdateScoreText ();
	}

	public void AddCoin(){
		coinAmount += 1;
		UpdateCoinText ();
	}

	public void UpdateScoreText(){
		scoreText.text = score.ToString();
		scoreTextFront.text = scoreText.text; 
	}
		
	public void UpdateCoinText(){
		coinText.text = coinAmount.ToString();
		coinTextFront.text = coinText.text; 
	}

	public void ResetScore(){
		score = 0;
		UpdateScoreText ();
	}

	public void ResetCoins(){
		coinAmount = PlayerPrefs.GetInt ("CoinAmount");
		startCoinText.text = coinAmount.ToString ();
		startCoinTextFront.text = startCoinText.text;
		UpdateCoinText ();
		UpdateOtherGUICoinText();
	}

	public void UpdateGameOverMenu(){
		PlayerPrefs.SetInt ("CoinAmount", coinAmount);

		gameOverScoreText.text = "SCORE " + score.ToString ();
		gameOverScoreTextFront.text = "SCORE " + score.ToString ();

		gameOverCoinText.text = coinAmount.ToString ();
		gameOverCoinTextFront.text = coinAmount.ToString ();

		gameOverStageText.text = "STAGE " + StageControllerScript.instance.StageNo.ToString();
		gameOverStageTextFront.text = "STAGE " + StageControllerScript.instance.StageNo.ToString();

		UpdateOtherGUICoinText();
	}

	public void UpdateOtherGUICoinText(){
		string coinAmount = this.coinAmount.ToString();

		startCoinText.text = coinAmount;
		startCoinTextFront.text = coinAmount;

		challengePanelCoinText.text = coinAmount;
		challengePanelCoinTextFront.text = coinAmount;

		knifePanelCoinText.text = coinAmount;
		knifePanelCoinTextFront.text = coinAmount;
	}

}
