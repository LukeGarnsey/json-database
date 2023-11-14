using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Garden
{
    public class Tulip : Flowers
    {

        public string _color;
        public Tulip()
        {
            _name = "Tulip";
        }
        public Tulip(string id):base(id)
        {

        }

        public override void Construct(JObject json)
        {
            base.Construct(json);

            _color = GetValueSafe<string>(json["color"], "White");
        }

        public override JObject Compile(bool recursiveSave)
        {
            JObject json =  base.Compile(recursiveSave);

            json.Add("color", _color);

            return json;
        }
    }
}
