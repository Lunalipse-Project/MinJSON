using MinJSON.JSON;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Writer
{
    public interface ISeqentialWriter
    {
        void WriteString(string value);
        void WriteBoolean(bool value);
        void WriteNull();
        void WriteDecimal(decimal value);
        void WriteInteger(int value);
        void WriteDouble(double value);
        void WriteSingle(float value);
        void WriteLong(long value);
        void WriteByte(byte value);
        void WriteProperty(string propertyName);
        void WriteObjectBegin();
        void WriteObjectEnd();
        void WriteArrayBegin();
        void WriteArrayEnd();
        JsonValue ToJsonValue();
        void WriterReset();
    }
}
