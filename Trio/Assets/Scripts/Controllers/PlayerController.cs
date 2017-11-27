using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {   

    private InputController inputController = new InputController();

    private GameController gameController;
    public GameController Game {
        set { gameController = value; }
    }

    private Vector3 centerOfFloor = Vector3.zero;
    public void SetTargetPos(Vector3 position) { centerOfFloor = position; }

    private Rigidbody cameraObject;
    public Rigidbody CameraObject {
        set { cameraObject = value; }
    }
    private Vector3 cameraOffset;
    public Vector3 CameraOffset {
        set { cameraOffset = value; }
    }
    private Rigidbody targetObject;
    private Rigidbody lightObject;

    private LevelController levelController;
    public LevelController Level {
        set { levelController = value; }
    }

    private bool canMove = true;
    private float rollDuration = 0.3f;

    /*
     * Called every frame
     */
    private void Update() {        
        Move();
        ControlTargetAndLight();
        ControlCamera();
    }

    /*
     * For a given direction, roll the player.
     */
    private void Move() {
        if (canMove) {
            Vector3 direction = inputController.GetInput();

            // If trying to move...            
            if (direction != Vector3.zero) {
                Vector3 endPosition = transform.position + direction;

                ACTION moveReaction = levelController.MoveTo(endPosition, this);
                bool puppetCollision = levelController.PuppetCollision(endPosition);

                if (moveReaction != ACTION.NOPE && !puppetCollision) {
                    StartCoroutine(Roll(transform.position, direction));
                    StartCoroutine(MoveNoise());
                }                
            }            
        }
    }


    private IEnumerator MoveNoise() {
        AudioClip clip = Resources.Load("Effects/Pu 0" + Random.Range(1, 4)) as AudioClip;
        AudioSource.PlayClipAtPoint(clip, transform.position);
        yield return new WaitForEndOfFrame();
    }


    /* Add forces to the camera in order
     * to move it around. Duh.
     */
    public void Initialise() {
        targetObject = new GameObject("Camera Target").AddComponent<Rigidbody>();
        targetObject.useGravity = false;
        targetObject.transform.position = transform.position;
        targetObject.mass = 0.1f;
        targetObject.drag = 10;

        lightObject = new GameObject("Light Target").AddComponent<Rigidbody>();
        lightObject.useGravity = false;
        Light illum = lightObject.gameObject.AddComponent<Light>();
        illum.type = LightType.Point;
        lightObject.mass = 0.1f;
        lightObject.drag = 10;
    }

    private void ControlTargetAndLight() {
        if (targetObject != null) {            
            targetObject.AddForce(((transform.position + centerOfFloor) * 0.5f) 
                - targetObject.transform.position);
        }
        if (lightObject != null) {
            lightObject.AddForce(transform.position + Vector3.up - lightObject.transform.position);
        }
    }
    
    public void Illuminate() {
        lightObject.GetComponent<Light>().intensity = 0.25f;
    }
    public void Delluminate() {
        lightObject.GetComponent<Light>().intensity = 0;
    }

    private void ControlCamera() {
        if(cameraOffset != null) {
            cameraObject.AddForce((targetObject.transform.position - cameraOffset) 
                - cameraObject.transform.position);
            cameraObject.transform.LookAt(targetObject.transform);
        }        
    }


    /*
     * Begin rolling, prevent any further movement until roll
     * is complete.
     */
    private IEnumerator Roll(Vector3 start, Vector3 direction) {
        canMove = false;

        float rollThrough = 90;
        Quaternion startRotation = transform.rotation;
        
        Vector3 rollPoint = 
            (transform.position - (new Vector3(0, transform.localScale.y / 2, 0))) 
            + (direction / 2);
        Vector3 rollAxis = RollAxis(direction);

        float completion = 0;
        while (completion < rollThrough) {
            float angle = Time.deltaTime * (rollThrough / rollDuration);
            transform.RotateAround(rollPoint, rollAxis, angle);
            completion += angle;
            yield return new WaitForEndOfFrame();
        }

        // Snap to correct position and angle
        GameObject temp = new GameObject("Temp");
        temp.transform.rotation = startRotation;
        temp.transform.RotateAround(rollPoint, rollAxis, rollThrough);
        transform.rotation = temp.transform.rotation;
        Destroy(temp);

        transform.position = start + direction;
        canMove = true;

        // Press upon landing
        levelController.Press(transform.position, this);
    }

    /*
     * For the given direction, return the correct roll axis.
     */ 
     private Vector3 RollAxis(Vector3 direction) {
        if (direction.normalized == Vector3.forward) {
            return Vector3.right;
        }
        else if (direction.normalized == Vector3.right) {
            return Vector3.back;
        }
        else if (direction.normalized == Vector3.back) {
            return Vector3.left;
        }
        else {
            return Vector3.forward;
        }
    }
}
