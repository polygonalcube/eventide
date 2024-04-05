using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    //

    //

    PlayerLogic player;

    [SerializeField] TextMeshProUGUI sanityText;
    [SerializeField] TextMeshProUGUI sirenText;
    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerLogic>();
    }

    void Update()
    {
        sanityText.text = "Sanity: " + player.sanity.ToString();
        if (player.debugSiren) sirenText.text = "Sirens are: " + "ON";
        else sirenText.text = "Sirens are: " + "OFF";
    }
}
