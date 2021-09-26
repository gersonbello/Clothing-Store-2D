using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour
{
    #region Particles
    [Tooltip("Particle to emit")]
    public ParticleSystem particleToEmit;
    public void EmitParticle(int amount)
    {
        particleToEmit.Emit(amount);
    }
    #endregion
}
