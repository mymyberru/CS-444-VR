using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchObject : MonoBehaviour
{
	public GameObject testcube;
	private bool is_on=false;
	public GameObject light;
	private bool is_selected=false;
    void Start()
    {
		testcube.SetActive(false);
		light.SetActive(false);
		is_selected=false;
		is_on=false;
    }

    // Update is called once per frame
    void Update()
    {

		
    }
	public void toggle_visibility(){
		testcube.SetActive(!testcube.activeSelf);
		is_on=!is_on;
	}
	public void selected(){
		is_selected=true;
		light.SetActive(true);
	}
	public void unselected(){
		is_selected=false;
		light.SetActive(false);
	}
}
