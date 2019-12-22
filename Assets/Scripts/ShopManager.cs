using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class ShopManager : MonoBehaviour
{
    public Sprite[] sprites;
    [SerializeField] private GameObject[] buttons;

    [SerializeField] private GameManager theGameManager;
    [SerializeField] private TextMeshProUGUI gemText, notEnoughText, selectedText;

    private StringBuilder boughtSprites;

    private Animator theAnimator;

    private bool animationFinished = true;
    
    private Dictionary<int, int> spritePrices = new Dictionary<int, int>(){
        //stored as ID, PRICE
        {0, 0},
        {1, 10},
        {2, 10},
        {3, 15},
        {4, 15},
        {5, 15},
        {6, 20},
        {7, 30},
        {8, 50},
    };
    
    private Dictionary<int, string> spriteIDs = new Dictionary<int, string>()
    {
        {0, "Circle"},
        {1, "Square"},
        {2, "Football"},
        {3, "Comet"},
        {4, "Candy"},
        {5, "Whale"},
        {6, "Penguin"},
        {7, "Rainbow"},
        {8, "Earth"}
    };

    // Start is called before the first frame update
    void Start() {
        boughtSprites = new StringBuilder(PlayerPrefs.GetString("BoughtSprites", "Circle"));
    }

    private void Awake()
    {
        boughtSprites = new StringBuilder(PlayerPrefs.GetString("BoughtSprites", "Circle"));
        theAnimator = gameObject.GetComponent<Animator>();
        ShowTick();
        ShowText();
    }

    // Update is called once per frame
    void Update()
    {
        gemText.text = theGameManager.Gems.ToString();
    }

    private bool CheckIfBought(int spriteID)
    {
        //see whether the playerpref string contains the value
        return PlayerPrefs.GetString("BoughtSprites").Contains(spriteIDs[spriteID]);
    }

    private void ShowTick()
    {
        for (int i = 0; i < spriteIDs.Count; i++)
        {
            if (!CheckIfBought(i)) continue;
            buttons[i].gameObject.transform.Find("Tick").gameObject.SetActive(true);
        }
    }

    private void ShowText()
    {
        for (int i = 0; i < spriteIDs.Count; i++)
        {
            if (!CheckIfBought(i)) continue;
            var currentButton = buttons[i].gameObject;
            if (currentButton.transform.Find("SelectedText(Clone)") != null)
            {
                Destroy(currentButton.transform.Find("SelectedText(Clone)").gameObject);
            }
                
            selectedText.text = i == PlayerPrefs.GetInt("Sprite") ? "SELECTED" : "NOT SELECTED";
            currentButton.transform.Find("Gems").gameObject.SetActive(false);
            Instantiate(selectedText.gameObject, currentButton.transform);
        }
    }

    private void SelectSprite(int newSprite)
    {
        PlayerPrefs.SetInt("Sprite", newSprite);
        ShowText();
    }

    public void BuySprite(int spriteID){
        SoundManager.Instance.PlaySFX("UI_Click");
        Debug.Log("BoughtSprites: " + PlayerPrefs.GetString("BoughtSprites"));
        //check if bought already...
        if (CheckIfBought(spriteID))         //if bought already, set the sprite
        {
            SelectSprite(spriteID);
        }
        else            //if not...
        {
            if(spritePrices[spriteID] <= theGameManager.Gems){ //if you have enough gems, buy the sprite
                theGameManager.Gems -= spritePrices[spriteID];
                //Set the Sprite
                SelectSprite(spriteID);
                //Store the purchase
                string newSprite = spriteIDs[spriteID];
                boughtSprites.Append(newSprite);
                PlayerPrefs.SetString("BoughtSprites", boughtSprites.ToString());
                //Set the Colour to show the purchase
                ShowTick();

            }else{ //if not, display message
                Debug.Log("Current TimeScale:" + Time.timeScale);
                notEnoughText.text = "YOU DON'T HAVE ENOUGH GEMS. YOU NEED " + (spritePrices[spriteID] - theGameManager.Gems) + " MORE GEMS TO BUY THIS SKIN"; 
                notEnoughText.gameObject.SetActive(true);
            }
        }
    }
}
