using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour
{

    

    public float startingHitPoints;
    public float maxHitPoints;

    public enum CharacterCategory
    {
        PLAYER,
        ENEMY
    }

    public CharacterCategory characterCategory;

    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }
    public abstract void ResetCharacter();
    public abstract IEnumerator DamageCharacter(int damage, float interval);
}