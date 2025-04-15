using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System.Collections.Generic;

public class HitTargetPin : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private TargetPin targetPinPrefab;
    [SerializeField] FieldPlayer fieldPlayer;
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
        if (!fieldPlayer.canMove) return;

        if (Input.GetMouseButtonDown(0))
        {
            // UIをクリックした場合は無視
            if (EventSystem.current.IsPointerOverGameObject()) return;

            // マウス位置をワールド座標に変換
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            Vector2 rayOrigin = new Vector2(worldPoint.x, worldPoint.y);

            // 全てのヒット情報を取得
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.zero);

            if (hits.Length == 0)
            {
                Debug.Log("No hit");
                return;
            }

            // Groundレイヤーが含まれているかどうかを判定
            bool hasGround = false;
            Vector2 groundHitPoint = Vector2.zero;

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    hasGround = true;
                    groundHitPoint = hit.point;
                    break;
                }
            }

            if (!hasGround) return;

            // Pinを正しい位置に配置
            Vector3 pinPosition = new Vector3(groundHitPoint.x, groundHitPoint.y, 0);

            if (targetPinInstance == null)
            {
                targetPinInstance = Instantiate(targetPinPrefab, pinPosition, Quaternion.identity);
            }
            else
            {
                targetPinInstance.transform.position = pinPosition;
            }

            fieldPlayer.MoveToTargetPin(pinPosition);
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

    public void RemoveTargetPin()
    {
        if (targetPinInstance != null)
        {
            Destroy(targetPinInstance.gameObject);
            targetPinInstance = null;
        }
    }
}
