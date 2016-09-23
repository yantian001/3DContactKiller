using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Gun))]
public class GunAnimation : MonoBehaviour
{

    public Gun gun;

    public Animator animator;

    public AnimationClip clipIdle;
    public AnimationClip clipBolt;
    public AnimationClip clipReload;
    public AnimationClip clipShoot;

    public void Awake()
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }
        if (animator)
        {
            AnimatorOverrideController overrideController = new AnimatorOverrideController();
            overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
            if (clipIdle)
            {
                overrideController["GunIdle"] = clipIdle;
            }
            if (clipBolt)
            {
                overrideController["GunBolt"] = clipBolt;
            }
            if (clipReload)
            {
                overrideController["GunReload"] = clipReload;
            }
            if (clipShoot)
            {
                overrideController["GunShoot"] = clipShoot;
            }
            animator.runtimeAnimatorController = overrideController;
        }
    }
}
