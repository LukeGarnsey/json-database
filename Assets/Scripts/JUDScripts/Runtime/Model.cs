using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace JUD
{
	/// <summary>
	/// Extend this class when you want to Compile/Construct an object and Read/Write to Data.
	/// </summary>
	abstract public class Model : Savable
	{
		protected string _guid = string.Empty;
		protected int _version = 1;

		private bool _saveInProgress = false;
		public Model()
		{

		}
		public Model(string guid)
		{
			Load(guid);
		}
		public virtual string BaseDirectoryPath => UnityEngine.Application.dataPath + "/JUDData";
		/// <summary>
		/// Location of this Model's File.
		/// </summary>
		/// <returns></returns>
		public virtual string DirectoryPath => BaseDirectoryPath + DirectoryFromType();
		private string DirectoryFromType(){
			string built = "";
			Type currentType = GetType();
			while(currentType.Name != "Model"){
				built = built.Insert(0, "/" + currentType.Name);
				currentType = currentType.BaseType;
			}
			
			return built;
		}
		/// <summary>
		/// Use to build JSON of the Model and write to File. [Do not call on self]
		/// </summary>
		/// <param name="recursive">TRUE -> This Model will have any referenced Model compile and save to file.</param>
		/// <returns></returns>
		public string Save(bool recursiveSave)
		{
			if(!_saveInProgress){
				_saveInProgress = true;
				_guid = Data.CompileAndWrite(this, recursiveSave, _guid);
				_saveInProgress = false;
			}
			return _guid;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="recursiveSave">TRUE -> This Model will have any referenced Model compile and save to file.</param>
		/// <returns></returns>
		public string SaveAsNew(bool recursiveSave){
			return Data.CompileAndWrite(this, recursiveSave);
		}
		public void Load(string guid)
		{
			ReadAndConstruct(this, guid);
		}
		public override JObject Compile(bool recursiveSave)
		{
			JObject json = new JObject();
			json.Add("version", _version);
			return json;
		}
		public override void Construct(JObject json)
		{
			_guid = GetValueSafe<string>(json["guid"], String.Empty);
			_version = GetValueSafe<int>(json["version"], 1);
		}
		/// <summary>
		/// Will return GUID string and <see cref="Save"/> if 'shouldSave' is TRUE. 
		/// </summary>
		/// <param name="shouldSave">TRUE -> This Model compile and save to file.</param>
		/// <param name="shouldSave">TRUE -> This Model will have any referenced Model compile and save to file.</param>
		/// <returns>GUID</returns>
		public string GUID(bool shouldSave = false, bool recursive = false)
		{
			if(!_saveInProgress && shouldSave || _guid == String.Empty)
				_guid = Save(recursive);

			return _guid;
		}

		/// <summary>
        /// File with associated model ID will be removed from storage.
        /// </summary>
        /// <param name="modelToDelete"></param>
        public void Delete()
        {
            FileInfo file = new FileInfo(DirectoryPath + "/" + GUID(false) + ".json");
            if (file.Exists)
                file.Delete();

        }
		/// <summary>
		/// Called from the Constructor of <see cref="Model"/> in order retrieve it's json file from data storage.
		/// </summary>
		/// <param name="modelToConstruct">Ref of model under construction.</param>
		/// <param name="fileID">Name of file in data storage.</param>
		static private void ReadAndConstruct(Model modelToConstruct, string fileID)
		{
			modelToConstruct.Construct(Data.FindAndReadFile(new FileInfo(modelToConstruct.DirectoryPath + "/" + fileID + ".json"), modelToConstruct.DirectoryPath + "/", fileID, true));
		}
		/// <summary>
		/// Use to get all <see cref="Model"/> of type.
		/// </summary>
		/// <typeparam name="t">Type of Model to find.</typeparam>
		/// <returns>All models of type.</returns>
		static public List<t> AllItems<t>() where t : Model, new()
		{
			t type = new t();
			if (!Directory.Exists(type.DirectoryPath))
			{
				Directory.CreateDirectory(type.DirectoryPath);
			}
			DirectoryInfo directory = new DirectoryInfo(type.DirectoryPath);

			FileInfo[] files = directory.GetFiles();

			List<t> objs = new List<t>();
			foreach (FileInfo file in files)
			{
				if (file.Extension != ".meta")
				{
					t newObj = new t();
					ReadAndConstruct(newObj, Path.GetFileNameWithoutExtension(file.Name));
					objs.Add(newObj);
				}
			}
			return objs;
		}
	}
}   