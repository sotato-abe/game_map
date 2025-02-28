using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlerStatusDialog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI battlerName;
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI techniqueText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] EnchantIcon enchantPrefab;
    [SerializeField] GameObject enchantList;

    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    public virtual void Setup(Battler battler)
    {
        battlerName.SetText(battler.Base.Name);
        attackText.SetText(battler.Attack.ToString());
        techniqueText.SetText(battler.Technique.ToString());
        defenseText.SetText(battler.Defense.ToString());
        speedText.SetText(battler.Speed.ToString());

        ResetList();
        SetEnchant(battler.Enchants);
    }

    public void ShowDescriptionPanel(bool showFlg)
    {
        Debug.Log($"Show BattlerStatusPanel:{showFlg}");
        transform.gameObject.SetActive(showFlg);
    }

    private void ResetList()
    {
        // skillList内のオブジェクトを削除
        foreach (Transform child in enchantList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetEnchant(List<Enchant> enchants)
    {
        // enchantList内にスキルを追加
        foreach (Enchant enchant in enchants)
        {
            EnchantIcon enchantObject = Instantiate(enchantPrefab, enchantList.transform);
            enchantObject.gameObject.SetActive(true);
            EnchantIcon enchantUnit = enchantObject.GetComponent<EnchantIcon>();

            enchantUnit.SetEnchant(enchant);
        }
    }

    public IEnumerator ShowDialog(bool showFlg)
    {
        float time = 0.05f;
        float currentTime = 0f;
        if (showFlg)
        {
            transform.gameObject.SetActive(true);
            Vector3 originalScale = transform.localScale;
            Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);
            while (currentTime < time)
            {
                transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = targetScale;
        }
        else
        {
            Vector3 originalScale = transform.localScale;
            Vector3 targetScale = new Vector3(1, 1, 1);
            while (currentTime < time)
            {
                transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = targetScale;
            transform.gameObject.SetActive(false);
        }

    }
}
