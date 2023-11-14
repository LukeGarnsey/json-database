using System.IO;
using Newtonsoft.Json.Linq;
using System;

namespace JUD
{
    /// <summary>
    /// Contains behavior for Accessing Data Storage.
    /// </summary>
    static internal class Data
    {
        /// <summary>
        /// Compile a <see cref="Model"/> to json and write it to a file.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="id"></param>
        /// <returns>Returns the guid object was saved with</returns>
        static public string CompileAndWrite(Model json, bool recursiveSave, string id = "")
        {
            string path = json.DirectoryPath;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            DirectoryInfo directory = new DirectoryInfo(path);
            if (id == "")
                id = Guid.NewGuid().ToString();

            if (!File.Exists(directory.FullName + "/" + id + ".json"))
            {
                using (File.Create(directory.FullName + "/" + id + ".json")){
                    
                }
            }

            FileInfo formationFile = new FileInfo(directory.FullName + "/" + id + ".json");
            JObject obj = json.Compile(recursiveSave);
            using (StreamWriter writer = formationFile.CreateText())
            {
                writer.WriteLine(obj);
            }

            return id;
        }
        static public JObject FindAndReadFile(FileInfo fileInfo, string path, string id, bool scanChildDirectories)
        {
            JObject json = ReadFileContents(fileInfo);
            
            if(json == null && scanChildDirectories){
                foreach(DirectoryInfo d in fileInfo.Directory.GetDirectories()){
                    json = ScanDirectoryForFile(d, path, id);
                    if(json != null)
                        break;
                }
            }
            if(json == null)
                return new JObject(){
                    {"guid", id},
                    {"version", 0}
                };

            return json;
        }
        static private JObject ReadFileContents(FileInfo fileInfo){
            if(fileInfo.Exists){
                using (StreamReader reader = fileInfo.OpenText())
                {
                    string temp = reader.ReadToEnd();
                    JObject json = JObject.Parse(temp);
                    json.Add("guid", Path.GetFileNameWithoutExtension(fileInfo.Name));
                    return json;
                }
            }
            return null;
        }
        static private JObject ScanDirectoryForFile(DirectoryInfo directory, string path, string id){
            
            JObject json = ReadFileContents(new FileInfo(path + directory.Name + "/" + id + ".json"));
            if(json == null){
                foreach(DirectoryInfo d in directory.GetDirectories()){
                    json = ScanDirectoryForFile(d, path + d.Name + "/", id);
                    if(json != null)
                        break;
                }
            }
            return json;
        }
        /// <summary>
        /// Check if the ID associated with the <see cref="Model"/> has a corresponding File in data storage.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        static public bool ModelExists(Model obj, string id){
            FileInfo fileInfo = new FileInfo(obj.DirectoryPath + "/" + id + ".json");
            return fileInfo.Exists;
        }
    }
}