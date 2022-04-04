using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeeParticlesController : MonoBehaviour
{
    public ParticleSystem PeeParticles;
    
    public void SetRateOverTime(float rate)
    {
        if(rate > 0)
        {
            PeeParticles.Play();
        }
        else
        {
            PeeParticles.Stop();
        }
        var emission = PeeParticles.emission;
        emission.rateOverTime = rate;
    }
}
