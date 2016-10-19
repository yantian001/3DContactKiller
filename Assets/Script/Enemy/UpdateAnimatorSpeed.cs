using UnityEngine;
using System.Collections;

public class UpdateAnimatorSpeed : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    // Use this for initialization
    void Start()
    {
        if (!animator)
            animator = GetComponent<Animator>();
        if (!agent)
            agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent && animator)
        {
            if (agent.enabled)
                animator.SetFloat("speed", agent.speed);
            else
                animator.SetFloat("speed", 0);

        }
    }
}
