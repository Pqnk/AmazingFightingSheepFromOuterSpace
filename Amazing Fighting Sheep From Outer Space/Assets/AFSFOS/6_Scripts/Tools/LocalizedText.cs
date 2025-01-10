using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public KeyLanguage keyLanguage;
    private string previousLanguage;

    void Start()
    {
        UpdateText();
    }

    private void Update()
    {
        string currentLanguage = SuperManager.instance.langageManager.currentLanguage;

        if (currentLanguage != previousLanguage)
        {
            previousLanguage = currentLanguage;
            UpdateText();
        }
    }

    public void UpdateText()
    {
        TMP_Text textComponent = GetComponent<TMP_Text>();
        if (textComponent != null)
        {
            textComponent.text = SuperManager.instance.langageManager.GetLocalizedText(keyLanguage);
        }
    }
}
