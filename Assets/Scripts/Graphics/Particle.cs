using UnityEngine;

public class Particle : MonoBehaviour
{
    #region Variables
    private float trueXPosition, trueZPosition;
    private float currentXPosition, currentZPosition;
    private Vector3 direction;
    private float speed = 8;
    private float lifeSpan;
    #endregion

    #region UnityMethods
    private void Update()
    {
        //Tant qu'on est en vie, on se bouge
        if (lifeSpan < 0) gameObject.SetActive(false);
        else
        {
            Trajectory();
            lifeSpan -= Time.deltaTime;
        }
    }
    #endregion

    #region PublicMethods
    /// <summary>
    /// Initialize nos variables
    /// </summary>
    /// <param name="truePosition">La ou on spawn</param>
    /// <param name="dir">La ou va</param>
    public void Initialize(Vector3 truePosition, Vector3 dir)
    {
        trueXPosition = truePosition.x;
        trueZPosition = truePosition.z;
        currentXPosition = currentZPosition = 0;
        direction = dir;
        lifeSpan = .6f;

        Trajectory();
    }
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Permet de bouger la particule selon la trajectoire donnee
    /// </summary>
    private void Trajectory()
    {
        trueXPosition += direction.x * speed * Time.deltaTime;
        trueZPosition += direction.z * speed * Time.deltaTime;

        if (currentXPosition != Mathf.RoundToInt(trueXPosition * 6f) ||
            currentZPosition != Mathf.RoundToInt(trueZPosition * 6f))
        {
            currentXPosition = Mathf.RoundToInt(trueXPosition * 6f);
            currentZPosition = Mathf.RoundToInt(trueZPosition * 6f);

            transform.position = new Vector3(currentXPosition / 6f,
                0f, currentZPosition / 6f);
        }
    }
    #endregion
}
