using System.Collections;
using UnityEngine;

public class AnimatorDelay : MonoBehaviour
{
    public Animator animator; // Animator của bạn
    public string boolName; // Tên của boolean parameter trong Animator
    public float delayTime = 1.0f; // Thời gian delay
    public bool boolValue = true; // Giá trị boolean muốn set

    void Start()
    {
        StartCoroutine(DelayAnimatorBool());
    }

    IEnumerator DelayAnimatorBool()
    {
        yield return new WaitForSeconds(delayTime);

        // Đặt giá trị boolean sau khi delay xong
        animator.SetBool(boolName, boolValue);
    }
}