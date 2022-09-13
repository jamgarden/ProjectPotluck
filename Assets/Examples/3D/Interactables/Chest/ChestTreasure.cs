using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestTreasure : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    [SerializeField] private int spawnCount = 10;

    private Animator animator;
    private bool closeEnough;
    private bool hasSpawnCoins = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && closeEnough && !hasSpawnCoins)
        {
            StartCoroutine(spitCoins());
        }
    }

    IEnumerator spitCoins()
    {
        animator.SetTrigger("open");
        int remainingCoins = spawnCount;
        hasSpawnCoins = true;

            for (int i = 0; i < spawnCount; i++)
            {
            Instantiate(coin, transform.position + new Vector3(0,1,0), Quaternion.Euler(0, 15 * i,0));
                yield return new WaitForSeconds(0.1f);
            }

        Debug.Log("You got " + spawnCount + " gold coins!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            closeEnough = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            closeEnough = false;
        }
    }
}
