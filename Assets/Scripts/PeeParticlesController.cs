using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeeParticlesController : MonoBehaviour
{
    public ParticleSystem PeeParticles;
    public ParticleSystem PeeParticles2;

    public void SetRateOverTime(float rate)
    {
        var particles = GameManager.Instance.Player.InAnimatedMode ? PeeParticles2 : PeeParticles;
        if (rate > 0)
        {
            particles.Play();
        }
        else
        {
            particles.Stop();
        }

        var emission = particles.emission;
        emission.rateOverTime = rate;
    }
}