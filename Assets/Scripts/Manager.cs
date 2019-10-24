using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NLua;
using UnityEngine.UI;
using System.Linq;

public class RankTupla
{
    public int points;
    public string nome;

    public RankTupla(int points, string nome)
    {
        this.points = points;
        this.nome = nome;
    }
}

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject tower;
    [SerializeField] GameObject player;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject endScreen;
    [SerializeField] Text rankShow;
    [SerializeField] GameObject EndStartChecker;
    [SerializeField] InputField inputField;
    [SerializeField] public int pointForKill = 1;
    [SerializeField] public int pointForCapture = 2;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] Color[] colors;
    public static Manager instance;
    public bool showGizmo = true;
    List<Player> players = new List<Player>();
    List<Tower> towers = new List<Tower>();
    List<RankTupla> rank = new List<RankTupla>();
    bool endGame = false;

    public List<Player> Players
    {
        get
        {
            return players;
        }
    }

    public List<Tower> Towers
    {
        get
        {
            return towers;
        }
    }

    public List<RankTupla> Rank
    {
        get
        {
            return rank;
        }
    }

    void Awake()
    {
        instance = this;
        canvas.SetActive(true);
        startScreen.SetActive(true);
        endScreen.SetActive(false);
    }

    void Update()
    {
        if (endGame)
            return;

        if (towers.Count < 1)
            return;

        var first = towers[0].owner;
        var queryResult = towers.Where((x) => (first == x.owner));

        if (players.Count == 1 || queryResult.Count() == towers.Count)
        {
            //TODO: acabou
            EndGame();
        }
    }


    public void GenerateRank()
    {
        foreach (Player player in players)
        {
            rank.Add(new RankTupla(0, player.nick));
        }
    }

    void EndGame()
    {
        //TODO: Fazer o fim do jogo
        endGame = true;

        foreach(Tower t in towers)
        {
            Destroy(t.gameObject);
        }

        foreach(Player p in players)
        {
            Destroy(p.gameObject);
        }

        IEnumerable ordered = Rank.OrderByDescending((x) => x.points);

        rankShow.text = "Rank: \n";

        int i = 1;

        foreach (RankTupla rankTupla in ordered)
        {
            rankShow.text += i + "° - Nome: " + rankTupla.nome  + " com " + rankTupla.points + "\n";
            i++;
        }

        canvas.SetActive(true);
        startScreen.SetActive(false);
        endScreen.SetActive(true);
    }

    public void LoadScripts()
    {
        if (spawnPoints == null)
            return;

        string playerScripts = "players";

        if (inputField.text != "")
            playerScripts = inputField.text;

        try
        {
            string[] filePaths = System.IO.Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, playerScripts), "*.lua");

            int i = 0;
            foreach (string filePath in filePaths)
            {
                var auxTower = Instantiate(tower, spawnPoints[i].position + Vector3.right * 2, spawnPoints[i].rotation)
                    .GetComponent<Tower>();

                var auxPlayer = Instantiate(player, spawnPoints[i].position, spawnPoints[i].rotation)
                    .GetComponent<Player>();
                auxPlayer.SetScript(filePath);
                auxPlayer.towers.Add(auxTower);

                players.Add(auxPlayer);
                towers.Add(auxTower);
                auxTower.owner = auxPlayer;
                i++;
            }

            Instantiate(EndStartChecker);

            startScreen.SetActive(false);
            endScreen.SetActive(false);
            canvas.SetActive(false);

        }
        catch (System.IO.DirectoryNotFoundException e)
        {
            //TODO: Mostrar erro de alguma forma
        }
    }

    void OnDrawGizmos()
    {
        if (!showGizmo)
            return;

        if (spawnPoints == null)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Gizmos.DrawCube(spawnPoints[i].position, spawnPoints[i].localScale * 5);
        }
    }
}