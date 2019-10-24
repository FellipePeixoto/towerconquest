using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bater : MonoBehaviour
{
   [SerializeField] int facada = 1;
   [SerializeField] Player player;
    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            coll.GetComponent<Player>().HP -= facada;
            if(coll.GetComponent<Player>().HP <= 0)
            {
                try
                {
                    RankTupla rankTupla = Manager.instance.Rank.Single((x) => (x.nome == player.nick));
                    rankTupla.points += Manager.instance.pointForKill;
                }
                catch (System.InvalidOperationException)
                {

                }
            }
        }
    
    }
}
