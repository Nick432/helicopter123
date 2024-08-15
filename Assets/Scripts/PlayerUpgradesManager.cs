using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUpgradesManager : MonoBehaviour
{
    public int money;
    

   public  List<int> itemupgradelevels = new List<int>();








    public void save_upgrade_level(int index,int level)
    {



        itemupgradelevels[index] = level;







    }

}
