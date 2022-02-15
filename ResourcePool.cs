using SpeechGenerator.Models;

namespace SpeechGenerator
{
    public class ResourcePool
    {
        private static ResourcePool pool = null;

        public static ResourcePool Instance
        {
            get
            {
                if (pool == null)
                    pool = new ResourcePool();
                return pool;
            }
        }

        public SpeechResource SpeechResource = null;
        public Config Config = null;

        private ResourcePool()
        {
            SpeechResource = SpeechResource.LoadSpeechResources();
            Config = Config.LoadConfig();
        }


    }
}
