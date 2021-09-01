using System.Collections;
using UnityEngine;
using TMPro;
using System;
using DentedPixel;

public class UIObserver : MonoBehaviour, IObserver
{
    [SerializeField] float countdownInterval = 0.2f;
    [SerializeField] float scaleFactor = 1.5f;
    [SerializeField] TMP_Text tmpText;
    [SerializeField] SubjectMonoBehaviour observable;
    [SerializeField] NotificationType notificationType;
    [SerializeField] bool useCountdown;

    Vector3 startScale;

    private void Awake()
    {
        if (observable == null)
            observable = GetComponent<SubjectMonoBehaviour>();
        observable.Subscribe(this);
        startScale = tmpText.rectTransform.localScale;
    }

    protected void UpdateText(string text)
    {
        tmpText.text = text;
        LeanTween.pause(tmpText.gameObject);
        tmpText.rectTransform.localScale = startScale;
        var sequence = LeanTween.sequence();
        sequence.append(LeanTween.scale(tmpText.rectTransform, startScale * scaleFactor, countdownInterval / 2));
        sequence.append(LeanTween.scale(tmpText.rectTransform, startScale, countdownInterval / 2));
    }

    protected IEnumerator CountdownRoutine(int newValue, int currentValue)
    {
        if (currentValue == newValue)
            yield break;
        int step = newValue > currentValue ? 1 : -1;
        for (int i = currentValue; i != newValue + step; i += step)
        {
            UpdateText(i.ToString());
            yield return new WaitForSeconds(countdownInterval);
        }
    }

    public virtual void OnNotify(object value, NotificationType notificationType = NotificationType.Default)
    {
        if (notificationType.Equals(this.notificationType))
        {
            if (useCountdown)
            {
                int newValue;
                try
                {
                    int currentValue = Int32.Parse(tmpText.text);
                    newValue = value is int ? (int)value : 0;
                    StartCoroutine(CountdownRoutine(newValue, currentValue));
                }
                catch (FormatException)
                {
                    UpdateText(value.ToString());
                }

            }
            else
            {
                UpdateText(value.ToString());
            }
        }
    }
}
