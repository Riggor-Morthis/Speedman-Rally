using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WFCTile
{
    public byte colorCode { get; private set; }
    public List<TileNeighbor> northNeig { get; private set; }
    public List<TileNeighbor> southNeig { get; private set; }
    public List<TileNeighbor> eastNeig { get; private set; }
    public List<TileNeighbor> westNeig { get; private set; }

    public WFCTile(byte cc)
    {
        colorCode = cc;
        northNeig = new();
        southNeig = new();
        eastNeig = new();
        westNeig = new();
    }

    public void UpdateNeighbor(List<TileNeighbor> neigh, byte newCC)
    {
        if (newCC != 0)
        {
            //Personne on ajoute juste
            if (neigh.Count == 0) neigh.Add(new TileNeighbor(newCC));
            //Quelqu'un
            else
            {
                int currentIndex = 0;
                while (currentIndex < neigh.Count && neigh[currentIndex].colorCode != newCC) currentIndex++;
                //Si jamais present on rajoute
                if (currentIndex == neigh.Count) neigh.Add(new TileNeighbor(newCC));
                //Sinon, update
                else neigh[currentIndex].UpdateProbability();
            }
        }
    }

    public void UpdateProbabilites()
    {
        float neighTotProbs = 0;
        foreach (TileNeighbor neigh in northNeig) neighTotProbs += neigh.probability;
        foreach (TileNeighbor neigh in northNeig) neigh.UpdateProbability(neighTotProbs);

        neighTotProbs = 0;
        foreach (TileNeighbor neigh in southNeig) neighTotProbs += neigh.probability;
        foreach (TileNeighbor neigh in southNeig) neigh.UpdateProbability(neighTotProbs);

        neighTotProbs = 0;
        foreach (TileNeighbor neigh in eastNeig) neighTotProbs += neigh.probability;
        foreach (TileNeighbor neigh in eastNeig) neigh.UpdateProbability(neighTotProbs);

        neighTotProbs = 0;
        foreach (TileNeighbor neigh in westNeig) neighTotProbs += neigh.probability;
        foreach (TileNeighbor neigh in westNeig) neigh.UpdateProbability(neighTotProbs);
    }
}

public class TileNeighbor
{
    public byte colorCode { get; private set; }
    public float probability { get; private set; }

    public TileNeighbor(byte cc)
    {
        colorCode = cc;
        probability = 1;
    }

    public void UpdateProbability() => probability++;

    public void UpdateProbability(float tot) => probability /= tot;
}

public class OutputTile
{
    public bool calculated { get; private set; }
    public byte chosenColor { get; private set; }
    public float entropy { get; private set; }

    private List<PossibleColor> possibleColors;

    public OutputTile(List<WFCTile> possibleTiles)
    {
        possibleColors = new();
        foreach (WFCTile possibleTile in possibleTiles)
            possibleColors.Add(new PossibleColor(possibleTile.colorCode));
        calculated = false;
        entropy = 0;
    }

    public void UpdateProbabilities(List<TileNeighbor> neighbors)
    {
        //Commence par maj probas
        foreach (TileNeighbor neighbor in neighbors)
        {
            foreach (PossibleColor possibleColor in possibleColors)
            {
                if (possibleColor.colorCode == neighbor.colorCode)
                    possibleColor.UpdateProbability(neighbor.probability);
            }
        }

        //Ensuite entropie
        entropy = 0;
        foreach (PossibleColor possibleColor in possibleColors)
            if (possibleColor.probability > entropy) entropy = possibleColor.probability;
    }

    public void SetChosenColor(byte cc)
    {
        chosenColor = cc;
        calculated = true;
    }

    public byte ChooseAColor()
    {
        if (entropy == 0)
        {
            chosenColor = 0;
            calculated = true;
        }
        else
        {
            float lottery = 1;
            while (lottery == 1f) lottery = Random.Range(0f, 1f);
            int index = 0;

            while (!calculated)
            {
                if (possibleColors[index].probability > lottery)
                {
                    chosenColor = possibleColors[index].colorCode;
                    calculated = true;
                }
                else lottery -= possibleColors[index].probability;

                index++;
            }
        }

        return chosenColor;
    }
}

public class PossibleColor
{
    public byte colorCode { get; private set; }
    public float probability { get; private set; }
    private int pass;

    public PossibleColor(byte cc)
    {
        colorCode = cc;
        probability = 0f;
        pass = 0;
    }

    /// <summary>
    /// Met a jour la probabilite d'une couleur, proba restant une moyenne
    /// </summary>
    public void UpdateProbability(float prob)
    {
        probability *= pass;
        pass++;
        probability += prob;
        probability /= pass;
    }
}

