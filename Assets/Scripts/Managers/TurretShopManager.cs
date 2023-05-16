using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShopManager : MonoBehaviour
{
    [SerializeField] private GameObject turretCardPrefab;
    [SerializeField] private Transform turretPanelContainer;

    [SerializeField] private TurretSettings[] turrets;

    private Node currentNodeSelected;
    
    private void Start()
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            CreateTurretCard(turrets[i]);
        }
    }

    private void CreateTurretCard(TurretSettings turretSettings)
    {
        GameObject newInstance = Instantiate(turretCardPrefab, turretPanelContainer.position, Quaternion.identity);
        newInstance.transform.SetParent(turretPanelContainer);
        newInstance.transform.localScale = Vector3.one;

        TurretCard cardButton = newInstance.GetComponent<TurretCard>();
        cardButton.SetupTurretButton(turretSettings);
    }
    
    private void NodeSelected(Node nodeSelected)
    {
        currentNodeSelected = nodeSelected;
    }
    
    private void PlaceTurret(TurretSettings turretLoaded)
    {
        if (currentNodeSelected != null)
        {
            GameObject turretInstance = Instantiate(turretLoaded.TurretPrefab);
            turretInstance.transform.localPosition = currentNodeSelected.transform.position;
            turretInstance.transform.parent = currentNodeSelected.transform;

            Turret turretPlaced = turretInstance.GetComponent<Turret>();
            currentNodeSelected.SetTurret(turretPlaced);
        }
    }

    private void TurretSold()
    {
        currentNodeSelected = null;
    }
    
    private void OnEnable()
    {
        Node.OnNodeSelected += NodeSelected;
        Node.OnTurretSold += TurretSold;
        TurretCard.OnPlaceTurret += PlaceTurret;
    }

    private void OnDisable()
    {
        Node.OnNodeSelected -= NodeSelected;
        Node.OnTurretSold -= TurretSold;
        TurretCard.OnPlaceTurret -= PlaceTurret;
    }
}
