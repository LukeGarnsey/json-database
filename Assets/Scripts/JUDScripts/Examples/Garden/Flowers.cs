using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Garden
{
    public class Flowers : JUD.Model
    {

        public string _name;
        public Flowers()
        {

        }
        public Flowers(string id):base(id)
        {

        }

        //public override string DirectoryPath=> base.DirectoryPath + "Flowers/";

        public override void Construct(JObject json)
        {
            base.Construct(json);

            _name = GetValueSafe<string>(json["name"], "Flower");
        }

        public override JObject Compile(bool recursiveSave)
        {
            JObject json =  base.Compile(recursiveSave);

            json.Add("name", _name);

            return json;
        }
    }
}
