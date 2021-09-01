using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

[RequireComponent(typeof(CardFactory))]
public class CardDeck : MonoBehaviour, IObserver
{
    [SerializeField] float arcAngle = 180f;
    [SerializeField] float maxRotationAngle = 10f;
    [SerializeField] float maxHeight;
    [SerializeField] float width;

    [Tooltip("Number of cards to form a line, not arc")]
    [SerializeField] int cardsNumberForLine;
    [SerializeField] float lineWidth;

    [SerializeField] float tweenTime;

    List<Card> cards = new List<Card>();
    Card cardToUpdate;
    int cardToUpdateIndex = 0;
    CardFactory cardFactory;

    private void Awake()
    {
        cardFactory = GetComponent<CardFactory>();
        int cardsAmount = Random.Range(4, 6);
        cards.AddRange(cardFactory.GetInstances(cardsAmount));
        foreach (var card in cards)
        {
            card.Subscribe(this);
        }
        cardToUpdate = cards[cardToUpdateIndex];
        PositionCards();
    }

    public void OnNotify(object value, NotificationType notificationType = NotificationType.Default)
    {
        if (notificationType.Equals(NotificationType.CardDestroyed))
        {
            Card card = (Card)value;
            RemoveCard(card);
        }
    }

    public void PositionCards()
    {
        if (cards.Count > cardsNumberForLine)
            ArcPosition();
        else
            LinePosition();
    }

    private void LinePosition()
    {
        float step = arcAngle / (cards.Count - 1);
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            float factor = Mathf.Sin((-arcAngle / 2 + step * i) * Mathf.Deg2Rad);
            float x = transform.position.x + lineWidth * factor;
            float y = transform.position.y;
            float z = transform.position.z;

            LeanTween.move(card.gameObject, new Vector3(x, y, z), tweenTime);
            LeanTween.rotate(card.gameObject, Vector3.zero, tweenTime);
        }
    }

    private void ArcPosition()
    {
        float step = arcAngle / (cards.Count - 1);
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            float factor = Mathf.Sin((-arcAngle / 2 + step * i) * Mathf.Deg2Rad);
            float x = transform.position.x + width * factor;
            float y = transform.position.y + maxHeight * (1 - Mathf.Abs(factor));
            float z = transform.position.z;

            card.transform.SetParent(null);
            card.transform.SetParent(transform);
            LeanTween.move(card.gameObject, new Vector3(x, y, z), tweenTime);
            LeanTween.rotate(card.gameObject, Vector3.back * maxRotationAngle * factor, tweenTime);
        }
    }

    public void RemoveCard(Card card)
    {
        cards.Remove(card);
        card.enabled = false;
        Destroy(card.gameObject);
        PositionCards();
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
        PositionCards();
    }

    public void RandomizeCard()
    {
        cardToUpdate.RandomizeValues();
        if (cardToUpdate.enabled)
            cardToUpdateIndex = (cardToUpdateIndex + 1) % cards.Count;
        else
            cardToUpdateIndex %= cards.Count;
        cardToUpdate = cards[cardToUpdateIndex];
    }
}