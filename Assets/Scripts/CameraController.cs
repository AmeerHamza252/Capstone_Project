using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	//Variables
	[SerializeField] private float mouseSensitivity;

	//References

	private Transform parent;

	private void Start()
	{
		parent = transform.parent;

		// hiding cursor on the screen
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		Rotate();
	}

	private void Rotate() 
	{
		// rotating player to mouse cursor direction
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		parent.Rotate(Vector3.up, mouseX);
	}
}
