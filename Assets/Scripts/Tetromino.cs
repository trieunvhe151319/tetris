using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    I, J, L, O, S, T, Z //ð?nh ngh?a các h?nh kh?i Tetris bao g?m: I, J, L, O, S, T, Z
}

[System.Serializable]
public struct TetrominoData
{

    public Tile tile; //ð?i di?n cho lo?i g?ch ðý?c s? d?ng ð? t?o thành kh?i Tetris.
    public Tetromino tetromino;//giá tr? c?a enum Tetromino ð? ð?i di?n cho lo?i h?nh kh?i.
    public Vector2Int[] cells { get; private set; } //m?t m?ng các ð?i tý?ng Vector2Int ð?i di?n cho t?a ð? c?a các ô trong kh?i Tetris.
    public Vector2Int[,] wallKicks { get; private set; }//m?t m?ng hai chi?u các ð?i tý?ng Vector2Int, ð?i di?n cho các v? trí c?n thay ð?i khi kh?i Tetris va ch?m v?i b?c tý?ng và ph?i ðý?c xoay ð? ð?t ðúng v? trí.

    public void Initialize()
    {
        cells = Data.Cells[tetromino]; 
        wallKicks = Data.WallKicks[tetromino];
    }
    //Phýõng th?c Initialize() ðý?c s? d?ng ð? kh?i t?o các giá tr?
    //cho các thành ph?n cells và wallKicks d?a trên giá tr? tetromino ðý?c cung c?p.

}
