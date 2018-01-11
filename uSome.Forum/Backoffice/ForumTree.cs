using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace uSome.Forum.Backoffice
{
    [PluginController("uSome")]
    [Umbraco.Web.Trees.Tree("uSome", "Forum", "Forum", "1")]
    public class ForumTreeController : TreeController
    {
        protected override Umbraco.Web.Models.Trees.MenuItemCollection GetMenuForNode(string id, System.Net.Http.Formatting.FormDataCollection queryStrings)
        {
            var menu = new Umbraco.Web.Models.Trees.MenuItemCollection();
            menu.Items.Clear();
            return menu;
        }

        protected override Umbraco.Web.Models.Trees.TreeNodeCollection GetTreeNodes(string id, System.Net.Http.Formatting.FormDataCollection queryStrings)
        {
            var tree = new TreeNodeCollection();
            //check if we're rendering the root node
            var forum = CreateTreeNode("Forum", "-1", queryStrings, "Forum", "icon-whhg-forumsalt");
            if (id == Constants.System.Root.ToInvariantString())
            {
                forum.HasChildren = true;
                tree.Add(forum);
            }
            else if (id == "Forum")
            {
                //this is a child of the root node
                var settings = CreateTreeNode("1-Settings", "Forum", queryStrings, "Settings", "icon-settings");
                var question = CreateTreeNode("1-Questions", "Forum", queryStrings, "Questions", "icon-whhg-question-sign");
                tree.Add(settings);
                tree.Add(question);
            }
            else if (id.StartsWith("1-"))
            {
                //this must be a third level node, e.g. one that's under the child nodes.
            }
            return tree;
            //this tree doesn't suport rendering more than 1 level
            throw new NotSupportedException();
        }
    }
}