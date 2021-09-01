using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CardFactory : GenericMonoBehaviourFactory<Card>
{
    Sprite cardSprite;
    Queue<Card> noImageCards = new Queue<Card>();

    private void Awake()
    {
        StartCoroutine(GetSprite());
    }

    public override Card GetInstance()
    {
        Card card = base.GetInstance();
        if (cardSprite == null)
            noImageCards.Enqueue(card);
        else
            card.SetCardImage(cardSprite);
        return card;
    }

    IEnumerator GetSprite()
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://picsum.photos/300/300");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(texture.width / 2, texture.height / 2)
                );
            cardSprite = sprite;

            while(noImageCards.Count != 0)
            {
                Card card = noImageCards.Dequeue();
                card.SetCardImage(cardSprite);
            }
        }
        else
        {
            Debug.LogError(request.error.ToString());
        }
    }
}
