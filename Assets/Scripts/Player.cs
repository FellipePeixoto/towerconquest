using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NLua;
using System.IO;
using TMPro;
using System.Linq;

public class Player : LuaBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshPro;

    //TODO: ARRUMAR ESSA BAGUNCA DE VARIAVEL
    float speed = 0.2f;
    [SerializeField] int hp;
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if (hp <= 0)
            {
                Manager.instance.Players.Remove(this);
                foreach (Tower tower in Manager.instance.Towers)
                {
                    tower.Invaders.Remove(this);
                    if (tower.owner == this)
                        tower.owner = null;
                }

                Destroy(gameObject);

            }
        }
    }
    public string nick;
    public Color color;
    public NavMeshAgent navMeshAgent;
    public List<Tower> towers = new List<Tower>();

    public void SetScript(string path)
    {
        env = new Lua();
        env.LoadCLRPackage();

        env["this"] = this;

        try
        {
            source = File.ReadAllText(path);
            env.DoString(source);
        }
        catch (NLua.Exceptions.LuaException e)
        {
            Debug.LogError(FormatException(e), gameObject);
        }
        catch (System.IO.IOException e)
        {
            Debug.LogError(e.Message, gameObject);
        }
    }

    public void MoveToTower(Tower tower)
    {
        navMeshAgent.SetDestination(tower.transform.position);
    }
    //todo in lua = player indetificar torre de cor diferente

    public void MoveToEnemy()
    {

    }

    public void MoveTo(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
    }

    //todo in lua = player idetificar inimigo in range
    public int MyHp()
    {
        return HP;
    }

    public void Attack()
    {

    }

    public void SetNick(string nick)
    {
        this.nick = nick;
        textMeshPro.text = nick;
    }

    public void SetColor(string color)
    {
        //TODO: Listar cores
        switch (color)
        {
            case "Red":
                ChangeColor(Color.red);
                break;

            case "Green":
                ChangeColor(Color.green);
                break;

            case "Blue":
                ChangeColor(Color.blue);
                break;
        }
    }

    public Vector3 GetNearestPlayer()
    {
        float minDistance = 1000000000;
        Vector3 nearestPlayer = Vector3.zero + Vector3.up * 10;
        Player pl = null;

        foreach (Player player in Manager.instance.Players)
        {
            if (player == this)
                continue;

            float dist = Vector3.Distance(player.transform.position, transform.position);

            if (dist < minDistance)
            {
                minDistance = dist;
                pl = player;
                nearestPlayer = player.transform.position;
            }
        }

        return nearestPlayer;
    }

    public Vector3 GetNearestTower()
    {
        float minDistance = 1000000000;
        Vector3 nearestTower = Vector3.zero + Vector3.up * 10;
        Player pl = null;

        try
        {
            IEnumerable notMyTowers = Manager.instance.Towers.Where((x) => (x.owner.nick != nick));

            foreach (Tower tower in notMyTowers)
            {
                float dist = Vector3.Distance(tower.transform.position, transform.position);

                if (dist < minDistance)
                {
                    minDistance = dist;
                    pl = tower.owner;
                    nearestTower = tower.transform.position;
                }
            }
        }
        catch
        {

        }        

        return nearestTower;
    }

    void ChangeColor(Color color)
    {
        this.color = color;
        GetComponent<MeshRenderer>().material.color = color;
        towers[0].ChangeColor(color);
        textMeshPro.color = color;
    }
}
