using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour
{
    [Tooltip("Particle to emit")]
    public ParticleSystem particleToEmit;
    public void EmitParticle(int amount)
    {
        particleToEmit.Emit(amount);
    }
}