public static class WaveFunctionCollapse
{
    #region Variables
    /// <summary>Texture d'entree</summary>
    private static Texture2D input;

    /// <summary>Bordures pour sampling texture entree</summary>
    private static int maxX, maxY;

    /// <summary>List de toutes tuiles possibles</summary>
    private static List<WFCTile> possibleTiles;

    /// <summary>Les tuiles qu'on sortira a la fin</summary>
    private static OutputTile[,] outputTiles;
    private static Vector2Int startCoordinates;
    #endregion

    #region PublicMethods
    public static Texture2D StartTheWave(Texture2D ipt)
    {
        input = ipt;

        GetTileList();
        CreateOutputTrack();
        return CreateOutputTexture();
    }
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Recupere la liste de toutes les tuiles possibles, avec les probabilites associees
    /// </summary>
    private static void GetTileList()
    {
        int currentTileListIndex;
        byte currentColorCode;
        WFCTile currentTile;

        //On trouve les dimensions
        maxX = input.width / 2;
        maxY = input.height / 2;

        //On trouve les tuiles possibles
        possibleTiles = new();
        //init
        currentColorCode = ExtractColorCode(0, 0);
        possibleTiles.Add(new WFCTile(currentColorCode));
        //boucle
        for (int x = 0; x < maxX; x++) for (int y = 0; y < maxY; y++)
            {
                //Recup tile actuelle
                currentColorCode = ExtractColorCode(x, y);

                //Ajout si necessaire
                currentTileListIndex = 0;
                while (currentTileListIndex < possibleTiles.Count &&
                    possibleTiles[currentTileListIndex].colorCode != currentColorCode) currentTileListIndex++;
                if (currentTileListIndex == possibleTiles.Count) possibleTiles.Add(new WFCTile(currentColorCode));

                //Maj des voisins
                currentTile = possibleTiles[currentTileListIndex];
                if (y + 1 < maxY) currentTile.UpdateNeighbor(currentTile.northNeig, ExtractColorCode(x, y + 1));
                if (y - 1 >= 0) currentTile.UpdateNeighbor(currentTile.southNeig, ExtractColorCode(x, y - 1));
                if (x + 1 < maxX) currentTile.UpdateNeighbor(currentTile.eastNeig, ExtractColorCode(x + 1, y));
                if (x - 1 >= 0) currentTile.UpdateNeighbor(currentTile.westNeig, ExtractColorCode(x - 1, y));
            }

        //Mise a jour des probabilites de nos tuiles
        foreach (WFCTile tile in possibleTiles) tile.UpdateProbabilites();
    }

    /// <summary>
    /// Permet de connaitre le code couleur d'un endroit precis de l'input
    /// </summary>
    private static byte ExtractColorCode(int x, int y)
    {
        return (byte)((input.GetPixel(x * 2, y * 2).r > 0f ? 8 : 0) +
            (input.GetPixel(x * 2 + 1, y * 2).r > 0f ? 4 : 0) +
            (input.GetPixel(x * 2, y * 2 + 1).r > 0f ? 2 : 0) +
            (input.GetPixel(x * 2 + 1, y * 2 + 1).r > 0f ? 1 : 0));
    }

    /// <summary>
    /// Permet de creer la liste des tuiles qui composeront l'image
    /// </summary>
    private static void CreateOutputTrack()
    {
        int currentX, currentY;
        float currentEntropy;
        byte currentColorCode;
        bool remaining = true;
        int checkX, checkY;

        //On cree une liste de la taille de l'image de depart
        outputTiles = new OutputTile[maxX, maxY];
        for (int i = 0; i < maxX; i++) for (int j = 0; j < maxY; j++)
            {
                outputTiles[i, j] = new OutputTile(possibleTiles);
            }

        //On retrouve la ligne de depart
        for (int i = 0; i < input.width; i++) for (int j = 0; j < input.height; j++)
            {
                if (input.GetPixel(i, j).r == 1f) startCoordinates = new Vector2Int(i, j);
            }

        //On fixe la tuile de depart a coup sur
        currentX = Mathf.FloorToInt(startCoordinates.x / 2f);
        currentY = Mathf.FloorToInt(startCoordinates.y / 2f);
        currentColorCode = ExtractColorCode(currentX, currentY);
        outputTiles[currentX, currentY].SetChosenColor(currentColorCode);
        //Update du coup
        UpdateTileNeighbors(currentColorCode, currentX, currentY);

        //Enfin, boucle pour creer toutes les tuiles
        while (remaining)
        {
            //Est-ce qu'il reste des tuiles a traiter ?
            remaining = false;
            checkX = 0;
            checkY = 0;
            while (!remaining && checkX < maxX)
            {
                if (!outputTiles[checkX, checkY].calculated) remaining = true;
                checkY++;
                if (checkY == maxY)
                {
                    checkY = 0;
                    checkX++;
                }
            }

            //Si on a des tuiles...
            if (remaining)
            {
                //On commence par trouver la tuile d'entropy maximale
                currentEntropy = 0;
                for (int i = 0; i < maxX; i++) for (int j = 0; j < maxY; j++)
                    {
                        //Si on a une entropie plus forte on va l'utiliser
                        if (!outputTiles[i, j].calculated && currentEntropy < outputTiles[i, j].entropy)
                        {
                            currentEntropy = outputTiles[i, j].entropy;
                            currentX = i;
                            currentY = j;
                        }
                    }

                //On lui dit de determiner sa couleur
                currentColorCode = outputTiles[currentX, currentY].ChooseAColor();
                //Update des voisins
                UpdateTileNeighbors(currentColorCode, currentX, currentY);
            }
        }
    }

