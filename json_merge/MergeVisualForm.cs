using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace json_merge
{
    public partial class MergeVisualForm : Form
    {
        IFormatter _af, _bf, _cf;
        bool _json;

        Dictionary<string, int> _line_number = new Dictionary<string, int>();
        bool _line_numbers_changed;

        List<int> _diff_lines = new List<int>();
        HashSet<int> _diff_lines_set = new HashSet<int>();
        int _last_diff_line = 0;
        int _current_diff = 0;

        public MergeVisualForm(Hashtable parent, Hashtable a, HashDiff adiff, 
            Hashtable b, HashDiff bdiff, Hashtable c, HashDiff cdiff, bool json)
        {
            InitializeComponent();

            _json = json;
            if (_json)
            {
                _af = new JsonFormatter();
                _bf = new JsonFormatter();
                _cf = new JsonFormatter();
            }
            else
            {
                _af = new SjsonFormatter();
                _bf = new SjsonFormatter();
                _cf = new SjsonFormatter();
            }

            /*

            if (_json)
            {
                SameText("{", "{");
                DisplayDiff(a, b, diff, 1);
                SameText("\n}", "\n}");
            }
            else
                
               DisplayDiff(a, b, diff, 0);
            */

            do
            {
                _diff_lines_set.Clear();

                aTextBox.Text = "";
                bTextBox.Text = "";
                cTextBox.Text = "";
                _line_numbers_changed = false;
                DisplayDiff(aTextBox, _af, parent, a, adiff, 0, "");
                DisplayDiff(bTextBox, _bf, parent, b, bdiff, 0, "");
                DisplayDiff(cTextBox, _cf, parent, c, cdiff, 0, "");
            } while (_line_numbers_changed);

            _diff_lines_set.Add(0);
            _diff_lines_set.Add(aTextBox.Lines.Count());
            _diff_lines = _diff_lines_set.OrderBy(i => i).ToList();
            _current_diff = 0;

            aTextBox.SelectionStart = 0;
            aTextBox.SelectionLength = 0;
            bTextBox.SelectionStart = 0;
            bTextBox.SelectionLength = 0;
            cTextBox.SelectionStart = 0;
            cTextBox.SelectionLength = 0;
        }

        public void SameText(RichTextBox rtb, string s)
        {
            rtb.AppendText(s);
        }

        public void RemovedText(RichTextBox rtb, string s)
        {
            if (aTextBox.Lines.Count() != _last_diff_line + 1)
                _diff_lines_set.Add(aTextBox.Lines.Count());
            _last_diff_line = aTextBox.Lines.Count();

            int start = rtb.Lines.Count();
            rtb.SelectionBackColor = Color.Pink;
            rtb.AppendText(s);
            rtb.SelectionBackColor = Color.White;
        }

        public void ChangedText(RichTextBox rtb, string before, string after)
        {
            if (aTextBox.Lines.Count() != _last_diff_line + 1)
                _diff_lines_set.Add(aTextBox.Lines.Count());
            _last_diff_line = aTextBox.Lines.Count();

            int start = rtb.Lines.Count();
            rtb.SelectionBackColor = Color.Pink;
            rtb.AppendText(before);
            rtb.SelectionBackColor = Color.Yellow;
            rtb.AppendText(after);
            rtb.SelectionBackColor = Color.White;
        }

        public void CheckLineNumber(RichTextBox rtb, string path)
        {
            int line_no = _line_number.GetValueOrDefault(path, 0);
            if (rtb.Lines.Count() > line_no)
            {
                _line_numbers_changed = true;
                line_no = rtb.Lines.Count();
                _line_number[path] = line_no;
            }
            while (rtb.Lines.Count() < line_no)
                rtb.AppendText("\n");
        }

        private void DisplayDiff(RichTextBox rtb, IFormatter f, Hashtable a, Hashtable b, HashDiff diff, 
            int indent, string path)
        {
            HashSet<string> keys = new HashSet<string>();
            foreach (string key in a.Keys) keys.Add(key);
            foreach (string key in b.Keys) keys.Add(key);

            foreach (string key in keys.OrderBy(i => i))
            {
                string subpath = path + "." + key;
                CheckLineNumber(rtb, subpath);

                if (diff.Operations.ContainsKey(key))
                {
                    DiffOperation dop = diff.Operations[key];
                    if (dop is RemoveOperation)
                        RemovedText(rtb, f.ObjectField(a, key, indent));
                    else if (dop is ChangeOperation)
                        ChangedText(rtb, f.ObjectField(a, key, indent), f.ObjectField(b, key, indent));
                    else if (dop is ChangeObjectOperation)
                    {
                        SameText(rtb, f.ObjectStart(key, indent));
                        DisplayDiff(rtb, f, a[key] as Hashtable, b[key] as Hashtable, (dop as ChangeObjectOperation).Diff, indent + 1, subpath);
                        SameText(rtb, f.ObjectEnd(indent));
                    }
                    else if (dop is ChangePositionArrayOperation)
                    {
                        SameText(rtb, f.ArrayStart(key, indent));
                        DisplayDiff(rtb, f, a[key] as ArrayList, b[key] as ArrayList, (dop as ChangePositionArrayOperation).Diff, indent + 1, subpath);
                        SameText(rtb, f.ArrayEnd(indent));
                    }
                    else if (dop is ChangeIdArrayOperation)
                    {
                        SameText(rtb, f.ArrayStart(key, indent));
                        DisplayDiff(rtb, f, a[key] as ArrayList, b[key] as ArrayList, (dop as ChangeIdArrayOperation).Diff, indent + 1, subpath);
                        SameText(rtb, f.ArrayEnd(indent));
                    }
                }
                else
                    SameText(rtb, f.ObjectField(b, key, indent));
            }
        }

        private void DisplayArrayDiff(RichTextBox rtb, IFormatter f, object ao, object bo, DiffOperation dop,
            int indent, string path)
        {
            CheckLineNumber(rtb, path);
            if (dop is RemoveOperation)
                RemovedText(rtb, f.ArrayItem(ao, indent));
            else if (dop is ChangeOperation)
                ChangedText(rtb, f.ArrayItem(ao, indent), f.ArrayItem(bo, indent));
            else if (dop is ChangeObjectOperation)
            {
                SameText(rtb, f.ObjectStart(indent));
                DisplayDiff(rtb, f, ao as Hashtable, bo as Hashtable, (dop as ChangeObjectOperation).Diff, indent + 1, path);
                SameText(rtb, f.ObjectEnd(indent));
            }
            else if (dop is ChangePositionArrayOperation)
            {
                SameText(rtb, f.ArrayStart(indent));
                DisplayDiff(rtb, f, ao as ArrayList, bo as ArrayList, (dop as ChangePositionArrayOperation).Diff, indent + 1, path);
                SameText(rtb, f.ArrayEnd(indent));
            }
            else if (dop is ChangeIdArrayOperation)
            {
                SameText(rtb, f.ArrayStart(indent));
                DisplayDiff(rtb, f, ao as ArrayList, bo as ArrayList, (dop as ChangeIdArrayOperation).Diff, indent + 1, path);
                SameText(rtb, f.ArrayEnd(indent));
            }
            else
                SameText(rtb, f.ArrayItem(ao, indent));
        }

        private void DisplayDiff(RichTextBox rtb, IFormatter f, ArrayList a, ArrayList b, PositionArrayDiff diff,
            int indent, string path)
        {
            int n = Math.Max(a.Count, b.Count);
            for (int i = 0; i < n; ++i)
            {
                object ao = i < a.Count ? a[i] : null;
                object bo = i < b.Count ? b[i] : null;
                string subpath = string.Format("{0}.{1}", path, i);
                DisplayArrayDiff(rtb, f, ao, bo, diff.Operations.GetValueOrDefault(i, null), indent, subpath);
            }
        }

        private void DisplayDiff(RichTextBox rtb, IFormatter f, ArrayList a, ArrayList b, HashDiff diff,
            int indent, string path)
        {
            HashSet<object> keys = new HashSet<object>();
            foreach (object h in a)
                keys.Add(Id.GetId(h));
            foreach (object h in b)
                keys.Add(Id.GetId(h));

            foreach (object key in keys.OrderBy(i => i))
            {
                object ao = Id.FindObjectWithId(a, key);
                object bo = Id.FindObjectWithId(b, key);
                string subpath = string.Format("{0}.{1}", path, key);
                DisplayArrayDiff(rtb, f, ao, bo, diff.Operations.GetValueOrDefault(key, null), indent, subpath);
            }
        }

        private void aTextBox_Scroll(object sender, EventArgs e)
        {
            Win32.SetScrollPos(bTextBox.Handle, Win32.GetScrollPos(aTextBox.Handle));
            Win32.SetScrollPos(cTextBox.Handle, Win32.GetScrollPos(aTextBox.Handle));
        }

        private void bTextBox_Scroll(object sender, EventArgs e)
        {
            Win32.SetScrollPos(aTextBox.Handle, Win32.GetScrollPos(bTextBox.Handle));
            Win32.SetScrollPos(cTextBox.Handle, Win32.GetScrollPos(bTextBox.Handle));
        }

        private void cTextBox_Scroll(object sender, EventArgs e)
        {
            Win32.SetScrollPos(aTextBox.Handle, Win32.GetScrollPos(cTextBox.Handle));
            Win32.SetScrollPos(bTextBox.Handle, Win32.GetScrollPos(cTextBox.Handle));
        }

        private void ScrollToCurrentDiff()
        {
            Win32.SendMessage(aTextBox.Handle, Win32.EM_LINESCROLL, 0, -100000);
            int line = _diff_lines[_current_diff] - 10;
            if (line < 0)
                line = 0;
            Win32.SendMessage(aTextBox.Handle, Win32.EM_LINESCROLL, 0, line);
            Win32.SetScrollPos(bTextBox.Handle, Win32.GetScrollPos(aTextBox.Handle));
            Win32.SetScrollPos(cTextBox.Handle, Win32.GetScrollPos(aTextBox.Handle));
        }

        private void nextDifferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ++_current_diff;
            if (_current_diff >= _diff_lines.Count)
                _current_diff = _diff_lines.Count - 1;
            ScrollToCurrentDiff();
        }

        private void previousDifferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            --_current_diff;
            if (_current_diff < 0)
                _current_diff = 0;
            ScrollToCurrentDiff();
        }
    }
}
