using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform dragHandle;
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private RectTransform parentRect;
    private Vector3 offset;
    private bool isDragging;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = rectTransform.parent as RectTransform;

        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        // Если dragHandle не назначен, используем весь объект (но тогда будет двигаться за любую часть)
        if (dragHandle == null)
            dragHandle = rectTransform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // ❗ ВАЖНО: Проверяем, что клик был именно по dragHandle
        if (!IsPointerOverDragHandle(eventData))
            return;

        transform.SetAsLastSibling();
        isDragging = true;

        // Сохраняем смещение между центром окна и точкой клика
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                eventData.position,
                null,
                out Vector2 localPoint
            );
            offset = rectTransform.anchoredPosition - localPoint;
        }
        else if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                eventData.position,
                canvas.worldCamera,
                out Vector2 localPoint
            );
            offset = rectTransform.anchoredPosition - localPoint;
        }
        else if (canvas.renderMode == RenderMode.WorldSpace)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            Plane plane = new Plane(canvas.transform.forward, canvas.transform.position);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 worldPoint = ray.GetPoint(distance);
                offset = rectTransform.position - worldPoint;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                eventData.position,
                null,
                out Vector2 localPoint
            );
            rectTransform.anchoredPosition = localPoint + (Vector2)offset;
        }
        else if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                eventData.position,
                canvas.worldCamera,
                out Vector2 localPoint
            );
            rectTransform.anchoredPosition = localPoint + (Vector2)offset;
        }
        else if (canvas.renderMode == RenderMode.WorldSpace)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            Plane plane = new Plane(canvas.transform.forward, canvas.transform.position);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 worldPoint = ray.GetPoint(distance);
                rectTransform.position = worldPoint + offset;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    /// <summary>
    /// Проверяет, находится ли курсор над dragHandle
    /// </summary>
    private bool IsPointerOverDragHandle(PointerEventData eventData)
    {
        // Если dragHandle не задан отдельно, разрешаем перетаскивание за любой элемент
        if (dragHandle == null || dragHandle == rectTransform)
            return true;

        // Получаем камеру для проверки
        Camera camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : canvas.worldCamera;

        // Проверяем, попадает ли курсор в прямоугольник dragHandle
        return RectTransformUtility.RectangleContainsScreenPoint(
            dragHandle,
            eventData.position,
            camera
        );
    }

    public void QuitButton()
    {
        gameObject.SetActive(false);
    }
}