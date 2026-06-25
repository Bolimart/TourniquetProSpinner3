using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public class SpinnerPillardScript : MonoBehaviour
{
    [SerializeField] private ShouldSpeedUp shouldSpeedUp;
    
    [SerializeField] private UnityEvent startEvent;
    
    int nbOfHands = 0;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Debug.Log("Player collided with SpinnerAccelerate");
            startEvent.Invoke();
            print("One hand on the spinner");
            nbOfHands++;
            if(nbOfHands == 2)
            {
                Debug.Log("Two hands detected, setting shouldSpeedUp to true");
                shouldSpeedUp.value = true;
            }
            if(nbOfHands > 2) Debug.Log("More than two hands detected, WTF IS HAPPENING !");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nbOfHands--;
            if(nbOfHands < 2)
            {
                shouldSpeedUp.value = false;
            }
        }
    }
}
