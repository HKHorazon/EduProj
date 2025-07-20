using NUnit.Framework;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class OneAnswer
{
    public string answerText;

    [Multiline] public string answerDesc;
}

public class GameMap : SerializedMonoBehaviour
{

    [SerializeField, ReadOnly] public List<Push> AllBoxes { get; private set; }

    public Grid grid;
    public Tilemap tilemap;
    public Vector3 Wposition = new Vector3(0, 0, 0);

    //AnswerText And Descriptions
    [SerializeField] public List<OneAnswer> answerList;

    [ShowInInspector] public Dictionary<int, char> characterTable = new Dictionary<int, char>();
    [ShowInInspector, ReadOnly] public Dictionary<char, int> antiCharacterTable = new Dictionary<char, int>();

    [NonSerialized] public Dictionary<int, List<Push>> boxTableByCharacters = new Dictionary<int, List<Push>>();

    public void Init()
    {
        AllBoxes = AllBoxes ?? new List<Push>();
        ComputeCharacterTable();
    }

    public void ComputeCharacterTable()
    {
        antiCharacterTable.Clear();
        foreach (var pair in characterTable)
        {
            antiCharacterTable[pair.Value] = pair.Key;
        }

        boxTableByCharacters.Clear();
        foreach (Push box in AllBoxes)
        {
            if (!boxTableByCharacters.ContainsKey(box.ID))
            {
                boxTableByCharacters[box.ID] = new List<Push>();
            }
            boxTableByCharacters[box.ID].Add(box);
        }
    }

    public bool block(Vector3 n_direction)
    {
        Vector3Int cellposition = tilemap.WorldToCell(n_direction);

        Tile tileM = tilemap.GetTile<Tile>(cellposition);


        if (tileM == null) { return true; }


        if (tileM.name.StartsWith(GameManager.WALL_TILE_NAME))
        {
            //Debug.Log(tileM.name);
            return false;
        }

        return true;
    }

    public void SetAllBoxes(List<Push> allBoxes)
    {
        this.AllBoxes = allBoxes;
        AllBoxes = AllBoxes ?? new List<Push>();
    }

    public List<int> getIDFromOneAnswer(OneAnswer answer)
    {
        List<int> ids = new List<int>();
        foreach (char ch in answer.answerText)
        {
            if (antiCharacterTable.ContainsKey(ch))
            {
                ids.Add(antiCharacterTable[ch]);
            }
            else
            {
                Debug.LogWarning($"Character '{ch}' not found in antiCharacterTable.");
            }
        }
        return ids;
    }

    public Vector3Int GetCellPos(Push box)
    {
        Vector3Int boxCell = tilemap.WorldToCell(box.transform.position);
        return boxCell;
    }

    public EDirection GetDirection(Push from, Push to)
    {
        Vector3Int fromCell = tilemap.WorldToCell(from.transform.position);
        Vector3Int toCell = tilemap.WorldToCell(to.transform.position);
        

        int xDiffer = toCell.x - fromCell.x;
        int yDiffer = toCell.y - fromCell.y; 

        if(xDiffer == 1 && yDiffer == 0)
        {
            return EDirection.Right;
        }
        else if (xDiffer == -1 && yDiffer == 0)
        {
            return EDirection.Left;
        }
        else if (xDiffer == 0 && yDiffer == 1)
        {
            return EDirection.Up;
        }
        else if (xDiffer == 0 && yDiffer == -1)
        {
            return EDirection.Down;
        }
        return EDirection.NotNeghibor;
    }


}
