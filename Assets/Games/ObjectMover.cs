using UnityEngine;
using System.Collections;

public class ObjectMover : MonoBehaviour
{
    public Transform target;
    public float MaxY = 0.0f;
    public float speed = 5f;
    public Animator animator;

    public Transform spriteTransform;
    private SpriteRenderer spriteRenderer;
    private Camera cam;
    private float halfWidth;
    private float halfHeight;
    private float minX;
    private float maxX;

    public bool isUnderControl = false;

    private Sprite IdleSprite;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        IdleSprite = spriteRenderer.sprite;
        animator.speed = 0;
        cam = Camera.main;
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
        
        float spriteHalfWidth = spriteTransform.GetComponent<SpriteRenderer>().bounds.extents.x;
        
        minX = spriteTransform.position.x - spriteHalfWidth + halfWidth;
        maxX = spriteTransform.position.x + spriteHalfWidth - halfWidth;
    }

    void Update()
    {
        if(isUnderControl){
            return;
        }

        if(animator.enabled == false){
            spriteRenderer.sprite = IdleSprite;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            if (mousePosition.y < MaxY)
            {
                target.position = mousePosition;
                Debug.Log(mousePosition);
                StopAllCoroutines();
                StartCoroutine(MoveToPosition(mousePosition));
            }
        }

        Vector3 newPosition = transform.position;
        newPosition.y = cam.transform.position.y;
        newPosition.z = cam.transform.position.z;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        cam.transform.position = newPosition;
    }

    private IEnumerator MoveToPosition(Vector3 position)
    {
        while (Mathf.Abs((target.position - transform.position).magnitude) > 0.1f)
        {
            animator.enabled = true;
            animator.speed = 1;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            if(target.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
            yield return null;
        }
        animator.speed = 0;
        animator.enabled = false;
        spriteRenderer.sprite = IdleSprite;
    }
}