using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// イベントごとに選択可能なアクションをアクションリストに表示する
// アクション選択を行う
// アクションごとにアクションダイヤログを切り替える
public class ActionController : MonoBehaviour
{
    [SerializeField] ActionIcon actionIconPrefab;
    [SerializeField] GameObject actionList;
}
