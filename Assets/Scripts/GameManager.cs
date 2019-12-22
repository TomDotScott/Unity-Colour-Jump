using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Monetization;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI scoreText, highScoreText, gemText, bestText, gemsText;
    //GameObjects
    [SerializeField] private GameObject gem, thePlayer;
    //Menus
    [SerializeField] private GameObject gameOverMenu, shopPanel, optionsPanel, respawnPrompt;

    [SerializeField] private float gemSpawnThreshold = 10;

    private int currentHighScore, scoreUponDeath;

    [SerializeField] private UnityAdsPlacement _adsPlacement;

    public bool respawned, adShown;

    [SerializeField] private int gems;

    public bool IsDead { get; set; }

    public int score;

    public int Gems
    {
        get
        {
            return gems;
        }

        set
        {
            gems = value;
            gemText.text = gems.ToString();
            PlayerPrefs.SetInt("Gems", gems);
        }
    }

    private void Start() {
        currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        Gems = PlayerPrefs.GetInt("Gems", 5);
    }

    // Use this for initialization
    void Awake () {
        currentHighScore = PlayerPrefs.GetInt("HighScore");
        Gems = PlayerPrefs.GetInt("Gems", 5);
        //Time.timeScale = 0f;
        scoreUponDeath = PlayerPrefs.GetInt("ScoreUponDeath");
        adShown = false;
        if (scoreUponDeath != 0)
        {
            AddScore(scoreUponDeath);
            PlayerPrefs.SetInt("ScoreUponDeath", 0);
            respawned = true;
            adShown = true;
        }
    }


    // Update is called once per frame
    void Update () {
        GameOver();
	}

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = score.ToString();
        if(score > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            currentHighScore = score;
            highScoreText.text = currentHighScore.ToString();
        }
        else
        {
            highScoreText.text = currentHighScore.ToString();
        }

        //whether to spawn a gem or not
        if(score % 5 == 0)
        {
            SpawnGem();
        }
    }

    public void Restart()
    {
        PlayUIClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Respawn()
    {
        PlayerPrefs.SetInt("ScoreUponDeath", score);
        Restart();
    }

    public void SpawnGem()
    {
        int random = Random.Range(0, 1000);
        if(random <= gemSpawnThreshold)
        {
            Instantiate(gem, new Vector3(0, thePlayer.transform.position.y + 10), Quaternion.identity);
        }

    }

    private void GameOver()
    {
        if (!IsDead) return;
        if (respawnPrompt && !respawned)
        {
            ChangeColour();
            respawnPrompt.SetActive(true);
        }
        else
        {
            ChangeColour();
            ShowAdvert();
            gameOverMenu.SetActive(true);
        }
    }

    private void ShowAdvert()
    {
        int threshold = 35;
        int random = Random.Range(0, 100);
        if (adShown == true) return;
        if (random <= threshold) return;
        adShown = true;
        _adsPlacement.ShowAd();
    }

    public void QuitGame()
    {
        PlayUIClick();
        Application.Quit();
    }

    public void ShowHideShop()
    {
        PlayUIClick();
        gameOverMenu.SetActive(!gameOverMenu.activeSelf);
        shopPanel.SetActive(!shopPanel.activeSelf);
    }

    public void ShowHideOptions()
    {
        PlayUIClick();
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    private void ChangeColour()
    {
        highScoreText.color = Color.white;
        scoreText.color = Color.white;
        bestText.color = Color.white;
        gemsText.color = Color.white;
        gemText.color = Color.white;
    }

    private void PlayUIClick()
    {
        SoundManager.Instance.PlaySFX("UI_Click");
    }
}
