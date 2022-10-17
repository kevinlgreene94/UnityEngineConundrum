using UnityEngine;
[CreateAssetMenu(menuName = "Hitpoints")]
public class HitPoints : ScriptableObject
{
    public float value;

    public void Damage(float amount)
    {
        if(amount > 0)
        {
            value = value - amount;
        }
    }
}
