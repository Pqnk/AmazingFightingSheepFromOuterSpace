using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    public ParticleSystem hitParticle;
    public ParticleSystem stunParticle;
    public ParticleSystem playerReadyParticle;
    public ParticleSystem spawnPlayerParticle;
    public ParticleSystem attackParticle;

    public void SpawnHitParticle(Vector3 position)
    {
        Instantiate(hitParticle, position, new Quaternion (0,0,0,0));
    }

    public void SpawnStunParticle(Vector3 position, Transform parent)
    {
        ParticleSystem stunP = Instantiate(stunParticle, position, new Quaternion(0, 0, 0, 0));
        stunP.transform.SetParent(parent);
    }

    public ParticleSystem SpawnPlayerReadyParticle(Vector3 position)
    {
        ParticleSystem playerReadyP = Instantiate(playerReadyParticle, position, new Quaternion(0, 0, 0, 0));
        return playerReadyP;
    }

    public void SpawnAttackParticle(Vector3 position, Transform parent)
    {
        ParticleSystem attackP = Instantiate(attackParticle, position, new Quaternion(0, 0, 0, 0));
        attackP.transform.SetParent(parent);
    }

}