    /// <summary>
    /// Met a jour les voisins d'une tuile donnee
    /// </summary>
    /// <param name="cc"></param>
    private static void UpdateTileNeighbors(byte cc, int currentX, int currentY)
    {
        WFCTile currentTile = GetTileByColor(cc);
        if (currentY + 1 < maxY) outputTiles[currentX, currentY + 1].UpdateProbabilities(currentTile.northNeig);
        if (currentY - 1 >= 0) outputTiles[currentX, currentY - 1].UpdateProbabilities(currentTile.southNeig);
        if (currentX + 1 < maxX) outputTiles[currentX + 1, currentY].UpdateProbabilities(currentTile.eastNeig);
        if (currentX - 1 >= 0) outputTiles[currentX - 1, currentY].UpdateProbabilities(currentTile.westNeig);
    }

    /// <summary>
    /// Trouve tuile avec CC correspondant
    /// </summary>
    private static WFCTile GetTileByColor(byte cc)
    {
        foreach (WFCTile tile in possibleTiles)
            if (tile.colorCode == cc) return tile;
        return null;
    }

    private static Texture2D CreateOutputTexture()
    {
        Texture2D firstDraft = new Texture2D(input.width, input.height);
        byte currentColor;
        Color trackColor = new Color(191f / 255f, 0f, 0f), offColor = new Color(0, 0, 0);

        //On colore chaque pixel en fonction du code couleur de la tuile
        for (int i = 0; i < maxX; i++) for (int j = 0; j < maxY; j++)
            {
                currentColor = outputTiles[i, j].chosenColor;
                if (currentColor >= 8)
                {
                    firstDraft.SetPixel(i * 2, j * 2, trackColor);
                    currentColor -= 8;
                }
                else firstDraft.SetPixel(i * 2, j * 2, offColor);

                if (currentColor >= 4)
                {
                    firstDraft.SetPixel(i * 2 + 1, j * 2, trackColor);
                    currentColor -= 4;
                }
                else firstDraft.SetPixel(i * 2 + 1, j * 2, offColor);

                if (currentColor >= 2)
                {
                    firstDraft.SetPixel(i * 2, j * 2 + 1, trackColor);
                    currentColor -= 2;
                }
                else firstDraft.SetPixel(i * 2, j * 2 + 1, offColor);

                if (currentColor >= 1) firstDraft.SetPixel(i * 2 + 1, j * 2 + 1, trackColor);
                else firstDraft.SetPixel(i * 2 + 1, j * 2 + 1, offColor);
            }
        //Fin du brouillon
        firstDraft.Apply();

        //On relie les tuiles avec 2 voisins opposes
        Texture2D output = firstDraft;
        for (int i = 1; i < firstDraft.width - 1; i++) for (int j = 1; j < firstDraft.height - 1; j++)
            {
                if (firstDraft.GetPixel(i + 1, j).r > 0 && firstDraft.GetPixel(i - 1, j).r > 0)
                {
                    if (firstDraft.GetPixel(i, j + 1).r == 0 && firstDraft.GetPixel(i, j - 1).r == 0)
                        output.SetPixel(i, j, trackColor);
                }
                else if (firstDraft.GetPixel(i, j + 1).r > 0 && firstDraft.GetPixel(i, j - 1).r > 0)
                {
                    if (firstDraft.GetPixel(i + 1, j).r == 0 && firstDraft.GetPixel(i - 1, j).r == 0)
                        output.SetPixel(i, j, trackColor);
                }
            }

        //On place la ligne et on envoie le tout
        firstDraft.SetPixel(startCoordinates.x, startCoordinates.y, new Color(1, 0, 0));
        output.Apply();

        byte[] bytes = output.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/wfc.png", bytes);
        Debug.Log(Application.dataPath + "/wfc.png");
        return output;
    }
    #endregion
}
