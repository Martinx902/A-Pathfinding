using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI humansLeft;

    [SerializeField]
    private TextMeshProUGUI zombies;

    [SerializeField]
    private TextMeshProUGUI time;

    public void UpdateCounters(int humans)
    {
        humansLeft.text = humans.ToString();

        zombies.text = (20 - humans).ToString();
    }

    public void TimeCompleted(float timeInSeconds)
    {
        int timeInSecondsInt = (int)timeInSeconds;  //We don't care about fractions of a second, so easy to drop them by just converting to an int
        int minutes = (int)timeInSeconds / 60;  //Get total minutes
        int seconds = timeInSecondsInt - (minutes * 60);  //Get seconds for display alongside minutes
        time.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");  //Create the string representation, where both seconds and minutes are at minimum 2 digits
    }
}