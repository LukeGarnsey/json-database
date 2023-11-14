using UnityEngine;
using JUD;
using Garden;
using System.Collections.Generic;
public class EasySave : MonoBehaviour
{

    void Awake()
    {
        List<Box> boxes = Model.AllItems<Box>();
        Box box;
        if (boxes.Count == 0)
        {
            box = new Box() { _flowers = Model.AllItems<Flowers>() };
            if (box._flowers.Count == 0)
            {
                box._flowers = new List<Flowers> {
                new Tulip() { _color = "Red" },
                    new Tulip() { _color = "Pink" },
                    new Flowers() { _name = "Rose" },
                    new Flowers() { _name = "Lilly" },
                    new Flowers() { _name = "Rose" }
                };
                box.Save(true);
                Debug.Log("New Box Saved.");
            }
        }
        else
        {
            box = boxes[0];
            Debug.Log("flowers in Box: ");
            foreach (Flowers f in box._flowers){
                Debug.Log("Type: " + f.GetType().Name + " Name: " + f._name);
            }
        }
    }


}