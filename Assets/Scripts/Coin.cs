using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AnimationClip glistenAnimation;
    [SerializeField] float interval;

    Animator animator;
    float timer;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        timer = Time.time % interval;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            StartCoroutine(PlayAnimation());
        }
    }

    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(interval);
        animator.SetBool("isGlistening", true);
        yield return new WaitForSeconds(glistenAnimation.length);
        animator.SetBool("isGlistening", false);       
    }

    public void OnCollected()
    {
        Scoring scoring = FindObjectOfType<Scoring>();
        if (scoring != null)
        {
            scoring.AddCoins(1);
        }

        Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
