
using UnityEngine;
using UnityEngine.EventSystems;


public class firstUISelect : MonoBehaviour
{
    private void OnEnable()
    {	        
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);
    }
}
