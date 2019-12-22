using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RespawnPrompt : MonoBehaviour {
    [SerializeField] private Slider theSlider;
    [SerializeField] private TextMeshProUGUI countdownText, scoreText;
    [SerializeField] private float countdownTimer;
    [SerializeField] private GameManager theGameManager;
    private bool respawnPlayer = false;

    void Start()
    {
        theSlider.minValue = 0f;
        theSlider.maxValue = countdownTimer;
        theGameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    private void Awake()
    {
        SoundManager.Instance.PlaySFX("CountdownTimer");
        respawnPlayer = false;
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            scoreText.text = theGameManager.score.ToString();
            countdownText.text = Math.Floor(countdownTimer).ToString();
            countdownTimer -= Time.deltaTime;
            theSlider.value = countdownTimer;
        }
        if (theSlider.value <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Respawn()
    {
        SoundManager.Instance.PlaySFX("UI_Click");
        if (theGameManager.Gems >= 5)
        {
            theGameManager.Gems -= 5;
            Debug.Log("Player Respawned");
            StartCoroutine(PlayRespawnSound());
            respawnPlayer = true;
            theGameManager.Respawn();
            SoundManager.Instance.StopSFX("CountdownTimer");
        }
    }

    public void CloseWindow()
    {
        SoundManager.Instance.StopSFX("CountdownTimer");
        SoundManager.Instance.PlaySFX("UI_Click");
        Destroy(gameObject);
    }

    private IEnumerator PlayRespawnSound()
    {
        SoundManager.Instance.PlaySFX("Respawn");
        yield return new WaitForSeconds(1);
    }
}
