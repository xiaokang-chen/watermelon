using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PuzzleNextLevel : MonoBehaviour
{
    public bool isChangePic  = false;
    public Image TargetImage;
    public Sprite NewSprite;
    public GameObject Tip;
    public GameObject[] puzzlePieces;
    public string NextSceneName;

    public bool isUsingBlack = false;
    public CanvasGroup BlackCanvasGroup;
    public Animator animator;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("NowScene", SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        if(isChangePic){
            int count = 0;
            foreach(GameObject puzzlePiece in puzzlePieces){
                if(puzzlePiece.activeSelf){
                    count++;
                }
            }
            if(count == puzzlePieces.Length){
                TargetImage.sprite = NewSprite;
                Tip.SetActive(true);
            }
        }
    }

    public void OnNextButtonClicked(){
        int count = 0;
        foreach(GameObject puzzlePiece in puzzlePieces){
            if(puzzlePiece.activeSelf){
                count++;
            }
        }
        if(count == puzzlePieces.Length){
            if(!isUsingBlack)
                UnityEngine.SceneManagement.SceneManager.LoadScene(NextSceneName);
            else{
                StartCoroutine(FadeOut());
            }
        }
    }

    IEnumerator FadeOut(){
        animator.SetBool("isFinish", true);
        animator.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        BlackCanvasGroup.gameObject.SetActive(true);
        BlackCanvasGroup.alpha = 0;
        while(BlackCanvasGroup.alpha < 1){
            BlackCanvasGroup.alpha += Time.deltaTime/2;
            animator.transform.position = Vector3.MoveTowards(animator.transform.position, target.position, Time.deltaTime * 2);
            yield return null;
        }
        BlackCanvasGroup.alpha = 1;

        UnityEngine.SceneManagement.SceneManager.LoadScene(NextSceneName);
    }
}
