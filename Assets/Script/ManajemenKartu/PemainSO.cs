using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DatabasePemain", menuName = "Database/Pemain")]
public class PemainSO : ScriptableObject
{
    public List<DataPemain> PlayerList = new List<DataPemain>();
}

[System.Serializable]
public class DataPemain
{
    public string Nama; // Nama Pemain
    public string Jurusan; // Jurusan Pemain
    public string JenisKelamin; // Jenis Kelamin Pemain
}
