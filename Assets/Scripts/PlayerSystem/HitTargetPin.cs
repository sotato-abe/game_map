using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System.Collections.Generic;

public class HitTargetPin : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private TargetPin targetPinPrefab;
    [SerializeField] FieldPlayerController fieldPlayerController;
    private TargetPin targetPinInstance;
    [SerializeField] private Canvas fieldCanvas;
    private GraphicRaycaster graphicRaycaster;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        graphicRaycaster = fieldCanvas.GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        if(fieldPlayerController.canMove == false) return;
        if (Input.GetMouseButtonDown(0))
        {
            // UIをクリックした場合は無視
            if (IsPointerOverUIObject()) return;

            // マウス位置をワールド座標に変換
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            Vector2 rayOrigin = new Vector2(worldPoint.x, worldPoint.y);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero);

            if (hit.collider != null)
            {
                // Pinを正しい位置に配置
                if (targetPinInstance == null)
                {
                    targetPinInstance = Instantiate(targetPinPrefab, new Vector3(hit.point.x, hit.point.y, 0), Quaternion.identity);
                    fieldPlayerController.MoveToTargetPin(targetPinInstance.transform.position);
                }
                else
                {
                    targetPinInstance.transform.position = new Vector3(hit.point.x, hit.point.y, 0);
                    fieldPlayerController.MoveToTargetPin(targetPinInstance.transform.position);
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }
    }


    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);
        return results.Count > 0;
    }
}
