using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class Keypad : Interactable
{

    [SerializeField]
    private GameObject door;

    [SerializeField]
    private int[] requiredSequence;

    [SerializeField]
    private float sequenceTimeout = 5f; // time allowed between gestures
    private KeypadHandTracker handTracker;
    private bool doorOpen;
    private List<int> currentSequence = new List<int>();
    private float lastGestureTime;

    // Start is called before the first frame update
    void Start()
    {
        if (handTracker == null) handTracker = GetComponent<KeypadHandTracker>();
        if (handTracker == null) Debug.LogError("missing KeypadHandTracker component.");
        promptMessage = "Code: " + string.Join("-", requiredSequence);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSequence.Count > 0 && Time.time - lastGestureTime > sequenceTimeout)
        {
            ResetSequence();
        }

        if (handTracker.TryGetStableGesture(out int gesture))
        {
            HandleNewGesture(gesture);
        }
    }

    public void HandleNewGesture(int gesture)
    {
        if (gesture >= 0)
        {
            currentSequence.Add(gesture);
            lastGestureTime = Time.time;
        }

        bool isValidSoFar = true;
        for (int i = 0; i < currentSequence.Count && i < requiredSequence.Length; i++)
        {
            if (currentSequence[i] != requiredSequence[i])
            {
                isValidSoFar = false;
                break;
            }
        }

        if (!isValidSoFar)
        {
            ResetSequence();
            return;
        }

        if (currentSequence.Count == requiredSequence.Length)
        {
            Interact();
            ResetSequence();
        }
    }

    private void ResetSequence()
    {
        currentSequence.Clear();
        lastGestureTime = 0f;
    }

    public override bool ValidateInteraction()
    {
        var number = handTracker.HandleGesture();
        Debug.Log(number.ToString());
        return currentSequence.Count < requiredSequence.Length && number == requiredSequence[currentSequence.Count];
    }

    protected override void Interact()
    {
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
    }
}
