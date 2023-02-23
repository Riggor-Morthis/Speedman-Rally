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

    private string mapPath = "Map/Forest3";
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
        mapTexture = WaveFunctionCollapse.StartTheWave(Resources.Load<Texture2D>(mapPath));

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
        short[,] level = new short[maxXDimension - minXDimension + 7, maxYDimension - minYDimension + 7];
        startPosition = new Vector2Int(startPosition.x - minXDimension + 3,
            startPosition.y - minYDimension + 3);

        //On retrace le chemin en reecrivant le trackPath
        for (int i = 0; i < trackPath.Count; i++)
        {
            trackPath[i] = new Vector2Int(trackPath[i].x -
                minXDimension + 3, trackPath[i].y - minYDimension + 3);
            if (i == 0 || i == trackPath.Count - 1) level[trackPath[i].x, trackPath[i].y] = 2;
            else level[trackPath[i].x, trackPath[i].y] = 3;
        }

        //On rajoute de la verdure
        bool neighbor, shortcut;
        for (int i = 0; i < level.GetLength(0); i++)
            for (int j = 0; j < level.GetLength(1); j++)
                if (level[i, j] == 0)
                {
                    if (i > 2 && i < level.GetLength(0) - 3 && j > 2 && j < level.GetLength(1) - 3)
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
        SpawnPlayer();
        SpawnDecorations(level);
        SpawnTrack(level, trackPath);
    }

    /// <summary>
    /// Spawn specifiquement le joueur
    /// </summary>
    private void SpawnPlayer()
    {
        GameObject.Instantiate(player, new Vector3(startPosition.x * 6, 0, startPosition.y * 6 + 2),
            Quaternion.identity);
    }

    /// <summary>
    /// Spawn specifiquement les tuiles "inutiles"
    /// </summary>
    private void SpawnDecorations(short[,] level)
    {
        for (int i = 0; i < level.GetLength(0); i++)
            for (int j = 0; j < level.GetLength(1); j++) if (level[i, j] <= 1)
                    GameObject.Instantiate(blokz[level[i, j]],
                        new Vector3(i * 6, 0, j * 6), Quaternion.identity, transform);
    }

    /// <summary>
    /// Assure la creation du circuit a proprement parle
    /// </summary>
    private void SpawnTrack(short[,] level, List<Vector2Int> trackPath)
    {
        float turnAngle = 0;

        //Premiere tuile
        //Instanciation
        Vector2Int direction = trackPath[1] - trackPath[0];
        float currentAngle = Vector2.SignedAngle(direction, Vector2.up);
        SpawnTile(blokz[2], trackPath[0] * 6, currentAngle);

        //On boucle de 1 a n-1
        for (int i = 1; i < trackPath.Count - 1; i++)
        {
            //Changement de direction = virage !
            if (direction != trackPath[i + 1] - trackPath[i])
            {
                //MaJ variables
                turnAngle = Vector2.SignedAngle(direction, trackPath[i + 1] - trackPath[i]);
                currentAngle += turnAngle;
                direction = trackPath[i + 1] - trackPath[i];
                //On cree un virage de force
                level[trackPath[i].x, trackPath[i].y] = 4;
            }
            //Sinon, pas virage
            else turnAngle = 0;

            //Spawn tuile
            SpawnTile(blokz[level[trackPath[i].x, trackPath[i].y]], trackPath[i] * 6, currentAngle);

            //Si y avait un virage, MaJ UI
            if (turnAngle != 0)
            {
                if (turnAngle > 0) SetUpUI(0);
                else SetUpUI(1);
            }
        }

        //Derniere tuile
        SpawnTile(blokz[2], trackPath[trackPath.Count - 1] * 6, currentAngle);
    }

    /// <summary>
    /// Faire apparaitre une seul tuile
    /// </summary>
    private void SpawnTile(GameObject gameObject, Vector2Int pos, float angle)
    {
        GameObject.Instantiate(gameObject, new Vector3(pos.x, 0, pos.y), Quaternion.Euler(0, angle, 0), transform);
    }

    /// <summary>
    /// Change l'UI pour les tuiles concernees
    /// </summary>
    private void SetUpUI(int uiIndex)
    {
        TileValueGiver tvg;
        transform.GetChild(transform.childCount - 1).
                        GetComponent<TileValueGiver>().RemoveUI(uiIndex);
        if ((tvg = transform.GetChild(transform.childCount - 2).
                        GetComponent<TileValueGiver>()) != null) tvg.AddUI(uiIndex);
        if ((tvg = transform.GetChild(transform.childCount - 3).
                        GetComponent<TileValueGiver>()) != null) tvg.AddUI(uiIndex);
        if ((tvg = transform.GetChild(transform.childCount - 4).
                        GetComponent<TileValueGiver>()) != null) tvg.AddUI(uiIndex);
    }
    #endregion
}
