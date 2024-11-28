using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShapesArray
{
    private GameObject[,] shapes = new GameObject[Constants.Rows, Constants.Columns];

    /// <summary>
    /// Indexer for accessing shapes array
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public GameObject this[int row, int column]
    {
        get
        {
            try
            {
                return shapes[row, column];
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        set
        {
            shapes[row, column] = value;
        }
    }

    /// <summary>
    /// Swaps the position of two GameObjects in the array
    /// </summary>
    /// <param name="g1"></param>
    /// <param name="g2"></param>
    public void Swap(GameObject g1, GameObject g2)
    {
        // Ensure the GameObjects are valid before proceeding
        if (g1 == null || g2 == null)
        {
            Debug.LogWarning("One or both GameObjects are null, unable to swap.");
            return;
        }

        backupG1 = g1;
        backupG2 = g2;

        var g1Shape = g1.GetComponent<Shape>();
        var g2Shape = g2.GetComponent<Shape>();

        if (g1Shape == null || g2Shape == null)
        {
            Debug.LogWarning("Shape component is missing from one of the GameObjects.");
            return;
        }

        // Get array indexes
        int g1Row = g1Shape.Row;
        int g1Column = g1Shape.Column;
        int g2Row = g2Shape.Row;
        int g2Column = g2Shape.Column;

        // Swap them in the array
        var temp = shapes[g1Row, g1Column];
        shapes[g1Row, g1Column] = shapes[g2Row, g2Column];
        shapes[g2Row, g2Column] = temp;

        // Swap their respective properties
        Shape.SwapColumnRow(g1Shape, g2Shape);
    }

    /// <summary>
    /// Undoes the swap operation
    /// </summary>
    public void UndoSwap()
    {
        if (backupG1 == null || backupG2 == null)
            throw new Exception("Backup is null");

        Swap(backupG1, backupG2);
    }

    private GameObject backupG1;
    private GameObject backupG2;

    /// <summary>
    /// Returns the matches found for a list of GameObjects
    /// </summary>
    /// <param name="gos"></param>
    /// <returns></returns>
    public IEnumerable<GameObject> GetMatches(IEnumerable<GameObject> gos)
    {
        List<GameObject> matches = new List<GameObject>();
        foreach (var go in gos)
        {
            if (go != null)
            {
                matches.AddRange(GetMatches(go).MatchedCandy);
            }
        }
        return matches.Distinct();
    }

    /// <summary>
    /// Returns the matches found for a single GameObject
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public MatchesInfo GetMatches(GameObject go)
    {
        MatchesInfo matchesInfo = new MatchesInfo();

        if (go == null)
        {
            Debug.LogWarning("The GameObject is null in GetMatches.");
            return matchesInfo;  // Return an empty result if the GameObject is null
        }

        var horizontalMatches = GetMatchesHorizontally(go);
        if (ContainsDestroyRowColumnBonus(horizontalMatches))
        {
            horizontalMatches = GetEntireRow(go);
            if (!BonusTypeUtilities.ContainsDestroyWholeRowColumn(matchesInfo.BonusesContained))
                matchesInfo.BonusesContained |= BonusType.DestroyWholeRowColumn;
        }
        matchesInfo.AddObjectRange(horizontalMatches.Where(item => item != null));  // Check null before adding

        var verticalMatches = GetMatchesVertically(go);
        if (ContainsDestroyRowColumnBonus(verticalMatches))
        {
            verticalMatches = GetEntireColumn(go);
            if (!BonusTypeUtilities.ContainsDestroyWholeRowColumn(matchesInfo.BonusesContained))
                matchesInfo.BonusesContained |= BonusType.DestroyWholeRowColumn;
        }
        matchesInfo.AddObjectRange(verticalMatches.Where(item => item != null));  // Check null before adding

        return matchesInfo;
    }

    private bool ContainsDestroyRowColumnBonus(IEnumerable<GameObject> matches)
    {
        if (matches.Count() >= Constants.MinimumMatches)
        {
            foreach (var go in matches)
            {
                if (go != null && BonusTypeUtilities.ContainsDestroyWholeRowColumn(go.GetComponent<Shape>().Bonus))
                    return true;
            }
        }
        return false;
    }

    private IEnumerable<GameObject> GetEntireRow(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        int row = go.GetComponent<Shape>().Row;
        for (int column = 0; column < Constants.Columns; column++)
        {
            if (shapes[row, column] != null)
                matches.Add(shapes[row, column]);
        }
        return matches;
    }

    private IEnumerable<GameObject> GetEntireColumn(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        int column = go.GetComponent<Shape>().Column;
        for (int row = 0; row < Constants.Rows; row++)
        {
            if (shapes[row, column] != null)
                matches.Add(shapes[row, column]);
        }
        return matches;
    }

    /// <summary>
    /// Searches horizontally for matches
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    private IEnumerable<GameObject> GetMatchesHorizontally(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        matches.Add(go);

        if (go == null) return matches;  // Check if GameObject is null

        var shape = go.GetComponent<Shape>();
        if (shape == null) return matches;  // Check if Shape component exists

        // Check left
        if (shape.Column != 0)
            for (int column = shape.Column - 1; column >= 0; column--)
            {
                var leftShape = shapes[shape.Row, column];
                if (leftShape != null && leftShape.GetComponent<Shape>().IsSameType(shape))
                {
                    matches.Add(leftShape);
                }
                else
                    break;
            }

        // Check right
        if (shape.Column != Constants.Columns - 1)
            for (int column = shape.Column + 1; column < Constants.Columns; column++)
            {
                var rightShape = shapes[shape.Row, column];
                if (rightShape != null && rightShape.GetComponent<Shape>().IsSameType(shape))
                {
                    matches.Add(rightShape);
                }
                else
                    break;
            }

        // We want more than three matches
        if (matches.Count < Constants.MinimumMatches)
            matches.Clear();

        return matches.Distinct();
    }

    /// <summary>
    /// Searches vertically for matches
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    private IEnumerable<GameObject> GetMatchesVertically(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        matches.Add(go);

        if (go == null) return matches;  // Check if GameObject is null

        var shape = go.GetComponent<Shape>();
        if (shape == null) return matches;  // Check if Shape component exists

        // Check bottom
        if (shape.Row != 0)
            for (int row = shape.Row - 1; row >= 0; row--)
            {
                var bottomShape = shapes[row, shape.Column];
                if (bottomShape != null && bottomShape.GetComponent<Shape>().IsSameType(shape))
                {
                    matches.Add(bottomShape);
                }
                else
                    break;
            }

        // Check top
        if (shape.Row != Constants.Rows - 1)
            for (int row = shape.Row + 1; row < Constants.Rows; row++)
            {
                var topShape = shapes[row, shape.Column];
                if (topShape != null && topShape.GetComponent<Shape>().IsSameType(shape))
                {
                    matches.Add(topShape);
                }
                else
                    break;
            }

        if (matches.Count < Constants.MinimumMatches)
            matches.Clear();

        return matches.Distinct();
    }

    /// <summary>
    /// Removes (sets as null) an item from the array
    /// </summary>
    /// <param name="item"></param>
    public void Remove(GameObject item)
    {
        if (item == null)
        {
            Debug.LogWarning("Attempted to remove a null item.");
            return;  // If item is null, do not proceed
        }

        var shape = item.GetComponent<Shape>();
        if (shape != null)
        {
            shapes[shape.Row, shape.Column] = null;
        }
        else
        {
            Debug.LogWarning("Shape component is null for the GameObject.");
        }
    }

    /// <summary>
    /// Collapses the array on the specific columns, after checking for empty items on them
    /// </summary>
    /// <param name="columns"></param>
    /// <returns>Info about the GameObjects that were moved</returns>
    public AlteredCandyInfo Collapse(IEnumerable<int> columns)
    {
        AlteredCandyInfo collapseInfo = new AlteredCandyInfo();

        // Check if columns is valid
        if (columns == null || !columns.Any()) return collapseInfo;

        foreach (var column in columns)
        {
            // Start from the bottom row
            for (int row = 0; row < Constants.Rows - 1; row++)
            {
                // If null item is found
                if (shapes[row, column] == null)
                {
                    // Search for the first non-null item
                    for (int row2 = row + 1; row2 < Constants.Rows; row2++)
                    {
                        // If a non-null item is found, bring it down (replace with null item)
                        if (shapes[row2, column] != null)
                        {
                            shapes[row, column] = shapes[row2, column];
                            shapes[row2, column] = null;

                            // Calculate the largest distance moved
                            if (row2 - row > collapseInfo.MaxDistance)
                                collapseInfo.MaxDistance = row2 - row;

                            // Assign new row and column (name doesn't change)
                            shapes[row, column].GetComponent<Shape>().Row = row;
                            shapes[row, column].GetComponent<Shape>().Column = column;

                            collapseInfo.AddCandy(shapes[row, column]);
                            break;
                        }
                    }
                }
            }
        }

        return collapseInfo;
    }

    /// <summary>
    /// Searches the specific column and returns info about null items
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    public IEnumerable<ShapeInfo> GetEmptyItemsOnColumn(int column)
    {
        List<ShapeInfo> emptyItems = new List<ShapeInfo>();
        for (int row = 0; row < Constants.Rows; row++)
        {
            if (shapes[row, column] == null)
                emptyItems.Add(new ShapeInfo() { Row = row, Column = column });
        }
        return emptyItems;
    }
}
