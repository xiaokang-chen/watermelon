using UnityEngine.UI;
using UnityEngine;

public class targetImage : MonoBehaviour
{
    public DragHandler LastDragHandler;

    private Color originalColor;
    private Sprite originalSprite;

    void Start()
    {
        originalColor = gameObject.GetComponent<Image>().color;
        originalSprite = gameObject.GetComponent<Image>().sprite;
    }

    public void ResetDragHandler(){
        if(LastDragHandler != null){
            LastDragHandler.gameObject.SetActive(true);
            gameObject.GetComponent<Image>().color = originalColor;
            gameObject.GetComponent<Image>().sprite = originalSprite;
            LastDragHandler = null;
        }
    }
}
