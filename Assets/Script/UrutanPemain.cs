using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrutanPemain : MonoBehaviour
{
    [SerializeField]
    public List<Pemain> ListPemain = new List<Pemain>();

    [SerializeField]
    public List<string> ListUrutanPemain = new List<string>();

    [SerializeField]
    private GameManager GameManager;

    public int PemainSelanjutnya = 0;

    private IEnumerator delaygiliranpemain()
    {
        GameManager.DisableRollDice();
        GameManager.DisableRollDiceP();
        yield return new WaitForSeconds(2);
        GameManager.PesanBawah.text = "";
        GameManager.EnableRollDice();
        GameManager.EnableRollDiceP();
        for (int i = 0; i < ListPemain.Count; i++)
        {

            Pemain pemain = ListPemain[i];
            if (pemain.ID_Pemain == ListUrutanPemain[PemainSelanjutnya])
            {

                pemain.gameObject.SetActive(true);

                pemain.MainkanGiliran(); // Memanggil fungsi MainkanGiliran
            }
            else
            {
                pemain.gameObject.SetActive(false);
            }
        }
    }


    // Method untuk mendapatkan ID pemain yang saat ini aktif
    public string DapatkanPemainAktif()
    {
        return ListUrutanPemain[PemainSelanjutnya];
    }

    // Method untuk mendapatkan indeks pemain yang saat ini aktif
    public int DapatkanIndeksPemainAktif()
    {
        return PemainSelanjutnya;
    }

    public void NextPemain()
    {
        GameManager.CheckKartuhabis();
        // Index giliran pemain harus ditambah
        if (PemainSelanjutnya >= ListUrutanPemain.Count - 1)
        {
            PemainSelanjutnya = 0;
        }
        else
        {
            PemainSelanjutnya++;
        }

        StartCoroutine(delaygiliranpemain());
        // Mengaktifkan pemain yang sesuai dan mematikan yang lainnya
        
    }
}
