//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class RandomTree : MonoBehaviour
//{
//    private Animator animator;

//    void Start()
//    {
//        animator = GetComponent<Animator>();

//        if (animator != null)
//        {
//            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
//            float clipLength = clipInfo[0].clip.length;

//            float randomStartTime = Random.Range(0f, clipLength);

//            animator.Play(clipInfo[0].clip.name, 0, randomStartTime / clipLength);
//        }
//    }
//}