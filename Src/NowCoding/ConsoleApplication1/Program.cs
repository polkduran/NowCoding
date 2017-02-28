using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {

        static Attribute[] _attributes;
        static string cat = "result";

        [STAThread]
        static void Main(string[] args)
        {
            var parents = new Attribute("parents  ", new[] { "usual", "pretentious", "great_pret", });
            var has_nurs = new Attribute("has_nurs ", new[] { "proper", "less_proper", "improper", "critical", "very_crit", });
            var form = new Attribute("form     ", new[] { "complete", "completed", "incomplete", "foster", });
            var children = new Attribute("children ", new[] { "1", "2", "3", "more", });
            var housing = new Attribute("housing  ", new[] { "convenient", "less_conv", "critical", });
            var finance = new Attribute("finance  ", new[] { "convenient", "inconv", });
            var social = new Attribute("social   ", new[] { "nonprob", "slightly_prob", "problematic", });
            var health = new Attribute("health   ", new[] { "recommended", "priority", "not_recom" });

        _attributes = new Attribute[] { parents      ,
                                                has_nurs      ,
                                                form         ,
                                                children      ,
                                                housing      ,
                                                finance      ,
                                                social       ,
                                                health        };

            DataTable samples = getDataTable();

            DecisionTreeID3 id3 = new DecisionTreeID3();
            TreeNode root = id3.mountTree(samples, cat, _attributes);

            printNode(root, "");
        }

        public static void printNode(TreeNode root, string tabs)
        {
            Console.WriteLine(tabs + '|' + root.attribute + '|');

            if (root.attribute.values != null)
            {
                for (int i = 0; i < root.attribute.values.Length; i++)
                {
                    Console.WriteLine(tabs + "\t" + "<" + root.attribute.values[i] + ">");
                    TreeNode childNode = root.getChildByBranchName(root.attribute.values[i]);
                    printNode(childNode, "\t" + tabs);
                }
            }
        }


        static DataTable getDataTable()
        {
            DataTable result = new DataTable("samples");

            foreach(var att in _attributes)
            {
                result.Columns.Add(att.AttributeName).DataType = typeof(string);
            }
            result.Columns.Add(cat).DataType = typeof(string);

            var lines = File.ReadLines(@"C:\Users\pablo\Desktop\nursery.data 2.txt")
                        .Select(line => line.Split(','));


            foreach (var line in lines)
            {
                result.Rows.Add(line);
            }
            

            return result;

        }

    }
}
