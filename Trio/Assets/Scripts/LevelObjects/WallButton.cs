using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallButton : LevelObject {
    public List<WallObject> walls;

    public override ACTION PressedReaction(PlayerController player) {
        for (int i = 0; i < walls.Count; i++) {
            walls[i].ToggleLowered();
        }
        StartCoroutine(PressNoise());
        return pressedReaction;
    }

    public override ACTION MoveReaction(PlayerController player) {
        StartCoroutine(MoveNoise());
        return ACTION.MOVE;
    }


    /* Puppets can control walls!
     */
    public override ACTION PressedReaction(Puppet puppet) {
        for (int i = 0; i < walls.Count; i++) {
            walls[i].ToggleLowered();
        }
        StartCoroutine(PressNoise());
        return pressedReaction;
    }

    public override ACTION MoveReaction(Puppet puppet) {
        StartCoroutine(MoveNoise());
        return ACTION.MOVE;
    }


    protected override IEnumerator MoveNoise() {
        AudioClip clip = Resources.Load("Effects/Tss 0" + Random.Range(1, 4)) as AudioClip;
        AudioSource.PlayClipAtPoint(clip, transform.position);
        yield return new WaitForEndOfFrame();
    }

    protected override IEnumerator PressNoise() {
        AudioClip clip = Resources.Load("Effects/Huu 0" + Random.Range(1, 4)) as AudioClip;
        AudioSource.PlayClipAtPoint(clip, transform.position);
        yield return new WaitForEndOfFrame();
    }
}
