using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using WebApp.Models;

namespace WebApp.Helper
{
    public class AppCache
    {
        public List<WordModel> GetValue()
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get("cachedValue") as List<WordModel>;
        }

        public bool Add(List<WordModel> wordModels)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add("cachedValue", wordModels, DateTime.Now.AddMinutes(10));
        }
    }
}