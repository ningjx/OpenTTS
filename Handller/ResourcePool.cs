using SpeechGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGenerator.Handller
{
    public class ResourcePool
    {
        private static ResourcePool pool = null;

        public static ResourcePool Instance
        {
            get
            {
                if (pool == null)
                {
                    pool = new ResourcePool();
                    return pool;
                }
                else
                {
                    return pool;
                }
            }
        }

        public SpeechResource SpeechResource = null;
        public Config Config = null;

        private ResourcePool()
        {


        }


    }
}
