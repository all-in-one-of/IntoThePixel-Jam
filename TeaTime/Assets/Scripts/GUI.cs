using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GUI : MonoBehaviour
{
    public GameObject Result;

    public Text ResultText;
    public Text CountDownText;
    public Text RoundText;

    public IEnumerator ShowRoundCountdown(int roundNumber, bool switchSides, UnityAction callback)
    {
        CountDownText.gameObject.SetActive(true);
        RoundText.text = "round " + roundNumber;
        if (switchSides)
        {
            CountDownText.text = "Switch Sides!";
            yield return new WaitForSeconds(1f);
        }
        CountDownText.text = "Ready?";
        yield return new WaitForSeconds(0.5f);
        CountDownText.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        CountDownText.gameObject.SetActive(false);
        callback();
    }

    public IEnumerator ShowResult(string message, UnityAction callback)
    {
        Result.SetActive(true);
        ResultText.text = message;
        yield return new WaitForSeconds(2f);
        Result.SetActive(false);
        callback();
    }
}
