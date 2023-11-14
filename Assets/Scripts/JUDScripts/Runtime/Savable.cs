

using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace JUD
{
    /// <summary>
    /// Extend this class when you want to Compile/Construct an object but not write to file.
    /// </summary>
    abstract public class Savable
    {
        public Savable(){

        }
        public Savable(JObject json){

        }
        /// <summary>
        /// Use to Load this object from Json.
        /// </summary>
        /// <param name="json"></param>
        abstract public void Construct(JObject json);
        /// <summary>
        /// Use to reduce this object to json.
        /// </summary>
        /// <param name="recursiveSave"></param>
        /// <returns></returns>
        abstract public JObject Compile(bool recursiveSave);

        /// <summary>
        /// Use when attempting to get a value from json objects.
        /// </summary>
        /// <param name="temp">json value to check.</param>
        /// <param name="defaultValue">value to return if json value is null</param>
        /// <typeparam name="t"></typeparam>
        /// <returns>json item or default value.</returns>
        protected t GetValueSafe<t>(JToken temp, t defaultValue)
        {
            if (temp == null)
                return defaultValue;

            return temp.ToObject<t>();
        }
        /// <summary>
        /// Reduce a list of <see cref="Savable"/> objects to json array.
        /// </summary>
        /// <param name="savableList">List of objects to compile.</param>
        /// <param name="recursiveSave">If found, whether a model should save it's contents.</param>
        /// <typeparam name="t"></typeparam>
        /// <returns></returns>
        protected JArray CompileSavableList<t>(List<t> savableList, bool recursiveSave) where t : Savable{
            JArray j = new JArray();
            foreach(t a in savableList)
                j.Add(a.Compile(recursiveSave));
            
            return j;
        }
        /// <summary>
        /// Reduce a list of <see cref="Model"/> objects to a json array of their GUIDs.
        /// </summary>
        /// <param name="models">List of objects to compile.</param>
        /// <param name="recursiveSave">Whether a model should save it's contents.</param>
        /// <typeparam name="t">Type in list.</typeparam>
        /// <returns></returns>
        protected JArray CompileModelList<t>(List<t> models, bool recursiveSave) where t : Model{
            JArray j = new JArray();
            foreach(t a in models){
                j.Add(a.GUID(recursiveSave, recursiveSave));
            }
            return j;
        }
        /// <summary>
        /// Load <see cref="Model"/> from file name(GUID).
        /// </summary>
        /// <param name="guid">ID of file to load.</param>
        /// <param name="mutationChildTypes">Allow the Model type to change to these types if appropriate.</param>
        /// <typeparam name="t">Return type of Model being constructed.</typeparam>
        /// <returns>Constructed Model.</returns>
        protected t ConstructModel<t>(string guid, params System.Type[] mutationChildTypes) where t : Model, new(){
            if(guid == ""){
                UnityEngine.Debug.LogWarning("GUID was empty: " + typeof(t).Name);
                return System.Activator.CreateInstance(typeof(t)) as t;
            }
            if(mutationChildTypes.Length > 0){
                Model check = System.Activator.CreateInstance(typeof(t)) as t;
                if(Data.ModelExists(check, guid)){
                    check.Load(guid);
                    return check as t;
                }
                foreach(System.Type type in mutationChildTypes){
                    check = System.Activator.CreateInstance(type) as t;
                    if(Data.ModelExists(check, guid)){
                        check.Load(guid);
                        return check as t;
                    }
                }
                UnityEngine.Debug.LogWarning("Could not find GUID in children");
            }
            return System.Activator.CreateInstance(typeof(t), guid) as t;
        }
        /// <summary>
        /// Load a list of <see cref="Model"/> from an array of file names(GUIDS).
        /// </summary>
        /// <param name="arrayOfGUIDs">JArray of file IDs to load.</param>
        /// <param name="mutationChildTypes">Allow the Model type to change to these types if appropriate.</param>
        /// <typeparam name="t">Return type of Model being constructed.</typeparam>
        /// <returns>List of constructed model.</returns>
        protected List<t> ConstructModelList<t>(JArray arrayOfGUIDs, params System.Type[] mutationChildTypes) where t : Model, new(){
            List<t> models = new List<t>();
            if(arrayOfGUIDs.Count == 0)
                return models;
            
            if(arrayOfGUIDs[0].GetType().Name == "JValue"){
                foreach(JValue o in arrayOfGUIDs)
                    models.Add(ConstructModel<t>(GetValueSafe<string>(o, ""), mutationChildTypes));
            }else if(arrayOfGUIDs[0].GetType().Name == "JObject"){
                UnityEngine.Debug.LogError("JArray for building Models was JObject");
            }
            
            return models;
        }
        /// <summary>
        /// Load <see cref="Savable"/> from json.
        /// </summary>
        /// <param name="json">Json this 'Savable' will use.</param>
        /// <typeparam name="t">Return type of 'Savable' being constructed.</typeparam>
        /// <returns>Constructed Savable.</returns>
        protected t ConstructSavable<t>(JObject json) where t : Savable, new(){
            return System.Activator.CreateInstance(typeof(t), json) as t;
        }
        /// <summary>
        /// Load a list of 'Savable' from json.
        /// </summary>
        /// <param name="jsonArray">Json this <see cref="Savable"/> will use, in the format of json array.</param>
        /// <typeparam name="t">Return type of 'Savable' being constructed.</typeparam>
        /// <returns>List of constructed Savable.</returns>
        protected List<t> ConstructSavableList<t>(JArray jsonArray) where t:Savable, new(){
            List<t> fields = new List<t>();
            foreach(JObject o in jsonArray)
                fields.Add(ConstructSavable<t>(o));
            return fields;
        }
    }
}
