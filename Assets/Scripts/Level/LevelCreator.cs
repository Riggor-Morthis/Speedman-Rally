using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject player;

    [Space]

    [SerializeField]
    private GameObject[] blokz;

    private string mapPath = "Map/Forest1";
    private Texture2D mapTexture;

    private Vector2Int startPosition;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        GetTilePath();
    }
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Permet de charger l'image, puis de lancer la construction de notre niveau
    /// </summary>
    private void GetTilePath()
    {
        //Recup map
        mapTexture = Resources.Load<Texture2D>(mapPath);

        //Cherche depart
        for (int i = 0; i < mapTexture.width; i++)
            for (int j = 0; j < mapTexture.height; j++)
                if (mapTexture.GetPixel(i, j) == new Color(255f / 255f, 0, 0)) startPosition = new Vector2Int(i, j);

        //On retrace le chemin le plus long
        BuildTiles(GetTrackPath(startPosition, new List<Vector2Int>()));
    }

    /// <summary>
    /// Recursif
    /// Trouve le chemin le plus long avec nos tuiles valides
    /// </summary>
    private List<Vector2Int> GetTrackPath(Vector2Int cursor, List<Vector2Int> currentList)
    {
        //Pour les comparaisons
        List<Vector2Int> tempList;

        //On se rajoute
        currentList.Add(cursor);

        //On cherche le chemin le plus long
        if (cursor.x - 1 >= 0 &&
            mapTexture.GetPixel(cursor.x - 1, cursor.y).r > 0 &&
            !currentList.Contains(new Vector2Int(cursor.x - 1, cursor.y)))
        {
            tempList = GetTrackPath(new Vector2Int(cursor.x - 1, cursor.y), currentList);
            if (tempList.Count > currentList.Count) currentList = tempList;
        }
        else if (cursor.x + 1 < mapTexture.width &&
            mapTexture.GetPixel(cursor.x + 1, cursor.y).r > 0 &&
            !currentList.Contains(new Vector2Int(cursor.x + 1, cursor.y)))
        {
            tempList = GetTrackPath(new Vector2Int(cursor.x + 1, cursor.y), currentList);
            if (tempList.Count > currentList.Count) currentList = tempList;
        }
        else if (cursor.y - 1 >= 0 &&
            mapTexture.GetPixel(cursor.x, cursor.y - 1).r > 0 &&
            !currentList.Contains(new Vector2Int(cursor.x, cursor.y - 1)))
        {
            tempList = GetTrackPath(new Vector2Int(cursor.x, cursor.y - 1), currentList);
            if (tempList.Count > currentList.Count) currentList = tempList;
        }
        else if (cursor.y + 1 < mapTexture.height &&
            mapTexture.GetPixel(cursor.x, cursor.y + 1).r > 0 &&
            !currentList.Contains(new Vector2Int(cursor.x, cursor.y + 1)))
        {
            tempList = GetTrackPath(new Vector2Int(cursor.x, cursor.y + 1), currentList);
            if (tempList.Count > currentList.Count) currentList = tempList;
        }
        //Pas de voisin valide ? Cas d'arret
        else return currentList;

        //On remonte recursif
        return currentList;
    }

    /// <summary>
    /// Creer un format de niveau pour lecture ulterieure
    /// </summary>
    private void BuildTiles(List<Vector2Int> trackPath)
    {
        //On cherche dimensions optimales
        int minXDimension = trackPath[0].x, maxXDimension = trackPath[0].x;
        int minYDimension = trackPath[0].y, maxYDimension = trackPath[0].y;
        for (int i = 1; i < trackPath.Count; i++)
        {
            if (trackPath[i].x < minXDimension) minXDimension = trackPath[i].x;
            else if (trackPath[i].x > maxXDimension) maxXDimension = trackPath[i].x;
            if (trackPath[i].y < minYDimension) minYDimension = trackPath[i].y;
            else if (trackPath[i].y > maxYDimension) maxYDimension = trackPath[i].y;
        }

        //On instancie niveau
        short[,] level = new short[maxXDimension - minXDimension + 3, maxYDimension - minYDimension + 3];
        startPosition = new Vector2Int(startPosition.x - minXDimension + 1,
            startPosition.y - minYDimension + 1);

        //On retrace le chemin en reecrivant le trackPath
        for (int i = 0; i < trackPath.Count; i++)
        {
            trackPath[i] = new Vector2Int(trackPath[i].x -
                minXDimension + 1, trackPath[i].y - minYDimension + 1);
            if (i == 0 || i == trackPath.Count - 1) level[trackPath[i].x, trackPath[i].y] = 2;
            else level[trackPath[i].x, trackPath[i].y] = 3;
        }

        //On rajoute des arbres
        bool neighbor, shortcut;
        for (int i = 0; i < level.GetLength(0); i++)
            for (int j = 0; j < level.GetLength(1); j++)
                if (level[i, j] == 0)
                {
                    if (i > 0 && i < level.GetLength(0) - 1 && j > 0 && j < level.GetLength(1) - 1)
                    {
                        neighbor = shortcut = false;
                        //On rajoute des bordures au bord de la route, mais pas la ou il y a des raccourcis
                        if (level[i - 1, j - 1] > 1)
                        {
                            neighbor = true;
                            if (level[i + 1, j + 1] > 1) shortcut = true;
                        }
                        if (level[i - 1, j] > 1)
                        {
                            neighbor = true;
                            if (level[i + 1, j] > 1) shortcut = true;
                        }
                        if (level[i - 1, j + 1] > 1)
                        {
                            neighbor = true;
                            if (level[i + 1, j - 1] > 1) shortcut = true;
                        }
                        if (level[i - 1, j] > 1)
                        {
                            neighbor = true;
                            if (level[i + 1, j] > 1) shortcut = true;
                        }
                        if (level[i + 1, j] > 1)
                        {
                            neighbor = true;
                            if (level[i - 1, j] > 1) shortcut = true;
                        }
                        if (level[i + 1, j - 1] > 1)
                        {
                            neighbor = true;
                            if (level[i - 1, j + 1] > 1) shortcut = true;
                        }
                        if (level[i + 1, j] > 1)
                        {
                            neighbor = true;
                            if (level[i - 1, j] > 1) shortcut = true;
                        }
                        if (level[i + 1, j + 1] > 1)
                        {
                            neighbor = true;
                            if (level[i - 1, j - 1] > 1) shortcut = true;
                        }
                        //Si on est bon, on est bon
                        if (neighbor && !shortcut) level[i, j] = 1;
                    }
                }

        //On cree le niveau a proprement dit
        SpawnLevel(level, trackPath);
    }

    /// <summary>
    /// Utilise le tableau, pour creer les tuiles, puis le joueur
    /// </summary>
    private void SpawnLevel(short[,] level, List<Vector2Int> trackPath)
    {
        //Le joueur
        GameObject.Instantiate(player, new Vector3(startPosition.x * 6, 0, startPosition.y * 6 + 2),
            Quaternion.identity);

        //Les tuiles decoratives
        for (int i = 0; i < level.GetLength(0); i++)
            for (int j = 0; j < level.GetLength(1); j++) if (level[i, j] <= 1)
                    GameObject.Instantiate(blokz[level[i, j]],
                        new Vector3(i * 6, 0, j * 6), Quaternion.identity, transform);

        //Les tuiles du circuit
        GameObject.Instantiate(blokz[level[trackPath[0].x, trackPath[0].y]],
            new Vector3(trackPath[0].x * 6, 0, trackPath[0].y * 6), Quaternion.identity, transform);
        Vector2Int direction = new Vector2Int(0, 1);
        float currentAngle = 0;

        for (int i = 1; i < trackPath.Count - 1; i++)
        {
            //Changement de direction = virage !
            if (direction != trackPath[i + 1] - trackPath[i])
            {
                //On s'occupe d'abord de rajouter le virage
                currentAngle += Vector2.SignedAngle(direction, trackPath[i + 1] - trackPath[i]);
                level[trackPath[i].x, trackPath[i].y] = 4;

                //Les deux derniers enfants (ie, les deux blokz precedents)
                //doivent etre capables d'envoyer des signaux
                if (Vector2.SignedAngle(direction, trackPath[i + 1] - trackPath[i]) > 0)
                {
                    if (i > 1) transform.GetChild(transform.childCount - 1).
                        GetComponent<TileValueGiver>().AddUI(0);
                    if (i > 2) transform.GetChild(transform.childCount - 2).
                        GetComponent<TileValueGiver>().AddUI(0);
                }
                else
                {
                    if (i > 1) transform.GetChild(transform.childCount - 1).
                        GetComponent<TileValueGiver>().AddUI(1);
                    if (i > 2) transform.GetChild(transform.childCount - 2).
                        GetComponent<TileValueGiver>().AddUI(1);
                }

                //On finit par mettre la direction a jour
                direction = trackPath[i + 1] - trackPath[i];
            }

            GameObject.Instantiate(blokz[level[trackPath[i].x, trackPath[i].y]],
                new Vector3(trackPath[i].x * 6, 0, trackPath[i].y * 6), Quaternion.Euler(0, currentAngle, 0), transform);
        }
        GameObject.Instantiate(blokz[level[trackPath[trackPath.Count - 1].x, trackPath[trackPath.Count - 1].y]],
                new Vector3(trackPath[trackPath.Count - 1].x * 6, 0, trackPath[trackPath.Count - 1].y * 6), Quaternion.Euler(0, currentAngle, 0), transform);
    }
    #endregion
}
