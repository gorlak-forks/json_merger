using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Globalization;

namespace json_merge
{
    class Program
    {
        static bool _use_json = false;
        static bool _window = false;

        static void Error(string s)
        {
            Show(s, "Error");
            Environment.Exit(-1);
        }

        static void Show(string s, string title)
        {
            if (_window)
            {
                string s2 = s.Replace("\n", "\r\n");
                TextForm f = new TextForm();
                f.Text = title;
                f.DisplayText = s2;
                f.ShowDialog();
            }
            else
                Console.WriteLine(s);
        }

        static Hashtable Load(string s)
        {
            if (!File.Exists(s))
                Error(String.Format("File not found: {0}", s));
            if (_use_json)
                return JSON.Load(s);
            else
                return SJSON.Load(s);
        }

        static void Save(Hashtable h, string s)
        {
            if (_use_json)
                JSON.Save(h, s);
            else
                SJSON.Save(h, s);
        }

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            if (args.Length == 0)
                PrintUsage();

            bool escape_hold = false;

            int i = 0;
            while (i < args.Length)
            {
                if (args[i] == "-json")
                {
                    _use_json = true;
                    ++i;
                }
                else if (args[i] == "-window")
                {
                    _window = true;
                    ++i;
                }
                else if (args[i] == "-diff")
                {
                    if (i + 2 >= args.Length)
                        Error("Not enough arguments for -diff");
                    Hashtable a = Load(args[i + 1]);
                    Hashtable b = Load(args[i + 2]);
                    HashDiff diff = JsonDiff.Diff(a, b);
                    if (_window) {
                        DiffVisualForm form = new DiffVisualForm(a, b, diff, _use_json);
                        form.ShowDialog();
                    } else {
                        StringBuilder sb = new StringBuilder();
                        diff.Write(sb);
                        Show(sb.ToString(), "Diff");
                    }
                    i += 3;
                }
                else if (args[i] == "-merge")
                {
                    if (i + 4 >= args.Length)
                        Error("Not enough arguments for -merge");
                    Hashtable parent = Load(args[i + 1]);
                    Hashtable theirs = Load(args[i + 2]);
                    Hashtable mine = Load(args[i + 3]);
                    Hashtable result = JsonDiff.Merge(parent, theirs, mine);
                    Save(result, args[i + 4]);
                    i += 5;
                }
                else if (args[i] == "-help")
                {
                    PrintUsage();
                    ++i;
                }
                else if (args[i] == "-hold")
                {
                    while (!escape_hold)
                        ;
                    ++i;
                }
                else if (args[i] == "-test")
                {
                    Test.TestAll();
                    ++i;
                }
            }
        }

        static void PrintUsage()
        {
            string usage =
@"NAME
    json_merge - compare and merge SJSON & JSON files

USAGE
    -diff FILE_A FILE_B
        prints the difference between the two SJSON files

    -merge BASE THEIRS MINE RESULT
        performs a 3-way merge between BASE, THEIRS and MINE and
        stores the result in result

    -help
        print this help text

OPTIONS
    -json
        Use JSON as input and output format (if not specified, SJSON is used).

    -window
        Display output in a window rather than in the console.";
            Show(usage, "Usage");
        }
    }
}
