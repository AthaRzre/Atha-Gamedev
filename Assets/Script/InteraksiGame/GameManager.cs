using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public RollPemain1 rollPemain;  // Referensi ke class RollPemain1 (dadu silver)
    [SerializeField]
    public RollPodium1 rollGold;    // Referensi ke class RollPodium1 (dadu gold)

    [SerializeField]
    public Pemainnya[] players;  // Referensi ke objek pemain

    [SerializeField]
    private UrutanPemain urutanPemain;  // Referensi ke class UrutanPemain

    [SerializeField]
    private PemainSO pemainDatabase;  // Referensi ke ScriptableObject PemainSO

    [SerializeField]
    private KartuSO kartuDatabase;  // Referensi ke ScriptableObject KartuSO

    [SerializeField]
    private TMP_Text sisaKartuText;  // TMP Text untuk menampilkan jumlah kartu yang tersisa di deck

    [SerializeField]
    public TMP_Text PesanAtas;  // TMP Text untuk menampilkan jumlah kartu yang tersisa di deck

    [SerializeField]
    public TMP_Text PesanBawah;  // TMP Text untuk menampilkan jumlah kartu yang tersisa di deck

    [SerializeField]
    private Image diceImage;  // Objek UI untuk menampilkan dadu

    [SerializeField]
    private Sprite[] diceSprites;  // Array Sprite untuk dadu (1-6)

    [SerializeField]
    private Button rollDiceButton;  // Referensi ke tombol Roll Dice

    [SerializeField]
    public GameObject popup;
    [SerializeField]
    private TMP_Text ScorePemenang;
    [SerializeField]
    private TMP_Text NamaPemenang;

    [SerializeField]
    public GameObject _canvas2d;

    [SerializeField]
    public GameObject _canvas3d;

    public Stack<Kartunya> deckStack = new Stack<Kartunya>();  // Stack untuk menyimpan kartu permainan
    public int currentPlayerIndex = 0;  // Indeks pemain yang saat ini mendapatkan giliran
    private bool isWaitingForDiscard = false;  // Menyimpan status apakah sedang menunggu pembuangan kartu
    private bool isDiceRollAllowed = true;  // Flag untuk mengontrol apakah dadu boleh diklik




    void Start()
    {
        AssignPlayerDataFromSO();   // Mengambil data dari PemainSO dan meng-assign ke pemain
        InitializeDeck();           // Inisialisasi deck kartu
        //DistribusiKartuKeMeja();  // Distribusi kartu modal ke meja pemain
        UpdateSisaKartuText();             // Perbarui jumlah sisa kartu setelah inisialisasi
        StartCoroutine(PesanAwaldanBagiKartuAwal());
        // Memulai giliran pemain pertama tanpa langsung memutar dadu
        //Debug.Log("Giliran pemain pertama: " + players[currentPlayerIndex].playerData.Jurusan);

    }

    // Coroutine untuk menampilkan dua pesan dengan jeda waktu, kemudian bagi kartu
    private IEnumerator PesanAwaldanBagiKartuAwal()
    {
        // Tampilkan pesan pertama
        PesanBawah.text = "Permainan Dimulai!";
        PesanBawah.gameObject.SetActive(true);  // Aktifkan teks agar terlihat

        // Tunggu 2 detik untuk pesan pertama
        yield return new WaitForSeconds(2f);

        // Sembunyikan pesan pertama
        PesanBawah.gameObject.SetActive(false);

        // Tampilkan pesan kedua
        PesanBawah.text = "Setiap Pemain Akan Mendapat 1 Kartu";
        PesanBawah.gameObject.SetActive(true);

        // Tunggu 2 detik untuk pesan kedua
        yield return new WaitForSeconds(2f);

        // Sembunyikan pesan kedua
        PesanBawah.gameObject.SetActive(false);

        // Lanjutkan ke fungsi untuk mengambil kartu
        DistribusiKartuKeMeja();

        // Tunggu 1 detik untuk pesan berikutnya
        yield return new WaitForSeconds(1f);

        Debug.Log("Giliran Pemain Pertama: " + players[currentPlayerIndex].playerData.Jurusan);
        PesanAtas.text = "Bermain :\n" + players[currentPlayerIndex].playerData.Jurusan;
        PesanAtas.gameObject.SetActive(true);

        urutanPemain.ListPemain[0].gameObject.SetActive(true);

        // Sembunyikan pesan 
        //PesanPesan.gameObject.SetActive(false);
    }


    // Fungsi yang dipanggil ketika dadu diklik
    public void OnDiceClick()
    {
        if (!isDiceRollAllowed)
        {
            Debug.Log("Pemain belum membuang kartu. Dadu tidak bisa diklik.");
            return;
        }

        Pemainnya currentPlayer = players[currentPlayerIndex];
        //currentPlayer.RollDice();  // Pemain mengocok dadu
    }

    public void DisableRollDice()
    {
        rollPemain.NonAktifDadu();
    }

    public void EnableRollDice()
    {
        rollPemain.AktifDadu();
    }

    public void DisableRollDiceP()
    {
        rollGold.NonAktifDadu();
    }

    public void EnableRollDiceP()
    {
        rollGold.AktifDadu();
    }


    void AssignPlayerDataFromSO()
    {
        for (int i = 0; i < players.Length && i < pemainDatabase.PlayerList.Count; i++)
        {
            players[i].SetPlayerData(pemainDatabase.PlayerList[i]);
        }
    }

    // Inisialisasi deck dengan kartu yang sesuai dengan jurusan pemain
    void InitializeDeck()
    {
        List<Kartunya> kartuList = new List<Kartunya>();

        // Ambil jurusan dari setiap pemain
        HashSet<string> jurusanPemain = new HashSet<string>();
        foreach (var player in players)
        {
            if (player.playerData != null)
            {
                jurusanPemain.Add(player.playerData.Jurusan);  // Ambil jurusan dari DataPemain
            }
        }

        // Filter kartu berdasarkan jurusan yang ada pemainnya
        foreach (var kartu in kartuDatabase.KartuList)
        {
            if (jurusanPemain.Contains(kartu.JurKartu))
            {
                kartuList.Add(kartu);  // Tambahkan kartu yang sesuai dengan jurusan
            }
        }

        // Acak kartu yang sesuai jurusan dan masukkan ke Stack
        while (kartuList.Count > 0)
        {
            int randomIndex = Random.Range(0, kartuList.Count);
            Kartunya kartuTerpilih = kartuList[randomIndex];
            deckStack.Push(kartuTerpilih);  // Masukkan kartu ke stack
            kartuList.RemoveAt(randomIndex);  // Hapus dari list
        }
    }

    // Fungsi untuk mendistribusikan kartu modal ke meja pemain
    void DistribusiKartuKeMeja()
    {
        foreach (Pemainnya player in players)
        {
            if (deckStack.Count > 0)
            {
                Kartunya kartuAwal = deckStack.Pop();  // Ambil kartu dari stack
                player.ReceiveStartingCardToTable(kartuAwal);  // Berikan kartu ke pemain langsung ke meja
                UpdateSisaKartuText();  // Perbarui jumlah sisa kartu setelah distribusi kartu
            }   
        }
    }

    // Fungsi untuk memperbarui jumlah sisa kartu di deck
    void UpdateSisaKartuText()
    {
                sisaKartuText.text = deckStack.Count.ToString(); // Tampilkan jumlah sisa kartu
    }

    void Update()
    {
        UpdateSisaKartuText();
    }

    public void CheckKartuhabis()
    {
        if (deckStack.Count <= 0)  
        {
                for(int i = 0; i < players.Length; i++)
            {
                players[i].HitungScore();
            }
                //tambah untuk pemenang di panggil
            Pemainnya Pemenang = players[0];
            for(int i = 1; i < players.Length; i++)
            {
                if (players[i].Score > Pemenang.Score)
                {
                    Pemenang = players[i];
                }


            } 
            //dipanggil saat popup endgame
            Debug.Log("Pemenang adalah " +  Pemenang.playerData.Nama);
            ScorePemenang.text=Pemenang.Score.ToString();
            NamaPemenang.text= Pemenang.playerData.Jurusan.ToString();
        }
    }


    // Fungsi untuk memperbarui gambar dadu
    public void UpdateDiceImage(int diceValue)
    {
        if (diceValue >= 1 && diceValue <= 6)
        {
            diceImage.sprite = diceSprites[diceValue - 1];  // Set sprite sesuai hasil dadu
        }
    }

    // Fungsi untuk mengakhiri giliran dan berpindah ke pemain berikutnya
    public void EndTurn()
    {
        if (players[currentPlayerIndex].IsChoosingToDiscard())
        {
            Debug.Log("Menunggu pemain membuang kartu...");
            isWaitingForDiscard = true;
            //isDiceRollAllowed = false;
            return;
        }

        isWaitingForDiscard = false;
        //isDiceRollAllowed = true;

        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Length)
        {
            currentPlayerIndex = 0;
        }

        //Debug.Log("Sekarang giliran pemain: " + players[currentPlayerIndex].playerData.Jurusan);
    }






    public void GantiKePemainPodium(Pemain pemain)
    {
        // Matikan pemain biasa
        pemain.gameObject.SetActive(false);

        // Aktifkan pemain di podium
        if (pemain.pemainPodium != null)
        {
            pemain.pemainPodium.SetActive(true);
        }
        else
        {
            Debug.LogError("Pemain podium tidak ditemukan untuk pemain ini.");
        }
    }



    //public void GantiKePemainPodium(Pemain pemain)
    //{
    //    pemain.AktifkanPemainPodium(); // Pindahkan pemain ke podium
    //    Debug.Log("Pemain berpindah ke podium.");
    //}

    //public void KembaliKePemainBiasa(Pemain pemain)
    //{
    //    pemain.AktifkanPemainBiasa(); // Kembalikan pemain ke posisi biasa
    //    Debug.Log("Pemain kembali ke posisi biasa.");
    //}



}
