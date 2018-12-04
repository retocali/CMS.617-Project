using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseButtonScript : MonoBehaviour, 
								 IPointerExitHandler,
								 IPointerClickHandler,
								 IPointerEnterHandler
{

	public bool pressed = false;
	public int index;
	private Color defaultColor;
	// Use this for initialization
	void Start () {
		defaultColor = transform.GetChild(1).GetComponent<Text>().color;
		pressed = false;
		Deselect();
	}
	
	public void OnPointerEnter(PointerEventData eventData) {
		Select();
	}
	public void OnPointerExit(PointerEventData eventData) {
		Deselect();
	}
	public void OnPointerClick(PointerEventData eventData) {
		pressed = true;
	}


	public void Select()
	{
		transform.parent.parent.gameObject.GetComponent<PauseMaster>().Select(index);
		transform.GetChild(0).gameObject.SetActive(true);
		transform.GetChild(1).GetComponent<Text>().color = Color.white;
	}

	public void Deselect()
	{
		transform.GetChild(0).gameObject.SetActive(false);	
		transform.GetChild(1).GetComponent<Text>().color = defaultColor;
	}
	
}
