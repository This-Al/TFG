using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUIController : MonoBehaviour
{
    public GameObject chargeContainer;
    private float fillValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fillValue = (float)GameController.TrailCharge;
        fillValue = fillValue / GameController.TrailMaxCharge;
        chargeContainer.GetComponent<Image>().fillAmount = fillValue;
    }
}
