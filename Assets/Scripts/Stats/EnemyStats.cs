using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyStats : CharacterStats
{
    [SerializeField] private int scoreValue = 100;
    public Stat attackSpeed;
    [SerializeField] private EnemyManager enemyManager;

    public override void TakeDamage(float damageTaken, Element element)
    {
        float elementModifier = CheckElement(element);
        float totalDamage = damageTaken * elementModifier;
        totalDamage -= armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, float.MaxValue);
        ChangeCurrentHP(totalDamage, false);
        
        DmgTextPopup(totalDamage, elementModifier);
        if (GetCurrentHP() <= 0)
        {
            Die();
        }
    }

    private void DmgTextPopup(float damage, float modifier)
    {
        GameObject textPopup = Instantiate(Resources.Load<GameObject>("Popups/DmgText"), new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + 3, transform.position.z + Random.Range(-0.5f, 0.5f)), Quaternion.Euler(90, 0, 0));
        TextMeshPro text = textPopup.GetComponent<TextMeshPro>();
        text.text = "" + damage;
        switch (modifier)
        {
            case 0.5f:
                text.fontSize = 4;
                text.color = Color.cyan;
                break;
            case 0.75f:
                text.fontSize = 5;
                text.color = Color.blue;
                break;
            case 1f:
                text.fontSize = 6;
                text.color = Color.white;
                break;
            case 1.25f:
                text.fontSize = 7;
                text.color = Color.red;
                break;
        }
    }

    public override void Die()
    {
        GameManager.instance.ChangeScore(scoreValue, true);
        enemyManager.OnDeath();
        //TODO death animation, run from EnemyManager
    }

    public void SetScoreValue(int value)
    {
        scoreValue = value;
    }
}
