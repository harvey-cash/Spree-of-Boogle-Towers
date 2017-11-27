using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Puppet : MonoBehaviour {

    private bool canMove = true;
    private float rollDuration = 0.3f;

    private Vector3 startPosition;

    private LevelController levelController;
    public void SetLevel (LevelController level) {
        levelController = level;
    }

    private void Start() {
        startPosition = transform.position;
    }

    protected IEnumerator MoveNoise() {
        AudioClip clip = Resources.Load("Effects/Clop 0" + Random.Range(1, 4)) as AudioClip;
        AudioSource.PlayClipAtPoint(clip, transform.position);
        yield return new WaitForEndOfFrame();
    }

    /*
     * For a given direction, roll the player.
     */
    public void Move(Vector3 direction) {
        if (canMove) {

            // If trying to move...            
            if (direction != Vector3.zero) {
                Vector3 endPosition = transform.position + direction;

                ACTION moveReaction = levelController.MoveTo(endPosition, this);
                bool collisions = levelController.PlayerCollision(endPosition) 
                    || levelController.PuppetCollision(endPosition);

                if (moveReaction != ACTION.NOPE && !collisions) {
                    StartCoroutine(Roll(transform.position, direction));
                    StartCoroutine(MoveNoise());
                }
            }
        }
    }

    /* Players and Puppets can't affect each other... yet
     */
    public ACTION MoveReaction() {
       return ACTION.NOPE;
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

        // Press upon landing. If win, teleport back from whence you came.
        ACTION pressResult = levelController.Press(transform.position, this);
        if (pressResult == ACTION.WIN) {
            transform.position = startPosition;
        }
    }

    /*
     * For the given direction, return the correct roll axis.
     */
    private Vector3 RollAxis(Vector3 direction) {
        if (direction.normalized == Vector3.forward) {
            return Vector3.right;
        } else if (direction.normalized == Vector3.right) {
            return Vector3.back;
        } else if (direction.normalized == Vector3.back) {
            return Vector3.left;
        } else {
            return Vector3.forward;
        }
    }
}
