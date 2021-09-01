using UnityEngine;
using UnityEngine.UI;

public abstract class Card : SubjectMonoBehaviour
{
    [SerializeField] int manaCost = 1;
    public int ManaCost
    {
        get => manaCost > 0 ? manaCost : 0;
        private set
        {
            manaCost = value;
            Notify(ManaCost, NotificationType.UpdateMana);
        }
    }

    [SerializeField] Image cardImage;

    private void Start()
    {
        OnStart();
    }

    public virtual void OnStart()
    {
        AssignAllValues();
    }

    public virtual bool RandomizeValues()
    {
        int choice = Random.Range(1, 10);
        int r = Random.Range(-2, 9);
        bool isUpdated = choice >= 5;
        if (isUpdated)
            ManaCost = r;
        return isUpdated;
    }

    public virtual void AssignAllValues()
    {
        ManaCost = manaCost;
    }

    public void SetCardImage(Sprite sprite)
    {
        cardImage.sprite = sprite;
    }
}