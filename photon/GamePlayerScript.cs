using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GamePlayerScript : MonoBehaviourPunCallbacks
{
    private PhotonView  m_photonView    = null;
    private Renderer    m_render        = null;

    private readonly Color[]    MATERIAL_COLORS = new Color[]
    {
        Color.white, Color.white, Color.white, Color.blue, Color.green,
    };

    void Awake()
    {
        m_photonView    = GetComponent<PhotonView>();
        m_render        = GetComponent<Renderer>();
    }

    void Update()
    {
        int ownerID             = m_photonView.OwnerActorNr;
        m_render.material.color = MATERIAL_COLORS[ ownerID ];
    }
}
