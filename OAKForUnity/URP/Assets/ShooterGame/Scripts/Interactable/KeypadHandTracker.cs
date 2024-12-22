using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class KeypadHandTracker : MonoBehaviour
{
    [SerializeField]
    private OAKForUnity.UBHandTracking handTracking;

    public void LogGesture()
    {
        if (string.IsNullOrEmpty(handTracking.ubHandTrackingResults)) return;
        Debug.Log(handTracking.ubHandTrackingResults);
    }

    public int CountFingers(JSONNode hand)
    {
        int count = 0;
        if (hand["gesture"] == "FIST") return 0;
        if (hand["gesture"] == "OK" || hand["gesture"] == "ONE") return 1;
        if (hand["gesture"] == "PEACE" || hand["gesture"] == "TWO") return 2;
        if (hand["gesture"] == "THREE") return 3;
        if (hand["gesture"] == "FOUR") return 4;
        if (hand["gesture"] == "FIVe") return 5;

        return count;
    }

    public int HandleGesture()
    {
        if (string.IsNullOrEmpty(handTracking.ubHandTrackingResults)) return -1;

        var json = JSON.Parse(handTracking.ubHandTrackingResults);
        var hand0 = json["hand_0"];
        var hand1 = json["hand_1"];

        if (hand0 == null && hand1 == null) return -1;

        var activeHand = (hand0 != null) ? hand0 : hand1;

        var numFingers = CountFingers(activeHand);
        return numFingers;

    }
}
