using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimator
{
    [TaskCategory("Basic/Animator")]
    [TaskDescription("Returns success if the specified AnimatorController layer in a transition.")]
    public class IsPlaying : Conditional
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        public SharedInt layerIndex;
        [Tooltip("The layer's index")]
        public SharedString stateName;

        private Animator animator;
        private GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                animator = currentGameObject.GetComponent<Animator>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (animator == null)
            {
                Debug.LogWarning("Animator is null");
                return TaskStatus.Failure;
            }
            var clips = animator.GetCurrentAnimatorClipInfo(layerIndex.Value);
            foreach(var clip in clips)
            {
                Debug.Log(clip.clip.name);
            }
          ///  for(int i=0;i<)
           
          //  animator.GetCurrentAnimatorClipInfo(0)[0].clip.name
            return animator.IsInTransition(layerIndex.Value) || animator.GetCurrentAnimatorClipInfo(layerIndex.Value)[0].clip.name == stateName.Value ? TaskStatus.Running : TaskStatus.Success;
            //return animator.IsInTransition(stateName.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            layerIndex = 0;
            stateName = "";
        }
    }
}