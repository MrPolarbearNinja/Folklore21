using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Magic Effects/Explosion")]
public class ExplosionMagicEffect : ScriptableObject, IMagicEffect
{
    public void Activate()
    {
        Debug.Log("Explosion Magic Activated: Wiping board.");
        GameManager.Instance.CurrentLane.ClearLane();
    }
}