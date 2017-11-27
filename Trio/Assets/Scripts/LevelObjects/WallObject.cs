using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : LevelObject {
    private float moveDuration = 0.5f;

    private bool moving = false;
    private bool lowered = false;
    public void ToggleLowered() {
        bool collision = (levelController.PuppetCollision(transform.position) 
            || levelController.PlayerCollision(transform.position));
        if (!moving && !collision) {
            if (lowered) {
                StartCoroutine(Move(Vector3.up));
            } else {
                StartCoroutine(Move(Vector3.down));
            }
            StartCoroutine(PressNoise());
        }        
    }

    private IEnumerator Move(Vector3 direction) {
        moving = true;

        if (direction.normalized == Vector3.up) {
            lowered = false;
        }

        float distance = 1;
        Vector3 startPosition = transform.position;

        float completion = 0;
        while (completion < distance) {
            float movement = Time.deltaTime * (distance / moveDuration);
            transform.Translate(direction * movement);
            completion += movement;
            yield return new WaitForEndOfFrame();
        }
        transform.position = startPosition + direction; //SNAP TO!

        if (direction.normalized == Vector3.down) {
            lowered = true;
        }
        moving = false;
    }

    public override ACTION PressedReaction(PlayerController player) {
        return pressedReaction;
    }

    public override ACTION MoveReaction(PlayerController player) {
        if (lowered) {
            return ACTION.MOVE;
        } else {
            return ACTION.NOPE;
        }
    }


    /* Puppets can interact with walls!
     */
    public override ACTION PressedReaction(Puppet puppet) {
        return pressedReaction;
    }

    public override ACTION MoveReaction(Puppet puppet) {
        if (lowered) {            
            return ACTION.MOVE;
        } else {
            StartCoroutine(MoveNoise());
            return ACTION.NOPE;
        }
    }


    protected override IEnumerator MoveNoise() {
        AudioClip clip = Resources.Load("Effects/Pu 0" + Random.Range(1, 4)) as AudioClip;
        AudioSource.PlayClipAtPoint(clip, transform.position);
        yield return new WaitForEndOfFrame();
    }

    protected override IEnumerator PressNoise() {
        AudioClip clip = Resources.Load("Effects/Shooo 0" + Random.Range(1, 4)) as AudioClip;
        AudioSource.PlayClipAtPoint(clip, transform.position);
        yield return new WaitForEndOfFrame();
    }
}