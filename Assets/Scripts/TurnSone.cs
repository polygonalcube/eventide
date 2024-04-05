using UnityEngine;

public class TurnStone : MonoBehaviour
{
    // The stones present in the attic.

    // These stones allow the player to rotate them with Rigidbody physics. If a stone points towards a target, it's gem will glow. 
    // If all the stones of the attic point towards a target, the attic puzzle will be solved.
    
    [SerializeField] SpriteRenderer sprRen;
    [SerializeField] GameObject[] influencees; // The stone that will rotate along with the one pushed by the player.
    float prevRot;
    Vector2 startPos;

    [SerializeField] Transform target;
    [SerializeField] float angleRange;
    [SerializeField] bool inDebugMode = false;

    public static int glowingNum = 0; // Amount of stones glowing.

    void Start()
    {
        prevRot = transform.eulerAngles.z;
        startPos = (Vector2)transform.position;
    }

    void FixedUpdate()
    {
        if (prevRot != transform.eulerAngles.z)
        {
            foreach (GameObject stone in influencees)
            {
                stone.transform.eulerAngles += new Vector3(0f, 0f, prevRot - transform.eulerAngles.z);
                stone.GetComponent<TurnStone>().prevRot = stone.transform.eulerAngles.z; // Hacky.
            }
            prevRot = transform.eulerAngles.z;
        }
        
        sprRen.gameObject.SetActive(IsPointingAtTarget());

        // For some reason, even though "Freeze Position" on both axes is enabled, the Rigidbody can still be moved; this line prevents this.
        if ((Vector2)transform.position != startPos) transform.position = (Vector3)startPos;
    }

    // Method of target recognition using raycasts.
    bool IsTargetHit()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + (Vector2)(transform.up * 2.1f), transform.up);
        return ((hit.collider != null) && (hit.collider.gameObject.tag == "Rune"));
    }

    // Method of target recognition using vector-derived angles.
    bool IsPointingAtTarget()
    {
        if (target == null) return false;
        // Find the vector of the stone pointing to the rune: inverse of (stone pos - rune pos).
        Vector2 posVec = (Vector2)transform.position - (Vector2)target.position;
        posVec = new Vector2(-posVec.x, -posVec.y).normalized;
        if (inDebugMode) Debug.Log("posVec: " + posVec.ToString());
        
        float posVecAng = Mathf.Atan2(posVec.y, posVec.x) * Mathf.Rad2Deg;
        if (inDebugMode) Debug.Log("posVecAng: " + posVecAng.ToString());

        bool isPointing = ((Mathf.Abs(Mathf.Repeat(transform.eulerAngles.z, 360f) - Mathf.Repeat((posVecAng + 270f), 360f)) <= angleRange) ||
        (Mathf.Abs(Mathf.Repeat(transform.eulerAngles.z, 360f) - (Mathf.Repeat((posVecAng + 270f), 360f)) - 360f) <= angleRange));

        if (inDebugMode) Debug.Log("isPointing: " + isPointing.ToString());
        return isPointing;
    }
}
