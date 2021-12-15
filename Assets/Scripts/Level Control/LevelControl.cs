using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelControl : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMovement player;
    //public string levelName;
    public int[] levelArray;
    const int levelNums = 6;

    void Start() 
    {
        levelArray = new int[levelNums];
        for (int i = 0; i < levelNums; ++i)
        {
            levelArray[i] = 0;
        }
        levelArray[0] = 1;    
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        
        if (collision.gameObject.tag == "Player")
        {
            player.animator.SetTrigger("disappear");
            StartCoroutine(WaitForPlayerDisappear());
            
        } 
    }
    
    IEnumerator WaitForPlayerDisappear()
    {
        yield return new WaitUntil(() => player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);
        for (int i = 0; i < levelArray.Length; ++i)
        {
            if (levelArray[i] == 1)
            {
                switch (i)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        this.gameObject.transform.Find("Level 3").gameObject.SetActive(true);
                        break;
                    case 3:
                        this.gameObject.transform.Find("Level 4").gameObject.SetActive(true);
                        break;
                    case 4:
                        this.gameObject.transform.Find("Level 5").gameObject.SetActive(true);
                        break;
                    case 5:
                        this.gameObject.transform.Find("Level 6").gameObject.SetActive(true);
                        break;
                }
            }
        }
        transform.GetChild(0).gameObject.SetActive(true);
    }
    
}
