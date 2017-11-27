using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetButton : LevelObject {
    public List<Puppet> puppets;

    public DIRECTION direction;
    private Vector3 VectorDirection () {
        if (direction == DIRECTION.FORWARD) { return Vector3.forward; }
        else if (direction == DIRECTION.RIGHT) { return Vector3.right; }
        else if (direction == DIRECTION.BACK) { return Vector3.back; }
        else { return Vector3.left; }
    }

    public override ACTION PressedReaction(PlayerController player) {
        for (int i = 0; i < puppets.Count; i++) {           
            puppets[i].Move(VectorDirection());
        }
        StartCoroutine(PressNoise());
        return pressedReaction;
    }

    public override ACTION MoveReaction(PlayerController player) {
        StartCoroutine(MoveNoise());
        return ACTION.MOVE;
    }

    /* PUPPETS CAN INTERACT WITH PUPPET CONTROLLERS (for now)
     * Could see that leading to some terrible, albeit interesting effects...
     */
    public override ACTION PressedReaction(Puppet puppet) {
        for (int i = 0; i < puppets.Count; i++) {
            puppets[i].Move(VectorDirection());
        }
        StartCoroutine(PressNoise());
        return pressedReaction;
    }
    public override ACTION MoveReaction(Puppet puppet) {
        return ACTION.MOVE;
    }


    protected override IEnumerator MoveNoise() {
        AudioClip clip = Resources.Load("Effects/Tss 0" + Random.Range(1, 4)) as AudioClip;
        AudioSource.PlayClipAtPoint(clip, transform.position);
        yield return new WaitForEndOfFrame();
    }

    protected override IEnumerator PressNoise() {
        AudioClip clip = Resources.Load("Effects/Bff 0" + Random.Range(1, 4)) as AudioClip;
        AudioSource.PlayClipAtPoint(clip, transform.position);
        yield return new WaitForEndOfFrame();
    }
}
