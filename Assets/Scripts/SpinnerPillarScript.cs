using System;
using NUnit.Framework;
using UnityEngine;

public class SpinnerPillardScript : MonoBehaviour
{
    [SerializeField] private ShouldSpeedUp shouldSpeedUp;

    int nbOfHands = 0;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           // Debug.Log("Player collided with TrucDuMilieuAccélérant");
            nbOfHands++;
            if(nbOfHands == 2)
            {
                Debug.Log("Two hands detected, setting shouldSpeedUp to true");
                shouldSpeedUp.value = true;
            }
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
