using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StaterV.PluginManager
{
    class PluginLoader
    {
        private readonly Dictionary<Assembly, BasePlugin> assembly2plugin = new Dictionary<Assembly, BasePlugin>();

        public Dictionary<Assembly, BasePlugin> Assembly2Plugin
        {
            get { return assembly2plugin; }
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
            Type basePluginType = typeof(BasePlugin);
            if (type.IsSubclassOf(basePluginType))
            {
                return true;
            }
            return false;
        }

        public BasePlugin LoadPlugin(string fileName)
        {
            Assembly assembly = LoadAssembly(null, fileName);

			// check if package is already loaded
            if (Assembly2Plugin.ContainsKey(assembly))
            {
                throw (new Exception("Already loaded"));
            }

            Type pluginType = null;

            var types = new List<Type>();
            foreach(var type in assembly.GetExportedTypes())
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
                if (ctor != null)
                {
                    var plugin = (BasePlugin)ctor.Invoke(new object[] { });
                    //loadedPackages.Add(package);
                    Assembly2Plugin[assembly] = plugin;
                    //mainApp.Log.WriteMessage(ToString(), MessageType.Information, Resources.PackageLoader_PackageLoaded, package.PackageName, fileName);
                    return plugin;
                }
            }
            return null;
        }
    }
}
