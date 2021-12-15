using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D collision;
    void Start() {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("destroy");
            StartCoroutine(DestroyCoin());
        }

    }

    IEnumerator DestroyCoin()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
