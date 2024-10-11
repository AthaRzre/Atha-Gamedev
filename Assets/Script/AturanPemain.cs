using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AturanPemain : MonoBehaviour
{
    [SerializeField]
    public RollPemain1 rollPemain;  // Referensi ke class RollPemain1 (dadu silver)
    [SerializeField]
    public RollPodium1 rollGold;    // Referensi ke class RollPodium1 (dadu gold)
    [SerializeField]
    private UrutanPemain urutanPemain;  // Referensi ke class UrutanPemain

    [SerializeField]
    private GameManager gameManager;  // Referensi ke class GameManager
    [SerializeField]
    private Pemainnya[] players;  // Referensi ke objek pemain

    //[SerializeField]
    //public List<Pemain> ListPemain = new List<Pemain>();

    //public GameObject pemainBiasa;  // Objek yang mewakili pemain saat biasa
    //public GameObject pemainPodium; // Objek yang mewakili pemain saat di podium




    public string pemainGoldTerakhir = null; // Pemain yang terakhir mendapatkan dadu gold
    public bool isGoldDiceScheduled = false; // Flag untuk melacak apakah dadu gold akan digunakan


    public void PeriksaHasilDadu()
    {
        string pemainAktif = urutanPemain.DapatkanPemainAktif(); // Dapatkan ID pemain aktif

        if (isGoldDiceScheduled && pemainAktif == pemainGoldTerakhir)
        {
            // Jika dadu gold sudah dijadwalkan dan ini adalah giliran pemain yang memiliki dadu gold
            Debug.Log("Aktifkan dadu gold.");

            Debug.Log("Status dadu gold: " + rollGold.gameObject.activeSelf); // Cek apakah dadu gold aktif

            // Kocok dadu gold sebelum mengambil nilai
            //KocokDaduGold();

            int hasilGold = rollGold.DapatkanNilaiDadu(); // Ambil nilai dadu gold
            Debug.Log("Nilai dadu gold: " + hasilGold);

            LakukanAksiDenganDaduGold(hasilGold);
            isGoldDiceScheduled = false; // Reset flag setelah dadu gold digunakan
        }
        else
        {


            int hasilDadu = rollPemain.DapatkanNilaiDadu();  // Ambil nilai dadu silver

            // Logika untuk aksi berdasarkan hasil dadu silver
            if (hasilDadu == 0)
            {
                Debug.Log("Nilai dadu silver 1, skip giliran.");
                Pemain pemain = urutanPemain.ListPemain.Find(item => item.ID_Pemain == urutanPemain.ListUrutanPemain[urutanPemain.PemainSelanjutnya]);
                gameManager.PesanBawah.text = pemain.Jurusan + "<br>Tidak Mendapatkan Kartu/Skip";
                gameManager.PesanBawah.gameObject.SetActive(true);
                urutanPemain.NextPemain(); // Skip giliran
            }
            else if (hasilDadu == 1)
            {
                Debug.Log("Nilai dadu silver 2, dapatkan 1 kartu.");
                Pemain pemain = urutanPemain.ListPemain.Find(item => item.ID_Pemain == urutanPemain.ListUrutanPemain[urutanPemain.PemainSelanjutnya]);
                gameManager.PesanBawah.text = pemain.Jurusan + "<br>Mendapatkan 1 Kartu";
                gameManager.PesanBawah.gameObject.SetActive(true);
                DapatkanKartu();
                urutanPemain.NextPemain();
            }
            else if (hasilDadu == 2)
            {
                Debug.Log("Nilai dadu silver 3, dapatkan 2 kartu.");
                Pemain pemain = urutanPemain.ListPemain.Find(item => item.ID_Pemain == urutanPemain.ListUrutanPemain[urutanPemain.PemainSelanjutnya]);
                gameManager.PesanBawah.text = pemain.Jurusan + "<br>Mendapatkan 2 Kartu";
                gameManager.PesanBawah.gameObject.SetActive(true);
                DapatkanKartu();
                DapatkanKartu();
                urutanPemain.NextPemain();
            }
            else if (hasilDadu == 3)
            {
                Debug.Log("Nilai dadu silver 4, Mendapatkan 3 Kartu.");
                Pemain pemain = urutanPemain.ListPemain.Find(item => item.ID_Pemain == urutanPemain.ListUrutanPemain[urutanPemain.PemainSelanjutnya]);
                gameManager.PesanBawah.text = pemain.Jurusan + "\nMendapatkan 3 Kartu";
                gameManager.PesanBawah.gameObject.SetActive(true);
                DapatkanKartu();
                DapatkanKartu();
                DapatkanKartu();
                urutanPemain.NextPemain();
            }
            else if (hasilDadu == 4)
            {
                Debug.Log("Nilai dadu silver 5, buang kartu lawan.");
                Pemain pemain = urutanPemain.ListPemain.Find(item => item.ID_Pemain == urutanPemain.ListUrutanPemain[urutanPemain.PemainSelanjutnya]);
                gameManager.PesanBawah.text = pemain.Jurusan + "\nHarus Membuang 1 Kartu Lawan";
                gameManager.PesanBawah.gameObject.SetActive(true);
                gameManager.DisableRollDice();
                players[urutanPemain.PemainSelanjutnya].BuangKartuLawan();

                urutanPemain.NextPemain();
            }
            else if (hasilDadu == 5)
            {
                Debug.Log("Nilai dadu silver 6, naik podium, buang kartu lawan dan ambil kartu.");
                Pemain pemain = urutanPemain.ListPemain.Find(item => item.ID_Pemain == urutanPemain.ListUrutanPemain[urutanPemain.PemainSelanjutnya]);
                gameManager.PesanBawah.text = pemain.Jurusan + "\nNaik Podium<br>Membuang Kartu Lawan<br>Mendapat 1 Kartu";
                gameManager.PesanBawah.gameObject.SetActive(true);
                gameManager.DisableRollDice();
                players[urutanPemain.PemainSelanjutnya].BuangKartuLawan();

                DapatkanKartu();
                isGoldDiceScheduled = true;
                pemainGoldTerakhir = pemainAktif;

                // Aktifkan pemain podium
                Pemainnya pemainSaatIni = players[urutanPemain.DapatkanIndeksPemainAktif()];
                pemainSaatIni.AktifkanPemainPodium(); // Naikkan pemain ke podium
                urutanPemain.NextPemain();
                //gameManager.GantiKePemainPodium(pemain);  // Pindahkan pemain ke podium
                // Pindahkan pemain ke podium
            }

        }
        for (int i = 0; i < players.Length; i++)
        { //mematikan fungsi acak dadu saat ada kartu di dalam tangan pemain 
            if (players[i].additionalTableCards.Count == 0)
            {
                //Debug.Log("Meja tambahan kosong, giliran berpindah.");
                gameManager.EndTurn();
            }
            else
            {
                gameManager.DisableRollDice();  // Nonaktifkan fungsi roll dice di GameManager
                Debug.Log("Masih ada kartu di meja tambahan, pemain harus membuang kartu sebelum giliran berpindah.");
            }
        }

        for (int i = 0; i < players.Length; i++)
        { //mematikan fungsi acak dadu saat belum melaksanakan buang kartu
            if (players[i].additionalTableCards.Count == 0)
            {
                //Debug.Log("Meja tambahan kosong, giliran berpindah.");
                gameManager.EndTurn();
            }
            else
            {
                gameManager.DisableRollDice();  // Nonaktifkan fungsi roll dice di GameManager
                Debug.Log("Masih ada kartu di meja tambahan, pemain harus membuang kartu sebelum giliran berpindah.");
            }
        }


    }

    // Fungsi untuk mengocok dadu gold
    private void KocokDaduGold()
    {
        Debug.Log("Kocok dadu gold.");
        // Implementasikan logika kocok dadu gold di sini
        // Misalnya, Anda dapat memanggil metode pada RollPodium1 yang mengocok dadu
        rollGold.AcakDaduPemain();
    }

    private void LakukanAksiDenganDaduGold(int hasilGold)
    {
        if (hasilGold == 0)
        {
            Debug.Log("Nilai dadu gold 1, dapatkan 1 kartu.");
            Pemain pemain = urutanPemain.ListPemain.Find(item => item.ID_Pemain == urutanPemain.ListUrutanPemain[urutanPemain.PemainSelanjutnya]);
            gameManager.PesanBawah.text = pemain.Jurusan + "<br>Mendapatkan 1 Kartu";
            gameManager.PesanBawah.gameObject.SetActive(true);
            DapatkanKartu();

            urutanPemain.NextPemain();
        }
        else if (hasilGold == 1)
        {
            Debug.Log("Nilai dadu gold 2, dapatkan 2 kartu.");
            Pemain pemain = urutanPemain.ListPemain.Find(item => item.ID_Pemain == urutanPemain.ListUrutanPemain[urutanPemain.PemainSelanjutnya]);
            gameManager.PesanBawah.text = pemain.Jurusan + "<br>Mendapatkan 2 Kartu";
            gameManager.PesanBawah.gameObject.SetActive(true);
            DapatkanKartu();
            DapatkanKartu();
            urutanPemain.NextPemain();
        }
        else if (hasilGold == 2)
        {
            Debug.Log("Nilai dadu gold 3, dapatkan 3 kartu.");
            Pemain pemain = urutanPemain.ListPemain.Find(item => item.ID_Pemain == urutanPemain.ListUrutanPemain[urutanPemain.PemainSelanjutnya]);
            gameManager.PesanBawah.text = pemain.Jurusan + "<br>Mendapatkan 3 Kartu";
            gameManager.PesanBawah.gameObject.SetActive(true);
            DapatkanKartu();
            DapatkanKartu();
            DapatkanKartu();
            urutanPemain.NextPemain();
        }
        else if (hasilGold == 3)
        {
            Debug.Log("Nilai dadu gold 4, Dapatkan 3 kartu.");
            Pemain pemain = urutanPemain.ListPemain.Find(item => item.ID_Pemain == urutanPemain.ListUrutanPemain[urutanPemain.PemainSelanjutnya]);
            gameManager.PesanBawah.text = pemain.Jurusan + "<br>Mendapatkan 3 Kartu";
            gameManager.PesanBawah.gameObject.SetActive(true);
            DapatkanKartu();
            DapatkanKartu();
            DapatkanKartu();
            urutanPemain.NextPemain();
        }
        else if (hasilGold == 4)
        {
            Debug.Log("Nilai dadu gold 5, buang 1 kartu lawan.");
            Pemain pemain = urutanPemain.ListPemain.Find(item => item.ID_Pemain == urutanPemain.ListUrutanPemain[urutanPemain.PemainSelanjutnya]);
            gameManager.PesanBawah.text = pemain.Jurusan + "<br>Harus Membuang 1 Kartu Lawan";
            gameManager.PesanBawah.gameObject.SetActive(true);

            gameManager.DisableRollDiceP();
            players[urutanPemain.PemainSelanjutnya].BuangKartuLawan();

            gameManager.DisableRollDiceP();
            //players[urutanPemain.PemainSelanjutnya].BuangKartuLawan();

            urutanPemain.NextPemain();
        }
        else if (hasilGold == 5)
        {
            Debug.Log("Nilai dadu gold 6, buang 1 kartu lawan dan dapatkan 1 kartu.");
            Pemain pemain = urutanPemain.ListPemain.Find(item => item.ID_Pemain == urutanPemain.ListUrutanPemain[urutanPemain.PemainSelanjutnya]);
            gameManager.PesanBawah.text = pemain.Jurusan + "<br>Naik Podium\nMembuang Kartu Lawan\nMendapat 1 Kartu";
            gameManager.PesanBawah.gameObject.SetActive(true);

            gameManager.DisableRollDiceP();
            players[urutanPemain.PemainSelanjutnya].BuangKartuLawan();


            DapatkanKartu();
            urutanPemain.NextPemain();
        }
    }

    private void DapatkanKartu()
    {
        // Ambil indeks pemain aktif dari urutanPemain
        int indeksPemainAktif = urutanPemain.DapatkanIndeksPemainAktif();

        if (indeksPemainAktif >= 0 && indeksPemainAktif < players.Length)
        {
            // Dapatkan pemain yang sedang aktif dari array pemainya
            Pemainnya pemainSaatIni = players[indeksPemainAktif];

            if (pemainSaatIni != null)
            {
                // Panggil metode untuk menambahkan kartu ke pemain yang sedang aktif
                pemainSaatIni.TakeCardFromDeck(1);
            }
        }
    }


    private void TukarKartu()
    {
        Debug.Log("Tukar 1 kartu dengan pemain lain!");
        // Logika untuk menukar kartu dengan pemain lain
    }


}