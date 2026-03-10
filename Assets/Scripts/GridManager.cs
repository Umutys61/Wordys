using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int rows = 6;
    [SerializeField] private int columns = 5;
    [SerializeField] private int maxRows = 6;

    [Header("References")]
    [SerializeField] private LetterCell cellPrefab;
    [SerializeField] private Transform gridContainer;
    [SerializeField] private WordManager wordManager;
    [SerializeField] private KeyboardManager keyboardManager;
    [SerializeField] private GameManager gameManager;

    private List<LetterCell> cells = new List<LetterCell>();
    private int currentRow;
    private int currentColumn;
    public bool gameEnded;

    public int CurrentRow => currentRow;

    public void SetupGrid(int cols)
    {
        columns = cols;
        
        foreach(Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }
        cells.Clear();

        var gridLayout = gridContainer.GetComponent<UnityEngine.UI.GridLayoutGroup>();
        if (gridLayout != null)
        {
            gridLayout.constraintCount = columns;
            
            RectTransform rt = gridContainer.GetComponent<RectTransform>();
            float width = rt.rect.width;

            var layoutElement = gridContainer.GetComponent<UnityEngine.UI.LayoutElement>();
            if (layoutElement != null && layoutElement.preferredWidth > 0)
            {
                width = layoutElement.preferredWidth;
            }
            
            if (width <= 1)
            {
                Canvas.ForceUpdateCanvases();
                width = rt.rect.width;
                if (width <= 1) width = 840f;
            }

            if (columns > 5)
            {
                gridLayout.spacing = new Vector2(8, 8);
            }
            else
            {
                gridLayout.spacing = new Vector2(15, 15);
            }

            float spacing = gridLayout.spacing.x;
            float totalSpacing = (columns - 1) * spacing;
            float availableWidth = width - totalSpacing - gridLayout.padding.left - gridLayout.padding.right;
            float cellSize = availableWidth / columns;
            
            float maxCellSize = (columns <= 5) ? 145f : 150f; 
            cellSize = Mathf.Min(cellSize, maxCellSize); 
            
            gridLayout.cellSize = new Vector2(cellSize, cellSize);
            
            if (columns > 5)
            {
                gridLayout.childAlignment = TextAnchor.MiddleCenter;
            }
            else
            {
                gridLayout.childAlignment = TextAnchor.LowerCenter;
            }
        }

        for (int i = 0; i < rows * columns; i++)
        {
            var cell = Instantiate(cellPrefab, gridContainer);
            cells.Add(cell);
        }
        
        ClearGrid();
    }

    #region Input

    public void AddLetter(char letter)
    {
        if (gameEnded)
            return;

        while (currentColumn < columns)
        {
            int index = GetIndex(currentRow, currentColumn);
            if (!cells[index].IsLocked)
                break;
            
            currentColumn++;
        }

        if (currentColumn >= columns || currentRow >= rows)
            return;

        int targetIndex = GetIndex(currentRow, currentColumn);
        cells[targetIndex].SetLetter(letter);

        currentColumn++;

        while (currentColumn < columns)
        {
            int index = GetIndex(currentRow, currentColumn);
            if (!cells[index].IsLocked)
                break;
            
            currentColumn++;
        }
    }

    public void RemoveLetter()
    {
        if (gameEnded)
            return;

        int col = currentColumn - 1;
        while (col >= 0)
        {
            int index = GetIndex(currentRow, col);
            if (!cells[index].IsLocked)
                break;
            
            col--;
        }

        if (col < 0)
            return;

        int targetIndex = GetIndex(currentRow, col);
        cells[targetIndex].Clear();
        currentColumn = col;
    }


    public void SubmitRow()
    {
        if (gameEnded)
            return;

        if (currentColumn < columns)
            return;

        string guess = GetCurrentRowWord();

        if (!wordManager.IsValidWord(guess))
        {
            Debug.Log("GEÇERSİZ KELİME: " + guess);
            ShakeCurrentRow();
            return;
        }

        LetterState[] states = wordManager.CheckGuess(guess);

        bool isCorrect = true;

        for (int i = 0; i < columns; i++)
        {
            int index = GetIndex(currentRow, i);
            cells[index].SetState(states[i]);

            if (states[i] != LetterState.Correct)
                isCorrect = false;
        }

        keyboardManager.UpdateKeyStates(guess, states);

       if (isCorrect)
        {
            gameEnded = true;
            gameManager.OnWin();
            return;
        }


        currentRow++;
        currentColumn = 0;

            if (currentRow >= maxRows)
        {
            gameEnded = true;
            gameManager.OnLose();
        }

    }

    #endregion

    #region Helpers

    private void ShakeCurrentRow()
    {
        for (int i = 0; i < columns; i++)
        {
            int index = GetIndex(currentRow, i);
            cells[index].Shake();
        }
    }

    private int GetIndex(int row, int column)
    {
        return row * columns + column;
    }

    private string GetCurrentRowWord()
    {
        char[] letters = new char[columns];

        for (int i = 0; i < columns; i++)
        {
            int index = GetIndex(currentRow, i);
            letters[i] = cells[index].Letter;
        }

        return new string(letters);
    }

    private void ClearGrid()
    {
        currentRow = 0;
        currentColumn = 0;
        gameEnded = false;

        foreach (var cell in cells)
        {
            cell.Clear();
        }
    }


    public void ResetGrid()
{
    ClearGrid();
    keyboardManager.ResetKeyboard();
}
    public bool RevealRandomCorrectLetter()
    {
        if (gameEnded)
            return false;

        string correctWord = wordManager.TargetWord;

        if (string.IsNullOrEmpty(correctWord))
        {
            Debug.LogError("Target word is null or empty!");
            return false;
        }

        if (correctWord.Length != columns)
        {
            Debug.LogError($"WORD LENGTH MISMATCH! Target: {correctWord.Length}, Grid: {columns}");
            return false;
        }

        List<int> availableIndexes = new List<int>();

        for (int i = 0; i < columns; i++)
        {
            int index = GetIndex(currentRow, i);

            if (cells[index].IsEmpty() && !cells[index].IsLocked)
                availableIndexes.Add(i);
        }

        if (availableIndexes.Count == 0)
            return false;

        int randomColumn = availableIndexes[Random.Range(0, availableIndexes.Count)];
        int revealIndex = GetIndex(currentRow, randomColumn);

        char correctLetter = correctWord[randomColumn];

        cells[revealIndex].SetLetter(correctLetter);
        cells[revealIndex].SetState(LetterState.Correct);
        cells[revealIndex].LockCell();

        if (randomColumn == currentColumn)
        {
            while (currentColumn < columns)
            {
                int index = GetIndex(currentRow, currentColumn);
                if (!cells[index].IsLocked)
                    break;
                
                currentColumn++;
            }
        }

        return true;
    }

private void RecalculateCurrentColumn()
{
    int filled = 0;

    for (int i = 0; i < columns; i++)
    {
        int index = GetIndex(currentRow, i);

        if (!cells[index].IsEmpty())
            filled++;
    }

    currentColumn = filled;
}




    #endregion
}
