using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUnit : Unit
{
    public Command Command { get; set; }
    [SerializeField] Image unitImage;
    [SerializeField] UnitStatusLayer unitStatusLayer;
    [SerializeField] CommandDialog dialog;


    public virtual void Setup(Command command)
    {
        Command = command;
        unitImage.sprite = Command.Base.Sprite;
        dialog.gameObject.SetActive(true);
        dialog.Setup(Command);
        SetStatus(UnitStatus.Active);
    }

    public void OnPointerEnter()
    {
        dialog.ShowDialog(true);
        StartCoroutine(OnPointer(true));
    }

    public void OnPointerExit()
    {
        dialog.ShowDialog(false);
        StartCoroutine(OnPointer(false));
    }

    public void SetStatus(UnitStatus status)
    {
        unitStatusLayer.Setup(status);
    }

    public void SetCommandMotion(UnitMotionType motion)
    {
        if (this == null || !gameObject.activeInHierarchy) return; // 破棄されている場合は処理しない

        switch (motion)
        {
            case UnitMotionType.Jump:
                StartCoroutine(JumpMotion());
                break;
        }
    }

    private IEnumerator JumpMotion()
    {
        float bounceHeight = 40f;
        float damping = 0.5f;
        float gravity = 5000f;
        float groundY = transform.position.y;

        while (bounceHeight >= 0.1f)
        {
            float verticalVelocity = Mathf.Sqrt(2 * gravity * bounceHeight);
            bool isFalling = false;

            while (transform.position.y >= groundY || !isFalling)
            {
                if (this == null) yield break; // 途中でオブジェクトが破棄されたら終了

                verticalVelocity -= gravity * Time.deltaTime;
                transform.position += Vector3.up * verticalVelocity * Time.deltaTime;

                if (transform.position.y <= groundY)
                {
                    isFalling = true;
                    break;
                }

                yield return null;
            }

            bounceHeight *= damping;
        }

        if (this != null)
        {
            transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
        }
    }
}
