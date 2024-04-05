using UnityEngine;

public class ShineBlock : MonoBehaviour
{
    // 

    // 
    
    [SerializeField] Collider2D blockCol;
    [SerializeField] SpriteRenderer sprRen;
    [SerializeField] bool revealedByLight = false;

    void Start()
    {
        //blockCol.isTrigger = revealedByLight;
        sprRen.gameObject.SetActive(!revealedByLight);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Light")
        {
            //blockCol.isTrigger = !revealedByLight;
            sprRen.gameObject.SetActive(revealedByLight);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Light")
        {
            //blockCol.isTrigger = revealedByLight;
            sprRen.gameObject.SetActive(!revealedByLight);
        }
    }
}
