using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinLevel : MonoBehaviour
{   void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerSingleton.Instance.winText.text = "You win!";
            PlayerSingleton.Instance.winText.gameObject.SetActive(true);

            Debug.Log("You win!");
        }
    }
}
