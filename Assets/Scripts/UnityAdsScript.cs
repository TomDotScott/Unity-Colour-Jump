using UnityEngine;
using UnityEngine.Monetization;

public class UnityAdsScript : MonoBehaviour { 

    string gameId = "3249371";
    bool testMode = false;

    void Start () {
        Monetization.Initialize (gameId, testMode);
    }
}
