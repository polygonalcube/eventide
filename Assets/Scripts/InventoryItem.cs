using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Image image;
    private TextMeshProUGUI countText;

    public Item item;

    [HideInInspector] public int Count = 1;
    [HideInInspector]public Transform parentAfterDrag;

    private bool dragging;
    
    private Vector3 targetPos, currentPos;
    private float targetRot, currentRot;
    private Vector3 normalScale;

    private void Awake()
    {
        image = GetComponent<Image>();
        countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        normalScale = transform.localScale;
    }
    private void Start()
    {

    }
    public void InitializeItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.Image;
        RefreshCount();
    }
    public void RefreshCount()
    {
        countText.text = Count.ToString();
        countText.gameObject.SetActive(Count > 1);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        targetPos = currentPos = transform.position;
        targetRot = currentRot = 0;
        //anim
        transform.localScale = normalScale;
        transform.DOPunchScale(Vector3.one * -0.2f, 0.15f);
    }
    public void OnDrag(PointerEventData eventData)
    {
        //unity won't let me remove this method.
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        transform.rotation = Quaternion.Euler(0,0,currentRot = targetRot = 0);
        //anim
        transform.localScale = normalScale;
        transform.DOPunchScale(Vector3.one * 0.2f, 0.15f);
    }
    private void Update()
    {
        if (!dragging) return;
        targetPos = Input.mousePosition;
        currentPos = transform.position = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * 10f);
        targetRot = Mathf.Clamp((targetPos.x - currentPos.x) * 2.5f, -15f, 15f);
        currentRot = Mathf.Lerp(currentRot, targetRot, Time.deltaTime * 5f);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentRot));
    }
}
