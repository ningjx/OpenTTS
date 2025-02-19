using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenTTS.Models
{
    public class RoleResource : Dictionary<RoleEnum, string>
    {
        public static RoleResource LoadRoleResource()
        {
            return JsonConvert.DeserializeObject<RoleResource>(Resources.AllRoles);
        }
    }
}
