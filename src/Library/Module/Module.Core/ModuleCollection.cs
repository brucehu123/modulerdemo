﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using YunHu.Lib.Module.Abstractions;
using YunHu.Lib.Utils.Core;
using YunHu.Lib.Utils.Core.Helpers;

namespace YunHu.Lib.Module.Core
{
    public class ModuleCollection : IModuleCollection
    {
        private readonly List<ModuleInfo> _moduleInfos = new List<ModuleInfo>();

        public IEnumerator<ModuleInfo> GetEnumerator()
        {
            return _moduleInfos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ModuleInfo item)
        {
            _moduleInfos.Add(item);
        }

        public void Clear()
        {
            _moduleInfos.Clear();
        }

        public bool Contains(ModuleInfo item)
        {
            return _moduleInfos.Contains(item);
        }

        public void CopyTo(ModuleInfo[] array, int arrayIndex)
        {
            _moduleInfos.CopyTo(array, arrayIndex);
        }

        public bool Remove(ModuleInfo item)
        {
            return _moduleInfos.Remove(item);
        }

        public int Count => _moduleInfos.Count;

        public bool IsReadOnly => true;

        public int IndexOf(ModuleInfo item)
        {
            return _moduleInfos.IndexOf(item);
        }

        public void Insert(int index, ModuleInfo item)
        {
            _moduleInfos.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _moduleInfos.RemoveAt(index);
        }

        public ModuleInfo this[int index]
        {
            get => _moduleInfos[index];
            set => _moduleInfos[index] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public ModuleCollection()
        {
            var moduleJsonFiles = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "modules"), "module.json", SearchOption.AllDirectories);

            foreach (var file in moduleJsonFiles)
            {
                var moduleInfo = JsonConvert.DeserializeObject<ModuleInfo>(File.ReadAllText(file));
                if (moduleInfo != null)
                {
                    //判断是否已存在
                    if (_moduleInfos.Any(m => m.Name.Equals(moduleInfo.Name)))
                        continue;
                    var assemblyHelper = new AssemblyHelper();
                    //此处默认模块命名空间前缀与当前项目相同
                    moduleInfo.AssembliesInfo = new ModuleAssembliesInfo
                    {
                        Domain = assemblyHelper.Load(m => m.Name.EndsWith("Module." + moduleInfo.Id + ".Domain")).FirstOrDefault(),
                        Infrastructure = assemblyHelper.Load(m => m.Name.EndsWith("Module." + moduleInfo.Id + ".Infrastructure")).FirstOrDefault(),
                        Application = assemblyHelper.Load(m => m.Name.EndsWith("Module." + moduleInfo.Id + ".Application")).FirstOrDefault(),
                        Web = assemblyHelper.Load(m => m.Name.EndsWith("Module." + moduleInfo.Id + ".Web")).FirstOrDefault(),
                    };

                    Check.NotNull(moduleInfo.AssembliesInfo.Domain, moduleInfo.Id + "模块的Domain程序集未发现");
                    Check.NotNull(moduleInfo.AssembliesInfo.Infrastructure, moduleInfo.Id + "模块的Infrastructure程序集未发现");
                    Check.NotNull(moduleInfo.AssembliesInfo.Application, moduleInfo.Id + "模块的Application程序集未发现");
                    Check.NotNull(moduleInfo.AssembliesInfo.Web, moduleInfo.Id + "模块的Web程序集未发现");

                    //加载模块初始化器
                    var moduleInitializerType = moduleInfo.AssembliesInfo.Web.GetTypes().FirstOrDefault(t => typeof(IModuleInitializer).IsAssignableFrom(t));
                    if (moduleInitializerType != null && (moduleInitializerType != typeof(IModuleInitializer)))
                    {
                        moduleInfo.Initializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
                    }

                    Add(moduleInfo);
                }
            }
        }
    }
}
