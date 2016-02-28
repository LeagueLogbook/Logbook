using System;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Logbook.Server.Infrastructure.NHibernate.UserTypes
{
    public class CompressedJson<T> : IUserType
    {
        public bool Equals(object x, object y)
        {
            if (object.ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            return JsonConvert.SerializeObject(x) == JsonConvert.SerializeObject(y);
        }

        public int GetHashCode(object x)
        {
            return JsonConvert.SerializeObject(x).GetHashCode();
        }

        public object DeepCopy(object value)
        {
            string json = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public SqlType[] SqlTypes => new[] { NHibernateUtil.BinaryBlob.SqlType };

        public Type ReturnedType => typeof (T);

        public bool IsMutable => true;

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            IDataParameter parameter = (IDataParameter)cmd.Parameters[index];

            if (value == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                string json = JsonConvert.SerializeObject(value);
                parameter.Value = this.Compress(json);
            }
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var obj = NHibernateUtil.BinaryBlob.NullSafeGet(rs, names[0]);

            if (obj == null)
                return null;

            var data = (byte[])obj;
            string json = this.Decompress(data);
            return JsonConvert.DeserializeObject<T>(json);
        }

        private byte[] Compress(string json)
        {
            byte[] jsonData = Encoding.UTF8.GetBytes(json);

            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gzipStream.Write(jsonData, 0, jsonData.Length);
                }

                return outputStream.ToArray();
            }
        }

        private string Decompress(byte[] data)
        {
            using (var inputStream = new MemoryStream(data))
            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    gzipStream.CopyTo(outputStream);
                }

                return Encoding.UTF8.GetString(outputStream.ToArray());
            }
        }
    }
}