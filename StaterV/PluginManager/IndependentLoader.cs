using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using PluginData;

namespace StaterV.PluginManager
{
    class IndependentLoader
    {
        private readonly Dictionary<Assembly, IndependentPlugin> assembly2plugin = new Dictionary<Assembly, IndependentPlugin>();
        public Dictionary<Assembly, BasePlugin> LoadedPlugins { get; set; }

        public IndependentLoader()
        {
            LoadedPlugins = new Dictionary<Assembly, BasePlugin>();
        }

        public IEnumerable<string> ListPlugins(string path)
        {
            List<string> names = new List<string>();
            try
            {
                foreach (var di in new DirectoryInfo(path).GetDirectories())
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        if ((file.Extension == ".exe" || file.Extension == ".dll"))
                        //&& IsValidPackage(file.FullName))
                        {
                            names.Add(file.FullName);
                        }
                    }
                }

            }
            catch (Exception)
            {

            }
            return names;
        }

        private Assembly LoadAssembly(AssemblyName name, string path)
        {
            return Assembly.LoadFile(path);
        }

        private bool IsValidPluginType(Type type)
        {
            //if (TypeUtils.
            Type basePluginType = typeof(IndependentPlugin);
            if (type.IsSubclassOf(basePluginType))
            {
                return true;
            }
            return false;
        }

        public IndependentPlugin LoadPlugin(string fileName)
        {
            Assembly assembly = LoadAssembly(null, fileName);

            // check if package is already loaded
            if (assembly2plugin.ContainsKey(assembly))
            {
                //throw (new Exception("Already loaded"));
                return null;
            }

            if (LoadedPlugins.ContainsKey(assembly))
            {
                return null;
            }

            Type pluginType = null;

            foreach (var type in assembly.GetExportedTypes())
            {
                if (IsValidPluginType(type))
                {
                    pluginType = type;
                }
            }

            if (pluginType != null) //TODO: Вставить условие, что все хорошо.
            {
                /*
                ConstructorInfo ctor = pluginType.GetConstructor(
                    pluginType.Assembly.ReflectionOnly ? packageCtorParamsReflectionOnly : packageCtorParams);
                 * */
                Type[] pluginParams = new Type[] { };
                ConstructorInfo ctor = pluginType.GetConstructor(pluginParams);
                var plugin = (IndependentPlugin)ctor.Invoke(new object[] { });
                //loadedPackages.Add(package);
                assembly2plugin[assembly] = plugin;
                //mainApp.Log.WriteMessage(ToString(), MessageType.Information, Resources.PackageLoader_PackageLoaded, package.PackageName, fileName);
                return plugin;
            }
            return null;
        }
    }
}
