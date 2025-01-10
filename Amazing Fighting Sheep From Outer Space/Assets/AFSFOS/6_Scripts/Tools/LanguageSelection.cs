using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LanguageSelection : MonoBehaviour
{
    public Button[] languageButtons;
    public Button validateButton;

    private Color defaultColor = Color.white;
    private Color highlightColor = Color.yellow;

    private int selectedIndex = 0;

    private PlayerInput playerInput;
    private InputAction navigateAction;
    private InputAction submitAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        // Récupérer les actions correspondantes dans l'ActionMap
        navigateAction = playerInput.actions["Move"];
        submitAction = playerInput.actions["Jump"];
    }

    private void OnEnable()
    {
        navigateAction.Enable();
        submitAction.Enable();

        navigateAction.performed += OnNavigate;
        submitAction.performed += OnSubmit;
    }

    private void OnDisable()
    {
        navigateAction.Disable();
        submitAction.Disable();

        navigateAction.performed -= OnNavigate;
        submitAction.performed -= OnSubmit;
    }

    private void Start()
    {
        HighlightButton(languageButtons[selectedIndex]);
    }

    private void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 navigationInput = context.ReadValue<Vector2>();

        if (navigationInput.y > 0.1f)  // Aller vers le haut
        {
            NavigateUp();
        }
        else if (navigationInput.y < -0.1f)  // Aller vers le bas
        {
            NavigateDown();
        }
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        SelectButton();
    }

    private void NavigateUp()
    {
        UnhighlightButton(languageButtons[selectedIndex]);  // Enlever la surbrillance du bouton actuel

        selectedIndex--;

        if (selectedIndex < 0)
        {
            selectedIndex = languageButtons.Length;  // Aller au bouton "Valider" qui est après les boutons de langue
        }
        else if (selectedIndex == languageButtons.Length)
        {
            HighlightButton(validateButton);  // Surbrillance du bouton Valider
            return;
        }

        HighlightButton(languageButtons[selectedIndex]); // Surbrillance sur le bouton nouvellement sélectionné
    }

    private void NavigateDown()
    {
        UnhighlightButton(languageButtons[selectedIndex]);  // Enlever la surbrillance du bouton actuel

        selectedIndex++;

        if (selectedIndex > languageButtons.Length)
        {
            selectedIndex = 0;  // Revenir au premier bouton de langue
        }
        else if (selectedIndex == languageButtons.Length)
        {
            HighlightButton(validateButton);  // Surbrillance du bouton Valider
            return;
        }

        HighlightButton(languageButtons[selectedIndex]); // Surbrillance sur le bouton nouvellement sélectionné
    }

    private void HighlightButton(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = highlightColor;
        button.colors = colors;
    }

    private void UnhighlightButton(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = defaultColor;
        button.colors = colors;
    }

    private void SelectButton()
    {
        // Simuler un clic sur le bouton sélectionné
        if (selectedIndex < languageButtons.Length)
        {
            languageButtons[selectedIndex].onClick.Invoke();
        }
        else
        {
            validateButton.onClick.Invoke();
        }
    }
}
