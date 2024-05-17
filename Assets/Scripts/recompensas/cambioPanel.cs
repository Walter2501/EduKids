using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambioPanel : MonoBehaviour
{
    // Start is called before the first frame update



    public GameObject ownerPanel;
    public GameObject customerPanel;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowOwnerPanel()
    {
        ownerPanel.SetActive(true);
        customerPanel.SetActive(false);
    }

    public void ShowCustomerPanel()
    {
        ownerPanel.SetActive(false);
        customerPanel.SetActive(true);
    }
}
