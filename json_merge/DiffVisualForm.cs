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
    public partial class DiffVisualForm : Form
    {
        IFormatter _af, _bf;
        bool _json;
        List<int> _diff_lines = new List<int>();
        int current_diff = 0;

        public DiffVisualForm(Hashtable a, Hashtable b, HashDiff diff, bool json)
        {
            InitializeComponent();

            _json = json;
            if (_json) {
                _af = new JsonFormatter();
                _bf = new JsonFormatter();
            }
            else {
                _af = new SjsonFormatter();
                _bf = new SjsonFormatter();
            }

            _diff_lines.Add(0);

            if (_json)
            {
                SameText("{", "{");
                DisplayDiff(a, b, diff, 1);
                SameText("\n}", "\n}");
            }
            else
                DisplayDiff(a, b, diff, 0);

            _diff_lines.Add(aTextBox.Lines.Count());
            aTextBox.SelectionStart = 0;
            aTextBox.SelectionLength = 0;
            bTextBox.SelectionStart = 0;
            bTextBox.SelectionLength = 0;
        }

        private void MakeEqualLength(ref string a, ref string b)
        {
            int na = 0; int nb = 0;
            for (int i = 0; i < a.Length; ++i)
                if (a[i] == '\n')
                    ++na;
            for (int i = 0; i < b.Length; ++i)
                if (b[i] == '\n')
                    ++nb;
            while (na < nb)
            {
                a = a + "\n";
                ++na;
            }
            while (nb < na)
            {
                b = b + "\n";
                ++nb;
            }
        }

        private void RemovedText(string astr, string bstr)
        {
            _diff_lines.Add(aTextBox.Lines.Count());

            aTextBox.SelectionBackColor = Color.Pink;
            bTextBox.SelectionBackColor = Color.Pink;

            MakeEqualLength(ref astr, ref bstr);
            aTextBox.AppendText(astr);
            bTextBox.AppendText(bstr);

            aTextBox.SelectionBackColor = Color.White;
            bTextBox.SelectionBackColor = Color.White;
        }

        private void ChangedText(string astr, string bstr)
        {
            _diff_lines.Add(aTextBox.Lines.Count());

            aTextBox.SelectionBackColor = Color.Yellow;
            bTextBox.SelectionBackColor = Color.Yellow;

            MakeEqualLength(ref astr, ref bstr);
            aTextBox.AppendText(astr);
            bTextBox.AppendText(bstr);

            aTextBox.SelectionBackColor = Color.White;
            bTextBox.SelectionBackColor = Color.White;
        }

        private void SameText(string a, string b)
        {
            aTextBox.AppendText(a);
            bTextBox.AppendText(b);
        }

        public void DisplayDiff(Hashtable a, Hashtable b, HashDiff diff, int indent)
        {
            HashSet<string> keys = new HashSet<string>();
            foreach (string key in a.Keys) keys.Add(key);
            foreach (string key in b.Keys) keys.Add(key);

            foreach (string key in keys.OrderBy(i => i))
            {
                if (diff.Operations.ContainsKey(key))
                {
                    DiffOperation dop = diff.Operations[key];
                    if (dop is RemoveOperation)
                        RemovedText(_af.ObjectField(a, key, indent), "");
                    else if (dop is ChangeOperation)
                        ChangedText(_af.ObjectField(a, key, indent), _bf.ObjectField(b, key, indent));
                    else if (dop is ChangeObjectOperation)
                    {
                        SameText(_af.ObjectStart(key, indent), _bf.ObjectStart(key, indent));
                        DisplayDiff(a[key] as Hashtable, b[key] as Hashtable, (dop as ChangeObjectOperation).Diff, indent+1);
                        SameText(_af.ObjectEnd(indent), _bf.ObjectEnd(indent));
                    }
                    else if (dop is ChangePositionArrayOperation)
                    {
                        SameText(_af.ArrayStart(key, indent), _bf.ArrayStart(key, indent));
                        DisplayDiff(a[key] as ArrayList, b[key] as ArrayList, (dop as ChangePositionArrayOperation).Diff, indent + 1);
                        SameText(_af.ArrayEnd(indent), _bf.ArrayEnd(indent));
                    }
                    else if (dop is ChangeIdArrayOperation)
                    {
                        SameText(_af.ArrayStart(key, indent), _bf.ArrayStart(key, indent));
                        DisplayDiff(a[key] as ArrayList, b[key] as ArrayList, (dop as ChangeIdArrayOperation).Diff, indent + 1);
                        SameText(_af.ArrayEnd(indent), _bf.ArrayEnd(indent));
                    }
                }
                else
                    SameText(_af.ObjectField(a, key, indent), _bf.ObjectField(a, key, indent));
            }
        }

        private void DisplayArrayDiff(object ao, object bo, DiffOperation dop, int indent)
        {
            if (dop is RemoveOperation)
                RemovedText(_af.ArrayItem(ao, indent), "");
            else if (dop is ChangeOperation)
                ChangedText(_af.ArrayItem(ao, indent), _bf.ArrayItem(bo, indent));
            else if (dop is ChangeObjectOperation)
            {
                SameText(_af.ObjectStart(indent), _bf.ObjectStart(indent));
                DisplayDiff(ao as Hashtable, bo as Hashtable, (dop as ChangeObjectOperation).Diff, indent + 1);
                SameText(_af.ObjectEnd(indent), _bf.ObjectEnd(indent));
            }
            else if (dop is ChangePositionArrayOperation)
            {
                SameText(_af.ArrayStart(indent), _bf.ArrayStart(indent));
                DisplayDiff(ao as ArrayList, bo as ArrayList, (dop as ChangePositionArrayOperation).Diff, indent + 1);
                SameText(_af.ArrayEnd(indent), _bf.ArrayEnd(indent));
            }
            else if (dop is ChangeIdArrayOperation)
            {
                SameText(_af.ArrayStart(indent), _bf.ArrayStart(indent));
                DisplayDiff(ao as ArrayList, bo as ArrayList, (dop as ChangeIdArrayOperation).Diff, indent + 1);
                SameText(_af.ArrayEnd(indent), _bf.ArrayEnd(indent));
            }
            else
                SameText(_af.ArrayItem(ao, indent), _bf.ArrayItem(bo, indent));
        }

        private void DisplayDiff(ArrayList a, ArrayList b, PositionArrayDiff diff, int indent)
        {
            int n = Math.Max(a.Count, b.Count);
            for (int i=0; i<n; ++i) {
                object ao = i < a.Count ? a[i] : null;
                object bo = i < b.Count ? b[i] : null;
                DisplayArrayDiff(ao, bo, diff.Operations.GetValueOrDefault(i,null), indent);
            }
        }

        private void DisplayDiff(ArrayList a, ArrayList b, HashDiff diff, int indent)
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
                DisplayArrayDiff(ao, bo, diff.Operations.GetValueOrDefault(key, null), indent);
            }
        }

        private unsafe Win32.POINT GetScrollPos(IntPtr handle)
        {
            Win32.POINT res = new Win32.POINT();
            IntPtr ptr = new IntPtr(&res);
            Win32.SendMessage(handle, Win32.EM_GETSCROLLPOS, 0, ptr);
            return res;
        }

        private unsafe void SetScrollPosRaw(IntPtr handle, Win32.POINT point)
        {
            IntPtr ptr = new IntPtr(&point);
            Win32.SendMessage(handle, Win32.EM_SETSCROLLPOS, 0, ptr);
        }

        static bool _recursing = false;

        private void SetScrollPos(IntPtr handle, Win32.POINT point)
        {
            if (_recursing)
                return;

            _recursing = true;
            double t0 = -1, t1 = 0, y0 = -1, y1 = 0;
            int iter_max = 100;
            while (iter_max-- > 0)
            {
                double t = (point.y - y0) / (y1 - y0) * (t1 - t0) + t0;
                SetScrollPosRaw(handle, new Win32.POINT(point.x, (int)t));
                Win32.POINT p = GetScrollPos(handle);
                double yres = p.y;
                if ((int)yres == point.y || (int)yres == y1 || (int)t == t1)
                    break;
                t0 = t1;
                y0 = y1;
                t1 = t;
                y1 = yres;
            }
            _recursing = false;
        }

        private void aTextBox_VScroll(object sender, EventArgs e)
        {
           SetScrollPos(bTextBox.Handle, GetScrollPos(aTextBox.Handle));
        }

        private void aTextBox_HScroll(object sender, EventArgs e)
        {
            SetScrollPos(bTextBox.Handle, GetScrollPos(aTextBox.Handle));
        }

        private void bTextBox_VScroll(object sender, EventArgs e)
        {
           SetScrollPos(aTextBox.Handle, GetScrollPos(bTextBox.Handle));
        }

        private void bTextBox_HScroll(object sender, EventArgs e)
        {
            SetScrollPos(aTextBox.Handle, GetScrollPos(bTextBox.Handle));
        }

        private void ScrollToCurrentDiff()
        {
            Win32.SendMessage(aTextBox.Handle, Win32.EM_LINESCROLL, 0, -100000);
            Win32.SendMessage(aTextBox.Handle, Win32.EM_LINESCROLL, 0, _diff_lines[current_diff]);
            SetScrollPos(bTextBox.Handle, GetScrollPos(aTextBox.Handle));
        }

        private void nextDifferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ++current_diff;
            if (current_diff >= _diff_lines.Count)
                current_diff = _diff_lines.Count - 1;
            ScrollToCurrentDiff();
        }

        private void previousDifferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            --current_diff;
            if (current_diff < 0)
                current_diff = 0;
            ScrollToCurrentDiff();
        }
    }
}
