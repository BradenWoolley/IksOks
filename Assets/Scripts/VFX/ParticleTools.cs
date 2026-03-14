using UnityEngine;

public static class ParticleTools
{

    #region Methods

    public static void SetColor(ParticleSystem particleSystem, Color color)
    {
        if (particleSystem != null)
        {
            ParticleSystem.MainModule main = particleSystem.main;
            main.startColor = new ParticleSystem.MinMaxGradient(color);
        }
    }

    #endregion

}