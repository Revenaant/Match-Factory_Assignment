using UnityEngine;
using UnityEngine.EventSystems;

[SelectionBase]
public class Interactable : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.LogError("Ayy you clicky on me");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
