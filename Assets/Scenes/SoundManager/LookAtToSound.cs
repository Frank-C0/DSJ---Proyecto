using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtToSound : SoundListener
{
    // override OnSoundHeard method
    public override void OnSoundHeard(SoundData soundData, float intensity)
    {
        // call base method
        base.OnSoundHeard(soundData, intensity);

        // get the direction from the sound to the listener
        Vector3 direction = soundData.position - transform.position;

        // look at the sound
        transform.rotation = Quaternion.LookRotation(direction);
    }

}
