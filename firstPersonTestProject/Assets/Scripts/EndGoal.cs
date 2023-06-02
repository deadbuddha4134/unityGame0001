using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGoal : MonoBehaviour
{   
    [SerializeField] GameObject endGoalMenu;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Time.timeScale = 0;

            endGoalMenu.SetActive(true);
        }
    }
}
