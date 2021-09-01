using UnityEngine;

public class CreatureCard : Card
{
    private int health;
    public int Health
    {
        get => health;
        private set
        {
            health = value;
            Notify(Health, NotificationType.UpdateHealth);
            if (health <= 0)
                Notify(this, NotificationType.CardDestroyed);
        }
    }

    int attackStrength;
    public int AttackStrength
    {
        get => attackStrength > 0 ? attackStrength : 0;
        private set
        {
            attackStrength = value;
            Notify(AttackStrength, NotificationType.UpdateAttack);
        }
    }

    public override bool RandomizeValues()
    {
        bool is_updated = base.RandomizeValues();
        if (is_updated)
            return true;

        int choice = Random.Range(1, 10);
        int r = Random.Range(-2, 9);
        if (choice >= 5)
            AttackStrength = r;
        else
            Health = r;
        return true;
    }

    public override void AssignAllValues()
    {
        base.AssignAllValues();
        AttackStrength = 1;
        Health = 1;
    }
}