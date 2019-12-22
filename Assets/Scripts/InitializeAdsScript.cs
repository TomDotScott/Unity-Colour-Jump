using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAdsScript : MonoBehaviour
{
    string gameId = "3249371";
    bool testMode = false;

    void Start () {
        Advertisement.Initialize (gameId, testMode);
    }
}
