using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wire : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool isLeftWire;

    public Color customColor;

    private Image image;
    // Start is called before the first frame update

    private LineRenderer lineRenderer;
    private bool isDragStarted = false;
    public Canvas canvas;

    private Wiretask wireTask;

    public bool isSuccessful = false;

    private void Awake()
    {
        image = GetComponent<Image>();
        lineRenderer = GetComponent<LineRenderer>();
        wireTask = GetComponentInParent<Wiretask>();
    }

    public void Reset()
    {
        isDragStarted = false;
        isSuccessful = false;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragStarted)
        {
            Vector2 movePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out movePos);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, canvas.transform.TransformPoint(movePos));
        } else
        {
            if (!isSuccessful)
            {
                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, Vector3.zero);
            }
        }

        bool isHovered = RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Input.mousePosition, canvas.worldCamera);
        if (isHovered)
        {
            wireTask.currentHoveredWire = this;
        }
    }

    public void setColor(Color color)
    {
        image.color = color;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        customColor = color;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isLeftWire) { return; }
        if (isSuccessful) { return; }

        isDragStarted = true;
        wireTask.currentDraggedWire = this;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (wireTask.currentHoveredWire != null)
        {
            if (wireTask.currentHoveredWire.customColor == customColor && !wireTask.currentHoveredWire.isLeftWire)
            {
                isSuccessful = true;
                wireTask.currentHoveredWire.isSuccessful = true;
            }
        }
        isDragStarted = false;
        wireTask.currentDraggedWire = null;
    }
}
