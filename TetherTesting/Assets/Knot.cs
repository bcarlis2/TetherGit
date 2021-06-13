using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knot : MonoBehaviour
{
    public Transform player;
    public int knotNum;
    public float boundsX;
    public float boundsY;
    // Start is called before the first frame update
    void Start()
    {
        getSpriteSize();
        transform.localPosition = Vector3.zero;
        knotNum = transform.GetSiblingIndex();
        transform.localPosition = new Vector3(0f,knotNum*0.2f,0f);
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 dir = player.position - transform.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    void getSpriteSize() {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>(); //Set the reference to our SpriteRenderer component
        boundsX = sprite.bounds.extents.x * 2; //Distance to the right side, from your center point
        //-sprite.bounds.extents.x //Distance to the left side
        boundsY = sprite.bounds.extents.y *2;//Distance to the top
        //-sprite.bounds.extents.y //Distance to the bottom
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Obstacle")) {
            Debug.Log("Tether found Obstacle");
            transform.parent.gameObject.GetComponent<Tether>().deleteTether();
        }
    }


    /*
    void findKnotNum() {
        int lookNum = 0;

        foreach (Transform child in transform.parent)
        {
            Debug.Log("LookNum: " +lookNum);
            if (child.gameObject == transform.gameObject)
            {
                Debug.Log("Knot found self!");
                knotNum = lookNum;
                return;
            }
            knotNum++;
        }
    }
    */
}
