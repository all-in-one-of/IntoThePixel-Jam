using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class GUI : MonoBehaviour
{
    public Image ResultOverlay;
    public Text ResultText;

    public Text CountDownText;
    public Text RoundText;

    public Text ScoreP1;
    public Text ScoreP2;

    public void UpdatePlayerScore(Dictionary<int, int> playerScore)
    {
        ScoreP1.text = playerScore[1].ToString();
        ScoreP2.text = playerScore[2].ToString();
    }

    public void IndicateScoreChange(int playerIndex)
    {
        RectTransform target = playerIndex == 1 ? ScoreP1.rectTransform : ScoreP2.rectTransform;
        ScaleGUIObject(target, 2f, 0.5f, 0.5f);
    }
    
    public IEnumerator ShowRoundCountdown(int roundNumber, bool switchSides, UnityAction callback)
    {
        CountDownText.gameObject.SetActive(true);
        CountDownText.text = "";

        RoundText.text = "round " + roundNumber;

        if (switchSides)
        {
            ScaleGUIObject(RoundText.rectTransform, 1.05f, 0.4f, 0.4f);
            yield return new WaitForSeconds(0.2f);
            CountDownText.text = "Switch Sides!";
            ScaleGUIObject(CountDownText.rectTransform, 1.1f, 0.4f, 0.4f);
            yield return new WaitForSeconds(0.8f);
        }
        else
        {
            yield return new WaitForSeconds(0.8f);
        }
        CountDownText.text = "Ready?";
        ScaleGUIObject(CountDownText.rectTransform, 1.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(1.0f);

        CountDownText.text = "Go!";
        ScaleGUIObject(CountDownText.rectTransform, 1.6f, 0.15f, 0.15f);
        yield return new WaitForSeconds(0.5f);

        CountDownText.gameObject.SetActive(false);

        callback();
    }

    public IEnumerator ShowResult(float duration, string message, UnityAction callback)
    {
        ResultOverlay.gameObject.SetActive(true);
        ResultOverlay.color = new Color(0, 0, 0, 0);
        AdjustColor(ResultOverlay, new Color(0, 0, 0, 0.8f), duration / 4f, duration / 4f, duration / 2f);

        ResultText.text = message;
        ScaleGUIObject(ResultText.rectTransform, 2f, duration / 2f, duration / 2f);

        yield return new WaitForSeconds(duration);
        ResultOverlay.gameObject.SetActive(false);

        callback();
    }

    private void ScaleGUIObject(RectTransform target, float targetScale, float durationA, float durationB)
    {
        target.DOScale(targetScale, durationA / 2f).OnComplete(() => target.DOScale(1f, durationB / 2f));
    }

    private void AdjustColor(Image target, Color targetColor, float durationA, float durationB, float delay)
    {
        Color oldColor = target.color;
        target.DOColor(new Color(0, 0, 0, 0.8f), durationA).OnComplete(() => target.DOColor(oldColor, durationB).SetDelay(delay));
    }
}