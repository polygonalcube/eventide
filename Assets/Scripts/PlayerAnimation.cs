using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] List<AnimationFrames> anims = new();
    MoveComponent mc;
    PlayerAnimationState s = PlayerAnimationState.WALK_RIGHT;
    int i = 0;
    void Start()
    {
        mc = GetComponent<MoveComponent>();
        InvokeRepeating(nameof(AnimationTick), 0f, 0.2f);
    }
    void AnimationTick()
    {
        if (mc.Speed.magnitude == 0) return;
        if (Mathf.Abs(mc.Speed.x) > 0)
            s = mc.Speed.x > 0 ? PlayerAnimationState.WALK_RIGHT : PlayerAnimationState.WALK_LEFT;
        else
            s = mc.Speed.y > 0 ? PlayerAnimationState.WALK_UP : PlayerAnimationState.WALK_DOWN;
        var selectedAnimation = anims.Where(a => a.State.Equals(s)).First().Frames;
        sr.sprite = selectedAnimation[i++ % (selectedAnimation.Count() - 1)];
    }
}
[System.Serializable]
public struct AnimationFrames
{
    public PlayerAnimationState State;
    public List<Sprite> Frames;
}
public enum PlayerAnimationState
{
    WALK_RIGHT, WALK_LEFT,
    WALK_UP, WALK_DOWN,
}
