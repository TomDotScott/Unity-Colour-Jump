using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairManager : MonoBehaviour {

    [SerializeField] private GameObject theStair;
    [SerializeField] private float stairWidth;

    [SerializeField] private GameManager theGameManager;

    private int stairIndex = -1;

    private float hueValue;

	// Use this for initialization
	void Start () {
        InitialiseColour();
        for (int i = 0; i < 2; i++)
        {
            MakeNewStair();
        } 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MakeNewStair()
    {
        float randomPosition = 0;

        if(theGameManager.score != 0){
            randomPosition = Random.Range(-6f, 6f);
        }

        Vector2 newPosition = new Vector2(randomPosition, stairIndex * 6);

        GameObject newStair = Instantiate(theStair, newPosition, Quaternion.identity);

        newStair.transform.parent = transform;

        //Get smaller as we spawn more...
        if (stairWidth >= 0.5 && stairWidth <= 10)
        {
            stairWidth -= 0.05f;
        }
        newStair.transform.localScale = new Vector2(stairWidth, 1);

        SetColour(newStair);

        stairIndex ++;
    }

    private void InitialiseColour()
    {
        hueValue = Random.Range(0, 1f);
        Camera.main.backgroundColor = Color.HSVToRGB(hueValue, 0.6f, 0.8f);
    }

    private void SetColour(GameObject newStair)
    {
        if(Random.Range(0, 3) != 0)
        {
            hueValue += 0.15f;
            if(hueValue >= 1)
            {
                hueValue -= 1;
            }
        }
        newStair.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(hueValue, 0.6f, 0.8f);
    }
}
