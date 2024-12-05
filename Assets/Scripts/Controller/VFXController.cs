using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public ParticleSystem WaterEffect;
    public ParticleSystem GroundEffect;
    public ParticleSystem ImpactEffect;
    private ParticleSystem particleToPlay;
    public static VFXController Instance;
    private void Start()
    {
        Instance = this;
    }
    public void PlayEffect(Transform transform, int effectID)
    {
        switch (effectID)
        {
            case 1:
                particleToPlay = WaterEffect;
                WaterEffect.gameObject.transform.position = transform.position;
                break;
            case 2:
                particleToPlay = GroundEffect;
                GroundEffect.gameObject.transform.position = transform.position;
                break;
            case 3:
                particleToPlay = ImpactEffect;
                ImpactEffect.gameObject.transform.position = transform.position;
                ImpactEffect.gameObject.transform.rotation = transform.rotation;
                break;
        }
        particleToPlay.Play();
    }

    // Start is called before the first frame update
}
