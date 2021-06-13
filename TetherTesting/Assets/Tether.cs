using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tether : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    Transform player;
    public float distance;
    public float angle;
    public int numOfKnots;
    public int children;
    public float boundsX;
    public float boundsY;
    public bool broken = false;
    // Start is called before the first frame update
    void Start()
    {
        children = 0;
        distance = 0f;
        angle = 0f;
        numOfKnots = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
            return;

        /* Track Child Count (For Debugging) */
        numOfKnots = transform.childCount;

        /* Face The Player */
        Vector3 dir = player.position - transform.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle -90, Vector3.forward);

        /* Knots */

        if (broken)
            return;
            
        distance = Vector2.Distance(transform.position,player.position);
        angle = Vector2.Angle(transform.position,player.position);

        int newKnots = (int) Mathf.Floor(distance/0.2f); //Floor or Ceil?

        if (newKnots > numOfKnots) {
            addKnots(newKnots - numOfKnots);
        } else if (newKnots < numOfKnots) {
            removeKnots(numOfKnots - newKnots);
        }

        //numOfKnots = newKnots;
        numOfKnots = transform.childCount;

        /* Track Child Count (For Debugging) */
        children = transform.childCount;
    }

    void addKnots(int numToAdd) {
        for (int i = 0; i < numToAdd; i++)
        {
            GameObject newKnot = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newKnot.transform.parent = transform;
            newKnot.GetComponent<Knot>().player = player;
        }
    }

    void removeKnots(int numToRemove) {
        for (int i=0; i < numToRemove; i++) 
        {
            Destroy(transform.GetChild(transform.childCount-1).gameObject);
        }
    }

    public void deleteTether() {
        Debug.Log("Deleting Tether");
        broken = true;
        foreach (Transform child in transform) {
            if (child)
                Destroy(child.gameObject);
        }
    }

    /*
    void setKnotNums() {
        int current = 0;
        foreach (Transform child in transform)
        {
            child.GetComponent<Knot>().knotNum = current;
            current++;
        }
    }
    */

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Tether found Player");
            player = other.gameObject.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Tether lost Player");
            deleteTether();
            player = null;
            broken = false;
        }
    }
}
