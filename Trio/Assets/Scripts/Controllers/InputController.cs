using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController  {

	public Vector3 GetInput() {

        bool inputLeft = false;
        bool inputRight = false;
        bool inputUp = false;
        bool inputDown = false;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            // Get movement of the finger since last frame
            Vector2 touchPosition = Input.GetTouch(0).position;

            if (touchPosition.x < (Screen.width * (1 / 3))) { inputLeft = true; }
            if (touchPosition.x > (Screen.width * (2 / 3))) { inputRight = true; }
            if (Screen.height - touchPosition.y < (Screen.height * (1 / 3))) { inputDown = true; }
            if (Screen.height - touchPosition.y > (Screen.height * (2 / 3))) { inputUp = true; }

        }

        if (Input.GetKey(KeyCode.UpArrow)
            || Input.GetKey(KeyCode.W)
            || inputUp) {
            return Vector3.forward;
        }
        if (Input.GetKey(KeyCode.DownArrow)
            || Input.GetKey(KeyCode.S)
            || inputDown) {
            return Vector3.back;
        }
        if (Input.GetKey(KeyCode.LeftArrow) 
            || Input.GetKey(KeyCode.A) 
            || inputLeft) {
            return Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow) 
            || Input.GetKey(KeyCode.D) 
            || inputRight) {
            return Vector3.right;
        }
        

        return Vector3.zero;
    }
}
