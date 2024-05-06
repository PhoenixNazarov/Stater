using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginData;

namespace StaterV.PluginManager
{
    /// <summary>
    /// Класс управления плагинами. Singleton.
    /// </summary>
    class PluginManager
    {
        #region Construction
        private PluginManager()
        {
            ButtonPluginList = new List<ButtonPlugin>();
        }
        private static PluginManager instance = new PluginManager();
        public static PluginManager Instance
        {
            get { return instance; }
        }
        #endregion

        #region PrivateFields
        private PluginLoader pluginLoader = new PluginLoader();
        private List<BasePlugin> plugins = new List<BasePlugin>();
        private string oldPluginFolder = "plugins\\old";
        private string defaultPluginFolder = "plugins";
        private IndependentLoader ipLoader = new IndependentLoader();
        #endregion

        #region Properties

        public List<ButtonPlugin> ButtonPluginList { get; private set; }
        public List<IndependentPlugin> IndependentPluginList { get; private set; }
        #endregion

        public void LoadPlugins()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, oldPluginFolder);
            var pluginList = new List<string>(pluginLoader.ListPlugins(path));

            foreach (var filename in pluginList)
            {
                try
                {
                    var plugin = pluginLoader.LoadPlugin(filename);
                    if (plugin.GetType().IsSubclassOf(typeof(ButtonPlugin)))
                    {
                        ButtonPluginList.Add((ButtonPlugin)plugin);
                    }
                }
                catch (Exception)
                { }

                LoadIndependent();
            }
        }

        private void LoadIndependent()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultPluginFolder);
            var pluginList = new List<string>(ipLoader.ListPlugins(path));

            if (IndependentPluginList == null)
            {
                IndependentPluginList = new List<IndependentPlugin>();
            }

            ipLoader.LoadedPlugins = pluginLoader.Assembly2Plugin;
            foreach (var pl in pluginList)
            {
                try
                {
                    var plugin = ipLoader.LoadPlugin(pl);
                    if (plugin != null)
                    {
                        if (plugin.GetType().IsSubclassOf(typeof(IndependentPlugin)))
                        {
                            IndependentPluginList.Add(plugin);
                        }
                    }

                }
                catch (Exception e)
                {
                }
            }
        }
    }
}
