using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadButton : LevelObject {
    public string launchBuild;

    public override ACTION PressedReaction(PlayerController player) {
        string path = Application.dataPath.Substring(0, Application.dataPath.Length - 11) + "Builds/" + launchBuild;
        Debug.Log(path);
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(path);
        StartCoroutine(PressNoise());
        return pressedReaction;
    }

    public override ACTION MoveReaction(PlayerController player) {
        StartCoroutine(MoveNoise());
        return ACTION.MOVE;
    }

    public override ACTION PressedReaction(Puppet puppet) {
        StartCoroutine(PressNoise());
        return pressedReaction;
    }

    public override ACTION MoveReaction(Puppet puppet) {
        StartCoroutine(MoveNoise());
        return ACTION.MOVE;
    }

    protected override IEnumerator MoveNoise() {
        AudioClip clip = Resources.Load("Effects/Bgooo 0" + Random.Range(1, 4)) as AudioClip;
        AudioSource.PlayClipAtPoint(clip, transform.position);
        yield return new WaitForEndOfFrame();
    }

    protected override IEnumerator PressNoise() {
        AudioClip clip = Resources.Load("Effects/Khuh 0" + Random.Range(1, 4)) as AudioClip;
        AudioSource.PlayClipAtPoint(clip, transform.position);
        yield return new WaitForEndOfFrame();
    }
}