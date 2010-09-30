using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace json_merge
{
    interface IFormatter
    {
        string ObjectField(Hashtable a, string key, int indent);
        string ObjectStart(int indent);
        string ObjectStart(string key, int indent);
        string ObjectEnd(int indent);
        string ArrayItem(object o, int indent);
        string ArrayStart(string key, int indent);
        string ArrayStart(int indent);
        string ArrayEnd(int indent);
    }

    class SjsonFormatter : IFormatter
    {
        public string ObjectField(Hashtable a, string key, int indent)
        {
            StringBuilder sb = new StringBuilder();
            SJSON.WriteObjectField(a, key, sb, indent);
            return sb.ToString();
        }

        public string ObjectStart(int indent)
        {
            StringBuilder sb = new StringBuilder();
            SJSON.WriteNewLine(sb, indent);
            sb.Append("{");
            return sb.ToString();
        }

        public string ObjectStart(string key, int indent)
        {
            StringBuilder sb = new StringBuilder();
            SJSON.WriteNewLine(sb, indent);
            sb.Append(key);
            sb.Append(" = {");
            return sb.ToString();
        }

        public string ObjectEnd(int indent)
        {
            StringBuilder sb = new StringBuilder();
            SJSON.WriteNewLine(sb, indent);
            sb.Append("}");
            return sb.ToString();
        }

        public string ArrayItem(object o, int indent)
        {
            if (o == null)
                return "";
            StringBuilder sb = new StringBuilder();
            SJSON.WriteNewLine(sb, indent);
            SJSON.Write(o, sb, indent);
            return sb.ToString();
        }

        public string ArrayStart(string key, int indent)
        {
            StringBuilder sb = new StringBuilder();
            SJSON.WriteNewLine(sb, indent);
            sb.Append(key);
            sb.Append(" = [");
            return sb.ToString();
        }

        public string ArrayStart(int indent)
        {
            StringBuilder sb = new StringBuilder();
            SJSON.WriteNewLine(sb, indent);
            sb.Append("[");
            return sb.ToString();
        }

        public string ArrayEnd(int indent)
        {
            StringBuilder sb = new StringBuilder();
            SJSON.WriteNewLine(sb, indent);
            sb.Append("]");
            return sb.ToString();
        }
    }

    class JsonFormatter : IFormatter
    {
        List<bool> _comma = new List<bool>();

        private bool Comma
        {
            get { bool v = _comma[_comma.Count - 1]; _comma[_comma.Count - 1] = true; return v; }
        }

        public JsonFormatter()
        {
            _comma.Add(false);
        }

        public string ObjectField(Hashtable a, string key, int indent)
        {
            StringBuilder sb = new StringBuilder();
            JSON.WriteObjectField(a, key, Comma, sb, indent);
            return sb.ToString();
        }

        public string ObjectStart(int indent)
        {
            StringBuilder sb = new StringBuilder();
            if (Comma) sb.Append(",");
            JSON.WriteNewLine(sb, indent);
            sb.Append("{");
            _comma.Add(false);
            return sb.ToString();
        }

        public string ObjectStart(string key, int indent)
        {
            StringBuilder sb = new StringBuilder();
            if (Comma) sb.Append(",");
            JSON.WriteNewLine(sb, indent);
            sb.Append(key);
            sb.Append(" : {");
            _comma.Add(false);
            return sb.ToString();
        }

        public string ObjectEnd(int indent)
        {
            StringBuilder sb = new StringBuilder();
            JSON.WriteNewLine(sb, indent);
            sb.Append("}");
            _comma.RemoveAt(_comma.Count - 1);
            return sb.ToString();
        }

        public string ArrayItem(object o, int indent)
        {
            if (o == null)
                return "";
            StringBuilder sb = new StringBuilder();
            if (Comma) sb.Append(",");
            JSON.WriteNewLine(sb, indent);
            JSON.Write(o, sb, indent);
            return sb.ToString();
        }

        public string ArrayStart(string key, int indent)
        {
            StringBuilder sb = new StringBuilder();
            if (Comma) sb.Append(",");
            JSON.WriteNewLine(sb, indent);
            sb.Append(key);
            sb.Append(" = [");
            _comma.Add(false);
            return sb.ToString();
        }

        public string ArrayStart(int indent)
        {
            StringBuilder sb = new StringBuilder();
            if (Comma) sb.Append(",");
            JSON.WriteNewLine(sb, indent);
            sb.Append("[");
            _comma.Add(false);
            return sb.ToString();
        }

        public string ArrayEnd(int indent)
        {
            StringBuilder sb = new StringBuilder();
            JSON.WriteNewLine(sb, indent);
            sb.Append("]");
            _comma.RemoveAt(_comma.Count - 1);
            return sb.ToString();
        }
    }
}
