using UnityEngine;

public class ParticleGenerator : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("La liste des particules possibles" +
        "a l'ecran")]
    private Particle[] particles = new Particle[8];

    private int currentIndex = 0;

    private PlayerMovement movement;
    private Vector3 particleDirection;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        GatherVariables();
    }

    private void Update()
    {
        if (Random.Range(0f, 12f) < movement.GetSpeedRatio()) SpawnParticle();
    }
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Recupere les variables utiles
    /// </summary>
    private void GatherVariables()
    {
        movement = GetComponentInParent<PlayerMovement>();

        //On "eteint" toutes les particules
        foreach (Particle part in particles) part.gameObject.SetActive(false);
    }

    /// <summary>
    /// Permet de lancer une particule
    /// </summary>
    private void SpawnParticle()
    {
        //On cree la direction de la particule a partir de
        //la direction du vehicule
        particleDirection = new Vector3(Random.Range(-1f, 1f) *
            (1 - Mathf.Abs(movement.GetDirection().x)) - movement.GetDirection().x,
            0f,
            Random.Range(-1f, 1f) *
            (1 - Mathf.Abs(movement.GetDirection().z)) - movement.GetDirection().z);
        particleDirection.Normalize();

        //On instancie une nouvelle particule
        particles[currentIndex].gameObject.SetActive(true);
        particles[currentIndex].Initialize(transform.position, particleDirection);

        //On bouge l'index
        currentIndex++;
        if (currentIndex == particles.Length) currentIndex = 0;
    }
    #endregion
}
