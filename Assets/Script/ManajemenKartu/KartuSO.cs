using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KartuDatabase", menuName = "Database/Kartu")]
public class KartuSO : ScriptableObject
{
    public List<Kartunya> KartuList;  // Daftar kartu dalam database
}

[System.Serializable]
public class Kartunya
{
    public int IDKartu;       // ID Kartu
    public string JurKartu;   // Jurusan yang sesuai dengan kartu
    public int NilaiKartu;    // Nilai dari kartu
    public Sprite GbrKartu;   // Gambar kartu
}
