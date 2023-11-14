
using Newtonsoft.Json.Linq;

namespace JUD
{
    /// <summary>
    /// Not in use currently.
    /// </summary>
    public interface ISavable
    {
        /// <summary>
        /// Use to reduce this object to json.
        /// </summary>
        /// <param name="recursiveSave"></param>
        /// <returns></returns>
        JObject Compile(bool recursiveSave);
        /// <summary>
        /// Use to Load this object from Json.
        /// </summary>
        /// <param name="json"></param>
        void Construct(JObject json);
    }
}
