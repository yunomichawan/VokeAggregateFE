﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VokeAggregateFE
{
    public abstract class Singleton<T> where T : class
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Activator.CreateInstance<T>();
                }

                return instance;
            }
        }

    }
}
