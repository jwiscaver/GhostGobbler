using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class LetterClickHandler : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text _tmpTextBox;
    private Canvas _canvasToCheck;
    [SerializeField] public Camera cameraToUse;
    
    public delegate void ClickOnLinkEvent(string keyword);
    public static event ClickOnLinkEvent OnClickedOnLinkEvent;

    private void Awake()
    {
        _tmpTextBox = GetComponent<TMP_Text>();
        _canvasToCheck = GetComponentInParent<Canvas>();

        // Assign camera if needed
        if (_canvasToCheck.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            cameraToUse = _canvasToCheck.worldCamera;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Mouse clicked at position: " + eventData.position);

        Vector2 mousePosition = eventData.position;
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(_tmpTextBox, mousePosition, cameraToUse);
        
       // Debug.Log("Link index found: " + linkIndex);
        
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = _tmpTextBox.textInfo.linkInfo[linkIndex];
            //Debug.Log("Clicked on link: " + linkInfo.GetLinkText());
            OnClickedOnLinkEvent?.Invoke(linkInfo.GetLinkText());
        }
        else
        {
            Debug.Log("No link clicked.");
        }
    }
}