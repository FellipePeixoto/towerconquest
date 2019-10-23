using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float tick = 0.1f;
    public Player owner;
    bool dominando = false;
    float progress;
    List<Player> invaders = new List<Player>();

    public List<Player> Invaders
    {
        get
        {
            return invaders;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (invaders.Count > 0 && invaders[0] != null)
            Debug.Log(invaders.Count);

        if (invaders.Count == 1 && owner != null && invaders[0] != null && !invaders[0].Equals(owner))
        {
            progress += tick * Time.deltaTime;

            if (progress >= 1)
            {
                owner = invaders[0];
                Manager.instance.Rank.Single((x) => (owner.nick == x.nome)).points
                    += Manager.instance.pointForCapture;
                progress = 0;
                Transition();
            }
        }
        else if (invaders.Count == 1)
        {
            progress += tick * Time.deltaTime;

            if (progress >= 1)
            {
                owner = invaders[0];
                Manager.instance.Rank.Single((x) => (owner.nick == x.nome)).points
                    += Manager.instance.pointForCapture;
                progress = 0;
                Transition();
            }
        }
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (invaders.Contains(coll.GetComponent<Player>()))
            return;

        if (coll.GetComponent<Player>() == null)
            return;

        invaders.Add(coll.GetComponent<Player>());
    }

    public void OnTriggerExit(Collider coll)
    {
        if (!invaders.Contains(coll.GetComponent<Player>()))
            return;

        if (coll.GetComponent<Player>() == null)
            return;

        invaders.Remove(coll.GetComponent<Player>());
    }

    public void Transition()
    {
        GetComponent<MeshRenderer>().material.color = invaders[0].GetComponent<Player>().color;
    }

    public void ChangeColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }
}
