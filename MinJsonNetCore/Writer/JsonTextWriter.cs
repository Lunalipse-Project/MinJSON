using System;
using System.Collections.Generic;
using System.Text;
using MinJSON.Exception;
using MinJSON.JSON;

namespace MinJSON.Writer
{
    public class JsonTextWriter : ISeqentialWriter
    {
        const char ARRAY_START = '[';
        const char ARRAY_END = ']';
        const char OBJECT_START = '{';
        const char OBJECT_END = '}';

        StringBuilder builder;
        Stack<Scope> scopeMarker;
        Scope currentScope;
        int elementCount = 0;

        public JsonTextWriter()
        {
            builder = new StringBuilder();
            scopeMarker = new Stack<Scope>();
        }

        public JsonValue ToJsonValue()
        {
            throw new NotSupportedException("TextWriter can only produce textual result of JSON.");
        }

        public void WriteArrayBegin()
        {
            _startNewScope(ARRAY_START);
        }

        public void WriteArrayEnd()
        {
            _leaveScope(ARRAY_START, ARRAY_END);
        }

        public void WriteBoolean(bool value)
        {
            _writeValue(value.ToString());
        }

        public void WriteByte(byte value)
        {
            _writeValue(value.ToString());
        }

        public void WriteDecimal(decimal value)
        {
            _writeValue(value.ToString());
        }

        public void WriteDouble(double value)
        {
            _writeValue(value.ToString());
        }

        public void WriteInteger(int value)
        {
            _writeValue(value.ToString());
        }

        public void WriteLong(long value)
        {
            _writeValue(value.ToString());
        }

        public void WriteNull()
        {
            _writeValue("null");
        }

        public void WriteObjectBegin()
        {
            _startNewScope(OBJECT_START);
        }

        public void WriteObjectEnd()
        {
            _leaveScope(OBJECT_START, OBJECT_END);
        }

        public void WriteProperty(string propertyName)
        {
            _writeProperty(propertyName);
        }

        public void WriterReset()
        {
            builder.Clear();
            scopeMarker.Clear();
        }

        public void WriteSingle(float value)
        {
            _writeValue(value.ToString());
        }

        public void WriteString(string value)
        {
            _writeValue($"\"{value}\"");
        }

        public override string ToString()
        {
            if (!isEmpty())
            {
                throw new JsonWriterException("Json is not closed");
            }
            return builder.ToString();
        }

        private void _writeProperty(string propertyName)
        {
            if (isEmpty() && (currentScope.ScopeId != OBJECT_START))
            {
                throw new JsonWriterException("A property can only exist in object scope");
            }
            if(currentScope.PropertyHold)
            {
                throw new JsonWriterException("Previous property not completed.");
            }
            if (!isFirstElement())
            {
                builder.Append(",");
            }
            builder.Append($"\"{propertyName}\":");
            currentScope.PropertyHold = true;
            currentScope.ElementCount++;
        }

        private void _writeValue(string value)
        {
            if (isEmpty() && (currentScope.ScopeId != ARRAY_START || !currentScope.PropertyHold))
            {
                throw new JsonWriterException("A value can only exist in array scope");
            }
            if (currentScope.ScopeId == ARRAY_START && !isFirstElement())
            {
                builder.Append(",");
            }
            builder.Append(value);
            if (currentScope.PropertyHold)
            {
                currentScope.PropertyHold = false;
            }
            currentScope.ElementCount++;
        }

        private void _startNewScope(char scopeId)
        {
            builder.Append(scopeId);
            scopeMarker.Push(new Scope() { ScopeId = scopeId, ElementCount = 0, PropertyHold = false });
            currentScope = scopeMarker.Peek();
        }

        private void _leaveScope(char start, char end_scopeId)
        {
            Scope scope;
            if(!isEmpty() && (scope = scopeMarker.Pop()).ScopeId != start)
            {
                throw new JsonWriterException("Scope not matched");
            }
            builder.Append(end_scopeId);
            currentScope = isEmpty() ? default(Scope) : scopeMarker.Peek();
        }

        private bool isFirstElement()
        {
            return currentScope.ElementCount == 0;
        }

        private bool isEmpty()
        {
            return scopeMarker.Count == 0;
        }
    }

    struct Scope
    {
        public char ScopeId;
        public int ElementCount;
        public bool PropertyHold;
    }
}
