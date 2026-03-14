using UnityEngine;


/// <summary>
/// Plays particle effects along the strike line as it draws.
/// Attach to the same GameObject as StrikeLine.
/// - Trail: continuous particles along the line path (particle4 - small stars)
/// - Burst: one-shot explosion at the endpoint when line finishes (particle1 - bright star)
/// </summary>
public class StrikeEffect : MonoBehaviour
{

    #region Fields

    [Header("Particle Systems")]
    [SerializeField]
    private ParticleSystem burstEffect;  // particle1 — bright cross star

    [SerializeField]
    private ParticleSystem trailEffect;  // particle4 — small stars

    #endregion


    #region Methods

    /// <summary>
    /// Stop trail and trigger burst at the endpoint.
    /// Called by StrikeLine when animation completes.
    /// </summary>
    public void EndTrail(Vector3 endWorldPosition)
    {
        if (trailEffect != null)
        {
            trailEffect.Stop();
        }

        if (burstEffect != null)
        {
            burstEffect.transform.position = endWorldPosition;
            burstEffect.Play();
        }
    }

    public void Reset()
    {
        trailEffect?.Stop();
        trailEffect?.Clear();
        burstEffect?.Stop();
        burstEffect?.Clear();
    }

    public void SetColor(Color color)
    {
        ParticleTools.SetColor(trailEffect, color);
        ParticleTools.SetColor(burstEffect, color);
    }

    /// <summary>
    /// Start emitting trail particles. Called by StrikeLine when animation begins.
    /// </summary>
    public void StartTrail()
    {
        if (trailEffect != null)
        {
            trailEffect.Play();
        }
    }

    #endregion

}