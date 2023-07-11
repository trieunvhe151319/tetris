using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    I, J, L, O, S, T, Z //�?nh ngh?a c�c h?nh kh?i Tetris bao g?m: I, J, L, O, S, T, Z
}

[System.Serializable]
public struct TetrominoData
{

    public Tile tile; //�?i di?n cho lo?i g?ch ��?c s? d?ng �? t?o th�nh kh?i Tetris.
    public Tetromino tetromino;//gi� tr? c?a enum Tetromino �? �?i di?n cho lo?i h?nh kh?i.
    public Vector2Int[] cells { get; private set; } //m?t m?ng c�c �?i t�?ng Vector2Int �?i di?n cho t?a �? c?a c�c � trong kh?i Tetris.
    public Vector2Int[,] wallKicks { get; private set; }//m?t m?ng hai chi?u c�c �?i t�?ng Vector2Int, �?i di?n cho c�c v? tr� c?n thay �?i khi kh?i Tetris va ch?m v?i b?c t�?ng v� ph?i ��?c xoay �? �?t ��ng v? tr�.

    public void Initialize()
    {
        cells = Data.Cells[tetromino]; 
        wallKicks = Data.WallKicks[tetromino];
    }
    //Ph��ng th?c Initialize() ��?c s? d?ng �? kh?i t?o c�c gi� tr?
    //cho c�c th�nh ph?n cells v� wallKicks d?a tr�n gi� tr? tetromino ��?c cung c?p.

}
