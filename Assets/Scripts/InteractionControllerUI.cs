using TMPro;
using UnityEngine;

public class InteractionControllerUI : MonoBehaviour
{
    [SerializeField]
    private InteractionController interactionController;

    [SerializeField]
    private TextMeshProUGUI hintLabel;

    private void OnEnable()
    {
        interactionController.OnInteractiveObjectSelected += ShowHint;
        interactionController.OnInteractiveObjectUnselected += HideHint;
    }

    private void ShowHint(InteractiveObject interactiveObject)
    {
        hintLabel.enabled = true;
        hintLabel.text = interactiveObject.Hint;
    }

    private void HideHint(InteractiveObject target)
    {
        hintLabel.enabled = false;
    }

    private void OnDisable()
    {
        interactionController.OnInteractiveObjectSelected -= ShowHint;
        interactionController.OnInteractiveObjectUnselected -= HideHint;
    }
}
