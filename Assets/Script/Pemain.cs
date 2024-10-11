using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pemain : MonoBehaviour
{
    [SerializeField]
    public string ID_Pemain;
    [SerializeField]
    private UrutanPemain urutanPemain;  // Referensi ke class UrutanPemain
    [SerializeField]
    private AturanPemain aturanPemain;  // Referensi ke class AturanPemain
    [SerializeField]
    private GameManager gameManager;  // Referensi ke class GameManager
    [SerializeField]
    public string Jurusan;

    //public GameObject pemainPodium; // GameObject podium pemain


    public GameObject pemainBiasa;  // Objek yang mewakili pemain saat biasa
    public GameObject pemainPodium; // Objek yang mewakili pemain saat di podium

    private IEnumerator delayaktifkandadusilver() 
    {
        aturanPemain.rollGold.NonAktifDadu();
        aturanPemain.rollPemain.NonAktifDadu();
        yield return new WaitForSeconds (2);
        aturanPemain.rollPemain.gameObject.SetActive(true);
        aturanPemain.rollGold.gameObject.SetActive(false);
        aturanPemain.rollGold.AktifDadu();
        aturanPemain.rollPemain.AktifDadu();
    }

    public void MainkanGiliran()
    {
        string pemainAktif = urutanPemain.DapatkanPemainAktif(); // Dapatkan ID pemain aktif
        Debug.Log("Pemain " + ID_Pemain + " Mulai Bermain");
        gameManager.PesanAtas.text = "Bermain :\n" + Jurusan;
        if (aturanPemain.isGoldDiceScheduled && pemainAktif == aturanPemain.pemainGoldTerakhir)
        {
            aturanPemain.rollPemain.gameObject.SetActive(false);  // Nonaktifkan dadu silver

            aturanPemain.rollGold.gameObject.SetActive(true);     // Aktifkan dadu gold
            AktifkanPemainPodium(); // Naikkan pemain ke podium
        }
        else
        {
            StartCoroutine(delayaktifkandadusilver());  // Pastikan dadu gold non-aktif
            AktifkanPemainBiasa(); // Turunkan pemain dari podium jika mereka sebelumnya di podium
        }
    }

    public void AktifkanPemainBiasa()
    {
        pemainBiasa.SetActive(true);  // Aktifkan objek pemain biasa
        pemainPodium.SetActive(false); // Nonaktifkan objek pemain podium
    }

    public void AktifkanPemainPodium()
    {
        pemainBiasa.SetActive(false);  // Nonaktifkan objek pemain biasa
        pemainPodium.SetActive(true);  // Aktifkan objek pemain podium
    }

}
