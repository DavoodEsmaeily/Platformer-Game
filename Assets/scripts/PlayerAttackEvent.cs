using UnityEngine;

public class PlayerAttackEvent : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player =GetComponentInParent<Player>();
    }

    private void AnimationTrigger()
    {
        player.AttackOver();
    }
}
