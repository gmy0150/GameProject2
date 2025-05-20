using UnityEngine;

public class AnimationManage {
    public Animator anim;
    public void TransAnim(Animator animator,string name,bool value)
    {
        animator.SetBool(name,value);
    }
}