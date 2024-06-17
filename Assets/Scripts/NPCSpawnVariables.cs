using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// DESIGN PATTERN : SINGLETON

namespace Assets
{
    public class NPCSpawnVariables
    {
        // Static instance of the NPCSpawnVariables class
        private static NPCSpawnVariables instance;

        // Public property to access the instance
        public static NPCSpawnVariables Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NPCSpawnVariables();
                }
                return instance;
            }
        }

        // Private constructor to prevent instantiation from outside
        private NPCSpawnVariables() { }

        // Public variables
        public int npcsalive = 0;
        public bool spawning = false;
    }
}
