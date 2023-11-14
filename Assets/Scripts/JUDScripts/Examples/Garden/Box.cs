using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Garden
{
    public class Box : JUD.Model
    {

        public List<Flowers> _flowers = new List<Flowers>();
        public Box()
        {

        }
        public Box(string id) : base(id)
        {

        }

        //public override string DirectoryPath => base.DirectoryPath + "Box/";

        public override void Construct(JObject json)
        {
            base.Construct(json);
            _flowers = ConstructModelList<Flowers>(GetValueSafe<JArray>(json["flowers"], new JArray()), typeof(Tulip));
            // JArray flowers = new JArray(json["flowers"].ToArray());
            // _flowers = new Flowers[flowers.Count];
            
            // for (int i = 0; i < _flowers.Length; i++)
            //     _flowers[i] = new Flowers(flowers[i].ToObject<string>());
        }

        public override JObject Compile(bool recursiveSave)
        {
            JObject json = base.Compile(recursiveSave);

            // JArray flowers = new JArray();
            // foreach (Flowers f in _flowers)
            //     flowers.Add(f.Save());

            json.Add("flowers", CompileModelList(_flowers, recursiveSave));

            return json;
        }
    }
}
